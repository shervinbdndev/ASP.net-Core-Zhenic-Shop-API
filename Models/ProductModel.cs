using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace ECommerceShopApi.Models {

    public class Product {

        [Key]
        public int Id {get; set;}


        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "نام محصول باید بین 3 تا 50 کاراکتر باشد")]
        public required string Name {get; set;}


        [Required]
        [StringLength(100, ErrorMessage = "توضیحات حداکثر تا 100 کاراکتر")]
        public required string Description {get; set;}


        [Required]
        [Range(0.01, 10000.00, ErrorMessage = "قیمت باید بین 0.01 تا 10000.00 باشد")]
        public decimal Price {get; set;}


        [Required]
        [Url(ErrorMessage = "آدرس عکس باید معتبر باشد")]
        public required string ImageUrl {get; set;}


        [Range(0, int.MaxValue, ErrorMessage = "تعداد موجود باید غیر منفی باشد")]
        public int Stock {get; set;}


        [JsonIgnore]
        public DateTime CreatedAt {get; set;}
    }
}