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
           // ServiceLayer.DataAccess.Requisition  req =   iRequisitionService.getUnsubmittedRequisitionOfEmployee("E001");
            //List<RequisitionDetail> reqdetails = (List<RequisitionDetail>) req.RequisitionDetails;

           // combinedView.Items = iStockService.getAllItems();

            combinedView.StockCountItems = iStockService.getAllStockCountItem();

            combinedView.Categories = iClassService.GetCategories();
            combinedView.AddedText = new List<string>();
            foreach (StockCountItem sci in combinedView.StockCountItems)
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
            passedmodel.StockCountItems = new List<StockCountItem>();
            passedmodel.AddedText = new List<string>();

            for (int i = 0; i < model.AddedText.Count; i++)
            {
                string textBoxValue = model.AddedText[i].ToString();
                string trimmed = textBoxValue.Trim();

                if (!(trimmed.Equals(null)) && !trimmed.Equals(""))
                {                    
                    StockCountItem sci = new StockCountItem()
                    {
                        ItemID = model.StockCountItems[i].ItemID,
                        ItemName = model.StockCountItems[i].ItemName,
                        UnitOfMeasure = model.StockCountItems[i].UnitOfMeasure
                    };
                    passedmodel.StockCountItems.Add(sci);
                    passedmodel.AddedText.Add(trimmed);
                  
                }


            }
            TempData["passedmodel"] = passedmodel;    
            return RedirectToAction("StationeryRequestForm");
        }

        [HttpGet]
        public ActionResult StationeryRequestForm()
        {
            
            CombinedViewModel newmodel = (CombinedViewModel)TempData["passedmodel"];
            newmodel.IsSave = false;
            return View(newmodel);

        }
        [HttpPost]
        public ActionResult StationeryRequestForm(CombinedViewModel model)
        {
            bool save = model.IsSave;
            ServiceLayer.DataAccess.Requisition req = iRequisitionService.createNewRequsitionForEmployee("E001");
            for (int i = 0; i < model.StockCountItems.Count; i++)
            {
                iRequisitionService.addNewRequisitionDetail(req.RequisitionID, model.StockCountItems[i].ItemID, Convert.ToInt32(model.AddedText[i]));
            }
            iRequisitionService.submitRequisition(req.RequisitionID);
            return RedirectToAction("Index","Home");
        }
        

    }


}