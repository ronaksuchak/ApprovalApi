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
       /* [ActionName("list")]
        public List<Approval> Get()
        {
            ApprovalService service = new ApprovalService();
            return service.GetApprovals();

        }*/
        //{"email":""}
        //GET api/approval/recived
       /*[ActionName("recived")]
      
        public List<HomeDto> GetRecivedApprovalTitle(string email)
        {
            ApprovalService service = new ApprovalService();
            return service.GetAllApprovalTitleRecived(email.Trim());

        }

        [ActionName("sent")]
        public List<HomeDto> GetSentApprovalTitle(string email)
        {
            ApprovalService service = new ApprovalService();
            return service.GetAllApprovalTitleSent(email.Trim());

        }*/
        [HttpPost]
        [ActionName("act")]
        public IHttpActionResult UpdateStatus(ActDto dto)
        {
            ApprovalService service = new ApprovalService();
            var change = service.StatusUpdate(dto);
            if (change > 0)
            {
                return Ok();//200
            }
            else
            {
                return BadRequest(); //400
            }

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
        public HttpResponseMessage Post([FromBody] ApprovalDto approval)
        {
            if (!approval.validate())
                return this.Request.CreateResponse(HttpStatusCode.BadRequest);
            else
            {
                ApprovalService service = new ApprovalService();
                Approval approvalNew = service.CreateApproval(approval);
                return this.Request.CreateResponse<Approval>(HttpStatusCode.Created ,approvalNew);
            }
           
        }


        [HttpGet]
        [ActionName("senderCount")]
        public CountDto getSenderCount(string email)
        {
            ApprovalService service = new ApprovalService();
            return service.GetSenderCount(email.Trim());
        }

        [HttpGet]
        [ActionName("recieverCount")]
        public CountDto GetRecieverCount(string email)
        {
            ApprovalService service = new ApprovalService();
            return service.GetRecieverCount(email.Trim());
        }



        [ActionName("totalRecived")]

        public List<HomeDto> GetRecivedApprovalTitle(string email)
        {
            ApprovalService service = new ApprovalService();
            return service.GetTotalCountRecived(email.Trim());
        }

        [ActionName("totalSent")]
        public List<HomeDto> GetSentApprovalTitle(string email)
        {
            ApprovalService service = new ApprovalService();
            return service.GetTotalCountSent(email.Trim());

        }

        [ActionName("sentStatus")]
        public List<HomeDto> GetSenderStatus(string email, string status)
        {
            ApprovalService service = new ApprovalService();
            return service.GetSentCountStatus(email.Trim(), status.Trim());
        }

        [ActionName("recievedStatus")]
        public List<HomeDto> GetRecieverStatus(string email, string status)
        {
            ApprovalService service = new ApprovalService();
            return service.GetCountReceivedStatus(email.Trim(), status.Trim());
        }

    }
}
