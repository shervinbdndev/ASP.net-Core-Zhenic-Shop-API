using System.ComponentModel.DataAnnotations;

namespace ECommerceShopApi.Models {

    public class Cart {


        [Key]
        public int Id {get; set;}


        [Required(ErrorMessage = "User ID is Required")]
        public required string UserId {get; set;}


        public List<CartItem> Items {get; set;} = new List<CartItem>();
    }
}