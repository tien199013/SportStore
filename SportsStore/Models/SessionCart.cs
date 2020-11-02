namespace SportsStore.Models
{
    using System;
    using System.Text.Json.Serialization;
    using Infrastructure;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;

    public class SessionCart : Cart
    {
        public static Cart GetCart(IServiceProvider serviceProvider)
        {
            var session = serviceProvider.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext.Session;

            var sessionCart = session?.GetJson<SessionCart>("Cart")
                              ?? new SessionCart();

            sessionCart.Session = session;
            return sessionCart;
        }

        [JsonIgnore] 
        public ISession Session { get; set; }

        public override void AddItem(Product product, int quantity)
        {
            base.AddItem(product, quantity);
            Session.SetJson("Cart", this);
        }

        public override void RemoveLine(Product product)
        {
            base.RemoveLine(product);
            Session.SetJson("Cart", this);
        }

        public override void Clear()
        {
            base.Clear();
            Session.Remove("Cart");
        }
    }
}