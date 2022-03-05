﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.Api.Models
{
    public class Party
    {
        public string PartyID { get; set; }

        public string StateID { get; set; }

        [Column("Party")]
        public string Party1 { get; set; }

        public string Description { get; set; }

        public string PartyType { get; set; }

        public string Address { get; set; }

        public string mobilePhone { get; set; }

        public string LandPhone { get; set; }

        public string Email { get; set; }

        public string Fax { get; set; }

        public string InsContact { get; set; }

        public string FinContact { get; set; }

        public Decimal? CreditLimit { get; set; }

        public Decimal? ComRate { get; set; }

        public string Remarks { get; set; }

        public byte? Deleted { get; set; }

        public byte? Active { get; set; }

        public string SubmittedBy { get; set; }

        public DateTime? SubmittedOn { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
