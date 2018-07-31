using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LogicUniversityTeam5
{
    public class RequisitionApiController : ApiController
    {
        private readonly IRequisitionService _requisitionService;

        public RequisitionApiController(IRequisitionService requisitionService)
        {
            _requisitionService = requisitionService;
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
                _requisitionService.processRequisition(reqId, empId, true);
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
                _requisitionService.processRequisition(reqId, empId, false);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}