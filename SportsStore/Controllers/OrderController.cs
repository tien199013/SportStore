using Microsoft.AspNetCore.Mvc;

namespace SportsStore.Controllers
{
    using Models;

    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly Cart _cartService;

        public OrderController(IOrderRepository orderRepository, Cart cartService)
        {
            _orderRepository = orderRepository;
            _cartService = cartService;
        }

        public ViewResult Checkout()
        {
            return View(new Order());
        }

        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            if (_cartService.Lines.Count == 0)
            {
                ModelState.AddModelError("", "Sorry, your cart is empty");
            }

            if (ModelState.IsValid)
            {
                order.Lines = _cartService.Lines.ToArray();
                _orderRepository.SaveOrder(order);
                _cartService.Clear();
                return RedirectToPage("/Completed", new {orderId = order.OrderId});
            }

            return View();
        }
    }
}
