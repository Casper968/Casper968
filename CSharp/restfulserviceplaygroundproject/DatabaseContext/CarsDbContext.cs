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

        public DbSet<CarModel>? WorldCarModel { get; set; }

        public async Task<List<CarBrand>> GetCarBrands(string? name)
        {
            var brandList = await this.WorldCarBrand.ToListAsync();
            if (brandList != null && brandList.Count > 0)
            {
                if (name != null)
                {
                    brandList = brandList.Where(x => x.Name.IndexOf(name) > -1).ToList();
                }
            }

            return brandList;
        }
        
        public async Task<List<CarModel>> GetCarModels(string? brandName, string? name)
        {
            var modelList = await this.WorldCarModel.ToListAsync();
            if (modelList != null && modelList.Count > 0)
            {
                int brandId = 0;
                var targetBrand = this.WorldCarBrand.FirstOrDefault(x => x.Name == brandName);
                if (targetBrand != null)
                {
                    brandId = targetBrand.ID;
                }

                if (brandId > 0)
                {
                    modelList = modelList.Where(x => x.BrandId == brandId).ToList();
                }
                
                if (name != null)
                {
                    modelList = modelList.Where(x => x.Name.IndexOf(name) > -1).ToList();
                }
            }

            return modelList;
        }
    }
}