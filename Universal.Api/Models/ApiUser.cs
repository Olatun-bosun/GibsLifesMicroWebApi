using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.Api.Data
{
    [Table("Webservice_ID")]
    public class ApiUser
    {
        [Key]
        public int CompanyID { get; set; }

        public string CompanyName { get; set; }

        public string Password { get; set; }

        public string Status { get; set; }

        public string Remarks { get; set; }

        public string Tag { get; set; }

        public string Deleted { get; set; }

    }
}
