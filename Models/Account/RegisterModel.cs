using System.ComponentModel.DataAnnotations;

namespace ECommerceShopApi.Models.Account {

    public class RegisterModel {

        [Required(ErrorMessage = "نام کاربری الزامی است")]
        public required string UserName {get; set;}
        

        [Required(ErrorMessage = "نام الزامی است")]
        public required string FirstName {get; set;}


        [Required(ErrorMessage = "نام خانوادگی الزامی است")]
        public required string LastName {get; set;}


        [Required(ErrorMessage = "ایمیل الزامی است")]
        [EmailAddress]
        public required string Email {get; set;}


        [Required(ErrorMessage = "رمز عبور الزامی است")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "رمز عبور باید حداقل 6 کاراکتر باشد")]
        public required string Password {get; set;}
    }
}