using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IKS_Approval_App.Models
{
    public class Recipient
    {
        public string Email { get; set; }
        public string Name { get; set; }

        public string Comment { get; set; }
        public string Role { get; set; }
        public Status status { get; set; }
        public int SequenceNumber { get; set; }

        public Recipient(string email, string name, string comment, string role, Status status, int sequenceNumber)
        {
            Email = email;
            Name = name;
            Comment = comment;
            Role = role;
            this.status = status;
            SequenceNumber = sequenceNumber;
        }
    }
}