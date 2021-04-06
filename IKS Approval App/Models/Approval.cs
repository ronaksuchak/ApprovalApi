using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IKS_Approval_App.Models
{

    /*{ 
    statusCode: 201,
    approval_id:int,
    msg:CREATED,
    title:string,
    Description:string,
    Sender_name:string,
    sender_email:string,
    due_date : string (YYYY-MM-DD)
    due_time:(HH:MM),
    attachment_url:string,
    Recipient:[
         {
            Email:string
         } ],

}*/

    public class Approval
    {

        public int ApprovalId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string ReleaseDate { get; set; }
        public string DueDate { get; set; }
        public string DueTime{ get; set; }
        public string Comment { get; set; }
        public Status Status { get; set; }
        public Type Type { get; set; }
        public List<Attachment> Attachment { get; set; }
        public List<Recipient> Recipient { get; set; }

        public Approval(int approvalId, string title, string description, string senderName, string senderEmail, string releaseDate, string dueDate, string dueTime, string comment, Status status, Type type, List<Attachment> attachment, List<Recipient> recipient)
        {
            ApprovalId = approvalId;
            Title = title;
            Description = description;
            SenderName = senderName;
            SenderEmail = senderEmail;
            ReleaseDate = releaseDate;
            DueDate = dueDate;
            DueTime = dueTime;
            Comment = comment;
            Status = status;
            Type = type;
            Attachment = attachment;
            Recipient = recipient;
        }
    }
}