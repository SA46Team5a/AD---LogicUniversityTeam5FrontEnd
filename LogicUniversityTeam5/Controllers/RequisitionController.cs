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
        public RequisitionController(StockManagementService sms, ClassificationService cs,RequisitionService rs)
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
            CombinedViewModel combinedView = new CombinedViewModel();
           

            combinedView.Items = iStockService.getAllItems();         

            combinedView.Categories = iClassService.GetCategories();
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
            // Dictionary<string,int> itemIdList = new Dictionary<string, int>();
            CombinedViewModel passedmodel = new CombinedViewModel();
            passedmodel.Requisitions = new List<RequisitionDetail>();
            List<RequisitionDetail> newRDlist = new List<RequisitionDetail>();
            List<string> itemidList = new List<string>();
            passedmodel.AddedText = new List<string>();
            ServiceLayer.DataAccess.Requisition req = iRequisitionService.getUnsubmittedRequisitionOfEmployee("E001");
            List<RequisitionDetail> reqDetail;
            if (req != null)
            {
                reqDetail = iRequisitionService.getRequisitionDetails(req.RequisitionID);
                List<string> reqDetailsItem = reqDetail.Select(r => r.ItemID).ToList();
                List<Item> itemsAlreadyRequested = model.Items.Where(i => reqDetailsItem.Contains(i.ItemID)).ToList();
                List<Item> newItemsForRequest = model.Items.Where(i => !reqDetailsItem.Contains(i.ItemID)).ToList();
                    
                for(int i=0;i<model.Items.Count;i++)
                {
                    string textBoxValue = model.AddedText[i].ToString().Trim();
                    if (reqDetailsItem.Contains(model.Items[i].ItemID) && (textBoxValue != null && textBoxValue != ""))
                    {
                        //string textBoxValue = model.AddedText[i].ToString().Trim();
                        RequisitionDetail rd =
                            req.RequisitionDetails.First(x => x.ItemID.Equals(model.Items[i].ItemID));
                        rd.Quantity += Convert.ToInt32(textBoxValue);
                        passedmodel.Requisitions.Add(rd);
                    }
                    else
                    {
                       //string textBoxValue = model.AddedText[i].ToString().Trim();
                        if (textBoxValue != null && textBoxValue != "")
                        {
                            RequisitionDetail newRD = new RequisitionDetail();
                            newRD.RequisitionID = req.RequisitionID;
                            newRD.RequisitionDetailsID = -1; //Still not added to database.
                            newRD.ItemID = model.Items[i].ItemID;
                            newRD.Quantity = Convert.ToInt32(textBoxValue);
                            newRD.Item = new Item();
                            newRD.Item.ItemName = model.Items[i].ItemName;
                            newRD.Item.UnitOfMeasure = model.Items[i].UnitOfMeasure;

                            passedmodel.Requisitions.Add(newRD);
                        }
                        
                    }
                }
                //foreach (RequisitionDetail rd in reqDetail)
                //{
                //    for(int i=0;i<model.AddedText.Count;i++)
                //    {
                //        string textBoxValue = model.AddedText[i].ToString().Trim();
                //        if (!textBoxValue.Equals(null) || !textBoxValue.Equals(""))
                //        {
                //            if (model.Items[i].ItemID.Equals(rd.ItemID)){
                //                rd.Quantity = rd.Quantity + Convert.ToInt32(textBoxValue);
                //                passedmodel.Requisitions.Add(rd);
                //            }
                //            else if(!itemidList.Contains(model.Items[i].ItemID))
                //            {
                //                RequisitionDetail newRD = new RequisitionDetail();
                //                newRD.RequisitionID = req.RequisitionID;
                //                newRD.RequisitionDetailsID = - 1; //Still not added to database.
                //                newRD.ItemID = model.Items[i].ItemID;
                //                newRD.Quantity = Convert.ToInt32(model.AddedText[i]);
                //                newRD.Item = new Item();
                //                newRD.Item.ItemName = model.Items[i].ItemName;
                //                newRD.Item.UnitOfMeasure = model.Items[i].UnitOfMeasure;
                //                itemidList.Add(newRD.ItemID);
                //                newRDlist.Add(newRD);
                //            }
                //        }

                //     }
                    

                // }
                }

            //for (int i = 0; i < model.AddedText.Count; i++)
            //{
            //    string textBoxValue = model.AddedText[i].ToString();
            //    string trimmed = textBoxValue.Trim();

            //    if (!(trimmed.Equals(null)) && !trimmed.Equals(""))
            //    {                    
            //        Item item = new Item()
            //        {
            //            ItemID = model.Items[i].ItemID,
            //            ItemName = model.Items[i].ItemName,
            //            UnitOfMeasure = model.Items[i].UnitOfMeasure
            //        };

            //passedmodel.Items.Add(item);
            //passedmodel.AddedText.Add(trimmed);

            //    }


            //}
           

            TempData["passedmodel"] = passedmodel;
            TempData.Keep("passedmodel");
            return RedirectToAction("StationeryRequestForm");
        }

        [HttpGet]
        public ActionResult StationeryRequestForm()
        {
            CombinedViewModel newmodel = (CombinedViewModel)TempData["passedmodel"];
            TempData.Keep("passedmodel");

            //ServiceLayer.DataAccess.Requisition req = iRequisitionService.getUnsubmittedRequisitionOfEmployee("E001");
            //List<RequisitionDetail> reqDetail ;
            
            //if (req != null)
            //{
            //    reqDetail = iRequisitionService.getRequisitionDetails(req.RequisitionID);
            //    if (reqDetail != null)
            //    {
            //            newmodel.Requisitions=reqDetail;      
            //    }      
            //}
           
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
                    else
                    {
                        iRequisitionService.editRequisitionDetailQty(model.Requisitions[i].RequisitionDetailsID, Convert.ToInt32(textBoxValue));
                    }
                }
            }
            if(save==true)
            {
               TempData["passedmodel"] = model;
               return RedirectToAction("StationeryRequestForm");
            }
            else
            {
                iRequisitionService.submitRequisition(req.RequisitionID);
                return RedirectToAction("Index", "Home");
            }
         //   iRequisitionService.submitRequisition(req.RequisitionID);
            
        }


        [HttpGet]
        public ActionResult ResubmitStationaryRequestForm()
        {
            CombinedViewModel combinedView = new CombinedViewModel();
            combinedView.AddedText = new List<string>();
            ServiceLayer.DataAccess.Requisition  req =   iRequisitionService.getUnsubmittedRequisitionOfEmployee("E001");        
            int count = req.RequisitionID;
            List<RequisitionDetail> reqdetails = iRequisitionService.getRequisitionDetails(req.RequisitionID);
            combinedView.Requisitions = reqdetails;
            for(int i=0;i<reqdetails.Count;i++)
            {
                combinedView.AddedText.Add(Convert.ToString(reqdetails[i].Quantity));
            }

            return View(combinedView);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            iRequisitionService.deleteRequisitionDetail(id);
            //CombinedViewModel combinedView = new CombinedViewModel();
            CombinedViewModel combinedView = (CombinedViewModel)TempData["passedmodel"];
          //  combinedView.AddedText = new List<string>();
            ServiceLayer.DataAccess.Requisition req = iRequisitionService.getUnsubmittedRequisitionOfEmployee("E001");
           // int count = req.RequisitionID;
            List<RequisitionDetail> reqdetails = iRequisitionService.getRequisitionDetails(req.RequisitionID);
            combinedView.Requisitions = reqdetails;
            for (int i = 0; i < reqdetails.Count; i++)
            {
                
                combinedView.AddedText.Add(Convert.ToString(reqdetails[i].Quantity));
            }
            TempData["passedmodel"] = combinedView;
            return RedirectToAction("StationeryRequestForm");
        }
           

    }


}