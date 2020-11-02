using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SportsStore.Pages
{
    using System.Linq;
    using Infrastructure;
    using Microsoft.AspNetCore.Mvc;
    using Models;

    public class CartModel : PageModel
    {
        private readonly IStoreRepository _storeRepository;

        public CartModel(IStoreRepository storeRepository, Cart cartService)
        {
            _storeRepository = storeRepository;
            Cart = cartService;
        }

        public Cart Cart { get; set; }
        public string ReturnUrl { get; set; }
        
        public void OnGet(string returnUrl)
        {
            ReturnUrl = returnUrl ?? "/";
        }

        public IActionResult OnPost(long productId, string returnUrl)
        {
            var product = _storeRepository.Products.FirstOrDefault(p => p.ProductID == productId);
            Cart.AddItem(product, 1);
            return RedirectToPage(new {returnUrl = returnUrl});
        }

        public IActionResult OnPostRemove(long productId, string returnUrl)
        {
            Cart.RemoveLine(Cart.Lines.First(l => l.Product.ProductID == productId).Product);
            return RedirectToPage(new {returnUrl = returnUrl});
        }
    }
}
