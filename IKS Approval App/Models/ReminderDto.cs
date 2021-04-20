using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IKS_Approval_App.Models
{
    public class ReminderDto
    {
        public int ApprovalId { get; set; }
        public string Title { get; set; }

        public string Email { get; set; }

    }
}