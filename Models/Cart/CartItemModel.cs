using ECommerceShopApi.Models.ProductModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceShopApi.Models.CartNameSpace {

    public class CartItem {


        [Key]
        public int Id {get; set;}


        [Required]
        [ForeignKey(nameof(Cart))]
        public int CartId {get; set;}


        [Required]
        [ForeignKey(nameof(Product))]
        public int ProductId {get; set;}


        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "حداقل مقدار 1")]
        public int Quantity {get; set;}


        [Required]
        public Product? Product {get; set;}


        [Required]
        public Cart? Cart {get; set;}
    }
}