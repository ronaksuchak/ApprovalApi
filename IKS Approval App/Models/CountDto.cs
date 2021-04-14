using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IKS_Approval_App.Models
{
    public class CountDto
    {
        public int TotalSenderCount { get; set; }
        public int AcceptedSenderCount { get; set; }
        public int RejectedSenderCount { get; set; }
        public int PendingSenderCount { get; set; }

        public int TotalRecieverCount { get; set; }
        public int AcceptedRecieverCount { get; set; }
        public int RejectedRecieverCount { get; set; }
        public int PendingRecieverCount { get; set; }


    }
}