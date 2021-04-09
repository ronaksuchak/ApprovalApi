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
        public string ReleaseDate { get; set; }
        public string DueDate { get; set; }
      
        public string Comment { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public Status Status { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public Type Type { get; set; }
        public List<Attachment> Attachment { get; set; }
        public List<Recipient> Recipient { get; set; }

       

        public Approval(int approvalId, string title, string description, string senderEmail, string releaseDate, string dueDate, string comment, Status status, Type type, List<Attachment> attachment, List<Recipient> recipient)
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
        

        public Approval()
        {

        }
    }
}