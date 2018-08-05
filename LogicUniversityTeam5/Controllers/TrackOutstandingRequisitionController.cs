using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServiceLayer;
using ServiceLayer.DataAccess;
using LogicUniversityTeam5.Models;

namespace LogicUniversityTeam5.Controllers
{
    [Authorize(Roles = "Store Clerk , Store Supervisor")]
    public class TrackOutstandingRequisitionController : Controller
    {
        StationeryStoreEntities context = StationeryStoreEntities.Instance;
        IDepartmentService departmentService;
        IOrderService orderService;
        public TrackOutstandingRequisitionController(DepartmentService ds, OrderService os)
        {
            departmentService = ds;
            orderService = os;
        }
        

        public ActionResult TrackOutstandingRequisition()
        {
            CombinedViewModel combinedViewModel = new CombinedViewModel();
            combinedViewModel.Departments = departmentService.getDepartments();
            combinedViewModel.OutstandingRequisitionRows = orderService.getOutstandingRequisitionRows();
               
            return View(combinedViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TrackOutstandingRequisition(CombinedViewModel model)
        {
            //getting the list of itemsId and itemQty from view
            List<string> itemIds = model.RequisitionDetails.Select(x => x.Item.ItemID).ToList();
            List<int?> itemQty = model.OutstandingRequisitionViews.Select(x => x.OutStandingQuantity).ToList();

            for(int i= model.OrderSupplierDetails.Count-1; i>=0; i--)
            {
                if (model.OrderSupplierDetails[i].OrderSupplierDetailsID != 0)
                {
                    //remove from itemIds and itemQty if OrderSupplierDetails already exist
                    itemIds.RemoveAt(i);
                    itemQty.RemoveAt(i);
                }
            }

            if(itemIds.Count > 0)
            {
                TempData["itemIds"] = itemIds;
                TempData["itemQty"] = itemQty;

                return RedirectToAction("OrderQuantity", "CreateOrders");
            }
            else
            {
                //no outstanding requisitions, return back to same page
                return RedirectToAction("TrackOutstandingRequisition");
            }
            
        }
    }
}