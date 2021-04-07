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

        public List<Approval> GetAllApprovalTitle()
        {

            List<Approval> list = new List<Approval>();
            List<Recipient> recipients = new List<Recipient>();
            var dataTable = ApprovalDB.ExecuteDataTable("CALL GetAll();");
            if(dataTable != null && dataTable.Rows.Count > 0)
            {
                
                Approval approval = new Approval();
                Recipient recipient = new Recipient();

                foreach (DataRow dr in dataTable.Rows)
                {
                    approval.ApprovalId = (int)dr["approval_id"];
                    approval.Title = (string)dr["approval_name"];
                    approval.SenderName = (string)dr["sender_name"];
                    approval.SenderEmail = (string)dr["sender_email"];
                    approval.ReleaseDate = (string)dr["release_date"];
                    approval.DueDate = (string)dr["due_date"];
                    approval.Status = (string)dr["status"];
                    approval.Comment = (string)dr["comment"];
                    approval.Attachment = (string)dr["attachment_url"];
                    
                    list.Add(approval);
                }
            }
            return list;
            /*List<string> list = new List<string>();

            DataTable dt = ApprovalDB.ExecuteDataTable("select approval_name from approval_table;");
            //  CALL create_approval('tejas','tejas@gmail.com','good','2020-11-11','2020-12-12','budget','parallel','ronak','ronak@gmail.com','vcdkbcoi',1); 
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(Convert.ToString(dr["approval_name"]));
                }
            }

            return list;*/
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