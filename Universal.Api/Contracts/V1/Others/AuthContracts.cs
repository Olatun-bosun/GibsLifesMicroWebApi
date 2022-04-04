using System.ComponentModel.DataAnnotations;

namespace Universal.Api.Contracts.V1
{
    public class LoginRequest
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class LoginResult
    {
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
        public int ExpiresIn { get; set; }
    }
}
