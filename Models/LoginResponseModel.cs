namespace ECommerceShopApi.Models {
    
    public class LoginResponse{
        public bool? Success {get; set;}
        public string? Message {get; set;}
        public string? Token { get; set; }
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
