using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace IKS_Approval_App.Models
{
    public class ApprovalDto
    {

        public string Title { get; set; }
        public string Description { get; set; }
        public string SenderEmail { get; set; }
        public DateTime DueDate { get; set; }
        public string Comment { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public Type Type { get; set; }
        public string Attachment { get; set; }
        public List<string> RecipientEmail { get; set; }

        public ApprovalDto(string title, string description, string senderEmail, DateTime dueDate, string comment, Type type, string attachment, List<string> recipientEmail)
        {
            Title = title;
            Description = description;
            SenderEmail = senderEmail;
            DueDate = dueDate;
            Comment = comment;
            Type = type;
            Attachment = attachment;
            RecipientEmail = recipientEmail;
        }

        public ApprovalDto()
        {

        }

        public bool validate()
        {
            if (Title.Equals("") || Title == null)
                return false;
            if (Description.Equals("") || Description == null)
                return false;
            if (SenderEmail.Equals("") || SenderEmail == null)
                return false;
            if (DueDate.Equals("") || DueDate == null)
                return false;
            if (Comment.Equals("") || Comment == null)
                return false;
            if (! (Type.IsDefined(typeof(Type),Type)))
                return false;
            if ( RecipientEmail == null)
                return false;
            
            return true;
        }
    }
}