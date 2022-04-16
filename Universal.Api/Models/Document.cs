using System;

namespace Universal.Api.Models
{
    public class Document
    {
        public string DocumentId { get; set; }
        public string PolicyNo { get; set; }
        public string DocumentName { get; set; }
        public byte[] Content { get; set; }
        public string ContentType { get; set; }

        public DateTime SubmitDate { get; set; }

    }
}
