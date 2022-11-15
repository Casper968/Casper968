using restfulserviceplaygroundproject.DatabaseContext;
using restfulserviceplaygroundproject.Filter;
using restfulserviceplaygroundproject.Helpers;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddMvc(options => {
    options.Filters.Add<ExceptionFilter>();
});

builder.Services.AddDbContext<DataContext>();
builder.Services.AddScoped<ProductDbContext>();
builder.Services.AddScoped<HomeDbContext>();
builder.Services.AddScoped<CarsDbContext>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
