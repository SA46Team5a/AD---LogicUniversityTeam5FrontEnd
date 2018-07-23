using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LogicUniversityTeam5.Controllers
{
    //Author: Benedict
    public class UtilitiesApiController : ApiController
    {

        //[HttpGet]
        //[Route("api/UtilitiesApi/ping")]
        //public string ping()
        //{
        //    /*
        //    string[] roles = new string[] { "Employee", "Department Head", "Department Representative",
        //                                    "Delegate", "Store Clerk", "Store Supervisor", "Store Manager" };

        //    List<string> stringlist = new List<string>();
        //    foreach (string empRole in roles)
        //    {
        //        stringlist.Add(empRole);
        //    }

        //    return stringlist;*/
        //    return "pong";

        //}

        //GET: api/UtilitiesApi
        [HttpGet]
        [Route("api/UtilitiesApi/GetCollectionPoints")]
        public IEnumerable<CollectionPoint> GetCollectionPoints()
        {
            return IDepartmentService.GetCollectionPoints();
        }

        // GET: api/UtilitiesApi/5
        [HttpGet]
        [Route("api/UtilitiesApi/GetCollectionPointOfDepartment/{id}")]
        public CollectionPoint GetCollectionPointOfDepartment(string id)
        {
            return IDepartmentService.GetCollectionPointOfDepartment;
        }

        // GET: api/UtilitiesApi/5
        [Authorize(Roles = "Department Representative")]
        [HttpGet]
        [Route("api/UtilitiesApi/GetPasscode/{id}")]
        public string GetPasscode(string id)
        {
            var employee = IDepartmentService.GetEmployeeById(id);
            return employee.DepartmentRepresentative.Disbursement.Passcode;
            //don't do tracing, ask service to find passcode
        }

        // POST: api/UtilitiesApi
        //[HttpPost]
        //[Route("api/UtilitiesApi/ChangeCollectionPoint")]
        //public void ChangeCollectionPoint([FromBody]string value)
        //public void ChangeCollectionPoint(DepartmentModel model)
        [HttpGet]
        [Route("api/UtilitiesApi/ChangeCollectionPoint/{deptId}/{collectionPointId}")]
        public void ChangeCollectionPoint(string deptId, int collectionPointId)
        {
            IDepartmentService.UpdateCollectionPoint(deptId, collectionPointId);
        }

    }
}
