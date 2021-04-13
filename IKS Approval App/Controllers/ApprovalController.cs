﻿using IKS_Approval_App.Services;
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
       [ActionName("recived")]
      
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

        }
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

        [HttpGet]
        [ActionName("count")]
        public CountDto getCount(string email)
        {
            ApprovalService service = new ApprovalService();
            return service.getHomeCount(email.Trim());
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

        // PUT api/approval/5
        /* public void Put(int id, [FromBody]string value)
         {
         }

         // DELETE api/approval/5
         public void Delete(int id)
         {
         }*/
    }
}
