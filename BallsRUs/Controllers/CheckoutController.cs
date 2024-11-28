using BallsRUs.Context;
using BallsRUs.Entities;
using BallsRUs.Models.Checkout;
using BallsRUs.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;
using System.Globalization;
using System.Security.Claims;
using System.Text;
using Address = BallsRUs.Entities.Address;
using Product = BallsRUs.Entities.Product;

namespace BallsRUs.Controllers
{
    public class CheckoutController(ApplicationDbContext context, UserManager<User> userManager, IOptions<StripeOptions> stripeOptions) : Controller
    {
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<User> _userManager = userManager;
        private readonly IOptions<StripeOptions> _stripeOptions = stripeOptions;

        public async Task<IActionResult> Information()
        {
            if (User.Identity!.IsAuthenticated)
            {
                string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId is null)
                    throw new Exception("The ID of the user is not valid.");

                if (Guid.TryParse(userId, out Guid userGuid))
                {
                    User? user = await _userManager.FindByIdAsync(userId);

                    if (user is null)
                        throw new Exception("The user was not found.");

                    CheckoutInformationVM vm = new CheckoutInformationVM()
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        EmailAddress = user.Email,
                        PhoneNumber = user.PhoneNumber
                    };

                    Address? address = _context.Addresses.FirstOrDefault(a => a.UserId == userGuid);

                    if (address is not null)
                    {
                        vm.AddressStreet = address.Street;
                        vm.AddressCity = address.City;
                        vm.AddressStateProvince = address.StateProvince;
                        vm.AddressCountry = address.Country;
                        vm.AddressPostalCode = address.PostalCode;
                        vm.HasExistingAddress = true;
                        vm.UseExistingAddress = true;
                    }
                    else
                    {
                        vm.HasExistingAddress = false;
                    }

                    return View(vm);
                }
                else
                {
                    throw new Exception("The ID of the user isn't valid.");
                }
            }

