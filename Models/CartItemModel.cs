using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceShopApi.Models {

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
        public required Product Product {get; set;} = null!;


        [Required]
        public required Cart Cart {get; set;} = null!;
    }
}