using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using IKS_Approval_App.Models;

namespace IKS_Approval_App.Services
{
    public class ApprovalService : BaseService
    {
        public Approval GetApprovalsById(int id)
        {
            Approval approval = new Approval();
            var dataTable = ApprovalDB.ExecuteDataTable("CALL Get_By_id(" + id + ")");
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                List<Recipient> recipients = new List<Recipient>();
                foreach (DataRow dr in dataTable.Rows)
                {

                    approval.ApprovalId = (int)dr["approval_id"];
                    approval.Title = (string)dr["approval_name"];
                    approval.Description = dr["description"].ToString();
                    approval.SenderEmail = (string)dr["sender_email"];
                    approval.ReleaseDate = DateTime.Parse(dr["release_date"].ToString());
                    approval.DueDate = DateTime.Parse(dr["due_date"].ToString());
                    approval.FinalStatus = (Status)Enum.Parse(typeof(Status), dr["approval_status"].ToString(), true);
                    approval.Type = (ApprovalType)Enum.Parse(typeof(ApprovalType), dr["type_name"].ToString(), true);
                    approval.Comment = dr["approval_comment"].ToString();

                    Recipient r = new Recipient(dr["recipient_email"].ToString(), dr["recipient_comment"].ToString(),
                        (Status)Enum.Parse(typeof(Status), dr["approval_status"].ToString(), true), Int32.Parse(dr["sequence_number"].ToString()));
                    r.ApprovedDateTime = dr["recipient_approved_date"].ToString();
                    recipients.Add(r);
                    approval.Recipient = recipients;
                    approval.Attachment = dr["attachment_url"].ToString();
                    /*List<Attachment> attachments = new List<Attachment>();
                    attachments.Add(new Attachment(dr["attachment_url"].ToString()));
                    approval.Attachment = attachments;*/


                }
            }
            if (approval.Recipient.Count!=0)
            {
                approval.Recipient.Sort((x, y) => { return x.SequenceNumber.CompareTo(y.SequenceNumber); });
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
                    approval.ApprovalId = Int32.Parse(dr["approval_id"].ToString());
                    approval.Title = (string)dr["approval_name"];
                    approval.Description = dr["description"].ToString();
                    approval.SenderEmail = (string)dr["sender_email"];
                    approval.ReleaseDate = DateTime.Parse(dr["release_date"].ToString());
                    approval.DueDate = DateTime.Parse(dr["due_date"].ToString());
                    approval.FinalStatus = (Status)Enum.Parse(typeof(Status), dr["approval_status"].ToString(), true);
                    approval.Comment = dr["approval_comment"].ToString();

                    approval.Attachment = dr["attachment_url"].ToString();

                    if (list.Contains(new Approval(approval.ApprovalId)))
                    {
                        Recipient r = new Recipient(dr["recipient_email"].ToString(), dr["recipient_comment"].ToString(),
                            (Status)Enum.Parse(typeof(Status), dr["approval_status"].ToString(), true), Int32.Parse(dr["sequence_number"].ToString()));

                        List<Recipient> recipients = new List<Recipient>();
                        recipients.Add(r);
                        approval.Recipient.Add(r);

                    }
                    else
                    {
                        Recipient r = new Recipient(dr["recipient_email"].ToString(), dr["recipient_comment"].ToString(),
                            (Status)Enum.Parse(typeof(Status), dr["approval_status"].ToString(), true), Int32.Parse(dr["sequence_number"].ToString()));

                        List<Recipient> recipients = new List<Recipient>();
                        recipients.Add(r);
                        approval.Recipient = recipients;
                        list.Add(approval);
                    }

                }
            }
            return list;
        }

        public Approval CreateApproval(ApprovalDto approval)
        {


            StringBuilder qry = new StringBuilder("CALL create_approval(");
            qry.Append("'" + approval.SenderEmail + "',");
            qry.Append("'" + approval.Description + "',");
            qry.Append("'" + approval.Comment + "',");
            qry.Append("'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "',");
            qry.Append("'" + approval.DueDate.ToString("yyyy-MM-dd hh:mm:ss") + "',");
            qry.Append("'" + approval.Title + "',");
            qry.Append("'" + approval.Type.ToString() + "',");
            qry.Append("'" + approval.Attachment + "');");
            var dataTable = ApprovalDB.ExecuteDataTable(qry.ToString());
            int approvalId = 0;
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                foreach (DataRow dr in dataTable.Rows)
                {
                    approvalId = Int32.Parse(dr["new_approval_id"].ToString());
                }
                int seq = 0;
                string all_qry=" ";
                approval.RecipientEmail.ForEach(r =>
                {
                    
                    StringBuilder recpientQry = new StringBuilder("CALL adding_recipients(");
                    recpientQry.Append("'" + r + "',");
                    seq += 1;
                    recpientQry.Append(seq + ",");
                    recpientQry.Append(approvalId + ",");
                    if (approval.Type.Equals(ApprovalType.PARALLEL))
                    {
                        recpientQry.Append("'" + Status.PENDING.ToString() + "');");
                    }else if ((approval.Type.Equals(ApprovalType.SEQUENTIAL)) && seq==1)
                    {
                        recpientQry.Append("'" + Status.PENDING.ToString() + "');");
                    }
                    else
                    {
                        recpientQry.Append("'" + Status.PENDING.ToString() + "');");
                    }
                    
                        
                    

                    Console.WriteLine(recpientQry.ToString());
                    all_qry += recpientQry + "\n";
                    ApprovalDB.ExecuteNonQuery(recpientQry.ToString());
                });

                return GetApprovalsById(approvalId);
            }
            else
            {
                return null;
            }
        }

        public async Task<ActResponceDto> StatusUpdateAsync(ActDto dto)
        {
            if (dto.Approval_id == 0 ||
                dto.RecipientComment.Equals("") || dto.RecipientComment == null ||
                dto.RecipientEmail == null || dto.RecipientEmail.Equals(""))
            {
                return await Task.FromResult(new ActResponceDto());
            }

           var list = await GetRecipientsForApproval(dto.Approval_id);

            ActResponceDto actResponceDto = new ActResponceDto();
            actResponceDto.FinalStatus = Status.PENDING;
            //Update indivadual status 
            StringBuilder qry = new StringBuilder("CALL Status_Update(");
            qry.Append("'" + dto.RecipientEmail + "',");
            qry.Append(dto.Approval_id + ",");
            qry.Append("'" + dto.RecipientComment + "',");
            qry.Append("'" + dto.Status.ToString() + "',");
            qry.Append("'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "',");
            qry.Append( list.Count + ");");
            var update = ApprovalDB.ExecuteNonQuery(qry.ToString());


            //check if final status can be change
            //get all recipients for approval 

             List<Recipient> recipients = await GetRecipientsForApproval(dto.Approval_id);

            

            /*CASE-1.p  for parallel
             * for parallel of any one accecpts final status will be changed to  approved
             * 
             * CASE-1.s for seq
             * if any one rejects approval then final status is Rejected
             * 
             */

             if(dto.ApprovalType.Equals(ApprovalType.PARALLEL))
            {
                int rejectCount = 0;
                recipients.ForEach(async r => 
                {
                    
                    if (r.status.Equals(Status.ACCEPTED))
                    {
                        //update final status and return
                        await UpdateFinalStatus(dto.Approval_id, Status.ACCEPTED);
                        actResponceDto.FinalStatus = Status.ACCEPTED;
                        actResponceDto.ApprovedTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                    }
                   else if (r.status.Equals(Status.REJECTED))
                    {
                        rejectCount++;
                    }

                });

              

                if (rejectCount == recipients.Count)
                {
                    await UpdateFinalStatus(dto.Approval_id, Status.REJECTED);
                    actResponceDto.FinalStatus = Status.REJECTED;
                    actResponceDto.ApprovedTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                }
             }
            if (dto.ApprovalType.Equals(ApprovalType.SEQUENTIAL))
            {
                int acceptedCount = 0;
                recipients.ForEach(async r =>
                {
                    if (r.status.Equals(Status.REJECTED))
                    {
                        //update final status and return
                        await UpdateFinalStatus(dto.Approval_id, Status.REJECTED);
                        actResponceDto.FinalStatus = Status.REJECTED;
                        actResponceDto.ApprovedTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

                        return;
                    }
                    else if (r.status.Equals(Status.ACCEPTED))
                    {
                        acceptedCount++;
                    }
                });

                if (acceptedCount == recipients.Count)
                {
                    await UpdateFinalStatus(dto.Approval_id, Status.ACCEPTED);
                    actResponceDto.FinalStatus = Status.ACCEPTED;
                    actResponceDto.ApprovedTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                }

              
            }

            actResponceDto.ApprovalId = dto.Approval_id;
            actResponceDto.Recipient = recipients;

            return await Task.FromResult(actResponceDto);
        }

        public Task<int> UpdateFinalStatus(int approvalid,Status s)
        {
            StringBuilder qry = new StringBuilder("CALL Final_Status_update(");
            qry.Append(approvalid + ",");
            qry.Append("'" + s.ToString() + "',");
            qry.Append("'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "');");
            var update = ApprovalDB.ExecuteNonQuery(qry.ToString());

            return Task.FromResult(update);
        }
        
        public async Task<List<Recipient>> GetRecipientsForApproval(int approvalId)
        {
              var dataTable = ApprovalDB.ExecuteDataTable("CALL List_Recipient_Status(" + approvalId + ")");
            List<Recipient> list = new List<Recipient>();
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dr in dataTable.Rows)
                {
                    Recipient r = new Recipient(dr["recipient_email"].ToString(),
                        (Status)Enum.Parse(typeof(Status), dr["approval_status"].ToString(), true),
                        Int32.Parse(dr["sequence_number"].ToString()));
                    r.ApprovedDateTime = dr["recipient_approved_date"].ToString();
                    list.Add(r);

                }
                list.Sort((x, y) => { return x.SequenceNumber.CompareTo(y.SequenceNumber); });

               
            }
            return await Task.FromResult(list);
            /*else
            {
                Console.WriteLine("Recipient NULL");
                return null;
            }*/

        }

        public CountDto GetSenderCount(string email)
        {
            CountDto dto = new CountDto();
            var dataTable = ApprovalDB.ExecuteDataTable("CALL count_approvalSender('" + email + "')");

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                foreach (DataRow dr in dataTable.Rows)
                {
                    dto.Accepted= Int32.Parse(dr["accepted"].ToString());
                    dto.Total= Int32.Parse(dr["Total"].ToString());
                    dto.Rejected= Int32.Parse(dr["rejected"].ToString());
                    dto.Pending= Int32.Parse(dr["pending"].ToString());
                }

            }
            return dto;
        }

        public CountDto GetRecieverCount(string email)
        {
            CountDto dto = new CountDto();
            var dataTable = ApprovalDB.ExecuteDataTable("CALL count_approvalReciever('" + email + "')");

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                foreach (DataRow dr in dataTable.Rows)
                {
                    dto.Accepted= Int32.Parse(dr["accepted"].ToString());
                    dto.Total= Int32.Parse(dr["total"].ToString());
                    dto.Rejected= Int32.Parse(dr["rejected"].ToString());
                    dto.Pending= Int32.Parse(dr["pending"].ToString());
                }

            }
            return dto;
        }

        public List<HomeDto> GetTotalCountRecived(string email)
        {
            List<HomeDto> title = new List<HomeDto>();
            var dataTable = ApprovalDB.ExecuteDataTable("CALL List_Of_approvals_Recipient('" + email + "')");
            foreach (DataRow dr in dataTable.Rows)
            {
                HomeDto dto = new HomeDto();
                dto.ApprovalId = Int32.Parse(dr["approval_id"].ToString());
                dto.Title = dr["approval_name"].ToString();
                title.Add(dto);
            }

            return title;
        }

        public List<HomeDto> GetTotalCountSent(string email)
        {
            List<HomeDto> title = new List<HomeDto>();
            var dataTable = ApprovalDB.ExecuteDataTable("List_Of_approvals_Sender('" + email + "')");
            foreach (DataRow dr in dataTable.Rows)
            {
                HomeDto dto = new HomeDto();
                dto.ApprovalId = Int32.Parse(dr["approval_id"].ToString());
                dto.Title = dr["approval_name"].ToString();
                title.Add(dto);
            }

            return title;
        }

        public List<HomeDto> GetSentCountStatus(string email, string status)
        {
            List<HomeDto> title = new List<HomeDto>();
            var dataTable = ApprovalDB.ExecuteDataTable("CALL List_of_sender_Status('" + email + "','" + status + "');");
            foreach (DataRow dr in dataTable.Rows)
            {
                HomeDto dto = new HomeDto();
                dto.ApprovalId = int.Parse(dr["approval_id"].ToString());
                dto.Title = dr["approval_name"].ToString();
                title.Add(dto);
            }
            return title;
        }

        public List<HomeDto> GetCountReceivedStatus(string email, string status)
        {
            List<HomeDto> title = new List<HomeDto>();
            var dataTable = ApprovalDB.ExecuteDataTable("CALL List_of_recipient_Status('" + email + "','" + status + "')");
            foreach (DataRow dr in dataTable.Rows)
            {
                HomeDto dto = new HomeDto();
                dto.ApprovalId = int.Parse(dr["approval_id"].ToString());
                dto.Title = dr["approval_name"].ToString();
                title.Add(dto);
            }
            return title;
        }

        public int updateStatusToPending(int approvalId,int seqNo,Status status)
        {
            string qry = "CALL Update_Status_To_Pending(" + approvalId + "," + seqNo +",'"+ status.ToString()+"');";
            var update = ApprovalDB.ExecuteNonQuery(qry);
            return update;
        }

        public int DeleteApproval(int approvalId)
        {
            return ApprovalDB.ExecuteNonQuery("CALL Delete_Approval('" + approvalId + "');");
           
        }

        public List<ReminderDto> GetReminders()
        {
            List<ReminderDto> reminder = new List<ReminderDto>();
            var dataTable = ApprovalDB.ExecuteDataTable("CALL Reminder();");
            foreach (DataRow dr in dataTable.Rows)
            {
                ReminderDto dto = new ReminderDto();
                dto.ApprovalId = int.Parse(dr["approval_id"].ToString());
                dto.Title = dr["approval_name"].ToString();
                dto.Email = dr["recipient_email"].ToString();
                reminder.Add(dto);
            }
            return reminder;
        }


    }

}