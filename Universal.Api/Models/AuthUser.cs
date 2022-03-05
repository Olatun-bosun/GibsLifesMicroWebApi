using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.Api.Models
{
    public class AuthUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string Password { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public string Tag { get; set; }
        public string Deleted { get; set; }
    }
}
