using Microsoft.AspNetCore.Mvc;

namespace SportsStore.Controllers
{
    using System.Linq;
    using Models;
    using Models.ViewModels;

    public class HomeController : Controller
    {
        private readonly IStoreRepository _storeRepository;
        public int PageSize = 4;

        public HomeController(IStoreRepository storeRepository)
        {
            _storeRepository = storeRepository;
        }

        public IActionResult Index(string category, int productPage = 1)
        {
            var viewModel = new ProductsListViewModel()
            {
                Products = _storeRepository.Products
                    .Where(p => category == null || p.Category == category)
                    .OrderBy(p => p.ProductID)
                    .Skip((productPage - 1) * PageSize)
                    .Take(PageSize),

                PagingInfo = new PagingInfo()
                {
                    CurrentPage = productPage,
                    ItemsPerPage = PageSize,
                    TotalItems = category == null
                        ? _storeRepository.Products.Count()
                        : _storeRepository.Products.Count(p => p.Category == category)
                },
                
                CurrentCategory = category
            };
            return View(viewModel);
        }
    }
}
