using Microsoft.EntityFrameworkCore;
using WebApiUsingEntityFramwork.Model;

namespace WebApiUsingEntityFramwork.Context
{
    public class ApplicationDBContext:DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }
        public DbSet<ProductModel> ProductMode { get; set; }
        public DbSet<ProductImages> ProductImages { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //old 

            //modelBuilder.Entity<ProductModel>()
            //.HasMany<ProductImages>()
            //.WithOne<ProductModel>()
            //.HasForeignKey<ProductImages>(p => p.ProductId);

            //more relatable
            modelBuilder.Entity<ProductModel>()
           .HasMany(p => p.Images)
           .WithOne()
           .HasForeignKey(i => i.ProductId);

        }
    }
}

