using BallsRUs.Context;
using BallsRUs.Entities;
using BallsRUs.Models.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.IO;

namespace BallsRUs.Controllers
{
    [Authorize(Roles = "Administrateur")]
    public class AdminController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment) : Controller
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IWebHostEnvironment _hostingEnvironment = hostingEnvironment;

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ManageProduct()
        {
            IQueryable<AdminManageProductVM> vm = _context.Products.Select(product => new AdminManageProductVM
            {
                Id = product.Id,
                SKU = product.SKU,
                Name = product.Name,
                Brand = product.Brand,
                Model = product.Model,
                Category = product.Categories!.FirstOrDefault()!.Name,
                Image = Path.GetFileName(product.ImagePath),
                WeightInGrams = product.WeightInGrams,
                Size = product.Size,
                Quantity = product.Quantity,
                RetailPrice = string.Format(new CultureInfo("fr-CA"), "{0:C}", product.RetailPrice),
                DiscountedPrice = string.Format(new CultureInfo("fr-CA"), "{0:C}", product.DiscountedPrice),
                PublicationDate = string.Format("{0:dd/MM/yyyy}", product.PublicationDate),
                LastUpdated = string.Format("{0:dd/MM/yyyy}", product.LastUpdated),
                IsArchived = product.IsArchived
            });

            return View(vm);
        }

        public IActionResult CreateProduct()
        {
            AdminCreateProductVM vm = new()
            {
                Categories = _context.Categories.Select(c => c.Name).ToList()!
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(AdminCreateProductVM vm)
        {
            vm.Categories = _context.Categories.Select(c => c.Name).ToList()!;

            if (!ModelState.IsValid)
                return View(vm);

            bool productAlreadyExists = _context.Products.Any(u => u.SKU == vm.SKU);

            if (productAlreadyExists)
            {
                ModelState.AddModelError(string.Empty, "Un produit avec le même SKU existe déjà.");
                return View(vm);
            }

            if (vm.Image is not null && vm.Image.Length > 0)
            {
                // Obtenez le chemin de destination pour enregistrer le fichier dans le répertoire img/products
                string webRootPath = _hostingEnvironment.WebRootPath;
                string filePath = Path.Combine(webRootPath, "img/products", vm.SKU! + ".jpg");

                using (FileStream stream = new(filePath, FileMode.Create))
                {
                    await vm.Image.CopyToAsync(stream);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Veuillez sélectionner une image.");
                return View(vm);
            }

            Category category = _context.Categories.FirstOrDefault(c => c.Name == vm.SelectedCategory)!;

            if (category is null)
            {
                ModelState.AddModelError(string.Empty, "Veuillez sélectionner une catégorie existante.");
                return View(vm);
            }

            Product product = new()
            {
                SKU = vm.SKU,
                Name = vm.Name,
                Brand = vm.Brand,
                Model = vm.Model,
                ImagePath = "~/img/products/" + vm.SKU + ".jpg",
                WeightInGrams = vm.WeightInGrams,
                Size = vm.Size,
                Quantity = vm.Quantity,
                RetailPrice = vm.RetailPrice,
                DiscountedPrice = vm.DiscountedPrice,
                ShortDescription = vm.ShortDescription,
                FullDescription = vm.FullDescription,
                PublicationDate = (DateTime?)DateTime.Now,
                LastUpdated = (DateTime?)DateTime.Now,
                IsArchived = false
            };

            product.Categories.Add(category);

            _context.Products.Add(product);
            _context.SaveChanges();

            return RedirectToAction(nameof(ManageProduct));
        }

        public IActionResult EditProduct(Guid id)
        {
            ViewBag.Id = id;

            Product? toEdit = _context.Products.Find(id);

            if (toEdit is null)
                throw new ArgumentOutOfRangeException(nameof(id));

            AdminEditProductVM vm = new()
            {
                SKU = toEdit.SKU,
                Name = toEdit.Name,
                Brand = toEdit.Brand,
                Model = toEdit.Model,
                WeightInGrams = toEdit.WeightInGrams,
                Size = toEdit.Size,
                Quantity = toEdit.Quantity,
                RetailPrice = toEdit.RetailPrice,
                DiscountedPrice = toEdit.DiscountedPrice,
                ShortDescription = toEdit.ShortDescription,
                FullDescription = toEdit.FullDescription
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(AdminEditProductVM vm, Guid id)
        {
            ViewBag.Id = id;

            if (!ModelState.IsValid)
                return View(vm);

            Product? toEdit = _context.Products.Find(id);

            if (toEdit is null)
                throw new ArgumentOutOfRangeException(nameof(id));

            if (vm.ModifyImage && vm.Image is not null && vm.Image.Length > 0)
            {
                string webRootPath = _hostingEnvironment.WebRootPath;
                string filePath = Path.Combine(webRootPath, "img/products", vm.SKU! + ".jpg");

                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);

                using (FileStream stream = new(filePath, FileMode.Create))
                {
                    await vm.Image.CopyToAsync(stream);
                }

                toEdit.ImagePath = "~/img/products/" + vm.SKU + ".jpg";
            }
            else if (vm.ModifyImage && (vm.Image is null || vm.Image.Length > 0))
            {
                ModelState.AddModelError(string.Empty, "Veuillez sélectionner une image.");
                return View(vm);
            }

            toEdit.SKU = vm.SKU;
            toEdit.Name = vm.Name;
            toEdit.Brand = vm.Brand;
            toEdit.Model = vm.Model;
            toEdit.WeightInGrams = vm.WeightInGrams;
            toEdit.Size = vm.Size;
            toEdit.Quantity = vm.Quantity;
            toEdit.RetailPrice = vm.RetailPrice;
            toEdit.DiscountedPrice = vm.DiscountedPrice;
            toEdit.ShortDescription = vm.ShortDescription;
            toEdit.FullDescription = vm.FullDescription;
            toEdit.LastUpdated = (DateTime?)DateTime.Now;

            _context.SaveChanges();

            return RedirectToAction(nameof(DetailsProduct), new { id = id });
        }

        public IActionResult DetailsProduct(Guid id)
        {
            Product? toShow = _context.Products.Find(id);

            if (toShow is null)
                throw new ArgumentOutOfRangeException(nameof(id));

            Category? category = _context.Categories.FirstOrDefault(c => c.Products!.Contains(toShow));

            if (category is null)
                throw new ArgumentOutOfRangeException(nameof(id));

            AdminDetailsProductVM vm = new()
            {
                Id = toShow.Id,
                SKU = toShow.SKU,
                Name = toShow.Name,
                Brand = toShow.Brand,
                Model = toShow.Model,
                Category = category!.Name,
                ImagePath = toShow.ImagePath,
                WeightInGrams = toShow.WeightInGrams,
                Size = toShow.Size,
                Quantity = toShow.Quantity,
                RetailPrice = string.Format(new CultureInfo("fr-CA"), "{0:C}", toShow.RetailPrice),
                DiscountedPrice = string.Format(new CultureInfo("fr-CA"), "{0:C}", toShow.DiscountedPrice),
                PublicationDate = string.Format("{0:dd/MM/yyyy}", toShow.PublicationDate),
                LastUpdated = string.Format("{0:dd/MM/yyyy}", toShow.LastUpdated),
                IsArchived = toShow.IsArchived
            };

            return View(vm);
        }

        public IActionResult ArchiveProduct(Guid id)
        {
            Product? toArchive = _context.Products.Find(id);

            if (toArchive is null)
                throw new ArgumentOutOfRangeException(nameof(id));

            toArchive.IsArchived = !toArchive.IsArchived;

            _context.SaveChanges();

            return RedirectToAction(nameof(ManageProduct));
        }

        public IActionResult DeleteProduct(Guid id)
        {
            Product? toDelete = _context.Products.Find(id);

            if (toDelete is null)
                throw new ArgumentOutOfRangeException(nameof(id));

            if (toDelete.IsArchived)
            {
                _context.Products.Remove(toDelete);
                _context.SaveChanges();
            }

            string webRootPath = _hostingEnvironment.WebRootPath;
            string filePath = Path.Combine(webRootPath, "img/products", toDelete.SKU! + ".jpg");

            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);

            return RedirectToAction(nameof(ManageProduct));
        }

        public IActionResult ManageOrder()
        {
            IQueryable<AdminManageOrderVM> vm = _context.Orders.Select(order => new AdminManageOrderVM
            {
                Id = order.Id,
                Number = order.Number,
                Status = order.Status,
                FirstName = order.FirstName,
                LastName = order.LastName,
                EmailAddress = order.EmailAddress,
                PhoneNumber = order.PhoneNumber,
                ProductQuantity = order.ProductQuantity,
                SubTotal = string.Format(new CultureInfo("fr-CA"), "{0:C}", order.SubTotal),
                Total = string.Format(new CultureInfo("fr-CA"), "{0:C}", order.Total),
                CreationDate = string.Format("{0:dd/MM/yyyy}", order.CreationDate)
            });

            return View(vm);
        }

        public IActionResult CancelOrder(Guid id)
        {
            Order? orderToCancel = _context.Orders.Find(id);

            if (orderToCancel is null)
                throw new ArgumentOutOfRangeException(nameof(id));

            if (orderToCancel.Status != OrderStatus.Canceled)
            {
                List<OrderItem> orderToCancelItems = _context.OrderItems.Where(oi => oi.OrderId == orderToCancel.Id).ToList();

                foreach (OrderItem item in orderToCancelItems)
                {
                    Product? product = _context.Products.Find(item.ProductId);

                    if (product is not null)
                        product.Quantity += item.Quantity;
                }

                orderToCancel.Status = OrderStatus.Canceled;
                orderToCancel.ModificationDate = DateTime.UtcNow;
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(ManageOrder));
        }
    }
}
