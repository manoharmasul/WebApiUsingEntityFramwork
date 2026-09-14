using Microsoft.EntityFrameworkCore;
using WebApiUsingEntityFramwork.Context;
using WebApiUsingEntityFramwork.Model;
using WebApiUsingEntityFramwork.Repository.Interface;

namespace WebApiUsingEntityFramwork.Repository
{
    public class ProductAsyncRepository:IProductAsyncRepository
    {
        private readonly ApplicationDBContext context;
        public ProductAsyncRepository(ApplicationDBContext context)
        {
            this.context = context; 
        }
        public async Task<int> AddNewProduct(ProductModel product)
        {

             await context.AddAsync(product); 
            return  context.SaveChanges();
           // throw new NotImplementedException();
        }

        public async Task<List<ProductModel>> GetAllProducts()
        {
           var result=await context.ProductMode.Include(p=>p.Images)
                .ToListAsync();
            return result;
        }

        public async Task<ProductModel> GetProductById(int id)
        {
           var result=await context.ProductMode.Include(p=>p.Images)
                .FirstOrDefaultAsync(p=>p.Id==id);
            return result;
        }

        public async Task<ProductModel> GetProductByIdExplicitLoading(int id)
        {
             var result=await context.ProductMode.FirstOrDefaultAsync(p=>p.Id==id);
            await context.Entry(result).Collection(p => p.Images).LoadAsync();
            return result;
        }

        public async Task<ProductModel> GetProductByIdLazyLoading(int id)
        {
            var result = await context.ProductMode
            .FirstOrDefaultAsync(p => p.Id == id);

            var isLoadedBefore = context.Entry(result)
                .Collection(p => p.Images)
                .IsLoaded;

            Console.WriteLine($"Before accessing Images: {isLoadedBefore}");

           // This accesses Images
           var images = result.Images;

            var isLoadedAfter = context.Entry(result)
                .Collection(p => p.Images)
                .IsLoaded;

            Console.WriteLine($"After accessing Images: {isLoadedAfter}");

            return result;
        }
    }
}
