using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace CSharp.Database.AddDatabaseContext
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base (options)
        {

        }

        public DbSet<ProductItem> ProductList {get; set; } = null;
    }
}