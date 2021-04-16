using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace IKS_Approval_App.Models
{
    public class Approval
    {

        public int ApprovalId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
       // public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime DueDate { get; set; }
      
        public string Comment { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public Status Status { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public ApprovalType Type { get; set; }
        public string Attachment { get; set; }
        public List<Recipient> Recipient { get; set; }

       

        public Approval(int approvalId, string title, string description, string senderEmail, DateTime releaseDate, DateTime dueDate, string comment, Status status, ApprovalType type, string attachment, List<Recipient> recipient)
        {
            ApprovalId = approvalId;
            Title = title;
            Description = description;
           
            SenderEmail = senderEmail;
            ReleaseDate = releaseDate;
            DueDate = dueDate;
           
            Comment = comment;
            Status = status;
            Type = type;
            Attachment = attachment;
            Recipient = recipient;
        }
        

        public Approval(int id)
        {
            ApprovalId = id;
        }
        public Approval() { }

        public bool validate()
        {
            if (Title.Equals("") || Title == null)
                return false;
            if (Description.Equals("") || Description == null)
                return false;
            if (SenderEmail.Equals("") || SenderEmail == null)
                return false;
            if (ReleaseDate.Equals("") || ReleaseDate == null)
                return false;
            if (DueDate.Equals("") || DueDate == null)
                return false;
            if (Comment.Equals("") || Comment == null)
                return false;
            if (! (Status.IsDefined(typeof(Status),Status)))
                return false;
            if (! (ApprovalType.IsDefined(typeof(ApprovalType),Type)))
                return false;
            if (Attachment == null)
                return false;
            if ( Recipient == null)
                return false;
            
            return true;
        }

        public override bool Equals(object obj)
        {
            return obj is Approval approval &&
                   ApprovalId == approval.ApprovalId;
        }
    }
}