using LogicUniversityTeam5.Models;
using ServiceLayer;
using ServiceLayer.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LogicUniversityTeam5.Controllers
{
    //Author: Benedict
    public class ViewPastRequestController : Controller
    {

        IRequisitionService requisitionService;
        StationeryStoreEntities context = StationeryStoreEntities.Instance;

        public ViewPastRequestController(RequisitionService requisitionService)
        {
            this.requisitionService = requisitionService;
        }

        //ViewPastRequest/SearchRequisitionForm/E026
        public ActionResult SearchRequisitionForm(string id)
        {
            List<ServiceLayer.DataAccess.Requisition> reqList = requisitionService.getRequisitionsOfEmployee(id);

            //reorder unsubmitted status to the top of list
            //foreach (ServiceLayer.DataAccess.Requisition r in reqList)
            //{
            //    if (r.ApprovalStatusID == 1)
            //    {
            //        reqList.Remove(r);
            //        reqList.Insert(0, r);
            //    }
            //}
            ViewBag.EmpId = id;
            return View(reqList);
        }

        [HttpPost]
        public ActionResult DeleteRequisitionForm(int id)
        {            
            requisitionService.deleteRequisition(id);
            
            //get employee id using reqId
            ServiceLayer.DataAccess.Requisition r = requisitionService.getRequisitionById(id);           
            return RedirectToAction("SearchRequisitionForm", new { id = r.EmployeeID });
        }

        [HttpPost]
        public ActionResult LoadSearchRequisitionForm(FormCollection form)
        {
            string approvalStatus = form["approvalstatus"].ToString();
            //DateTime startDate = Convert.ToDateTime(form["startdate"]);
            //DateTime endDate = Convert.ToDateTime(form["enddate"]);

            List<ServiceLayer.DataAccess.Requisition> reqList = new List<ServiceLayer.DataAccess.Requisition>();
            string empId = form["empId"];
            if (approvalStatus.Equals("All"))
            {
                reqList = requisitionService.getRequisitionsOfEmployee(empId);
            }
            else
            {
                reqList = context.Requisitions.Where(r => r.EmployeeID == empId && r.ApprovalStatus.ApprovalStatusName == approvalStatus).ToList();
            }

            ViewBag.SelectedApprovalStatus = approvalStatus; ViewBag.EmpId = empId;
            return View("SearchRequisitionForm", reqList);
        }

        //Go to Screen 2.2.2.3 View Stationery Request Form
        //ViewPastRequest/ProcessedStationeryRequestForm/15
        public ActionResult ProcessedStationeryRequestForm(int id)
        {
            ServiceLayer.DataAccess.Requisition r = requisitionService.getRequisitionById(id);
            List<ServiceLayer.DataAccess.RequisitionDetail> rdList = r.RequisitionDetails.ToList();
            return View("ViewStationeryRequestForm", rdList);          
        }

        //Go to Screen 2.2.1.2b Resubmit Stationery Request Form
        public ActionResult EditSubmittedStationeryRequestForm(int id)
        {
            ServiceLayer.DataAccess.Requisition r = requisitionService.getRequisitionById(id);
            List<ServiceLayer.DataAccess.RequisitionDetail> rdList = r.RequisitionDetails.ToList();
            return View("ResumbitStationeryRequestForm", rdList);
        }

        //This is for delete in Screen 2.2.1.2b
        public ActionResult DeleteRequestedItems(int id)
        {
            requisitionService.deleteRequisitionDetail(id);

            //get reqId using reqdetailId
            ServiceLayer.DataAccess.RequisitionDetail rd = context.RequisitionDetails.FirstOrDefault(rq => rq.RequisitionDetailsID == id);
            return RedirectToAction("EditSubmittedStationeryRequestForm", new { id = rd.Requisition.RequisitionID });
        }

        public ActionResult EmployeeHome()
        {
            return View();
        }

        /*
        public ActionResult StationeryCatalogueView(int id)
        {
            ServiceLayer.DataAccess.Requisition r = requisitionService.getRequisitionById(id);
            return View(r);
        }

        //Add item?
        public ActionResult RequestItem(int itemId, int qty, int reqId)
        {
            return View();
        }*/

    }
}