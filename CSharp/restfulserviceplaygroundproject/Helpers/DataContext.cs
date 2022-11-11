using Microsoft.EntityFrameworkCore;

namespace restfulserviceplaygroundproject.Helpers;

public class DataContext : DbContext
{
    protected readonly IConfiguration _configuration;

    public DataContext(IConfiguration configuration)
    {
        this._configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite(this._configuration.GetConnectionString("WebApiDatabase"));
    }
}