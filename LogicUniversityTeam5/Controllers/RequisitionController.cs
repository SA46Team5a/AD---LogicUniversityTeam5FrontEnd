﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServiceLayer.DataAccess;
using ServiceLayer;
using LogicUniversityTeam5.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using LogicUniversityTeam5.IdentityHelper;

namespace LogicUniversityTeam5.Controllers.Requisition
{
    public class RequisitionController : Controller
    {
        // GET: Requisition

        IStockManagementService iStockService;
        IClassificationService iClassService;
        IRequisitionService iRequisitionService;
        IDepartmentService iDepartmentService;

        public RequisitionController(StockManagementService sms, ClassificationService cs, RequisitionService rs, DepartmentService ds)
        {
            iStockService = sms;
            iClassService = cs;
            iRequisitionService = rs;
            iDepartmentService = ds;

        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ManageRequisition()
        {
            return View();
        }

        public ActionResult ViewStationeryCatalogue()
        {
            //to remove if saved in DB is successfull
            if (TempData["passedmodel"] != null)
            {
                TempData.Keep("passedmodel");
            };

            CombinedViewModel combinedView = new CombinedViewModel();

            combinedView.Items = iClassService.GetItems();
            combinedView.Categories = iClassService.GetCategories();
            combinedView.categorySelected = "All";
            combinedView.AddedText = new List<string>();
            foreach (Item i in combinedView.Items)
            {
                combinedView.AddedText.Add(" ");
            }
            return View(combinedView);
        }


        [HttpPost]
        public ActionResult ViewStationeryCatalogue(CombinedViewModel model)
        {
            //getting the logged in user
            string currentLoggedInEmployeeId = User.Identity.GetEmployeeId();

            //to delete
            CombinedViewModel passedmodel = new CombinedViewModel();
            passedmodel.Requisitions = new List<RequisitionDetail>();
            List<RequisitionDetail> newRDlist = new List<RequisitionDetail>();

            List<RequisitionDetail> tempReqDetail = new List<RequisitionDetail>();
            passedmodel.AddedText = new List<string>();
            if (TempData["passedmodel"] != null)
            {
                CombinedViewModel cvm = (CombinedViewModel)TempData["passedmodel"];
                tempReqDetail = cvm.Requisitions;

            }

            //getting existing req for logged in user
            ServiceLayer.DataAccess.Requisition existingReqOfEmployee = iRequisitionService.getUnsubmittedRequisitionOfEmployee(currentLoggedInEmployeeId);
            //if there is no existing reqs, create a new req
            if(existingReqOfEmployee == null)
            {
                existingReqOfEmployee = iRequisitionService.createNewRequsitionForEmployee(currentLoggedInEmployeeId);
            }
            List<RequisitionDetail> existingReqDetail = iRequisitionService.getRequisitionDetails(existingReqOfEmployee.RequisitionID);

            List<string> existingReqDetailsItemIds = existingReqDetail.Select(r => r.ItemID).ToList();
            List<Item> itemsAlreadyRequested = model.Items.Where(i => existingReqDetailsItemIds.Contains(i.ItemID)).ToList();
            List<Item> newItemsForRequest = model.Items.Where(i => !existingReqDetailsItemIds.Contains(i.ItemID)).ToList();


            for (int i = 0; i < model.Items.Count; i++)
            {
                string textBoxValue = model.AddedText[i].ToString().Trim();
                string itemIdInView = model.Items[i].ItemID;
                if (existingReqDetailsItemIds.Contains(itemIdInView) && (textBoxValue != null && textBoxValue != ""))
                {
                    RequisitionDetail rd =
                        existingReqOfEmployee.RequisitionDetails.First(x => x.ItemID.Equals(itemIdInView));

                    rd.Quantity += Convert.ToInt32(textBoxValue);
                    iRequisitionService.editRequisitionDetailQty(rd.RequisitionDetailsID, rd.Quantity);
                }

                else if (textBoxValue != null && textBoxValue != "")
                {

                    RequisitionDetail newRD = new RequisitionDetail();
                    newRD.RequisitionID = existingReqOfEmployee.RequisitionID;
                    iRequisitionService
                        .addNewRequisitionDetail(existingReqOfEmployee.RequisitionID,
                                                itemIdInView,
                                                Convert.ToInt32(textBoxValue));
                }
            }
           
            return RedirectToAction("StationeryRequestForm", new { Contains = true });
        }

        [HttpGet]
        public ActionResult StationeryRequestForm()
        {
            //getting the logged in user
            string currentLoggedInEmployeeId = User.Identity.GetEmployeeId();

            //getting the existing requisitionDetails for user, passing to newModel
            CombinedViewModel newmodel = new CombinedViewModel();
            ServiceLayer.DataAccess.Requisition existingReq = 
                iRequisitionService.getUnsubmittedRequisitionOfEmployee(currentLoggedInEmployeeId);
            newmodel.Requisitions = existingReq.RequisitionDetails.ToList();

            newmodel.IsSave = false;

            return View(newmodel);

        }
        [HttpPost]
        public ActionResult StationeryRequestForm(CombinedViewModel model)
        {
            bool save = model.IsSave;
            string textBoxValue;
            //getting the logged in user
            string currentLoggedInEmployeeId = User.Identity.GetEmployeeId();
            ServiceLayer.DataAccess.Requisition req =
                iRequisitionService.getUnsubmittedRequisitionOfEmployee(currentLoggedInEmployeeId);
            for (int i = 0; i < model.Requisitions.Count; i++)
            {
                textBoxValue = model.Requisitions[i].Quantity.ToString().Trim();
                if (!(textBoxValue.Equals(null)) && !textBoxValue.Equals(""))
                {
                    iRequisitionService.editRequisitionDetailQty(model.Requisitions[i].RequisitionDetailsID, Convert.ToInt32(textBoxValue));
                }
            }
            if (save == true)
            {
                return RedirectToAction("StationeryRequestForm", new { isSave = true });
            }
            else
            {
                iRequisitionService.submitRequisition(req.RequisitionID);

                //Send email
                EmailNotificationController emailNotificationController = new EmailNotificationController((DepartmentService)iDepartmentService);
                string deptId = iDepartmentService.getDepartmentID(currentLoggedInEmployeeId);
                emailNotificationController.SendEmailToDeptHeadToApproveRequisitions(deptId, req.RequisitionID);

                return RedirectToAction("Index", "Home");
            }

        }


        [HttpGet]
        public ActionResult ResubmitStationaryRequestForm()
        {
            CombinedViewModel combinedView = new CombinedViewModel();
            combinedView.AddedText = new List<string>();

            //getting the logged in user
            string currentLoggedInEmployeeId = User.Identity.GetEmployeeId();
            ServiceLayer.DataAccess.Requisition req = iRequisitionService.getUnsubmittedRequisitionOfEmployee(currentLoggedInEmployeeId);

            List<RequisitionDetail> reqdetails = iRequisitionService.getRequisitionDetails(req.RequisitionID);
            combinedView.Requisitions = reqdetails;
            //for (int i = 0; i < reqdetails.Count; i++)
            //{
            //    combinedView.AddedText.Add(Convert.ToString(reqdetails[i].Quantity));
            //}
            TempData["passmodel"] = combinedView;
            return View(combinedView);
        }

        [HttpPost]
        public ActionResult ResubmitStationaryRequestForm(CombinedViewModel model)
        {
            ServiceLayer.DataAccess.Requisition req = iRequisitionService.getUnsubmittedRequisitionOfEmployee("E001");
            string textBoxValue;
            for (int i = 0; i < model.Requisitions.Count; i++)
            {

                textBoxValue = model.Requisitions[i].Quantity.ToString().Trim();
                if (!(textBoxValue.Equals(null)) && !textBoxValue.Equals(""))
                {
                    iRequisitionService.editRequisitionDetailQty(model.Requisitions[i].RequisitionDetailsID, Convert.ToInt32(textBoxValue));
                }
            }

            iRequisitionService.submitRequisition(req.RequisitionID);
            return RedirectToAction("ResubmitStationaryRequestForm", new { isSubmit = true });
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            //Delete from existing reqDetails in DB
            if (id > 0)
            {
                iRequisitionService.deleteRequisitionDetail(id);
            }

            return RedirectToAction("StationeryRequestForm");
        }


    }


}