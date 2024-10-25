using System.ComponentModel.DataAnnotations;

namespace ECommerceShopApi.Models {

    public class RegisterModel {

        public required string UserName {get; set;}
        

        public required string FirstName {get; set;}


        public required string LastName {get; set;}


        [EmailAddress]
        public required string Email {get; set;}


        [DataType(DataType.Password)]
        public required string Password {get; set;}
    }
}