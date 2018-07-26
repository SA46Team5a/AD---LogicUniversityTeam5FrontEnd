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
            return View(reqList);
        }

        public ActionResult DeleteRequisitionForm(int id)
        {            
            requisitionService.deleteRequisition(id);
            
            //get employee id using reqId
            ServiceLayer.DataAccess.Requisition r = requisitionService.getRequisitionById(id);           
            return RedirectToAction("SearchRequisitionForm", new { id = r.EmployeeID });
        }

        //[HttpPost]
        //done dynamically by client side?
        //public ActionResult LoadSearchRequisitionForm(DateTime startDate, DateTime endDate, int approvalStatus, FormCollection form)
        //{
        //string approvalStatus = form["approvalstatus"].ToString();
        //DateTime startDate = form["startdate"];

        //}

        //Is this for on click View button, which goes to Screen 2.2.2.3 View Stationery Request Form
        public ActionResult ProcessedStationaryRequestForm(int id)
        {
            ServiceLayer.DataAccess.Requisition r = requisitionService.getRequisitionById(id);
            //r.RequisitionDetails.ToList();
            return View(r);
        }

        //Go to Screen 2.2.1.2b Resubmit Stationery Request Form
        public ActionResult EditSubmittedStationaryRequestForm(int id)
        {
            ServiceLayer.DataAccess.Requisition r = requisitionService.getRequisitionById(id);
            //r.RequisitionDetails.ToList();
            return View(r);
        }

        //Is this repeated in Requisition/DeleteRequestedItems
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

        public ActionResult StationeryCatalogueView(int id)
        {
            ServiceLayer.DataAccess.Requisition r = requisitionService.getRequisitionById(id);
            return View(r);
        }

        //Add item?
        public ActionResult RequestItem(int itemId, int qty, int reqId)
        {
            return View();
        }

    }
}