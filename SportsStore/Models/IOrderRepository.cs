using System.Linq;

namespace SportsStore.Models
{
    using Microsoft.EntityFrameworkCore;

    public interface IOrderRepository
    {
        IQueryable<Order> Orders { get; }
        void SaveOrder(Order order);
    }

    class EFOrderRepository : IOrderRepository
    {
        private readonly StoreDbContext _storeDbContext;

        public EFOrderRepository(StoreDbContext storeDbContext)
        {
            _storeDbContext = storeDbContext;
        }

        public IQueryable<Order> Orders => _storeDbContext
            .Orders
            .Include(o => o.Lines)
            .ThenInclude(l => l.Product);
        
        public void SaveOrder(Order order)
        {
            _storeDbContext.AttachRange(order.Lines.Select(l => l.Product));
            if (order.OrderId == 0)
            {
                _storeDbContext.Orders.Add(order);
            }

            _storeDbContext.SaveChanges();
        }
    }
}
