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
        [ActionName("list")]
        public List<Approval> Get()
        {
            ApprovalService service = new ApprovalService();
            return service.GetApprovals();

        }
        //{"email":""}
        //GET api/approval/Title
       [ActionName("title")]
      
        public List<HomeDto> GetApprovalTitle(string email)
        {
            ApprovalService service = new ApprovalService();
            return service.GetAllApprovalTitle(email.Trim());

        }

        // GET api/approval/5
        [ActionName("list")]
        public Approval Get(int id)
        {
            ApprovalService service = new ApprovalService();
            return service.GetApprovalsById(id);
        }

 
        [HttpPost]
        [ActionName("add")]
        public int Post()
        {

            /*   ApprovalService service = new ApprovalService();

               List<Attachment> attachments = new List<Attachment>();
               attachments.Add(new Attachment("http://fromApi.com"));

               List<Recipient> recipients = new List<Recipient>();
               recipients.Add(new Recipient("REMAIL","AJ", "API", "ADMIN", Status.ACCEPTED, 2));

           var date =new DateTime(2020,04,02,02,01,00).ToString("HH:mm:ss");
               var data = service.CreateApproval(new Approval(30, "demo", "from API", "Ronak", "FROMAPIat.com", date, date,
                   date, "FROMAPI", Status.ACCEPTED, Models.Type.PARALLEL, attachments, recipients));
               return data;*/
            return 0;
          
            
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
