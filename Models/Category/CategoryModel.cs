using ECommerceShopApi.Models.ProductModel;
using System.ComponentModel.DataAnnotations;

namespace ECommerceShopApi.Models.Category {

    public class CategoryModel {

        public int Id {get; set;}

        

        [Required]
        [StringLength(50)]
        public required string Name {get; set;} = string.Empty;


        
        public required string Description {get; set;}



        public ICollection<Product> Products {get; set;} = new List<Product>();
    }
}