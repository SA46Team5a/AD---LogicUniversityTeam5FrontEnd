using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace LogicUniversityTeam5.Controllers
{
    public class EmailNotificationController : Controller
    {
 
        public EmailNotificationController()
        {

        }
        public void SendEmailToAppointingDepRep()
        {
           
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.Credentials = new System.Net.NetworkCredential("meitingtonia@gmail.com", "GMTtonia1995");
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //client.UseDefaultCredentials = true;
            MailMessage mm = new MailMessage("meitingtonia@gmail.com", "meitingtonia@gmail.com");
            mm.Subject = "Notification to appointing department representative";
            mm.Body = "Dear employee: You have been appointed as department representative. " +
                "Now you have right to collect stationaries and maintain the department list." + "Regards" +
                "This is a system generated email. Do not reply to this email.";
          client.Send(mm);
            
        }
        public void SendEmailToLoseDepRep()
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.Credentials = new System.Net.NetworkCredential("meitingtonia@gmail.com", "GMTtonia1995");
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //client.UseDefaultCredentials = true;
            MailMessage mm = new MailMessage("meitingtonia@gmail.com", "meitingtonia@gmail.com");
            mm.Subject = "  Notification change of department representative";

            mm.Body = "Dear employee:   Please take note, you are no long the department representative.  "+                     
                      "Regards " +
                      "This is a system generated email. Do not reply to this email.";
            client.Send(mm);
        }
        public void SendToLostApproveAuthority()
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.Credentials = new System.Net.NetworkCredential("meitingtonia@gmail.com", "GMTtonia1995");
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //client.UseDefaultCredentials = true;
            MailMessage mm = new MailMessage("meitingtonia@gmail.com", "meitingtonia@gmail.com");
            mm.Subject = "Notification for authority change";

            mm.Body = "Dear employee:  "+ "your authority to approve stationary requisition forms has been ceased" +
                      "Regards " +
                      "This is a system generated email. Do not reply to this email.";
            client.Send(mm);
        }
        public void SendEmailToDelegatePerson()
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.Credentials = new System.Net.NetworkCredential("meitingtonia@gmail.com", "GMTtonia1995");
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //client.UseDefaultCredentials = true;
            MailMessage mm = new MailMessage("meitingtonia@gmail.com", "meitingtonia@gmail.com");
            mm.Subject = "Notification for authority change";

            mm.Body = "Dear employee: " + "your authority to approve stationary requisition forms has been ceased" +
                      "Regards " +
                      "This is a system generated email. Do not reply to this email.";
            client.Send(mm);
        }
        public void SendEmailToDelegatePerson(string StartDate, string EndDate)
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.Credentials = new System.Net.NetworkCredential("meitingtonia@gmail.com", "GMTtonia1995");
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //client.UseDefaultCredentials = true;
            MailMessage mm = new MailMessage("meitingtonia@gmail.com", "meitingtonia@gmail.com");
            mm.Subject = "Notification to delegating authority";

            mm.Body = "Dear employee:                                                                " +
                      " You have been granted authority from"+ StartDate+" to"+ EndDate+" to approve Stationery Requisition Forms." +
                      "Regards                                                            " +
                      "This is a system generated email. Do not reply to this email.";
            client.Send(mm);
        }
        public void SendEmailToRequisitionStatus(int RequisitionID, bool ToApprove)
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.Credentials = new System.Net.NetworkCredential("meitingtonia@gmail.com", "GMTtonia1995");
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //client.UseDefaultCredentials = true;
            MailMessage mm = new MailMessage("meitingtonia@gmail.com", "meitingtonia@gmail.com");
            mm.Subject = "Notification of status of Requisition Form"+RequisitionID;
            if (ToApprove == true)
            {
                mm.Body = "Dear employee: " + "The items of Requisition Form"+RequisitionID+" has been approved " +
                      "Regards " +
                      "This is a system generated email. Do not reply to this email.";
            }
            if (ToApprove == false)
            {
                mm.Body = "Dear employee: " + "The items of Requisition Form" + " RequisitionID" + " has been rejected " +
                   "Regards " +
                   "This is a system generated email. Do not reply to this email.";
            }
            client.Send(mm);
        }
        public void SendEmailForChangeAuthorityDuration(string StartDate, string EndDate)
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.Credentials = new System.Net.NetworkCredential("meitingtonia@gmail.com", "GMTtonia1995");
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //client.UseDefaultCredentials = true;
            MailMessage mm = new MailMessage("meitingtonia@gmail.com", "meitingtonia@gmail.com");
            mm.Subject = "Notification for authority duration change";

            mm.Body = "Dear employee: " + "Your authority period has been change from " + StartDate + " to" + EndDate + " to approve Stationery Requisition Forms." +
                      "Regards " +
                      "This is a system generated email. Do not reply to this email.";
            client.Send(mm);
        }
        public void SendEmailForChangeCollectionPoint(string DepartmentName, string CollectionPoint)
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.Credentials = new System.Net.NetworkCredential("meitingtonia@gmail.com", "GMTtonia1995");
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //client.UseDefaultCredentials = true;
            MailMessage mm = new MailMessage("meitingtonia@gmail.com", "meitingtonia@gmail.com");
            mm.Subject = "Notification for change of collection point";

            mm.Body = "Dear employee: " + "The collection point for " + DepartmentName + "has been change to" + CollectionPoint + 
                      "Regards " +
                      "This is a system generated email. Do not reply to this email.";
            client.Send(mm);
        }
        public void ApprovStoreAdjustment(string name)
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.Credentials = new System.Net.NetworkCredential("meitingtonia@gmail.com", "GMTtonia1995");
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //client.UseDefaultCredentials = true;
            MailMessage mm = new MailMessage("meitingtonia@gmail.com", "meitingtonia@gmail.com");
            mm.Subject = "Notification for approve store adjustment";

            mm.Body = "Dear"+ name + ": New stock voucher has been added. Please review the voucher. " +
                      "Regards " +
                      "This is a system generated email. Do not reply to this email.";
            client.Send(mm);
        }
        public void SendtoApprovStoreAdjustment(string name)
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.Credentials = new System.Net.NetworkCredential("meitingtonia@gmail.com", "GMTtonia1995");
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //client.UseDefaultCredentials = true;
            MailMessage mm = new MailMessage("meitingtonia@gmail.com", "meitingtonia@gmail.com");
            mm.Subject = "Notification for approve store adjustment";

            mm.Body = "Dear" + name + "New stock voucher has been added. Please review the voucher. " +
                      "Regards " +
                      "This is a system generated email. Do not reply to this email.";
            client.Send(mm);
        }
        public void SendtoConfirmDisbursement(string Repname)
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.Credentials = new System.Net.NetworkCredential("meitingtonia@gmail.com", "GMTtonia1995");
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //client.UseDefaultCredentials = true;
            MailMessage mm = new MailMessage("meitingtonia@gmail.com", "meitingtonia@gmail.com");
            mm.Subject = "Notification for collection of items";

            mm.Body = "Dear all" + " The items are available for collection, please collect from" + Repname+
                      "Regards " +
                      "This is a system generated email. Do not reply to this email.";
            client.Send(mm);
        }
        public void SendtoConfirmDisbursement_Mobile(string Repname)
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.Credentials = new System.Net.NetworkCredential("meitingtonia@gmail.com", "GMTtonia1995");
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //client.UseDefaultCredentials = true;
            MailMessage mm = new MailMessage("meitingtonia@gmail.com", "meitingtonia@gmail.com");
            MailAddress copy1 = new MailAddress("e0283995@u.nus.edu");
            MailAddress copy2 = new MailAddress("Khimyang22@gmail.com");
            mm.CC.Add(copy1);
            mm.CC.Add(copy2);
            mm.Subject = "Notification for collection of items";

            mm.Body = "Dear all" + " The items are available for collection, please collect from " + Repname +
                      
                      "This is a system generated email. Do not reply to this email.";
            client.Send(mm);
        }
        public void SubmitStoreAdhustment_Mobile(string Manager)
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.Credentials = new System.Net.NetworkCredential("meitingtonia@gmail.com", "GMTtonia1995");
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //client.UseDefaultCredentials = true;
            MailMessage mm = new MailMessage("meitingtonia@gmail.com", "meitingtonia@gmail.com");
            mm.Subject = "Notification for approve store adjustment";

            mm.Body = "Dear"+Manager + "New stock voucher has been added, please review the voucher" + 
                      "Regards " +
                      "This is a system generated email. Do not reply to this email.";
            client.Send(mm);
        }
    }
}