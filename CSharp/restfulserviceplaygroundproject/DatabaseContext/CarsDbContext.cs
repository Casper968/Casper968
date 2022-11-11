using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using restfulserviceplaygroundproject.Model;

namespace restfulserviceplaygroundproject.DatabaseContext
{
    public class CarsDbContext : DbContext
    {
        protected readonly IConfiguration _configuration;

        public CarsDbContext(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite(this._configuration.GetConnectionString("WebApiDatabase"));
        }
        
        public DbSet<CarBrand>? WorldCarBrand { get; set; }
    }
}