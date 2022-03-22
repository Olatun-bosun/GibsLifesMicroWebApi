using System;
using System.ComponentModel.DataAnnotations;

namespace Universal.Api.Contracts.V1
{
    public class CustomerDto
    {
        public CustomerDto()
        {
        }

        public CustomerDto(Models.InsuredClient client)
        {
            //todo
            var splittedArray = client.ApiId.Split('/');


            CustomerId = splittedArray[1];
            //CustomerType=customer.
            //Title = Custom
            FirstName = client.FirstName;
            LastName = client.Surname;
            OtherName = client.OtherNames;
            Address = client.Address;
            PhoneLine2 = client.LandPhone;
            PhoneLine1 = client.MobilePhone;
            Email = client.Email;
            //IdentificationNo = customer.
            //IdentificationType

            Industry = client.Occupation;

        }

        public CustomerDto(Models.Policy policy)
        {
            //todo
            var splittedArray = policy.InsFaxNo.Split('/');


            CustomerId = splittedArray[1];
            LastName = policy.InsSurname;
            FirstName = policy.InsFirstname;
            OtherName = policy.InsOthernames;
            Address = policy.InsAddress;
            PhoneLine1 = policy.InsMobilePhone;
            PhoneLine2 = policy.InsLandPhone;
            Email = policy.InsEmail;
            Industry = policy.InsOccupation;
            StateOfOrigin = policy.InsStateID;
        }

        public string CustomerId { get; set; }
        [Required]
        public string CustomerType { get; set; } //TODO: make enum list
        public string Title { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string OtherName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string PhoneLine1 { get; set; }
        public string PhoneLine2 { get; set; }
        [Required]
        public string Email { get; set; }
        public string LocalGovtArea { get; set; }
        public string StateOfOrigin { get; set; }
        public string Nationality { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string IdentificationType { get; set; }
        public string IdentificationNo { get; set; }
        public string Industry { get; set; }
    }
}
