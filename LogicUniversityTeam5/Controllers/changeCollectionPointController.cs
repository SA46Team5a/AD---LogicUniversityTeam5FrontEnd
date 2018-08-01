using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LogicUniversityTeam5.IdentityHelper;
using LogicUniversityTeam5.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using ServiceLayer;
using ServiceLayer.DataAccess;


namespace LogicUniversityTeam5.Controllers
{
    [Authorize(Roles = "Department Representative")]
    public class ChangeCollectionPointController : Controller
    {
        IDepartmentService departmentService;
        IClassificationService classificationService;
        static StationeryStoreEntities context = StationeryStoreEntities.Instance;

        public ChangeCollectionPointController(DepartmentService ds,ClassificationService cs)
        {
            departmentService = ds;
            classificationService = cs;
        }
        // GET: CollectionPoint
        public ActionResult ChangeCollectionPoint(Department dep)
        {

            CombinedViewModel combinedView = new CombinedViewModel();
            combinedView.CollectionPoint = classificationService.GetCollectionPoints();

            string empId = User.Identity.GetEmployeeId();
            string deptId = departmentService.getDepartmentID(empId);
            combinedView.DepartmentRepresentative = new List<DepartmentRepresentative>();            
            DepartmentRepresentative departmentRepresentative = departmentService.getCurrentDepartmentRepresentative(deptId);

            combinedView.DepartmentRepresentative.Add(departmentRepresentative);
            string currentCollectionPoint = departmentService.getCollectionPointOfEmployee(empId).CollectionPointDetails;

            //AddedText[0] for checking radio button is selected, AddedText[1] to display current CollectionPointDetails
            combinedView.AddedText = new List<string>(2) { "", currentCollectionPoint };         
            return View(combinedView);
        }


        [HttpPost]
        public ActionResult ChangeCollectionPoint(CombinedViewModel model)
        {
            List<CollectionPoint> collectionPoint = model.CollectionPoint;
            List<bool> isSelected = model.IsSelected;

            //get empid from the login session
            string empId = User.Identity.GetEmployeeId();

            string deptID = departmentService.getDepartmentID(empId);
            string deptName = departmentService.getDepartments()
                                .First(x => x.DepartmentID == deptID)
                                .DepartmentName;

            for (int i=0;i<model.AddedText.Count;i++)
            {
                if(model.AddedText[0] != null)
                {
                    departmentService.updateCollectionPoint(deptID, Convert.ToInt32(model.AddedText[0]));
                    EmailNotificationController.SendEmailForChangeCollectionPoint(deptName, model.AddedText[0]);
                }
            }
            
            return RedirectToAction("ChangeCollectionPoint", "ChangeCollectionPoint",new { isCollectionPointChanged = true});
         
        }
    }
}