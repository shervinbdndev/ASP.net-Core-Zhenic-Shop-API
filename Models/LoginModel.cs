using System.ComponentModel.DataAnnotations;

namespace ECommerceShopApi.Models {

    public class LoginModel {

        [Required(ErrorMessage = "نام کاربری الزامی است")]
        public required string UserName {get; set;}
        

        [Required(ErrorMessage = "رمز عبور الزامی است")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "رمز عبور باید حداقل 6 کاراکتر باشد")]
        public required string Password {get; set;}
    }
}