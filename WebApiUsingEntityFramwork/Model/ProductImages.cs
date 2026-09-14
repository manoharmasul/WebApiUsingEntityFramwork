using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebApiUsingEntityFramwork.Model
{
    [Table("tblProdImages")]
    public class ProductImages
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ImageUrl { get; set; }
        //old
       // [JsonIgnore] //to avoid the problem during get api
        //instead of this we can also comment this
        // .HasForeignKey(i => i.ProductId);  //this also works insert and get
        //public ProductModel Product { get; set; }

    }

}
