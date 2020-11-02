using System.Linq;

namespace SportsStore.Models
{
    class EFStoreRepository : IStoreRepository
    {
        private readonly StoreDbContext _context;

        public EFStoreRepository(StoreDbContext context)
        {
            _context = context;
        }

        public IQueryable<Product> Products => _context.Products;
    }
}