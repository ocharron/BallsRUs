using BallsRUs.Context;
using BallsRUs.Entities;
using BallsRUs.Models.ShoppingCart;
using BallsRUs.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BallsRUs.Controllers
{
    public class ShoppingCartController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        public IActionResult Index()
        {
            Guid scId = GetShoppingCartId();

            object? errorPassedToShoppingCart = TempData["PassErrorToShoppingCart"];

            if (errorPassedToShoppingCart is not null)
                ModelState.AddModelError(string.Empty, errorPassedToShoppingCart.ToString()!);

            ShoppingCart? shoppingCart = _context.ShoppingCarts.FirstOrDefault(sc => sc.Id == scId);

            if (shoppingCart is null)
                throw new Exception("The shopping cart is not valid.");

            List<ShoppingCartItem>? items = _context.ShoppingCartItems.Where(sci => sci.ShoppingCartId == scId).ToList();
            List<ShoppingCartProductVM>? productsVM = new();

            int quantity = 0;
            decimal? productsCost = 0m;

            ViewBag.AllProductsAreAvailable = true;

            if (items.Count > 0)
            {
                foreach (ShoppingCartItem item in items)
                {
                    Product? product = _context.Products.FirstOrDefault(p => p.Id == item.ProductId);

                    if (product is null)
                        throw new Exception("The product is not valid.");

                    ShoppingCartProductVM p = new()
                    {
                        Id = item.Id,
                        ProductId = product.Id,
                        Name = product.Name,
                        RetailPrice = product.RetailPrice,
                        DiscountedPrice = product.DiscountedPrice,
                        ImagePath = product.ImagePath,
                        Quantity = item.Quantity
                    };

                    if (product.Quantity < item.Quantity)
                    {
                        ViewBag.AllProductsAreAvailable = false;
                        p.IsAvailable = false;
                    }
                    else
                    {
                        p.IsAvailable = true;
                    }

                    productsVM.Add(p);

                    quantity += item.Quantity;
                    productsCost += product.DiscountedPrice is not null ? product.DiscountedPrice * item.Quantity : product.RetailPrice * item.Quantity;
                }
            }

            ShoppingCartListVM vm = new()
            {
                Quantity = quantity,
                ShippingCost = quantity > 0 ? Constants.ESTIMATED_SHIPPING_COST : 0m,
                ProductsCost = productsCost,
                Items = productsVM
            };

            return View(vm);
        }

        public IActionResult AddProductToCart(Guid productId)
        {
            Guid scId = GetShoppingCartId();

            Product? product = _context.Products.Find(productId);

            if (product is null)
                throw new ArgumentOutOfRangeException(nameof(productId));

            if (product.Quantity > 0)
            {
                ShoppingCart? shoppingCart = _context.ShoppingCarts.Find(scId);

                if (shoppingCart is null)
                    throw new Exception("The shopping cart is not valid.");

                ShoppingCartItem? item = _context.ShoppingCartItems.FirstOrDefault(i => i.ShoppingCartId == scId && i.ProductId == product.Id);

                if (item is null)
                {
                    ShoppingCartItem cartItem = new()
                    {
                        Quantity = 1,
                        CreationDate = DateTime.Now,
                        ShoppingCartId = scId,
                        ProductId = product.Id
                    };

                    shoppingCart.ProductsQuantity++;

                    TempData["PassMessageToProductDetails"] = "Le produit a été ajouté à votre panier.";

                    _context.ShoppingCartItems.Add(cartItem);
                    _context.SaveChanges();
                }
                else
                {
                    if (product.Quantity - (item.Quantity + 1) >= 0)
                    {
                        shoppingCart.ProductsQuantity++;
                        item.Quantity++;

                        TempData["PassMessageToProductDetails"] = "Le produit a été ajouté à votre panier.";

                        _context.SaveChanges();
                    }
                    else
                    {
                        TempData["PassErrorToProductDetails"] = "Ce produit n'est plus en réserve. Votre panier contient déjà les derniers items de ce produit.";
                    }
                }
            }
            else
            {
                TempData["PassErrorToProductDetails"] = "Le produit n'est actuellement pas disponible.";
            }

            return RedirectToAction("Details", "Product", new { productId });
        }

        public IActionResult RemoveProductFromCart(Guid itemId)
        {
            Guid scId = GetShoppingCartId();

            ShoppingCartItem? item = _context.ShoppingCartItems.Find(itemId);

            if (item is null)
                throw new ArgumentOutOfRangeException(nameof(itemId));

            if (item.ShoppingCartId == scId)
            {
                ShoppingCart? shoppingCart = _context.ShoppingCarts.Find(scId);

                if (shoppingCart is null)
                    throw new Exception("The shopping cart is not valid.");

                shoppingCart.ProductsQuantity -= item.Quantity;

                _context.ShoppingCartItems.Remove(item);
                _context.SaveChanges();
            }
            else
            {
                TempData["PassErrorToShoppingCart"] = "L'item ne fait pas partie de votre panier d'achat.";
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult IncrementItem(Guid itemId)
        {
            Guid scId = GetShoppingCartId();

            ShoppingCartItem? item = _context.ShoppingCartItems.Find(itemId);

            if (item is null)
                throw new ArgumentOutOfRangeException(nameof(itemId));

            if (item.ShoppingCartId == scId)
            {
                ShoppingCart? shoppingCart = _context.ShoppingCarts.Find(scId);

                if (shoppingCart is null)
                    throw new Exception("The shopping cart is not valid.");

                Product? product = _context.Products.Find(item.ProductId);

                if (product is null)
                    throw new Exception("The product is not valid.");

                if (product.Quantity - item.Quantity > 0)
                {
                    item.Quantity++;
                    shoppingCart.ProductsQuantity++;

                    _context.SaveChanges();
                }
                else
                {
                    TempData["PassErrorToShoppingCart"] = "Ce produit n'est plus en réserve. Votre panier contient déjà les derniers items de ce produit.";
                }
            }
            else
            {
                TempData["PassErrorToShoppingCart"] = "L'item ne fait pas partie de votre panier d'achat.";
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult DecrementItem(Guid itemId)
        {
            Guid scId = GetShoppingCartId();

            ShoppingCartItem? item = _context.ShoppingCartItems.Find(itemId);

            if (item is null)
                throw new ArgumentOutOfRangeException(nameof(itemId));

            if (item.ShoppingCartId == scId)
            {
                ShoppingCart? shoppingCart = _context.ShoppingCarts.Find(scId);

                if (shoppingCart is null)
                    throw new Exception("The shopping cart is not valid.");

                if (item.Quantity > 1)
                {
                    item.Quantity--;
                    shoppingCart.ProductsQuantity--;

                    _context.SaveChanges();
                }
                else if (item.Quantity == 1)
                {
                    shoppingCart.ProductsQuantity--;

                    _context.ShoppingCartItems.Remove(item);
                    _context.SaveChanges();
                }
            }
            else
            {
                TempData["PassErrorToShoppingCart"] = "L'item ne fait pas partie de votre panier d'achat.";
            }

            return RedirectToAction(nameof(Index));
        }

        private Guid GetShoppingCartId()
        {
            if (User.Identity!.IsAuthenticated)
            {
                string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId is null)
                    throw new Exception("No user was found.");

                if (Guid.TryParse(userId, out Guid userGuid))
                {
                    ShoppingCart? shoppingCart = _context.ShoppingCarts.FirstOrDefault(sc => sc.UserId == userGuid);

                    if (shoppingCart is null)
                    {
                        ShoppingCart newSC = new()
                        {
                            Id = Guid.NewGuid(),
                            ProductsQuantity = 0,
                            CreationDate = DateTime.Now,
                            UserId = userGuid
                        };

                        _context.ShoppingCarts.Add(newSC);
                        _context.SaveChanges();

                        return newSC.Id;
                    }
                    else
                    {
                        return shoppingCart.Id;
                    }
                }
                else
                {
                    throw new Exception("The ID of the user isn't valid.");
                }
            }
            else
            {
                string? shoppingCartId = HttpContext.Session.GetString(Constants.SHOPPING_CART_SESSION_KEY);

                if (shoppingCartId is null)
                {
                    ShoppingCart newSC = new()
                    {
                        Id = Guid.NewGuid(),
                        ProductsQuantity = 0,
                        CreationDate = DateTime.Now
                    };

                    _context.ShoppingCarts.Add(newSC);
                    _context.SaveChanges();

                    HttpContext.Session.SetString(Constants.SHOPPING_CART_SESSION_KEY, newSC.Id.ToString());

                    return newSC.Id;
                }
                else
                {
                    if (Guid.TryParse(shoppingCartId, out Guid shoppingCartGuid))
                        return shoppingCartGuid;
                    else
                        throw new Exception("The key of the cart isn't valid.");
                }
            }
        }
    }
}
