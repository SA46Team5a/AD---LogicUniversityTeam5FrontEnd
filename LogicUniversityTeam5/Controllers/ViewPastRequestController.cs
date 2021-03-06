﻿using LogicUniversityTeam5.IdentityHelper;
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
    [Authorize(Roles = "Department Representative, Employee")]
    public class ViewPastRequestController : Controller
    {

        IRequisitionService requisitionService;
        IClassificationService classificationService;
        IDisbursementService disbursementService;
        StationeryStoreEntities context = StationeryStoreEntities.Instance;

        public ViewPastRequestController(RequisitionService requisitionService, ClassificationService cs, DisbursementService ds)
        {
            this.requisitionService = requisitionService;
            classificationService = cs;
            disbursementService = ds;
        }


        public ActionResult SearchRequisitionForm(string id)
        {
            CombinedViewModel combinedViewModel = new CombinedViewModel();
            combinedViewModel.Requisition = requisitionService.getRequisitionsOfEmployee(id);
            combinedViewModel.ApprovalStatusNames = classificationService.GetApprovalStatusNames();
            
            ViewBag.EmpId = id;
            combinedViewModel.Requisition = combinedViewModel.Requisition.OrderByDescending(r => r.RequisitionID).ToList();

            return View(combinedViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
            string start = form["startdate"].ToString();
            string end = form["enddate"].ToString();
            DateTime startDate, endDate;

            //if ((start+end) != null && (start + end) != "")
            //{
            startDate = start == null || start == "" ? DateTime.MinValue : Convert.ToDateTime(form["startdate"]);
            endDate = end == null || end == "" ? DateTime.MaxValue : Convert.ToDateTime(form["enddate"]);
            //}

            List <ServiceLayer.DataAccess.Requisition> reqList = new List<ServiceLayer.DataAccess.Requisition>();
            string empId = form["empId"];
            if (approvalStatus.Equals("All"))
            {
                reqList = requisitionService.getRequisitionsOfEmployee(empId);
            }
            else
            {
                //reqList = requisitionService.getRequisitionsByEmpIdAndStatus(empId, approvalStatus);
                reqList = context.Requisitions.Where(r => r.EmployeeID == empId && r.ApprovalStatus.ApprovalStatusName == approvalStatus).ToList();
            }

            if (approvalStatus.Equals("All"))
            {
                reqList = reqList.Where(r => (r.RequestedDate >= startDate && r.RequestedDate <= endDate) || r.ApprovalStatusID == 1).ToList();

            }
            else if (!approvalStatus.Equals("Unsubmitted"))
            {
                reqList = reqList.Where(r => r.RequestedDate >= startDate && r.RequestedDate <= endDate).ToList();
            }

            ViewBag.SelectedApprovalStatus = approvalStatus;
            ViewBag.EmpId = empId;
            reqList= reqList.OrderByDescending(r => r.RequisitionID).ToList();

            CombinedViewModel combinedViewModel = new CombinedViewModel();
            combinedViewModel.Requisition = reqList;
            combinedViewModel.ApprovalStatusNames = classificationService.GetApprovalStatusNames();

            return View("SearchRequisitionForm", combinedViewModel);
        }

        //Go to Screen 2.2.2.3 View Stationery Request Form
        //ViewPastRequest/ProcessedStationeryRequestForm/15
        public ActionResult ProcessedStationeryRequestForm(int id)
        {
            ServiceLayer.DataAccess.Requisition r = requisitionService.getRequisitionById(id);
            List<ServiceLayer.DataAccess.RequisitionDetail> rdList = r.RequisitionDetails.ToList();
            List<int> qtyReceived = new List<int>();
            foreach(RequisitionDetail rd in rdList)
            {
                int count = disbursementService.getTotalCountOfItemDisbursedForReqDetailId(rd.RequisitionDetailsID);
               if (disbursementService.getTotalCountOfItemDisbursedForReqDetailId(rd.RequisitionDetailsID)>0)
                {
                    qtyReceived.Add(disbursementService.getTotalCountOfItemDisbursedForReqDetailId(rd.RequisitionDetailsID));
                }
                else
                {
                    qtyReceived.Add(0);
                }
            }
            CombinedViewModel combinedViewModel = new CombinedViewModel();
            combinedViewModel.Requisitions = rdList;
            combinedViewModel.QtyReceived = qtyReceived;
            TempData["viewmodel"] = combinedViewModel;
            TempData.Keep();
            return View("ViewStationeryRequestForm", combinedViewModel);          
        }

        //Go to Screen 2.2.1.2b Resubmit Stationery Request Form
        //Pavana
        [HttpGet]
        public ActionResult EditSubmittedStationeryRequestForm(int id)
        {
            CombinedViewModel combinedViewModel = new CombinedViewModel();
            ServiceLayer.DataAccess.Requisition r = requisitionService.getRequisitionById(id);
            combinedViewModel.Requisitions = r.RequisitionDetails.ToList();
            TempData["reqID"] = r.RequisitionID;
            TempData.Keep("reqID");
            
            return View("ResubmitStationeryRequestForm", combinedViewModel);
        }

        //Pavana
        [HttpPost]
        public ActionResult ResubmitStationaryRequestForm(CombinedViewModel model)
        {
            string empId = User.Identity.GetEmployeeId();

            int reqId = (int)TempData["reqID"];
            ServiceLayer.DataAccess.Requisition req = requisitionService.getRequisitionById(reqId);
            string textBoxValue;
            for (int i = 0; i < model.Requisitions.Count; i++)
            {
                textBoxValue = model.Requisitions[i].Quantity.ToString().Trim();
                if (!(textBoxValue.Equals(null)) && !textBoxValue.Equals(""))
                {
                    requisitionService.editRequisitionDetailQty(model.Requisitions[i].RequisitionDetailsID, Convert.ToInt32(textBoxValue));
                }
            }

            requisitionService.submitRequisition(req.RequisitionID);
            return RedirectToAction("SearchRequisitionForm", new { id = empId });
          


        }

        //This is for delete in Screen 2.2.1.2b
        //Pavana
        [HttpGet]
        public ActionResult DeleteRequestedItems(int id)
        {
            int reqId = (int)TempData["reqID"];
            string empId = User.Identity.GetEmployeeId();
            RequisitionDetail reqDetail = requisitionService.getRequisitionDetailById(id);
            ServiceLayer.DataAccess.Requisition requisition = reqDetail.Requisition;

            //Delete from existing reqDetails in DB
            if (id > 0)
            {
                requisitionService.deleteRequisitionDetail(id);
            }
            
            if(requisition.RequisitionDetails.Count == 0)
            {
                requisitionService.deleteRequisition(reqDetail.RequisitionID);
                return RedirectToAction("SearchRequisitionForm", new { id = empId });
            }
            

            return RedirectToAction("EditSubmittedStationeryRequestForm", new { id = reqId});
        }

        public ActionResult ViewStationeryCatalogue()
        {
            TempData.Keep("reqID");
            CombinedViewModel combinedView = new CombinedViewModel();

            combinedView.Items = classificationService.GetItems();
            combinedView.Categories = classificationService.GetCategories();
            combinedView.categorySelected = "All";
            combinedView.AddedText = new List<string>();
            foreach (Item i in combinedView.Items)
            {
                combinedView.AddedText.Add(" ");
            }
            return View(combinedView);
        }

        [HttpGet]
        public ActionResult ViewStationeryRequestForm()
        {
            //TempData.Keep("reqID");
            CombinedViewModel combinedView = new CombinedViewModel();
            combinedView =(CombinedViewModel) TempData["viewmodel"];
            
            return View(combinedView);
        }

        [HttpPost]
        public ActionResult ViewStationeryCatalogue(CombinedViewModel model)
        {
            //getting the logged in user
            string currentLoggedInEmployeeId = User.Identity.GetEmployeeId();

            //getting requisitionID
            int reqId = (int)TempData["reqID"];
            TempData.Keep("reqID");

            //getting req for  the reqId
            ServiceLayer.DataAccess.Requisition existingReqOfEmployee = requisitionService.getRequisitionById(reqId);

            List<RequisitionDetail> existingReqDetail = requisitionService.getRequisitionDetails(reqId);

            List<string> existingReqDetailsItemIds = existingReqDetail.Select(r => r.ItemID).ToList();

            //Looping each item in view
            for (int i = 0; i < model.Items.Count; i++)
            {
                string textBoxValue = model.AddedText[i].ToString().Trim();
                string itemIdInView = model.Items[i].ItemID;
                //if item is existing item in requistion, add to quantity
                if (existingReqDetailsItemIds.Contains(itemIdInView) && (textBoxValue != null && textBoxValue != ""))
                {
                    RequisitionDetail rd =
                        existingReqOfEmployee.RequisitionDetails.First(x => x.ItemID.Equals(itemIdInView));

                    rd.Quantity += Convert.ToInt32(textBoxValue);
                    requisitionService.editRequisitionDetailQty(rd.RequisitionDetailsID, rd.Quantity);
                }
                //if item is not existing item in requistion, create new reqdetails
                else if (textBoxValue != null && textBoxValue != "")
                {

                    RequisitionDetail newRD = new RequisitionDetail();
                    newRD.RequisitionID = existingReqOfEmployee.RequisitionID;
                    requisitionService
                        .addNewRequisitionDetail(existingReqOfEmployee.RequisitionID,
                                                itemIdInView,
                                                Convert.ToInt32(textBoxValue));
                }
            }

            return RedirectToAction("EditSubmittedStationeryRequestForm", new { id = reqId });
        }

    }
}