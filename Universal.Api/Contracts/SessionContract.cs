using System.ComponentModel.DataAnnotations;

namespace Universal.Api.Contracts
{
    public class SessionContract
    {
        public class LoginRequest
        {
            [Required]
            public string sid { get; set; }
            [Required]
            public string token { get; set; }
        }

        public class LoginResponse
        {
            public string AccessToken { get; set; }
            public string TokenType { get; set; }
            public int ExpiresIn { get; set; }
        }
    }
}
