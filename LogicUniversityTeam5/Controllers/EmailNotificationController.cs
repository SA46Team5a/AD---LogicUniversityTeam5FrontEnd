using ServiceLayer;
using ServiceLayer.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace LogicUniversityTeam5.Controllers
{
    //To change all email from and Email to addresses
    public class EmailNotificationController : Controller
    {

        public EmailNotificationController()
        {

        }

        //public EmailNotificationController(IDepartmentService departmentService)
        //{
        //    this.departmentService = departmentService;
        //}

        //Requisitions
        public static void SendEmailToDeptHeadToApproveRequisitions(string emailTo, int reqId)
        {

            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.Credentials = new System.Net.NetworkCredential("LogicstationeryTeam5@gmail.com", "logicteam5@");
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //client.UseDefaultCredentials = true;

            MailMessage mm = new MailMessage("divyashree.ga@gmail.com", "divyashree.ga@gmail.com");

            mm.Subject = "Notification to approve Requisition Form ";
            mm.Body = "Dear Department Head ,You have the requisition  id(" + reqId + ")for your approval.";
            client.Send(mm);
        }
        public static void SendEmailToAppointingDepRep(string passcode)
        {

            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);

            client.Credentials = new System.Net.NetworkCredential("LogicstationeryTeam5@gmail.com", "logicteam5@");
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;

            
            MailMessage mm = new MailMessage("LogicstationeryTeam5@gmail.com", "wanglu19@u.nus.edu");

            mm.Subject = "Notification to appointing department representative";
            mm.Body = "Dear employee:\r\n You have been appointed as department representative. " +
                "Now you have right to collect stationaries and maintain the department list. Your passcode is " + passcode + ".\r\n " + "Regards\r\n" +
                "This is a system generated email. Do not reply to this email.";
            client.Send(mm);

        }
        public void SendEmailToLoseDepRep()
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.Credentials = new System.Net.NetworkCredential("LogicstationeryTeam5@gmail.com", "logicteam5@");
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //client.UseDefaultCredentials = true;
            MailMessage mm = new MailMessage("LogicstationeryTeam5@gmail.com", "divyashree.ga@gmail.com");
            mm.Subject = "  Notification change of department representative";

            mm.Body = "Dear employee:\r\nPlease take note, you are no long the department representative.\r\n" +
                      "Regards \r\n" +
                      "This is a system generated email. Do not reply to this email.";
            client.Send(mm);
        }
        public static void SendToLostApproveAuthority()
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.Credentials = new System.Net.NetworkCredential("LogicstationeryTeam5@gmail.com", "logicteam5@");
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //client.UseDefaultCredentials = true;
            MailMessage mm = new MailMessage("LogicstationeryTeam5@gmail.com", "divyashree.ga@gmail.com");
            mm.Subject = "Notification for authority change";

            mm.Body = "Dear employee:\r\nyour authority to approve stationary requisition forms has been ceased\r\n" +
                      "Regards\r\n" +

                      "This is a system generated email. Do not reply to this email.";
            client.Send(mm);
        }
        public static void SendToApproveAuthority()
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.Credentials = new System.Net.NetworkCredential("LogicstationeryTeam5@gmail.com", "logicteam5@");
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //client.UseDefaultCredentials = true;
            MailMessage mm = new MailMessage("LogicstationeryTeam5@gmail.com", "divyashree.ga@gmail.com");
            mm.Subject = "Notification for authority change";

            mm.Body = "Dear employee:\r\n Now you have  authority to approve stationary requisition forms " +
                      "Regards\r\n" +

                      "This is a system generated email. Do not reply to this email.";
            client.Send(mm);
        }
        public static void SendEmailToDelegatePerson()
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.Credentials = new System.Net.NetworkCredential("LogicstationeryTeam5@gmail.com", "logicteam5@");
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //client.UseDefaultCredentials = true;
            MailMessage mm = new MailMessage("LogicstationeryTeam5@gmail.com", "divyashree.ga@gmail.com");
            mm.Subject = "Notification for authority change";

            mm.Body = "Dear employee:\r\n" + "your authority to approve stationary requisition forms has been ceased\r\n" +
                      "Regards\r\n" +
                      "This is a system generated email. Do not reply to this email.";
            client.Send(mm);
        }
        public static void SendEmailToDelegatePerson(string StartDate, string EndDate)
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.Credentials = new System.Net.NetworkCredential("LogicstationeryTeam5@gmail.com", "logicteam5@");
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //client.UseDefaultCredentials = true;
            MailMessage mm = new MailMessage("LogicstationeryTeam5@gmail.com", "divyashree.ga@gmail.com");
            mm.Subject = "Notification to delegating authority";

            mm.Body = "Dear employee:\r\n" +
                      "You have been granted authority from" + StartDate + " to" + EndDate + " to approve Stationery Requisition Forms.\r\n" +
                      "Regards\r\n" +
                      "This is a system generated email. Do not reply to this email.";
            client.Send(mm);
        }
        public static void SendEmailToRequisitionStatus(bool ToApprove)
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.Credentials = new System.Net.NetworkCredential("LogicstationeryTeam5@gmail.com", "logicteam5@");
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //client.UseDefaultCredentials = true;
            MailMessage mm = new MailMessage("LogicstationeryTeam5@gmail.com", "divyashree.ga@gmail.com");
            mm.Subject = "Notification of status of Requisition Form";
            if (ToApprove == true)
            {
                mm.Body = "Dear employee:\r\n" + "The items of Requisition Form" + " has been approved\r\n" +
                      "Regards\r\n" +
                      "This is a system generated email. Do not reply to this email.";
            }
            if (ToApprove == false)
            {
                mm.Body = "Dear employee:\r\n" + "The items of Requisition Form" + " RequisitionID" + " has been rejected\r\n" +
                   "Regards\r\n" +
                   "This is a system generated email. Do not reply to this email.";
            }
            client.Send(mm);
        }
        public void SendEmailForChangeAuthorityDuration(string StartDate, string EndDate)
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.Credentials = new System.Net.NetworkCredential("LogicstationeryTeam5@gmail.com", "logicteam5@");
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //client.UseDefaultCredentials = true;
            MailMessage mm = new MailMessage("LogicstationeryTeam5@gmail.com", "divyashree.ga@gmail.com");
            mm.Subject = "Notification for authority duration change";

            mm.Body = "Dear employee:\r\n" + "Your authority period has been change from " + StartDate + " to" + EndDate + " to approve Stationery Requisition Forms.\r\n" +
                      "Regards\r\n" +
                      "This is a system generated email. Do not reply to this email.";
            client.Send(mm);
        }
        public static void SendEmailForChangeCollectionPoint(string DepartmentName, string CollectionPoint, string passcode)
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.Credentials = new System.Net.NetworkCredential("LogicstationeryTeam5@gmail.com", "logicteam5@");
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //client.UseDefaultCredentials = true;
            MailMessage mm = new MailMessage("LogicstationeryTeam5@gmail.com", "divyashree.ga@gmail.com");
            mm.Subject = "Notification for change of collection point";
            mm.Body = "Dear employee:\r\n" + "The collection point for " + DepartmentName + " has been change to " + CollectionPoint + "\r\n" +
                      "The Passcode is " + passcode + ".\n\n" +
                      "Regards\r\n" +
                      "This is a system generated email. Do not reply to this email.";
            client.Send(mm);
        }
        public void ApprovStoreAdjustment(string name)
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.Credentials = new System.Net.NetworkCredential("LogicstationeryTeam5@gmail.com", "logicteam5@");
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //client.UseDefaultCredentials = true;
            MailMessage mm = new MailMessage("LogicstationeryTeam5@gmail.com", "divyashree.ga@gmail.com");
            mm.Subject = "Notification for approve store adjustment";

            mm.Body = "Dear" + name + ":\r\n" + "New stock voucher has been added. Please review the voucher.\r\n" +
                      "Regards\r\n" +
                      "This is a system generated email. Do not reply to this email.";
            client.Send(mm);
        }
        public void SendtoApprovStoreAdjustment(string name)
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.Credentials = new System.Net.NetworkCredential("LogicstationeryTeam5@gmail.com", "logicteam5@");
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //client.UseDefaultCredentials = true;
            MailMessage mm = new MailMessage("LogicstationeryTeam5@gmail.com", "divyashree.ga@gmail.com");
            mm.Subject = "Notification for approve store adjustment";

            mm.Body = "Dear" + name + ":\r\n" + "New stock voucher has been added. Please review the voucher.\r\n" +
                      "Regards\r\n" +
                      "This is a system generated email. Do not reply to this email.";
            client.Send(mm);
        }
        public void SendtoConfirmDisbursement(string Repname)
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.Credentials = new System.Net.NetworkCredential("LogicstationeryTeam5@gmail.com", "logicteam5@");
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //client.UseDefaultCredentials = true;
            MailMessage mm = new MailMessage("LogicstationeryTeam5@gmail.com", "divyashree.ga@gmail.com");
            mm.Subject = "Notification for collection of items";

            mm.Body = "Dear all:\r\n" + "The items are available for collection, please collect from" + Repname + "\r\n" +
                      "Regards\r\n" +
                      "This is a system generated email. Do not reply to this email.";
            client.Send(mm);
        }
        public void SendtoConfirmDisbursement_Mobile(string Repname)
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.Credentials = new System.Net.NetworkCredential("LogicstationeryTeam5@gmail.com", "logicteam5@");
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //client.UseDefaultCredentials = true;
            MailMessage mm = new MailMessage("LogicstationeryTeam5@gmail.com", "wanglu19@u.nus.edu");
            MailAddress copy1 = new MailAddress("e0283995@u.nus.edu");
            MailAddress copy2 = new MailAddress("Khimyang22@gmail.com");
            mm.CC.Add(copy1);
            mm.CC.Add(copy2);
            mm.Subject = "Notification for collection of items";

            mm.Body = "Dear all:\r\n" + " The items are available for collection, please collect from " + Repname + "\r\n" +
                      "Regards\r\n" +
                      "This is a system generated email. Do not reply to this email.";
            client.Send(mm);
        }
        public void SubmitStoreAdhustment_Mobile(string Manager)
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.Credentials = new System.Net.NetworkCredential("LogicstationeryTeam5@gmail.com", "logicteam5@");
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //client.UseDefaultCredentials = true;
            MailMessage mm = new MailMessage("LogicstationeryTeam5@gmail.com", "divyashree.ga@gmail.com");
            mm.Subject = "Notification for approve store adjustment";

            mm.Body = "Dear" + Manager + ":\r\n" + "New stock voucher has been added, please review the voucher\r\n" +
                      "Regards\r\n" +
                      "This is a system generated email. Do not reply to this email.";
            client.Send(mm);
        }

        public static void SendEmailForNewPasscode(string emailTo, string newPasscode)
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.Credentials = new System.Net.NetworkCredential("LogicstationeryTeam5@gmail.com", "logicteam5@");
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //client.UseDefaultCredentials = true;
            //EmailTo should be directed to emailTo (hardcoded for testing)
            MailMessage mm = new MailMessage("LogicstationeryTeam5@gmail.com", "LogicfinanceTeam5@gmail.com");
            mm.Subject = "Notification for new passcode";

            mm.Body = String.Format("Dear Department Representative:\r\n\n" + "The new passcode for disbursement is : {0}\r\n\n" +
                      "Regards\r\n" +
                      "This is a system generated email. Do not reply to this email.", newPasscode);
            client.Send(mm);
        }
    }
}