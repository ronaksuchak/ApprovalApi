using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IKS_Approval_App.Models
{
    public class CountDto
    {
        public int RecivedCount { get; set; }
        public int AcceptedCount { get; set; }
        public int RejectedCount { get; set; }
        public int PendingCount { get; set; }

    }
}