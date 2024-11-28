using BallsRUs.Context;
using BallsRUs.Entities;
using BallsRUs.Models.Product;
using BallsRUs.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace BallsRUs.Controllers
{
    public class ProductController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        public IActionResult Catalog(string? category = null, string? search = null, bool discounted = false, string? sorting = null)
        {
            if (!string.IsNullOrWhiteSpace(search))
                ViewBag.Search = search;

            if (!string.IsNullOrWhiteSpace(category))
                ViewBag.Category = category;

            // Appliquer le tri
            if (sorting is not null && sorting.Equals(Constants.PRICE_HIGH_TO_LOW, StringComparison.CurrentCultureIgnoreCase))
                ViewBag.SortingType = Constants.PRICE_HIGH_TO_LOW;
            else if (sorting is not null && sorting.Equals(Constants.PRICE_LOW_TO_HIGH, StringComparison.CurrentCultureIgnoreCase))
                ViewBag.SortingType = Constants.PRICE_LOW_TO_HIGH;
            else if (sorting is not null && sorting.Equals(Constants.BRAND_ALPHABETICAL, StringComparison.CurrentCultureIgnoreCase))
                ViewBag.SortingType = Constants.BRAND_ALPHABETICAL;
            else if (sorting is not null && sorting.Equals(Constants.RELEASE_NEW_TO_OLD, StringComparison.CurrentCultureIgnoreCase))
                ViewBag.SortingType = Constants.RELEASE_NEW_TO_OLD;

            // Appliquer les filtres
            ViewBag.FilterDiscounted = discounted;

            return View();
        }

        public IActionResult Details(Guid productId)
        {
            object? messagePassedToPage = TempData["PassMessageToProductDetails"];
            object? errorPassedToPage = TempData["PassErrorToProductDetails"];
            ViewBag.IsMessage = false;

            if (messagePassedToPage is not null)
            {
                ModelState.AddModelError(string.Empty, messagePassedToPage.ToString()!);
                ViewBag.isMessage = true;
            }
            else if (errorPassedToPage is not null)
            {
                ModelState.AddModelError(string.Empty, errorPassedToPage.ToString()!);
            }

            Product? productToShow = _context.Products.Find(productId);

            if (productToShow is null)
                return NotFound();

            List<Category> categories = _context.Categories.Where(c => productToShow.Categories.Contains(c)).ToList();

            ProductDetailsVM vm = new()
            {
                Id = productToShow.Id,
                SKU = productToShow.SKU,
                Name = productToShow.Name,
                Brand = productToShow.Brand,
                Model = productToShow.Model,
                ImagePath = productToShow.ImagePath,
                ShortDescription = productToShow.ShortDescription,
                FullDescription = productToShow.FullDescription,
                WeightInGrams = productToShow.WeightInGrams,
                Size = productToShow.Size,
                Quantity = productToShow.Quantity,
                RetailPrice = string.Format(new CultureInfo("fr-CA"), "{0:C}", productToShow.RetailPrice),
                DiscountedPrice = string.Format(new CultureInfo("fr-CA"), "{0:C}", productToShow.DiscountedPrice),
                PublicationDate = string.Format("{0:dd/MM/yyyy}", productToShow.PublicationDate),
                Categories = categories
            };

            return View(vm);
        }
    }
}
