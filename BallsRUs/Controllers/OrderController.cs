using BallsRUs.Context;
using BallsRUs.Entities;
using BallsRUs.Models.Order;
using BallsRUs.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace BallsRUs.Controllers
{
    public class OrderController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        public IActionResult Details(Guid orderId)
        {
            Order? order = _context.Orders.Find(orderId);

            if (order is null)
                throw new ArgumentOutOfRangeException(nameof(orderId));

            Address? address = _context.Addresses.Find(order.AddressId);

            if (address is null)
                throw new ArgumentOutOfRangeException(nameof(address));

            Payment? payment = _context.Payments.Find(order.PaymentId);

            List<OrderItem> items = _context.OrderItems.Where(oi => oi.OrderId == orderId).ToList();

            List<OrderDetailsItemVM> itemListVM = new();

            foreach (OrderItem item in items)
            {
                Product? itemProduct = _context.Products.Find(item.ProductId);

                OrderDetailsItemVM itemVM = new()
                {
                    Id = item.Id,
                    Quantity = item.Quantity,
                    TotalCost = string.Format(new CultureInfo("fr-CA"), "{0:C}", item.TotalCost),
                    ProductName = itemProduct is not null ? itemProduct.Name : Constants.NA
                };

                itemListVM.Add(itemVM);
            }

            OrderDetailsVM vm = new()
            {
                Id = order.Id,
                ConfirmationDate = string.Format("{0:dd/MM/yyyy}", order.ConfirmationDate),
                CreationDate = string.Format("{0:dd/MM/yyyy}", order.CreationDate),
                EmailAddress = order.EmailAddress,
                FirstName = order.FirstName,
                LastName = order.LastName,
                ModificationDate = string.Format("{0:dd/MM/yyyy}", order.ModificationDate),
                Number = order.Number,
                PhoneNumber = order.PhoneNumber,
                ProductQuantity = order.ProductQuantity,
                ProductsCost = string.Format(new CultureInfo("fr-CA"), "{0:C}", order.ProductsCost),
                ShippingCost = string.Format(new CultureInfo("fr-CA"), "{0:C}", order.ShippingCost),
                SubTotal = string.Format(new CultureInfo("fr-CA"), "{0:C}", order.SubTotal),
                Taxes = string.Format(new CultureInfo("fr-CA"), "{0:C}", order.Taxes),
                Total = string.Format(new CultureInfo("fr-CA"), "{0:C}", order.Total),
                AddressStreet = address.Street,
                AddressCity = address.City,
                AddressStateProvince = address.StateProvince,
                AddressCountry = address.Country,
                AddressPostalCode = address.PostalCode,
                IsPayed = false,
                Status = order.Status,
                OrderItems = itemListVM
            };

            if (payment is not null)
                vm.IsPayed = true;

            return View(vm);
        }

        public IActionResult CancelOrder(Guid OrderId)
        {
            Order? orderToCancel = _context.Orders.Find(OrderId);

            if (orderToCancel is null)
                throw new ArgumentOutOfRangeException(nameof(OrderId));

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

            return RedirectToAction("OrdersHistory", "Account");
        }
    }
}
