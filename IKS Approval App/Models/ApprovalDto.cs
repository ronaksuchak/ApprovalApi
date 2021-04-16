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

        /*[JsonConverter(typeof(StringEnumConverter))]
        public Status status { get; set; }*/

        [JsonConverter(typeof(StringEnumConverter))]
        public ApprovalType Type { get; set; }
        public string Attachment { get; set; }
        public List<string> RecipientEmail { get; set; }

       

     
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
            if (! (ApprovalType.IsDefined(typeof(ApprovalType),Type)))
                return false;
            if ( RecipientEmail == null)
                return false;   
            
            return true;
        }
    }
}