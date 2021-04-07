using DataLayerLibrary;
using System;
using System.Text;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using IKS_Approval_App.Models;

namespace IKS_Approval_App.Services
{
    public class ApprovalService : BaseService
    {

        public ApprovalService()
        {


        }

        public List<HomeDto> GetAllApprovalTitle()
        {
            
            List<HomeDto> title = new List<HomeDto>();
            var data = GetApprovals();
            data.ForEach((a) =>
            {
                
                HomeDto dto = new HomeDto();
                dto.ApprovalId = a.ApprovalId;
                dto.Title = a.Title;
                title.Add(dto);

            });
            return title;



        }


        public List<Approval> GetApprovals()
        {
            List<Approval> list = new List<Approval>();
            List<Recipient> recipients = new List<Recipient>();
            var dataTable = ApprovalDB.ExecuteDataTable("CALL GetAll();");
            if (dataTable != null && dataTable.Rows.Count > 0)
            {


                foreach (DataRow dr in dataTable.Rows)
                {
                    Approval approval = new Approval();
                    approval.ApprovalId = (int)dr["approval_id"];
                    approval.Title = (string)dr["approval_name"];
                    approval.SenderName = (string)dr["sender_name"];
                    approval.SenderEmail = (string)dr["sender_email"];
                    approval.ReleaseDate = dr["release_date"].ToString();
                    approval.DueDate = dr["due_date"].ToString();
                    //approval.Status = (string)dr["status"];
                    approval.Status = (Status)Enum.Parse(typeof(Status), dr["approval_status"].ToString(), true);

                    approval.Comment = (string)dr["comments"];
                    Recipient r = new Recipient(dr["recipient_email"].ToString(), dr["recipient_name"].ToString(), dr["comments"].ToString(),
                        (Status)Enum.Parse(typeof(Status), dr["approval_status"].ToString(), true), Int32.Parse(dr["sequence_number"].ToString()));

                    recipients.Add(r);
                    approval.Recipient = recipients;
                    List<Attachment> attachments = new List<Attachment>();
                    attachments.Add(new Attachment(dr["attachment_url"].ToString()));
                    approval.Attachment = attachments;
                    list.Add(approval);

                }
            }
            return list;
        }

        public int CreateApproval(Approval approval)
        {
            //var data =  ApprovalDB.ExecuteNonQuery("CALL create_approval(" + approval.SenderName + "," + approval.SenderEmail + "," + approval.Comment + "," + approval.ReleaseDate + ","+approval.DueDate+","+approval.Title+","+approval.Type+","+approval.Recipient.First+");");
            StringBuilder stringBuilder = new StringBuilder("CALL create_approval(");
            stringBuilder.Append("'"+approval.SenderName+"'")
                         .Append(",")
                         .Append("'"+approval.SenderEmail+ "'")
                         .Append(",")
                         .Append("'"+approval.Comment+ "'")
                         .Append(",")
                         .Append("'"+approval.ReleaseDate+ "'")
                         .Append(",")
                         .Append("'"+approval.DueDate+ "'")
                         .Append(",")
                         .Append("'"+approval.Title+"'")
                         .Append(",")
                         .Append("'"+approval.Type+ "'")
                         .Append(",");
            approval.Recipient.ForEach((r)=> {
                stringBuilder.Append("'"+r.Name+ "'");
                stringBuilder.Append(",");
                stringBuilder.Append("'"+r.Email+ "'");
            });
            stringBuilder.Append(",");
            approval.Attachment.ForEach((a) => {
                stringBuilder.Append("'"+a.AttachmentUrl+ "'");
                stringBuilder.Append(",");
             }) ;
            stringBuilder.Append("1");
            stringBuilder.Append(");");
            var query = stringBuilder.ToString();
            Console.WriteLine("QUERY STRING =" + query);
            
            var data = ApprovalDB.ExecuteNonQuery(query);

            return data;
        }
    }
}