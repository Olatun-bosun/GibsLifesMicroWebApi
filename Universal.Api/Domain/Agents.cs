using System;
using System.ComponentModel.DataAnnotations;

namespace GibsLifesMicroWebApi.Domain
{
    public class Agents
    {
        [Key]
        public int INT  {get; set;}
        public string AgentID  {get; set;}
        public string UnitID  {get; set;}
        public string Agent  {get; set;}
        public string Address  {get; set;}
        public string Area  {get; set;}
        public string City  {get; set;}
        public string State  {get; set;}
        public string Phone1  {get; set;}
        public string Phone2  {get; set;}
        public string FaxNo  {get; set;}
        public string Email  {get; set;}
        public string InsPerson  {get; set;}
        public string FinPerson  {get; set;}
        public decimal Balance { get; set; }
        public decimal CreditLimit { get; set; }
        public double ComRate { get; set; }
        public string Remark { get; set; }
        public string AccountNo { get; set; }
        public string Bankname { get; set; }
        public string Tag { get; set; }
        public byte Deleted { get; set; }
        public string SubmittedBy { get; set; }
        public DateTime SubmittedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
