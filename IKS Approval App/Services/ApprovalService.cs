using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using IKS_Approval_App.Models;
using MySql.Data.MySqlClient;
namespace IKS_Approval_App.Services
{
    public class ApprovalService : BaseService
    {

        public List<HomeDto> GetAllApprovalTitleRecived(string email)
        {
            List<HomeDto> title = new List<HomeDto>();
            var dataTable = ApprovalDB.ExecuteDataTable("CALL List_Of_Approvals_Recipient('" + email + "')");
            foreach (DataRow dr in dataTable.Rows)
            {
                HomeDto dto = new HomeDto();
                dto.ApprovalId = Int32.Parse(dr["approval_id"].ToString());
                dto.Title = dr["approval_name"].ToString();
                title.Add(dto);
            }

            return title;
        }

        public List<HomeDto> GetAllApprovalTitleSent(string email)
        {
            List<HomeDto> title = new List<HomeDto>();
            var dataTable = ApprovalDB.ExecuteDataTable("CALL List_of_approvals_Send('" + email + "')");
            foreach (DataRow dr in dataTable.Rows)
            {
                HomeDto dto = new HomeDto();
                dto.ApprovalId = Int32.Parse(dr["approval_id"].ToString());
                dto.Title = dr["approval_name"].ToString();
                title.Add(dto);
            }

            return title;
        }

        public Approval GetApprovalsById(int id)
        {
            Approval approval = new Approval();
            var dataTable = ApprovalDB.ExecuteDataTable("CALL Get_By_id(" + id + ")");
            if (dataTable != null && dataTable.Rows.Count == 1)
            {
                foreach (DataRow dr in dataTable.Rows)
                {

                    approval.ApprovalId = (int)dr["approval_id"];
                    approval.Title = (string)dr["approval_name"];
                    approval.Description = dr["description"].ToString();
                    approval.SenderEmail = (string)dr["sender_email"];
                    approval.ReleaseDate = dr["release_date"].ToString();
                    approval.DueDate = dr["due_date"].ToString();
                    approval.Status = (Status)Enum.Parse(typeof(Status), dr["approval_status"].ToString(), true);
                    approval.Comment = dr["approval_comment"].ToString();

                    Recipient r = new Recipient(dr["recipient_email"].ToString(), dr["recipient_comment"].ToString(),
                        (Status)Enum.Parse(typeof(Status), dr["approval_status"].ToString(), true), Int32.Parse(dr["sequence_number"].ToString()));
                    List<Recipient> recipients = new List<Recipient>();
                    recipients.Add(r);
                    approval.Recipient = recipients;

                    List<Attachment> attachments = new List<Attachment>();
                    attachments.Add(new Attachment(dr["attachment_url"].ToString()));
                    approval.Attachment = attachments;


                }
            }
            return approval;
        }

        public List<Approval> GetApprovals()
        {
            List<Approval> list = new List<Approval>();

            var dataTable = ApprovalDB.ExecuteDataTable("CALL GetAll();");
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                foreach (DataRow dr in dataTable.Rows)
                {
                    Approval approval = new Approval();
                    approval.ApprovalId = (int)dr["approval_id"];
                    approval.Title = (string)dr["approval_name"];
                    approval.Description = dr["description"].ToString();
                    //approval.SenderName = (string)dr["sender_name"];
                    approval.SenderEmail = (string)dr["sender_email"];
                    approval.ReleaseDate = dr["release_date"].ToString();
                    approval.DueDate = dr["due_date"].ToString();
                    //approval.Status = (string)dr["status"];
                    approval.Status = (Status)Enum.Parse(typeof(Status), dr["approval_status"].ToString(), true);

                    approval.Comment = dr["approval_comment"].ToString();
                    Recipient r = new Recipient(dr["recipient_email"].ToString(), dr["recipient_comment"].ToString(),
                        (Status)Enum.Parse(typeof(Status), dr["approval_status"].ToString(), true), Int32.Parse(dr["sequence_number"].ToString()));
                    List<Recipient> recipients = new List<Recipient>();
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
            stringBuilder.Append("'" + approval.SenderEmail + "'")
                         .Append(",")
                         .Append("'" + approval.Comment + "'")
                         .Append(",")
                         .Append("'" + approval.ReleaseDate + "'")
                         .Append(",")
                         .Append("'" + approval.DueDate + "'")
                         .Append(",")
                         .Append("'" + approval.Title + "'")
                         .Append(",")
                         .Append("'" + approval.Type + "'")
                         .Append(",");
            approval.Recipient.ForEach((r) =>
            {

                stringBuilder.Append("'" + r.Email + "'");
            });
            stringBuilder.Append(",");
            approval.Attachment.ForEach((a) =>
            {
                stringBuilder.Append("'" + a.AttachmentUrl + "'");
                stringBuilder.Append(",");
            });
            stringBuilder.Append("1");
            stringBuilder.Append(");");
            var query = stringBuilder.ToString();
            Console.WriteLine("QUERY STRING =" + query);

            var data = ApprovalDB.ExecuteNonQuery(query);

            return data;
        }


         public CountDto getHomeCount(string email)
        {
            var recivedCount =  GetAllApprovalTitleSent(email).Count;
            var sentCount = GetAllApprovalTitleRecived(email).Count;
            var result = new CountDto();
            result.RecivedCount = recivedCount;
            result.SentCount = sentCount;
            return result;
        }
        public int StatusUpdate(ActDto dto)
        {

            if (dto.Approval_id == 0 || 
                dto.RecipientComment.Equals("")|| dto.RecipientComment==null||
                dto.RecipientEmail == null || dto.RecipientEmail.Equals(""))
            {
                return 0;
            }


           

            StringBuilder qry = new StringBuilder("CALL Status_Update(");
            qry.Append("'"+dto.RecipientEmail+"',");
            qry.Append(dto.Approval_id+",");
            qry.Append("'"+dto.RecipientComment+"',");
            qry.Append("'"+dto.Status.ToString()+"',");
            qry.Append("'"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") +"');");
            var update = ApprovalDB.ExecuteNonQuery(qry.ToString());


            return update;
        }
    }
}