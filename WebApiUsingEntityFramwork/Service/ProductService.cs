using Microsoft.Extensions.Caching.Memory;
using WebApiUsingEntityFramwork.Model;
using WebApiUsingEntityFramwork.Repository.Interface;

namespace WebApiUsingEntityFramwork.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductAsyncRepository _productRepository;
        private readonly IMemoryCache _memoryCache;

        public ProductService(
            IProductAsyncRepository productRepository,
            IMemoryCache memoryCache)
        {
            _productRepository = productRepository;
            _memoryCache = memoryCache;
        }
        public async Task<List<ProductModel>> GetProducsService()
        {
            // 1. Create cache key
            string cacheKey = "all_products";

            // 2. Check whether products are already in cache
            if (_memoryCache.TryGetValue(
                    cacheKey,
                    out List<ProductModel>? products))
            {
                Console.WriteLine("Products retrieved from CACHE");

                return products!;
            }

            // 3. Cache miss
            Console.WriteLine("Products retrieved from DATABASE");

            // 4. Get products from repository
            products = await _productRepository.GetAllProducts();

            // 5. Store products in cache
            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow =
                    TimeSpan.FromMinutes(5)
            };

            _memoryCache.Set(
                cacheKey,
                products,
                cacheOptions
            );

            // 6. Return products
            return products;
        }
    }
}
