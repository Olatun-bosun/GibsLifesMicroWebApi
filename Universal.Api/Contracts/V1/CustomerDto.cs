using System;
using System.ComponentModel.DataAnnotations;

namespace Universal.Api.Contracts.V1
{
    public class CustomerDto
    {
        public CustomerDto()
        {
        }

        public CustomerDto(Models.InsuredClient customer)
        {
            //todo
            var splittedArray = customer.ApiId.Split('/');


            CustomerId = splittedArray[1];
            AgentId = splittedArray[0];
            FirstName = customer.FirstName;
            LastName = customer.Surname;
            OtherName = customer.OtherNames;
            Address = customer.Address;
            Telephone = customer.LandPhone;
            MobilePhone = customer.MobilePhone;
            Email = customer.Email;
            Industry = customer.Occupation;
            Status = customer.ApiStatus;

        }

        public string CustomerId { get; set; }
        [Required]
        public string AgentId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string OtherName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string MobilePhone { get; set; }
        public string Telephone { get; set; }
        [Required]
        public string Industry { get; set; }
        [Required]
        public string Email { get; set; }
        public string Status { get; set; }
        public string StateOfOrigin { get; set; }
    }

    //public class InsuredDto : CustomerDto
    //{
    //    public string InsuredType { get; set; }
    //    public string Title { get; set; }
    //    public string InsuredCode { get; set; }
    //    public string AddressProof { get; set; }
    //    public DateTime DateOfBirth { get; set; }
    //    public string Identification { get; set; }
    //    public string Nationality { get; set; }
    //    public string LocalGovtArea { get; set; }
    //    public string RiskProfiling { get; set; }
    //    public string IdentificationNo { get; set; }
    //}
}
