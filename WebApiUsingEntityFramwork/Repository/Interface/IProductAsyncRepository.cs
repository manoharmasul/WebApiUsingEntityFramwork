using WebApiUsingEntityFramwork.Model;

namespace WebApiUsingEntityFramwork.Repository.Interface
{
    public interface IProductAsyncRepository
    {
        Task<int> AddNewProduct(ProductModel product);
        Task<ProductModel> GetProductById(int id);
        Task<ProductModel> GetProductByIdLazyLoading(int id);
        Task<ProductModel> GetProductByIdExplicitLoading(int id);
        Task<List<ProductModel>> GetAllProducts();

    }
}
