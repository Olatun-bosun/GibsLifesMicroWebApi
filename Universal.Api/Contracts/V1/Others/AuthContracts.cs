using System.ComponentModel.DataAnnotations;

namespace Universal.Api.Contracts.V1
{
    public class AppLoginRequest
    {
        [Required]
        public string AppId { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class AgentLoginRequest
    {
        [Required]
        public string AppId { get; set; }
        [Required]
        public string AgentId { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class CustomerLoginRequest
    {
        [Required]
        public string AppId { get; set; }
        [Required]
        public string CustomerId { get; set; }
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
