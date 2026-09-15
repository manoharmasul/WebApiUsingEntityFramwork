using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;
using WebApiUsingEntityFramwork.Model;
using WebApiUsingEntityFramwork.Repository.Interface;

namespace WebApiUsingEntityFramwork.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductAsyncRepository _productRepository;
        private readonly IMemoryCache _memoryCache;
        private readonly IDistributedCache _cache;
        public ProductService(
            IProductAsyncRepository productRepository,
            IMemoryCache memoryCache, IDistributedCache distributedCache)
        {
            _productRepository = productRepository;
            _memoryCache = memoryCache;
            _cache = distributedCache;
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
                    TimeSpan.FromMinutes(1)
            };

            _memoryCache.Set(
                cacheKey,
                products,
                cacheOptions
            );

            // 6. Return products
            return products;
        }

        public async Task<List<ProductModel>> GetProducsServiceDistributedCaching()
        {
            //Task<List<ProductModel>> GetProducsServiceDistributedCaching();

            string cacheKey = "all_productsditributed";

            // 1. Check Redis cache
            var cachedData = await _cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedData))
            {
                Console.WriteLine("Data coming from Redis cache");

                var cachedProducts =
                    JsonSerializer.Deserialize<List<ProductModel>>(cachedData);

                return cachedProducts ?? new List<ProductModel>();
            }

            // 2. If not found in cache, get data from database
            Console.WriteLine("Data coming from Database");

            var products = await _productRepository.GetAllProducts();

            // 3. Serialize data
            var jsonData = JsonSerializer.Serialize(products);

            // 4. Store data in Redis
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow =
                    //TimeSpan.FromMinutes(10),
                    TimeSpan.FromMinutes(1),
                //The cache entry will expire 10 minutes after it was created,
                // no matter how many times you access it.

                SlidingExpiration = 
               // TimeSpan.FromMinutes(5)  //The cache entry expires if it is not accessed for 5 minutes.
                TimeSpan.FromMinutes(1)  
            };

            await _cache.SetStringAsync(
                cacheKey,
                jsonData,
                cacheOptions
            );

            return products;
        }

        public void Remove(string cacheKey)
        {
            _memoryCache.Remove(cacheKey);

            Console.WriteLine($"Cache removed: {cacheKey}");
        }
        public async Task RemoveDistributedCaching(string key)
        {
            await _cache.RemoveAsync(key);
        }
    }
}
