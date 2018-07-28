using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServiceLayer.DataAccess;
using ServiceLayer;
using LogicUniversityTeam5.Models;

namespace LogicUniversityTeam5.Controllers.Requisition
{
    public class RequisitionController : Controller
    {
        // GET: Requisition

        IStockManagementService iStockService;
        IClassificationService iClassService;
        IRequisitionService iRequisitionService;

        public RequisitionController(StockManagementService sms, ClassificationService cs, RequisitionService rs)
        {
            iStockService = sms;
            iClassService = cs;
            iRequisitionService = rs;

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

        //[HttpPost]
        //public ActionResult ViewStationeryCatalogue(CombinedViewModel model,string categoryName)
        //{
        //    if (TempData["passedmodel"] != null)
        //    {
        //        TempData.Keep("passedmodel");
        //    };

        //}

        [HttpPost]
        public ActionResult ViewStationeryCatalogue(CombinedViewModel model)
        {
            // Dictionary<string,int> itemIdList = new Dictionary<string, int>();
            CombinedViewModel passedmodel = new CombinedViewModel();
            passedmodel.Requisitions = new List<RequisitionDetail>();
            List<RequisitionDetail> newRDlist = new List<RequisitionDetail>();
            List<string> itemidList = new List<string>();
            List<RequisitionDetail> tempReqDetail = new List<RequisitionDetail>();
            passedmodel.AddedText = new List<string>();
            if (TempData["passedmodel"] != null)
            {
                CombinedViewModel cvm = (CombinedViewModel)TempData["passedmodel"];
                tempReqDetail = cvm.Requisitions;

            }
            ServiceLayer.DataAccess.Requisition req = iRequisitionService.getUnsubmittedRequisitionOfEmployee("E001");
            List<RequisitionDetail> reqDetail;
            reqDetail = iRequisitionService.getRequisitionDetails(req.RequisitionID);
            List<string> reqDetailsItem = reqDetail.Select(r => r.ItemID).ToList();
            List<Item> itemsAlreadyRequested = model.Items.Where(i => reqDetailsItem.Contains(i.ItemID)).ToList();
            List<Item> newItemsForRequest = model.Items.Where(i => !reqDetailsItem.Contains(i.ItemID)).ToList();


            for (int i = 0; i < model.Items.Count; i++)
            {
                string textBoxValue = model.AddedText[i].ToString().Trim();
                if (reqDetailsItem.Contains(model.Items[i].ItemID) && (textBoxValue != null && textBoxValue != ""))
                {

                    RequisitionDetail rd =
                        req.RequisitionDetails.First(x => x.ItemID.Equals(model.Items[i].ItemID));
                    rd.Quantity += Convert.ToInt32(textBoxValue);


                    passedmodel.Requisitions.Add(rd);
                }

                else if (textBoxValue != null && textBoxValue != "")
                {


                    RequisitionDetail newRD = new RequisitionDetail();
                    newRD.RequisitionID = req.RequisitionID;
                    newRD.RequisitionDetailsID = -(i + 1); //Still not added to database.
                    newRD.ItemID = model.Items[i].ItemID;
                    newRD.Quantity = Convert.ToInt32(textBoxValue);
                    newRD.Item = new Item();
                    newRD.Item.ItemName = model.Items[i].ItemName;
                    newRD.Item.UnitOfMeasure = model.Items[i].UnitOfMeasure;

                    passedmodel.Requisitions.Add(newRD);
                }
            }
            foreach (Item i in itemsAlreadyRequested)
            {
                if (!newItemsForRequest.Contains(i))
                {
                    RequisitionDetail oldReq = req.RequisitionDetails.First(x => x.ItemID.Equals(i.ItemID));
                    passedmodel.Requisitions.Add(oldReq);
                }
            }


            TempData["passedmodel"] = passedmodel;
            TempData.Keep("passedmodel");
            return RedirectToAction("StationeryRequestForm", new { Contains = true });
        }

        [HttpGet]
        public ActionResult StationeryRequestForm()
        {
            CombinedViewModel newmodel = (CombinedViewModel)TempData["passedmodel"];
            TempData.Keep("passedmodel");

            newmodel.IsSave = false;
            return View(newmodel);

        }
        [HttpPost]
        public ActionResult StationeryRequestForm(CombinedViewModel model)
        {
            bool save = model.IsSave;
            string textBoxValue;
            ServiceLayer.DataAccess.Requisition req = iRequisitionService.getUnsubmittedRequisitionOfEmployee("E001");
            for (int i = 0; i < model.Requisitions.Count; i++)
            {
                textBoxValue = model.Requisitions[i].Quantity.ToString().Trim();
                if (!(textBoxValue.Equals(null)) && !textBoxValue.Equals(""))
                {
                    if (model.Requisitions[i].RequisitionDetailsID < 0)
                    {
                        iRequisitionService.addNewRequisitionDetail(req.RequisitionID, model.Requisitions[i].ItemID, Convert.ToInt32(textBoxValue));
                    }
                    else if (model.Requisitions[i].RequisitionDetailsID >= 0)
                    {
                        iRequisitionService.editRequisitionDetailQty(model.Requisitions[i].RequisitionDetailsID, Convert.ToInt32(textBoxValue));
                    }
                }
            }
            if (save == true)
            {
                TempData["passedmodel"] = model;
                return RedirectToAction("StationeryRequestForm", new { isSave = true });

            }
            else
            {
                iRequisitionService.submitRequisition(req.RequisitionID);
                return RedirectToAction("Index", "Home");
            }

        }


        [HttpGet]
        public ActionResult ResubmitStationaryRequestForm()
        {
            CombinedViewModel combinedView = new CombinedViewModel();
            combinedView.AddedText = new List<string>();
            ServiceLayer.DataAccess.Requisition req = iRequisitionService.getUnsubmittedRequisitionOfEmployee("E001");

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

            CombinedViewModel combinedView = (CombinedViewModel)TempData["passedmodel"];
            if (combinedView != null)
            {
                List<RequisitionDetail> rDetailList = combinedView.Requisitions;

                foreach (RequisitionDetail rd in rDetailList.ToList())
                {
                    if (rd.RequisitionDetailsID > 0 || rd.RequisitionDetailsID == id)
                        rDetailList.Remove(rd);

                }
                combinedView.Requisitions = rDetailList;
            }
            else
            {
                combinedView = new CombinedViewModel();
                combinedView.AddedText = new List<string>();
                combinedView.Requisitions = new List<RequisitionDetail>();

            }

            //Delete from existing reqDetails in DB
            if (id > 0)
            {
                iRequisitionService.deleteRequisitionDetail(id);
            }


            ServiceLayer.DataAccess.Requisition req = iRequisitionService.getUnsubmittedRequisitionOfEmployee("E001");
            List<RequisitionDetail> reqdetails = iRequisitionService.getRequisitionDetails(req.RequisitionID);

            for (int i = 0; i < reqdetails.Count; i++)
            {
                combinedView.Requisitions.Add(reqdetails[i]);
            }
            TempData["passedmodel"] = combinedView;
            return RedirectToAction("StationeryRequestForm");
        }


    }


}