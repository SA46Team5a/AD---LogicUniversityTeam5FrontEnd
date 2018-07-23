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
        public RequisitionController(StockManagementService sms, ClassificationService cs)
        {
            iStockService = sms;
            iClassService = cs;
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

            combinedView.StockCountItems = iStockService.getAllStockCountItem();

            combinedView.Categories = iClassService.GetCategories();
            combinedView.AddedText = new List<string>();
            foreach (StockCountItem sci in combinedView.StockCountItems)
            {
                combinedView.AddedText.Add(" ");
            }

            return View(combinedView);
        }

        //public ActionResult ViewStationeryCatalogue()
        //{

        //    List<StockCountItem> stockcountitemList = iStockService.getAllStockCountItem();
        //    List<Category> catList = iClassService.GetCategories();
        //    CombinedViewModel combinedViewModel = new CombinedViewModel();

        //    ViewBag.dropdowncat = new SelectList(catList, "categoryId", "categoryName");
        //    ViewBag.IsSelected = new List<bool>();

        //    return View(stockcountitemList);
        //}

        [HttpPost]
        public ActionResult ViewStationeryCatalogue(CombinedViewModel model)
        {
            // Dictionary<string,int> itemIdList = new Dictionary<string, int>();
            CombinedViewModel model1 = new CombinedViewModel();
            model1.StockCountItems = new List<StockCountItem>();
            model1.AddedText = new List<string>();

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
                    model1.StockCountItems.Add(sci);

                    // model1.AddedText[i]=trimmed;
                    model1.AddedText.Add(trimmed);
                    //model1.AddedText[count++] = model.AddedText[i];
                }


            }
            TempData["model1"] = model1;
           

            return RedirectToAction("UnsubmittedStationeryRequestForm");
        }

        [HttpGet]
        public ActionResult UnsubmittedStationeryRequestForm()
        {
            
            CombinedViewModel newmodel = (CombinedViewModel)TempData["model1"];           
            return View(newmodel);

        }
        [HttpPost]
        public ActionResult UnsubmittedStationeryRequestForm(CombinedViewModel model)
        {
           
            return View(model);
        }
        

    }


}