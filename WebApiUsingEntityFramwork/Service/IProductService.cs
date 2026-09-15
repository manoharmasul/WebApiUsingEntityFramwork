using WebApiUsingEntityFramwork.Model;

namespace WebApiUsingEntityFramwork.Service
{
    public interface IProductService
    {
        Task<List<ProductModel>> GetProducsService();
        Task<List<ProductModel>> GetProducsServiceDistributedCaching();
        void Remove(string cacheKey);
        Task RemoveDistributedCaching(string key);
    }
}
