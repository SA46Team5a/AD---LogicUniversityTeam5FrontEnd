using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LogicUniversityTeam5.Models;
using ServiceLayer;
using ServiceLayer.DataAccess;

namespace LogicUniversityTeam5
{
    public class ApproveRequisitionController : Controller
    {
        // GET: approveRequisitionForm
        StationeryStoreEntities context = StationeryStoreEntities.Instance;
        IRequisitionService requisitionService;

        public ApproveRequisitionController(RequisitionService rs)
        {
            requisitionService = rs;
        }

        public ActionResult ApproveRequisitionForm()
        {
            SpecialModel model = new SpecialModel();
            model.specialmodel = new List<CombinedViewModel>();
            CombinedViewModel smallmodel = new CombinedViewModel();
            smallmodel.Requisition = new List<Requisition>();
            smallmodel.Requisition = context.Requisitions.Where(x => x.ApprovalStatusID == 2).ToList();

            for (int i = 0; i < smallmodel.Requisition.Count; i++)
            {
                var value = smallmodel.Requisition[i].RequisitionID;

                CombinedViewModel innerModel = new CombinedViewModel();
                innerModel.Requisition = new List<Requisition>();
                Requisition requisition = context.Requisitions.First(l => l.RequisitionID == value);
                innerModel.Requisition.Add(requisition);
                innerModel.Employee = new List<Employee>();
                string employeeid = requisition.EmployeeID;
                Employee employee = context.Employees.First(x => x.EmployeeID == employeeid);
                innerModel.Employee.Add(employee);
                innerModel.Details = new List<RequisitionDetail>();
                innerModel.Items = new List<Item>();
                List<string> itemid = new List<string>();
                innerModel.Details = context.RequisitionDetails.Where(x => x.RequisitionID == value).ToList();


                for (int m = 0; m < innerModel.Details.Count; m++)
                {
                    string itemidelement = innerModel.Details[m].ItemID;
                    itemid.Add(itemidelement);

                }
                for (int m = 0; m < itemid.Count; m++)
                {
                    string innervalue = itemid[m];
                    Item item = context.Items.First(z => z.ItemID == innervalue);
                    innerModel.Items.Add(item);
                }
                model.specialmodel.Add(innerModel);
            }

            return View(model );
        }
        [HttpPost]
        public ActionResult ApproveRequisitionForm(SpecialModel model, string Approve, string Reject)
        {
            if (Approve != null)
            {
                for (int i = 0; i < model.specialmodel.Count; i++)
                {
                    CombinedViewModel model1 = model.specialmodel[i];
                    int reqid = model1.Requisition[0].RequisitionID;
                    string empid = model1.Requisition[0].EmployeeID;
                    bool toApprove = true;
                    requisitionService.processRequisition(reqid, empid, toApprove);

                }
            }
            if(Reject!=null)
            {
                for(int i = 0; i<model.specialmodel.Count; i++)
                {
                    CombinedViewModel model1 = model.specialmodel[i];
                    int reqid = model1.Requisition[0].RequisitionID;
                    string empid = model1.Requisition[0].EmployeeID;
                    bool toApprove = false;
                    requisitionService.processRequisition(reqid, empid, toApprove);
                }


            }

            return RedirectToAction("ApproveRequisitionForm");
        }
    }
}