            return View();
        }

        [HttpPost]
        public IActionResult Information(CheckoutInformationVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            Guid orderGuid;

            if (User.Identity!.IsAuthenticated)
            {
                string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId is null)
                    throw new Exception("The ID of the user is not valid.");

                if (Guid.TryParse(userId, out Guid userGuid))
                {
                    Address? address;

                    if (vm.HasExistingAddress && vm.UseExistingAddress)
                    {
                        address = _context.Addresses.FirstOrDefault(a => a.UserId == userGuid);

                        if (address is null)
                            throw new Exception("The address of the user wasn't found.");
                    }
                    else
                    {
                        address = new()
                        {
                            Id = Guid.NewGuid(),
                            Street = vm.AddressStreet!,
                            City = vm.AddressCity!,
                            StateProvince = vm.AddressStateProvince!,
                            Country = vm.AddressCountry!,
                            PostalCode = vm.AddressPostalCode!
                        };

                        if (vm.SaveAddress)
                            address.UserId = userGuid;

                        _context.Addresses.Add(address);
                    }

                    orderGuid = Guid.NewGuid();

                    Order order = new()
                    {
                        Id = orderGuid,
                        Number = GenerateRandomOrderNumber(),
                        FirstName = vm.FirstName,
                        LastName = vm.LastName,
                        EmailAddress = vm.EmailAddress,
                        PhoneNumber = vm.PhoneNumber,
                        AddressId = address.Id,
                        UserId = userGuid,
                        CreationDate = DateTime.UtcNow,
                        ShippingCost = Constants.ESTIMATED_SHIPPING_COST
                    };

                    ShoppingCart? shoppingCart = _context.ShoppingCarts.FirstOrDefault(sc => sc.UserId == userGuid);

                    if (shoppingCart is null)
                        throw new Exception("The shopping cart of the user wasn't found");

                    decimal productCost = 0.0m;
                    int productQuantity = 0;

                    List<ShoppingCartItem> shoppingCartItems = _context.ShoppingCartItems.Where(sci => sci.ShoppingCartId == shoppingCart.Id).ToList();

                    foreach (ShoppingCartItem item in shoppingCartItems)
                    {
                        Product? product = _context.Products.Find(item.ProductId);

                        if (product is null)
                            throw new Exception("The product of the item wasn't found.");

                        if (product.Quantity < item.Quantity)
                        {
                            TempData["PassErrorToShoppingCart"] = "Un item de votre panier n'est plus en stock. Veuillez le retirer avant de compléter votre commande.";
                            return RedirectToAction("Index", "ShoppingCart");
                        }

                        OrderItem oi = new OrderItem()
                        {
                            Quantity = item.Quantity,
                            UnitaryPrice = product.DiscountedPrice is not null ? (decimal)product.DiscountedPrice! : (decimal)product.RetailPrice!,
                            TotalCost = product.DiscountedPrice is not null
                                            ? (decimal)(product.DiscountedPrice! * item.Quantity!)
                                            : (decimal)(product.RetailPrice! * item.Quantity!),
                            CreationDate = DateTime.UtcNow,
                            OrderId = orderGuid,
                            ProductId = product.Id
                        };

                        _context.OrderItems.Add(oi);

                        productCost += product.DiscountedPrice is not null
                                            ? (decimal)(product.DiscountedPrice! * item.Quantity!)
                                            : (decimal)(product.RetailPrice! * item.Quantity!);

                        productQuantity += item.Quantity;

                        product.Quantity -= item.Quantity;

                        _context.ShoppingCartItems.Remove(item);
                    }

                    _context.ShoppingCarts.Remove(shoppingCart);

                    order.ProductQuantity = productQuantity;
                    order.ProductsCost = productCost;
                    order.SubTotal = productCost + order.ShippingCost;
                    order.Taxes = order.SubTotal * Constants.TAXES_PERCENTAGE;
                    order.Total = order.SubTotal + order.Taxes;

                    _context.Orders.Add(order);
                    _context.SaveChanges();
                }
                else
                {
                    throw new Exception("The ID of the user isn't valid.");
                }
            }
            else
            {
                Address address = new()
                {
                    Id = Guid.NewGuid(),
                    Street = vm.AddressStreet!,
                    City = vm.AddressCity!,
                    StateProvince = vm.AddressStateProvince!,
                    Country = vm.AddressCountry!,
                    PostalCode = vm.AddressPostalCode!
                };

                _context.Addresses.Add(address);

                orderGuid = Guid.NewGuid();

                Order order = new()
                {
                    Id = orderGuid,
                    Number = GenerateRandomOrderNumber(),
                    FirstName = vm.FirstName,
                    LastName = vm.LastName,
                    EmailAddress = vm.EmailAddress,
                    PhoneNumber = vm.PhoneNumber,
                    AddressId = address.Id,
                    CreationDate = DateTime.UtcNow,
                    ShippingCost = Constants.ESTIMATED_SHIPPING_COST
                };

                string? shoppingCartId = HttpContext.Session.GetString(Constants.SHOPPING_CART_SESSION_KEY);

                if (shoppingCartId is null)
                    throw new Exception("The session key for the ID of the shopping cart doesn't exist.");

                if (Guid.TryParse(shoppingCartId, out Guid shoppingCartGuid))
                {
                    ShoppingCart? shoppingCart = _context.ShoppingCarts.FirstOrDefault(sc => sc.Id == shoppingCartGuid);

                    if (shoppingCart is null)
                        throw new Exception("The shopping cart of the anonym user wasn't found");

                    decimal productCost = 0.0m;
                    int productQuantity = 0;

                    List<ShoppingCartItem> shoppingCartItems = _context.ShoppingCartItems.Where(sci => sci.ShoppingCartId == shoppingCart.Id).ToList();

                    foreach (ShoppingCartItem item in shoppingCartItems)
                    {
                        Product? product = _context.Products.Find(item.ProductId);

                        if (product is null)
                            throw new Exception("The product of the item wasn't found.");

                        if (product.Quantity < item.Quantity)
                        {
                            TempData["PassErrorToShoppingCart"] = "Un item de votre panier n'est plus en stock. Veuillez le retirer avant de compléter votre commande.";
                            return RedirectToAction("Index", "ShoppingCart");
                        }

                        OrderItem oi = new OrderItem()
                        {
                            Quantity = item.Quantity,
                            UnitaryPrice = product.DiscountedPrice is not null ? (decimal)product.DiscountedPrice! : (decimal)product.RetailPrice!,
                            TotalCost = product.DiscountedPrice is not null
                                            ? (decimal)(product.DiscountedPrice! * item.Quantity!)
                                            : (decimal)(product.RetailPrice! * item.Quantity!),
                            CreationDate = DateTime.UtcNow,
                            OrderId = orderGuid,
                            ProductId = product.Id
                        };

                        _context.OrderItems.Add(oi);

                        productCost += product.DiscountedPrice is not null
                                            ? (decimal)(product.DiscountedPrice! * item.Quantity!)
                                            : (decimal)(product.RetailPrice! * item.Quantity!);

                        productQuantity += item.Quantity;

                        product.Quantity -= item.Quantity;

                        _context.ShoppingCartItems.Remove(item);
                    }

                    _context.ShoppingCarts.Remove(shoppingCart);

                    order.ProductQuantity = productQuantity;
                    order.ProductsCost = productCost;
                    order.SubTotal = productCost + order.ShippingCost;
                    order.Taxes = order.SubTotal * Constants.TAXES_PERCENTAGE;
                    order.Total = order.SubTotal + order.Taxes;

                    _context.Orders.Add(order);
                    _context.SaveChanges();
                }
                else
                {
                    throw new Exception("The ID of shopping cart isn't valid.");
                }
            }

            return RedirectToAction(nameof(Confirmation), new { orderId = orderGuid });
        }

        public IActionResult Confirmation(Guid orderId)
        {
            Order? order = _context.Orders.Find(orderId);

            if (order is null)
                throw new ArgumentOutOfRangeException(nameof(orderId));

            Address? address = _context.Addresses.Find(order.AddressId);

            if (address is null)
                throw new Exception("The address wasn't found.");

            List<OrderItem> items = _context.OrderItems.Where(oi => oi.OrderId == orderId).ToList();

            List<CheckoutConfirmationItemVM> itemListVM = new List<CheckoutConfirmationItemVM>();

            foreach (OrderItem item in items)
            {
                Product? itemProduct = _context.Products.Find(item.ProductId);

                CheckoutConfirmationItemVM itemVM = new CheckoutConfirmationItemVM()
                {
                    Id = item.Id,
                    Quantity = item.Quantity,
                    TotalCost = string.Format(new CultureInfo("fr-CA"), "{0:C}", item.TotalCost),
                    ProductName = itemProduct is not null ? itemProduct.Name : Constants.NA
                };

                itemListVM.Add(itemVM);
            }

            CheckoutConfirmationVM vm = new CheckoutConfirmationVM()
            {
                Id = orderId,
                Number = order.Number,
                FirstName = order.FirstName,
                LastName = order.LastName,
                EmailAddress = order.EmailAddress,
                PhoneNumber = order.PhoneNumber,
                ProductQuantity = order.ProductQuantity,
                ProductCost = string.Format(new CultureInfo("fr-CA"), "{0:C}", order.ProductsCost),
                ShippingCost = string.Format(new CultureInfo("fr-CA"), "{0:C}", order.ShippingCost),
                SubTotal = string.Format(new CultureInfo("fr-CA"), "{0:C}", order.SubTotal),
                Taxes = string.Format(new CultureInfo("fr-CA"), "{0:C}", order.Taxes),
                Total = string.Format(new CultureInfo("fr-CA"), "{0:C}", order.Total),
                AddressStreet = address.Street,
                AddressCity = address.City,
                AddressStateProvince = address.StateProvince,
                AddressCountry = address.Country,
                AddressPostalCode = address.PostalCode,
                OrderItems = itemListVM
            };

            if (order.Status != OrderStatus.Opened)
                vm.OrderAlreadyConfirmed = true;

            return View(vm);
        }

        [HttpPost]
        public IActionResult Confirmation(CheckoutConfirmationVM vm, Guid orderId)
        {
            Order? order = _context.Orders.Find(orderId);

            if (order is null)
                throw new ArgumentOutOfRangeException(nameof(orderId));

            if (!ModelState.IsValid)
            {
                Address? address = _context.Addresses.Find(order.AddressId);

                if (address is null)
                    throw new Exception("The address wasn't found.");

                List<OrderItem> items = _context.OrderItems.Where(oi => oi.OrderId == orderId).ToList();

                List<CheckoutConfirmationItemVM> itemListVM = new List<CheckoutConfirmationItemVM>();

                foreach (OrderItem item in items)
                {
                    Product? itemProduct = _context.Products.Find(item.ProductId);

                    CheckoutConfirmationItemVM itemVM = new CheckoutConfirmationItemVM()
                    {
                        Id = item.Id,
                        Quantity = item.Quantity,
                        TotalCost = string.Format(new CultureInfo("fr-CA"), "{0:C}", item.TotalCost),
                        ProductName = itemProduct is not null ? itemProduct.Name : Constants.NA
                    };

                    itemListVM.Add(itemVM);
                }

                vm.Id = orderId;
                vm.Number = order.Number;
                vm.FirstName = order.FirstName;
                vm.LastName = order.LastName;
                vm.EmailAddress = order.EmailAddress;
                vm.PhoneNumber = order.PhoneNumber;
                vm.ProductQuantity = order.ProductQuantity;
                vm.ProductCost = string.Format(new CultureInfo("fr-CA"), "{0:C}", order.ProductsCost);
                vm.ShippingCost = string.Format(new CultureInfo("fr-CA"), "{0:C}", order.ShippingCost);
                vm.SubTotal = string.Format(new CultureInfo("fr-CA"), "{0:C}", order.SubTotal);
                vm.Taxes = string.Format(new CultureInfo("fr-CA"), "{0:C}", order.Taxes);
                vm.Total = string.Format(new CultureInfo("fr-CA"), "{0:C}", order.Total);
                vm.AddressStreet = address.Street;
                vm.AddressCity = address.City;
                vm.AddressStateProvince = address.StateProvince;
                vm.AddressCountry = address.Country;
                vm.AddressPostalCode = address.PostalCode;
                vm.OrderItems = itemListVM;

                return View(vm);
            }

            order.Status = OrderStatus.Confirmed;
            order.ConfirmationDate = DateTime.Now;
            _context.SaveChanges();

            return RedirectToAction(nameof(Payment), new { orderId });
        }

        public IActionResult Payment(Guid orderId)
        {
            Order? order = _context.Orders.Find(orderId);

            if (order is null)
                throw new ArgumentOutOfRangeException(nameof(orderId));

            if (order.Status == OrderStatus.Confirmed)
            {
                ViewBag.IsPayed = false;
                Address? address = _context.Addresses.Find(order.AddressId);

                if (address is null)
                    throw new Exception("The address wasn't found.");

                CheckoutPaymentVM vm = new CheckoutPaymentVM()
                {
                    Id = orderId,
                    FullName = order.FirstName + " " + order.LastName,
                    EmailAddress = order.EmailAddress,
                    PhoneNumber = order.PhoneNumber,
                    Total = order.Total,
                    AddressStreet = address.Street,
                    AddressCity = address.City,
                    AddressStateProvince = address.StateProvince,
                    AddressCountry = address.Country,
                    AddressPostalCode = address.PostalCode
                };

                return View(vm);
            }
            else
            {
                ViewBag.IsPayed = true;
                return View();
            }
        }

        public IActionResult Charge(Guid orderId, [FromBody] PaymentData paymentData)
        {
            Order? order = _context.Orders.Find(orderId);

            if (order is null)
                throw new ArgumentOutOfRangeException(nameof(orderId));

            if (order.Status == OrderStatus.Confirmed)
            {
                StripeConfiguration.ApiKey = _stripeOptions.Value.SecretKey;

                ChargeCreateOptions chargeOptions = new()
                {
                    Amount = (long?)(order.Total * 100), // Price in cents
                    Source = paymentData.Token,
                    Currency = _stripeOptions.Value.CurrencyCode,
                    Metadata = new Dictionary<string, string>
                    {
                        { "address_country", paymentData.Country! },
                        { "address_line1", paymentData.Street! },
                        { "address_city", paymentData.City! },
                        { "address_zip", paymentData.PostalCode! },
                        { "address_state", paymentData.StateProvince! },
                        { "phone", paymentData.Phone! },
                        { "name", paymentData.Name! },
                        { "email", paymentData.Email! }
                    }
                };

                ChargeService chargeService = new();
                Charge charge = chargeService.Create(chargeOptions);

                Payment payment = new Payment()
                {
                    Id = Guid.NewGuid(),
                    Name = paymentData.Name,
                    Digits = paymentData.Digits,
                    Email = paymentData.Email,
                    Phone = paymentData.Phone,
                    Street = paymentData.Street,
                    City = paymentData.City,
                    StateProvince = paymentData.StateProvince,
                    Country = paymentData.Country,
                    PostalCode = paymentData.PostalCode,
                    PaymentDate = DateTime.UtcNow,
                    StripePaymentId = paymentData.Token
                };

                order.Status = OrderStatus.Payed;
                order.PaymentId = payment.Id;

                _context.Payments.Add(payment);
                _context.SaveChanges();

                return Ok(charge.ToJson());
            }
            else
            {
                return NotFound();
            }
        }

        public IActionResult Receipt(Guid orderId)
        {
            Order? order = _context.Orders.Find(orderId);

            if (order is null)
                throw new ArgumentOutOfRangeException(nameof(orderId));

            Address? address = _context.Addresses.Find(order.AddressId);

            if (address is null)
                throw new Exception("The address wasn't found.");

            Payment? payment = _context.Payments.Find(order.PaymentId);

            if (payment is null)
                throw new Exception("The payment wasn't found.");

            CheckoutReceiptVM vm = new()
            {
                Id = orderId,
                OrderNumber = order.Number,
                OrderFullName = order.FirstName + " " + order.LastName,
                OrderEmailAddress = order.EmailAddress,
                OrderPhoneNumber = order.PhoneNumber,
                OrderTotal = string.Format(new CultureInfo("fr-CA"), "{0:C}", order.Total),
                PaymentFullName = payment.Name,
                PaymentLast4 = payment.Digits,
                PaymentDate = string.Format("{0:dd/MM/yyyy}", payment.PaymentDate),
                AddressStreet = payment.Street,
                AddressCity = payment.City,
                AddressStateProvince = payment.StateProvince,
                AddressCountry = payment.Country,
                AddressPostalCode = payment.PostalCode
            };

            return View(vm);
        }

        private static string GenerateRandomOrderNumber()
        {
            Random random = new();
            StringBuilder stringBuilder = new(16);

            for (int i = 0; i < 16; i++)
            {
                int index = random.Next(Constants.CHARACTERS_BANK.Length);
                stringBuilder.Append(Constants.CHARACTERS_BANK[index]);
            }

            return stringBuilder.ToString();
        }
    }
}