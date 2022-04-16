using System;
using System.ComponentModel.DataAnnotations;

namespace Universal.Api.Contracts.V1
{
    public class CreateNewCustomerRequest
    {
        [Required]
        public bool IsCorporate { get; set; } 
        [Required]
        public string Password { get; set; }
        public string Title { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string OtherName { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Address { get; set; }
        [Required, Phone]
        public string PhoneLine1 { get; set; }
        [Phone]
        public string PhoneLine2 { get; set; }
        public string LocalGovtArea { get; set; }
        public string StateOfOrigin { get; set; }
        public string Nationality { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string IdentificationType { get; set; }
        public string IdentificationNo { get; set; }
        public string Industry { get; set; }
    }

    public class CustomerResult : CreateNewCustomerRequest
    {
        public CustomerResult()
        {
        }

        public CustomerResult(Models.InsuredClient client)
        {
            IsCorporate = false;
            Password = null;
            Title = null;

            CustomerId = client.InsuredID;
            FirstName = client.FirstName;
            LastName = client.Surname;
            OtherName = client.OtherNames;
            Email = client.Email;
            Address = client.Address;
            PhoneLine2 = client.LandPhone;
            PhoneLine1 = client.MobilePhone;

            LocalGovtArea = null;
            StateOfOrigin = null;
            Nationality = null;
            //DateOfBirth = null;
            IdentificationType = null;
            IdentificationNo = null;
            Industry = client.Occupation;
        }

        public CustomerResult(Models.Policy policy)
        {
            IsCorporate = false;
            Password = null;
            Title = null;

            //CustomerId = policy.InsFaxNo; //ApiId
            CustomerId = policy.InsuredID; 
            LastName = policy.InsSurname;
            FirstName = policy.InsFirstname;
            OtherName = policy.InsOthernames;
            Address = policy.InsAddress;
            PhoneLine1 = policy.InsMobilePhone;
            PhoneLine2 = policy.InsLandPhone;
            Email = policy.InsEmail;
            Industry = policy.InsOccupation;
            StateOfOrigin = policy.InsStateID;

            LocalGovtArea = null;
            //StateOfOrigin = null;
            Nationality = null;
            //DateOfBirth = null;
            IdentificationType = null;
            IdentificationNo = null;
            //Industry = client.Occupation;
        }

        public string CustomerId { get; set; }
    }
}
