using Microsoft.EntityFrameworkCore;
using WebApiUsingEntityFramwork.Context;
using WebApiUsingEntityFramwork.Repository;
using WebApiUsingEntityFramwork.Repository.Interface;
using WebApiUsingEntityFramwork.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddDbContext<ApplicationDBContext>(options =>
//{
//    options.UseSqlServer(
//        builder.Configuration.GetConnectionString("SqlConnection")
//    );
//});
builder.Services.AddDbContext<ApplicationDBContext>(options =>
{
   // options.UseLazyLoadingProxies(); //for Lazy Loading

    options.UseSqlServer(
        builder.Configuration.GetConnectionString("SqlConnection")
    );
});
//builder.Services.AddDbContext<ApplicationDBContext>(options =>
//{
//    options.UseLazyLoadingProxies();

//    options.UseSqlServer(
//        builder.Configuration.GetConnectionString("SqlConnection")
//    );
//});

// Register Memory Cache
builder.Services.AddMemoryCache();

builder.Services.AddControllers();
//builder.Services.AddSingleton<IProductAsyncRepository,IProductAsyncRepository>();
builder.Services.AddScoped<IProductAsyncRepository, ProductAsyncRepository>();
builder.Services.AddScoped<IProductService,ProductService>();
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
