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
        // GET: TrackOutstandingRequisition
        public ActionResult TrackOutstandingRequisition()
        {
            CombinedViewModel combinedViewModel = new CombinedViewModel();
            combinedViewModel.OutstandingRequisitionViews = context.OutstandingRequisitionViews.ToList();
            combinedViewModel.Departments = departmentService.getDepartments();
            combinedViewModel.RequisitionDetails = new List<RequisitionDetail>();

            //Linking each ReqDetail to OrderSupplierDetail
            Dictionary<int, List<OrderSupplierDetail>> ReqDetailIdAndOrderSupplierId = new Dictionary<int, List<OrderSupplierDetail>>();

            //Adding OrderSupplierDetails for each RequisitionDetails to combinedViewModel.OrderSupplierDetails
            combinedViewModel.OrderSupplierDetails = new List<OrderSupplierDetail>();

            combinedViewModel.OutstandingRequisitionViews.ForEach(orv => {
                combinedViewModel.OrderSupplierDetails
                    .AddRange(orderService.getOrdersServingOutstandingRequisitions(orv.RequisitionDetailsID));

                //Linking each ReqDetail to OrderSupplierDetail
                ReqDetailIdAndOrderSupplierId.Add(orv.RequisitionDetailsID, orderService.getOrdersServingOutstandingRequisitions(orv.RequisitionDetailsID));
            });


            // Match size of combinedViewModel.RequisitionDetails and combinedViewModel.OrderSupplierDetails
            foreach (KeyValuePair <int, List<OrderSupplierDetail>> entry in ReqDetailIdAndOrderSupplierId)
            {
                List<OrderSupplierDetail> orderSupplierDetails = entry.Value;
                foreach (OrderSupplierDetail osd in orderSupplierDetails)
                {
                    combinedViewModel.RequisitionDetails.Add(context.RequisitionDetails.First(rd => rd.RequisitionDetailsID == entry.Key));
                }
            }

            return View(combinedViewModel);
        }

        [HttpPost]
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