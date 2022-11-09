using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// db context service must be registered with the DI
builder.Services.AddDbContext<ProductDbContext>(
    option => option.UseInMemoryDatabase("ProductList")
);

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();