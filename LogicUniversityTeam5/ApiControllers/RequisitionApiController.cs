using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LogicUniversityTeam5.Controllers;

namespace LogicUniversityTeam5
{
    public class RequisitionApiController : ApiController
    {
        private readonly IRequisitionService _requisitionService;
        private readonly IDepartmentService _departmentService;

        public RequisitionApiController(IRequisitionService requisitionService, IDepartmentService departmentService)
        {
            _requisitionService = requisitionService;
            _departmentService = departmentService;
        }

        [HttpGet]
        [Route("api/requisitions/pending/{depId}")]
        public List<RequisitionPayload> getPendingRequisitionsOfDepartment(string depId)
            => RequisitionPayload.ConvertEntityToPayload(_requisitionService.getPendingRequisitionsOfDep(depId));

        [HttpGet]
        [Route("api/requisition/approve/{empId}/{reqId}")]
        public bool approveRequisition(string empId, int reqId)
        {
            try
            {
          
                _requisitionService.processRequisition(reqId, empId, true, _departmentService);
               
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [HttpGet]
        [Route("api/requisition/reject/{empId}/{reqId}")]
        public bool rejectRequisition(string empId, int reqId)
        {
            try
            {
                _requisitionService.processRequisition(reqId, empId, false, _departmentService);
               
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}