using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ECommerceShopApi.Models {

    public class ApplicationUser : IdentityUser {


        [Required(ErrorMessage = "نام الزامی است")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "نام باید بین 2 تا 50 کاراکتر باشد")]
        public required string FirstName {get; set;}


        [Required(ErrorMessage = "نام خانوادگی الزامی است")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "نام خانوادگی باید بین  تا 50 کاراکتر باشد")]
        public required string LastName {get; set;}
    }
}