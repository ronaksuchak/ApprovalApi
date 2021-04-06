using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IKS_Approval_App.Models
{
    public class Attachment
    {
        public string AttachmentUrl { get; set; }

        public Attachment(string attachmentUrl)
        {
            AttachmentUrl = attachmentUrl;
        }
    }
}