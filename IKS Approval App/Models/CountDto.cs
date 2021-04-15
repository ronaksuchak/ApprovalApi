using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IKS_Approval_App.Models
{
    public class CountDto
    {
        public int Total { get; set; }
        public int Accepted { get; set; }
        public int Rejected { get; set; }
        public int Pending { get; set; }

    }
}