using System.ComponentModel.DataAnnotations;

namespace Universal.Api.Contracts
{
    public class LoginRequest
    {
        [Required]
        public string id { get; set; }
        [Required]
        public string password { get; set; }
    }

    public class LoginResponse
    {
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
        public int ExpiresIn { get; set; }
    }
}
