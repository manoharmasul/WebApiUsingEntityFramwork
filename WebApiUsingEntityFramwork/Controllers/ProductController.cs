using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;
using WebApiUsingEntityFramwork.Model;
using WebApiUsingEntityFramwork.Repository.Interface;
using WebApiUsingEntityFramwork.Service;

namespace WebApiUsingEntityFramwork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductAsyncRepository productAsync;
        private readonly IProductService _productService;
        public ProductController(
      IProductAsyncRepository productAsync,
         IProductService productService)
        {
            this.productAsync = productAsync;
            this._productService = productService;
        }


        [HttpPost]
        public async  Task<IActionResult> AddNewProduct(CreateProductRequest model)
        {
            var prod = new ProductModel()
            {
                ProductName = model.ProductName,
                Price = model.Price,
                Discription = model.Discription,
                
            };
            foreach (var image in model.Images)
            {
                prod.Images.Add(new ProductImages
                {
                    ImageUrl = image
                });
            }
            //foreach (var image in model.Images)
            //{
            //    prod.Images.Add(new ProductImages
            //    {
            //        ImageUrl = image
            //    });
            //}

            int result=await productAsync.AddNewProduct(prod);
            _productService.Remove("all_products");
            _productService.RemoveDistributedCaching("all_productsditributed");
        
            return Ok(result);


        }
        [HttpGet]
        public async Task<IActionResult> GetProductByIdEgerLoading(int Id)
        {
            var result  = await productAsync.GetProductById(Id);
            return Ok(result);
        }

        [HttpGet("LazyLoading")]
        public async Task<IActionResult> GetProductByIdLazyLoading(int Id)
        {
            var result = await productAsync.GetProductByIdLazyLoading(Id);
            if (result == null)
                return NotFound();
            //getrequest ret = new getrequest()
            //{
            //    ProductName=result.ProductName,
            //    Price=result.Price, 
            //};
            //return Ok(ret
            //);
            //object obj = result;
            return Ok(result);
        }
        [HttpGet("GetProductByIdExplicitLoading")]
        public async Task<IActionResult> GetProductByIdExplicitLoading(int Id)
        {
            var result = await productAsync.GetProductByIdExplicitLoading(Id);
            return Ok(result);
        }

        [HttpGet("GetAllProducts")]   
        public async Task<IActionResult> GetAllProducts() 
        {
            //var result=await productAsync.GetAllProducts();
            //   return Ok(result);

            var products = await _productService.GetProducsService();

            return Ok(products);
        }
        [HttpGet("GetAllProductsByDistributedCaching")]
        public async Task<IActionResult> GetAllProductsDistributedCatch()
        {
            //var result=await productAsync.GetAllProducts();
            //   return Ok(result);

            var products = await _productService.GetProducsServiceDistributedCaching();

            return Ok(products);
        }
        [HttpGet("GetAllProductsByResponseCaching")]
        [ResponseCache(
    Duration = 60,
    Location = ResponseCacheLocation.Any)]
        //[ResponseCache(Duration = 60)]  //means the response can be cached for 60 seconds.
        public async Task<IActionResult> GetAllProductsByResponseCaching()
        {
            //var result=await productAsync.GetAllProducts();
            //   return Ok(result);

            var products = await productAsync.GetAllProducts();

            return Ok(products);
        }
        //[HttpGet("GetAllProductsByOutPutCaching")]   dot net version 7 or greater
        //[OutputCache(Duration = 60)]
        ////[ResponseCache(Duration = 60)]  //means the response can be cached for 60 seconds.
        //public async Task<IActionResult> GetAllProductsByOutPutCaching()
        //{
        //    //var result=await productAsync.GetAllProducts();
        //    //   return Ok(result);

        //    var products = await productAsync.GetAllProducts();

        //    return Ok(products);
        //}
    }
}
