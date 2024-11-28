using BallsRUs.Context;
using BallsRUs.Entities;
using BallsRUs.Models.Account;
using BallsRUs.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Security.Claims;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace BallsRUs.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;

        public AccountController(SignInManager<User> signInManager, UserManager<User> userManager, ApplicationDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }

        [AllowAnonymous]
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LogIn(LogInVM vm, bool redirectToCheckout)
        {
            if (!ModelState.IsValid)
                return View(vm);

            try
            {
                string? anonymUserShoppingCartId = HttpContext.Session.GetString(Constants.SHOPPING_CART_SESSION_KEY);
                Guid? anonymUserShoppingCartGuid = null;

                if (!string.IsNullOrWhiteSpace(anonymUserShoppingCartId))
                {
                    if (!Guid.TryParse(anonymUserShoppingCartId, out Guid sessionShoppingCartGuid))
                        throw new Exception("The ID of the user isn't valid.");
                    
                    anonymUserShoppingCartGuid = sessionShoppingCartGuid;
                }

                SignInResult result = await _signInManager.PasswordSignInAsync(vm.NomUtilisateur!, vm.MotDePasse!, false, false);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, "Le nom d'utilisateur et le mot de passe ne correspondent pas. Veuillez réessayer");
                    return View(vm);
                }

                if (anonymUserShoppingCartGuid is not null)
                {
                    if (vm.NomUtilisateur is not null)
                    {
                        User? user = await _userManager.FindByNameAsync(vm.NomUtilisateur);

                        if (user is null)
                            throw new Exception("The user was not found.");

                        Guid userGuid = user.Id;

                        ShoppingCart? accountShoppingCart = _context.ShoppingCarts.FirstOrDefault(sc => sc.UserId == userGuid);
                        ShoppingCart? anonymUserShoppingCart = _context.ShoppingCarts.FirstOrDefault(sc => sc.Id == anonymUserShoppingCartGuid);

                        if (anonymUserShoppingCart is not null)
                        {
                            if (accountShoppingCart is null)
                            {
                                anonymUserShoppingCart.UserId = userGuid;
                                _context.SaveChanges();
                            }
                            else
                            {
                                if (accountShoppingCart.ProductsQuantity == 0)
                                {
                                    anonymUserShoppingCart.UserId = userGuid;
                                    DeleteShoppingCart(accountShoppingCart.Id);
                                }
                                else
                                {
                                    DeleteShoppingCart(anonymUserShoppingCart.Id);
                                }
                            }

                            HttpContext.Session.Remove(Constants.SHOPPING_CART_SESSION_KEY);
                        }
                        else
                        {
                            throw new Exception("The ID of the anonym user's shopping cart isn't valid.");
                        }
                    }
                    else
                    {
                        throw new Exception("The ID of the user isn't valid.");
                    }
                }
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Une erreur est survenue. Veuillez réessayer.");
                return View(vm);
            }

            if (redirectToCheckout)
                return RedirectToAction("Index", "ShoppingCart");
            else
                return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            bool userAlreadyExists = _context.Users.Any(u => u.UserName == vm.UserName);

            if (userAlreadyExists)
            {
                ModelState.AddModelError(string.Empty, "Un utilisateur avec ce courriel existe déjà.");
                return View(vm);
            }

            try
            {
                User newUser = new(vm.UserName!)
                {
                    FirstName = vm.FirstName,
                    LastName = vm.LastName,
                    Email = vm.UserName,
                    NormalizedEmail = vm.UserName!.ToUpper(),
                    PhoneNumber = vm.PhoneNumber
                };

                IdentityResult result = await _userManager.CreateAsync(newUser, vm.Password!);

                if (vm.addAddress)
                {
                    Address address = new()
                    {
                        StateProvince = vm.StateProvince!,
                        Street = vm.Street!,
                        City = vm.City!,
                        Country = vm.Country!,
                        PostalCode = vm.PostalCode!,
                        UserId = newUser.Id
                    };
                    _context.Addresses.Add(address);
                    _context.SaveChanges();
                }

                if (!result.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, "Impossible de créer le compte. Veuillez réessayer.");
                    return View(vm);
                }

                result = await _userManager.AddToRoleAsync(newUser, Constants.ROLE_UTILISATEUR);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, "Impossible de créer le compte. Veuillez réessayer.");
                    return View(vm);
                }
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Une erreur est survenue. Veuillez réessayer.");
                return View(vm);
            }

            return RedirectToAction("LogIn", "Account");
        }

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Details()
        {
            string? userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userIdString is null)
                throw new Exception("The user id wasn't found.");
            
            Guid userId = Guid.Parse(userIdString);
            User? userToShow = _context.Users.Find(userId);

            if (userToShow is null)
                throw new Exception("The user wasn't found.");

            Address? addressToShow = _context.Addresses.FirstOrDefault(x => x.UserId == userId);

            AccountDetailsVM vm = new AccountDetailsVM()
            {
                FirstName = userToShow.FirstName,
                LastName = userToShow.LastName,
                Email = userToShow.Email,
                PhoneNumber = userToShow.PhoneNumber,
                AddressCity = addressToShow?.City,
                AddressCountry = addressToShow?.Country,
                AddressPostalCode = addressToShow?.PostalCode,
                AddressStateProvince = addressToShow?.StateProvince,
                AddressStreet = addressToShow?.Street,
            };

            return View(vm);
        }

        public IActionResult EditInfo()
        {
            string? userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userIdString is null)
                throw new Exception("The id of the user doesn't exist.");

            Guid userId = Guid.Parse(userIdString);
            User? userToShow = _context.Users.Find(userId);

            if (userToShow is null)
                throw new Exception("The user wasn't found.");

            AccountEditInfoVM vm = new AccountEditInfoVM()
            {
                FirstName = userToShow.FirstName,
                LastName = userToShow.LastName,
                Email = userToShow.Email,
                PhoneNumber = userToShow.PhoneNumber
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult EditInfo(AccountEditInfoVM vm)
        {
            try
            {
                string? userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userIdString is null)
                    throw new Exception("The id of the user doesn't exist.");

                Guid userId = Guid.Parse(userIdString);
                User? userToChange = _context.Users.Find(userId);

                if (userToChange is null)
                    throw new Exception("The user wasn't found.");

                userToChange.FirstName = vm.FirstName;
                userToChange.LastName = vm.LastName;
                userToChange.PhoneNumber = vm.PhoneNumber;

                if (userToChange.Email != vm.Email)
                {
                    bool userAlreadyExists = _context.Users.Any(u => u.UserName == vm.Email);

                    if (userAlreadyExists)
                    {
                        ModelState.AddModelError(string.Empty, "Un utilisateur avec ce courriel existe déjà.");
                        return View(vm);
                    }
                    else
                    {
                        userToChange.Email = vm.Email;
                        userToChange.UserName = vm.Email;
                        userToChange.NormalizedUserName = vm.Email!.ToUpper();
                        userToChange.NormalizedEmail = vm.Email!.ToUpper();
                    }
                }
                
                _context.SaveChanges();

                return RedirectToAction(nameof(Details));
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Une erreur est survenue. Veuillez réessayer.");
                return View(vm);
            }
        }

        public IActionResult Address()
        {
            string? userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userIdString is null)
                throw new Exception("The user id wasn't found.");

            Guid userId = Guid.Parse(userIdString);
            User? userToShow = _context.Users.Find(userId);

            if (userToShow is null)
                throw new Exception("The user wasn't found.");

            Address? address = _context.Addresses.FirstOrDefault(a => a.UserId == userId);

            if (address is not null)
            {
                AccountAddressVM vm = new AccountAddressVM()
                {
                    Street = address.Street,
                    City = address.City,
                    Country = address.Country,
                    StateProvince = address.StateProvince,
                    PostalCode = address.PostalCode,
                    HasExistingAddress = true
                };

                return View(vm);
            }
            else
            {
                AccountAddressVM vm = new AccountAddressVM()
                {
                    HasExistingAddress = false
                };

                return View(vm);
            }
        }

        [HttpPost]
        public IActionResult Address(AccountAddressVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            string? userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userIdString is null)
                throw new Exception("The user id wasn't found.");

            Guid userId = Guid.Parse(userIdString);
            User? userToShow = _context.Users.Find(userId);

            if (userToShow is null)
                throw new Exception("The user wasn't found.");

            if (vm.HasExistingAddress)
            {
                Address? address = _context.Addresses.FirstOrDefault(a => a.UserId == userId);

                if (address is null)
                    throw new Exception("The address of the user wasn't found.");

                address.Street = vm.Street!;
                address.City = vm.City!;
                address.StateProvince = vm.StateProvince!;
                address.Country = vm.Country!;
                address.PostalCode = vm.PostalCode!;

                _context.SaveChanges();
            }
            else
            {
                Address address = new()
                {
                    UserId = userId,
                    Street = vm.Street!,
                    City = vm.City!,
                    StateProvince = vm.StateProvince!,
                    Country = vm.Country!,
                    PostalCode = vm.PostalCode!,
                };

                _context.Addresses.Add(address);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Details));
        }

        public IActionResult OrdersHistory()
        {
            string? userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userIdString is null)
                throw new Exception("The user id wasn't found.");

            Guid userId = Guid.Parse(userIdString);

            IEnumerable<AccountOrderHistoryVM> orders = _context.Orders.Where(x => x.UserId == userId).OrderByDescending(o => o.ConfirmationDate).Select(order => new AccountOrderHistoryVM
            {
                Id = order.Id,
                Number = order.Number,
                Status = order.Status == OrderStatus.Opened ? "Ouvert"
                    : order.Status == OrderStatus.Confirmed ? "Confirmée"
                    : order.Status == OrderStatus.Payed ? "Payée"
                    : order.Status == OrderStatus.Refunded ? "Remboursée"
                    : order.Status == OrderStatus.Canceled ? "Annulée"
                    : Constants.NA,
                FullName = order.FirstName + " " + order.LastName,
                ProductQuantity = order.ProductQuantity,
                Total = string.Format(new CultureInfo("fr-CA"), "{0:C}", order.Total),
                CreationDate = order.CreationDate
            });

            return View(orders);
        }

        private void DeleteShoppingCart(Guid shoppingCartId)
        {
            ShoppingCart? shoppingCart = _context.ShoppingCarts.Find(shoppingCartId);

            if (shoppingCart is null)
                throw new ArgumentOutOfRangeException(nameof(shoppingCartId));

            if (shoppingCart.ProductsQuantity == 0)
            {
                _context.ShoppingCarts.Remove(shoppingCart);
                _context.SaveChanges();
            }
            else
            {
                IEnumerable<ShoppingCartItem> items = _context.ShoppingCartItems.Where(i => i.ShoppingCartId == shoppingCartId);
                foreach (ShoppingCartItem item in items)
                {
                    Entities.Product? product = _context.Products.Find(item.ProductId);

                    if (product is null)
                        throw new Exception("The product of the item wasn't found.");

                    product.Quantity += item.Quantity;

                    _context.ShoppingCartItems.Remove(item);
                }

                _context.ShoppingCarts.Remove(shoppingCart);
                _context.SaveChanges();
            }
        }
    }
}