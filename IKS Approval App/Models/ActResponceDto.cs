using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace IKS_Approval_App.Models
{
    public class ActResponceDto
    {
        public int ApprovalId { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Status FinalStatus { get; set; }
        public List<Recipient> Recipient { get; set; }
        public String ApprovedTime { get; set; }

    }
}