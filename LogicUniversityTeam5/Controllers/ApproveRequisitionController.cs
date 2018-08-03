﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LogicUniversityTeam5.IdentityHelper;
using LogicUniversityTeam5.Models;
using ServiceLayer;
using ServiceLayer.DataAccess;

namespace LogicUniversityTeam5
{
    [Authorize(Roles = "Department Head, Delegate")]
    public class ApproveRequisitionController : Controller
    {
        // GET: approveRequisitionForm
        StationeryStoreEntities context = StationeryStoreEntities.Instance;
        IRequisitionService requisitionService;
        IDepartmentService departmentService;
        IStockManagementService stockManagementService;
        public ApproveRequisitionController(RequisitionService rs, DepartmentService ds)
        {
            requisitionService = rs;
            departmentService = ds;
        }

        public ActionResult ApproveRequisitionForm()
        {
            string currentLoggedInEmployeeId = User.Identity.GetEmployeeId();
            string deptID = departmentService.getDepartmentID(currentLoggedInEmployeeId);

            SpecialModel model = new SpecialModel();
            model.specialmodel = new List<CombinedViewModel>();
            CombinedViewModel smallmodel = new CombinedViewModel();
            smallmodel.Requisition = new List<Requisition>();
            smallmodel.Requisition = requisitionService.getPendingRequisitionsOfDep(deptID);
            //smallmodel.Requisition = context.Requisitions.Where(x => x.ApprovalStatusID == 2).ToList();

            for (int i = 0; i < smallmodel.Requisition.Count; i++)
            {
                var value = smallmodel.Requisition[i].RequisitionID;

                CombinedViewModel innerModel = new CombinedViewModel();
                innerModel.Requisition = new List<Requisition>();

                Requisition requisition = requisitionService.getRequisitionById(value);

                if(requisition.RequisitionDetails.Count > 0)
                {
                    innerModel.Requisition.Add(requisition);
                    innerModel.Employee = new List<Employee>();
                    string employeeid = requisition.EmployeeID;
                    Employee employee = departmentService.getEmployeeById(employeeid);
                    innerModel.Employee.Add(employee);
                    innerModel.Details = new List<RequisitionDetail>();
                    innerModel.Items = new List<Item>();
                    List<string> itemid = new List<string>();

                    innerModel.Details = requisitionService.getRequisitionDetails(value);

                    for (int m = 0; m < innerModel.Details.Count; m++)
                    {
                        string itemidelement = innerModel.Details[m].ItemID;
                        itemid.Add(itemidelement);

                    }
                    for (int m = 0; m < itemid.Count; m++)
                    {
                        string innervalue = itemid[m];
                        Item item = context.Items.First(z => z.ItemID == innervalue);
                        //Item item = stockManagementService.getItemById(innervalue) ;
                        innerModel.Items.Add(item);
                    }
                    model.specialmodel.Add(innerModel);
                }
                
            }

            return View(model);
        }
        [HttpPost]
        public ActionResult ApproveRequisitionForm(SpecialModel model, int? Approve, int? Reject)
        {
            string empid = User.Identity.GetEmployeeId();

            if (Approve != null)
            {
                    int reqid = (int) Approve;
                    bool toApprove = true;
                    requisitionService.processRequisition(reqid, empid, toApprove, departmentService);
            }
            if(Reject != null)
            {
                for(int i = 0; i<model.specialmodel.Count; i++)
                {
                    int reqid = (int) Reject;
                    bool toApprove = false;
                    requisitionService.processRequisition(reqid, empid, toApprove, departmentService);
                }

            }

            return RedirectToAction("ApproveRequisitionForm");
        }
    }
}