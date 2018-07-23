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
            => new AuthorityPayload(_departmentService.getCurrentAuthority(id));

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

        // DepartmentRepresentative
        [HttpGet]
        [Route("api/deprep/{id}")]
        public DepartmentRepresentativePayload getDepartmentRepresentative(string id)
            => new DepartmentRepresentativePayload(_departmentService.getCurrentDepartmentRepresentative(id));

        [HttpPost]
        [Route("api/deprep/new")]
        public bool addDepartmentRepresentative(Dictionary<string, string> depRep)
        {
            try
            {
                _departmentService.updateDepartmentRepresentative(Int32.Parse(depRep["DepRepID"]), depRep["EmpID"]);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}