using BallsRUs.Context;
using BallsRUs.Entities;
using BallsRUs.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BallsRUs.ViewComponents
{
    public class ProductList(ApplicationDbContext context) : ViewComponent
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IViewComponentResult> InvokeAsync(bool isHomepageShowcase = false, string? category = null, string? search = null, string? sortingType = null, bool discounted = false)
        {
            ViewBag.IsHomePageShowcase = isHomepageShowcase;

            IQueryable<Product> products = _context.Products.Where(p => !p.IsArchived).AsQueryable();

            if (isHomepageShowcase)
            {
                // Take the 4 products with the biggest discount percentage.
                products = products.Where(p => p.DiscountedPrice.HasValue)
                                   .OrderByDescending(p => (p.RetailPrice - p.DiscountedPrice) / p.RetailPrice * 100)
                                   .Take(Constants.NB_OF_SHOWCASED_PRODUCTS)
                                   .AsQueryable();
            }
            else if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();
                string[] searchTerms = search.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                foreach (string term in searchTerms)
                {
                    products = products.Where(p => p.Name!.Contains(term, StringComparison.CurrentCultureIgnoreCase)
                        || p.Brand!.Contains(term, StringComparison.CurrentCultureIgnoreCase)
                        || p.Model!.Contains(term, StringComparison.CurrentCultureIgnoreCase)
                        || p.SKU!.Equals(term, StringComparison.CurrentCultureIgnoreCase)
                        || p.Categories.Any(c => c.Name!.Contains(term, StringComparison.CurrentCultureIgnoreCase))
                    );
                }

                if (!products.Any())
                    ViewBag.MessageBasDePage = "Aucun produit ne correspond à la recherche.";
            }
            else if (!string.IsNullOrWhiteSpace(category))
            {
                IEnumerable<Category> categories = _context.Categories.AsEnumerable();

                if (categories.Any(c => c.Name!.ToLower() == category.ToLower()))
                {
                    Category? selectedCategory = categories.FirstOrDefault(c => c.Name!.ToLower() == category.ToLower());

                    List<Category> categoryAndChildren = GetCategoryAndChildren(categories, selectedCategory!);

                    products = products.Where(p => p.Categories.Any(productCategory => categoryAndChildren.Contains(productCategory)));
                }
                else
                {
                    products = products.Where(p => !p.Categories.Any());

                    ViewBag.MessageBasDePage = "Aucune catégorie trouvée.";
                }
            }

            // Filter the products to only take the discounted products (if specified)
            if (discounted)
                products = products.Where(p => p.DiscountedPrice != null);

            // Sort the products
            if (!string.IsNullOrWhiteSpace(sortingType))
            {
                switch (sortingType)
                {
                    case Constants.PRICE_HIGH_TO_LOW: products = products.OrderByDescending(p => p.DiscountedPrice ?? p.RetailPrice);
                        break;
                    case Constants.PRICE_LOW_TO_HIGH: products = products.OrderBy(p => p.DiscountedPrice ?? p.RetailPrice);
                        break;
                    case Constants.BRAND_ALPHABETICAL: products = products.OrderBy(p => p.Brand);
                        break;
                    case Constants.RELEASE_NEW_TO_OLD: products = products.OrderByDescending(p => p.PublicationDate);
                        break;
                    default:
                        break;
                }
            }

            List<Product>? filteredProducts = await products.ToListAsync();
            
            return View(filteredProducts);
        }

        private static List<Category> GetCategoryAndChildren(IEnumerable<Category> allCategories, Category parentCategory)
        {
            List<Category> result = new() { parentCategory };

            IEnumerable<Category> children = allCategories.Where(c => c.ParentCategoryId == parentCategory.Id).ToList();

            foreach (Category child in children)
                result.AddRange(GetCategoryAndChildren(allCategories, child));

            return result;
        }
    }
}