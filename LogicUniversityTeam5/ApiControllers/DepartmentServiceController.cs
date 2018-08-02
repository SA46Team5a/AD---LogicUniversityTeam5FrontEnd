using LogicUniversityTeam5.Models;
using ServiceLayer;
using ServiceLayer.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LogicUniversityTeam5.ApiControllers
{
    // Author: Jack
    public class DepartmentServiceController : ApiController
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentServiceController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        [Route("api/department/employees/{id}")]
        public List<EmployeePayload> getEmployeesOfDepartment(string id)
            => EmployeePayload.ConvertEntityToPayload(_departmentService.getEmployeesOfDepartment(id));

        // Authority
        [HttpGet]
        [Route("api/authority/{id}")]
        public AuthorityPayload getCurrentAuthorityOfDepartment(string id)
        {
            Authority a = _departmentService.getDelegatedAuthority(id);
            if (a is null)
                return null;
            else
                return new AuthorityPayload(a);
        }


        [HttpGet]
        [Route("api/authority/employees/{deptId}")]
        public List<EmployeePayload> getEligibleDelegatedAuthorities(string deptId)
            => EmployeePayload.ConvertEntityToPayload(_departmentService.getEligibleDelegatedAuthority(deptId));

        [HttpPost]
        [Route("api/authority/new")]
        public bool addNewAuthority(Authority a)
        {
            Employee emp = _departmentService.getEmployeeById(a.EmployeeID);
            try
            {
                _departmentService.addAuthority(emp, a.StartDate, (DateTime) a.EndDate);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [HttpPost]
        [Route("api/authority/update")]
        public bool updateAuthority(Authority a)
        {
            try
            {
                _departmentService.updateAuthority(a);
                return true; 
            }
            catch (Exception)
            {
                return false;
            }
        }

        [HttpGet]
        [Route("api/authority/rescind/{empId}")]
        public bool rescindAuthority(string empId)
        {
            try
            {
                _departmentService.rescindAuthority(empId);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // DepartmentRepresentative
        [HttpGet]
        [Route("api/deprep/{id}")]
        public DepartmentRepresentativePayload getDepartmentRepresentative(string id)
            => DepartmentRepresentativePayload.ConvertToDepartmentRepresentativePayload(_departmentService.getCurrentDepartmentRepresentative(id));

        [HttpGet]
        [Route("api/deprep/employees/{deptId}")]
        public List<EmployeePayload> getEligibleDepartmentRepresentatives(string deptId)
            => EmployeePayload.ConvertEntityToPayload(_departmentService.getEligibleDepartmentRepresentatives(deptId));

        [HttpPost]
        [Route("api/deprep/new")]
        public bool addDepartmentRepresentative(Dictionary<string, string> depRep)
        {
            try
            {
                _departmentService.updateDepartmentRepresentative(Int32.Parse(depRep["DepRepID"]), depRep["EmployeeID"]);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
}

        [HttpGet]
        [Route("api/deprep/passcode/{id}")]
        public Dictionary<string, string> getPasscodeOfDep(string id)
        {
            Dictionary<string, string> passcode = new Dictionary<string, string>();
            passcode.Add("passcode", _departmentService.getCurrentDepartmentRepresentative(id).Passcode);
            return passcode;
        }

        // get Collection Point
        [HttpGet]
        [Route("api/deprep/collectionpoint/{depId}")]
        public CollectionPointPayload getCollectionPointOfDepartment(string depId)
            => new CollectionPointPayload(_departmentService.getCollectionPointOfDepartment(depId));

        // set Collection Point
        [HttpGet]
        [Route("api/deprep/collectionpoint/{depId}/{collectionPointId}")]
        public bool setCollectionPointOfDepartment(string depId, int collectionPointId)
        {
            try
            {
                _departmentService.updateCollectionPoint(depId, collectionPointId);
                return true;
            } catch (Exception)
            {
                return false;
            }
        }
    }
}