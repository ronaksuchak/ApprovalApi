using IKS_Approval_App.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using IKS_Approval_App.Models;

namespace IKS_Approval_App.Controllers
{
    
    public class ApprovalController : ApiController
    {
        

       public ApprovalController()
        {
            
        }

        // GET api/approval
        public IEnumerable<string> Get()
        {
            ApprovalService service = new ApprovalService();
            return service.GetAllApprovals();

        }

        // GET api/approval/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/approval
        [HttpPost]
        /*[FromBody]
        Approval value*/
        public int Post()
        {
           
                ApprovalService service = new ApprovalService();

                List<Attachment> attachments = new List<Attachment>();
                attachments.Add(new Attachment("http://fromApi.com"));

                List<Recipient> recipients = new List<Recipient>();
                recipients.Add(new Recipient("REMAIL","AJ", "API", "ADMIN", Status.ACCEPTED, 2));

            var date =new DateTime(2020,04,02,02,01,00).ToString("HH:mm:ss");
                var data = service.CreateApproval(new Approval(30, "demo", "from API", "Ronak", "FROMAPIat.com", date, date,
                    date, "FROMAPI", Status.ACCEPTED, Models.Type.PARALLEL, attachments, recipients));
                return data;

          
            
        }

        // PUT api/approval/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/approval/5
        public void Delete(int id)
        {
        }
    }
}
