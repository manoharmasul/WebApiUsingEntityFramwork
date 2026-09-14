using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiUsingEntityFramwork.Model
{
    [Table("tblProducts")]
    public class ProductModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string Discription { get; set; }
        public ICollection<ProductImages> Images { get; set; } = new List<ProductImages>();

      //  public virtual ICollection<ProductImages> Images { get; set; } = new List<ProductImages>(); 
        //for lazy loading
    }
    public class getrequest
    {
        public string ProductName { get; set; }
        public decimal Price { get; set; }
      
    }
    public class CreateProductRequest
    {
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string Discription { get; set; }

       // public List<string> Images { get; set; } = new List<string>();
        public List<string> Images { get; set; }
       = new List<string>();
    }
}
