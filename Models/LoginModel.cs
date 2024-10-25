using System.ComponentModel.DataAnnotations;

namespace ECommerceShopApi.Models {

    public class LoginModel {

        public required string UserName {get; set;}
        

        [DataType(DataType.Password)]
        public required string Password {get; set;}
    }
}