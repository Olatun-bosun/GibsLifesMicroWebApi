using System;
using System.ComponentModel.DataAnnotations;

namespace Universal.Api.Contracts.V1
{
    public class CreateNewAgentRequest : PersonRequest
    {
        [Required]
        public string AgentName { get; set; }
        [Required]
        public string Password { get; set; }


        public string CityLGA { get; set; }

        public string StateID { get; set; }

        public string Nationality { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public KycTypeEnum? KycType { get; set; }

        public string KycNumber { get; set; }

        public DateTime? KycIssueDate { get; set; }

        public DateTime? KycExpiryDate { get; set; }

        public PersonRequest NextOfKin { get; set; }
    }

    public class AgentResult : CreateNewAgentRequest
    {
        public AgentResult(Models.Party party)
        {
            AgentID = party.PartyID;
            AgentName = party.PartyName;

            //split into 3
            LastName = party.PartyName;
            FirstName = party.FirstName;
            OtherName = party.OtherName;

            //Gender = party.Gender;
            Email = party.Email;
            Address = party.Address;
            //PhoneLine1 = party.PhoneNumber1;
            //PhoneLine2 = party.PhoneNumber2;

            //CityLGA = party.CityLGA;
            StateID = party.StateID;
            //Nationality = party.Nationality;
            //DateOfBirth = party.DateOfBirth;

            //KycType = party.KycIdType;
            //KycNumber = party.KycIdNumber;
            //KycIssueDate = party.KycIdIssueDate;
            //KycExpiryDate = party.KycIdExpiryDate;

            //NextOfKin = party.NextOfKin;
        }

        public string AgentID { get; set; }
    }
}
