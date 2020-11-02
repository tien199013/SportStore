using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models
{
    using Microsoft.EntityFrameworkCore;

    public class StoreDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public StoreDbContext(DbContextOptions<StoreDbContext> options) :base(options)
        {
        }
    }
}
