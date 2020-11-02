using Microsoft.AspNetCore.Mvc;

namespace SportsStore.Controllers
{
    using Models;

    public class OrderController : Controller
    {
        public ViewResult Checkout()
        {
            return View(new Order());
        }
    }
}
