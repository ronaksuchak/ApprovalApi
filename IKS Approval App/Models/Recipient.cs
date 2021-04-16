using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


namespace IKS_Approval_App.Models
{
    public class Recipient
    {
        public string Email { get; set; }
       

        public string Comment { get; set; }
        //public string Role { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public Status status { get; set; }
        public int SequenceNumber { get; set; }

        public Recipient(string email, string comment,  Status status, int sequenceNumber)
        {
            Email = email;
           
            Comment = comment;
           
            this.status = status;

            SequenceNumber = sequenceNumber;
        }

        public Recipient(string email, Status status, int sequenceNumber)
        {
            Email = email;
            this.status = status;
            SequenceNumber = sequenceNumber;
        }
    }
}