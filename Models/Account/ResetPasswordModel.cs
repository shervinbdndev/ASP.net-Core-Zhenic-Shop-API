using System.ComponentModel.DataAnnotations;

namespace ECommerceShopApi.Models.Account {

    public class ResetPasswordModel {

        [Required]
        [EmailAddress(ErrorMessage = "فرمت ایمیل نادرست است")]
        public required string Email {get; set;}


        [Required]
        public required string token {get; set;}


        [Required]
        [MinLength(8, ErrorMessage = "حداقل طول رمز ورود تا 8 کاراکتر")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "رمز ورود شما باید حداقل یک حرف بزرگ، یک عدد و یک حرف خاص داشته باشد")]
        public required string newPassword {get; set;}
    }
}