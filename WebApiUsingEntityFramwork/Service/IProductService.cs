using WebApiUsingEntityFramwork.Model;

namespace WebApiUsingEntityFramwork.Service
{
    public interface IProductService
    {
        Task<List<ProductModel>> GetProducsService();
    }
}
