using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace IKS_Approval_App.Models
{
    public class ActDto
    {
        public int Approval_id { get; set; }
        public string RecipientEmail { get; set; }
        public string RecipientComment { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Status Status { get; set; }
        //public DateTime ApprovedDateTime { get; set; }
    }
}