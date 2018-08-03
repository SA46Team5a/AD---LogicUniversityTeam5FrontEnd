using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ServiceLayer;
using ServiceLayer.DataAccess;
using LogicUniversityTeam5.Models;

namespace LogicUniversityTeam5.ApiControllers
{
    // Author: Jack
    public class DisbursementApiController : ApiController
    {
        private readonly IDisbursementService _disbursementService;
        private readonly IDepartmentService _departmentService;

        public DisbursementApiController(IDisbursementService disbursementService, IDepartmentService departmentService)
        {
            _disbursementService = disbursementService;
            _departmentService = departmentService;
        }

        [HttpGet]
        [Route("api/store/retrieval/{empId}")]
        public RetrievalFormPayload getRetrievalForm(string empId)
            => _disbursementService.getRetrievalForm(empId);

        [HttpPost]
        [Route("api/store/retrieval/{disDutyId}")]
        public bool submitRetrievalFormOfDisbursementDuty(int disDutyId, List<ItemAndQtyPayload> itemAndQty)
        {
            try
            {
                _disbursementService.submitRetrievalForm(disDutyId, ItemAndQtyPayload.ConvertListToDictionary(itemAndQty));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [HttpGet]
        [Route("api/store/disbursement/departments")]
        public List<DepartmentPayload> getDepartmentsWithDisbursement()
            => DepartmentPayload.ConvertEntityToPayload(_disbursementService.getDepartmentsWithDisbursements());

        [HttpGet]
        [Route("api/store/disbursement/{id}")]
        public List<DisbursementDetailPayload> getUncollectedDisbursementItemsOfDepartment(string id)
        {
            List<DisbursementDetailPayload> payload = _disbursementService.getUncollectedDisbursementDetailsByDep(id);
            return payload;
        }


        [HttpPost]
        [Route("api/store/disbursement/{depId}/{empId}/{passcode}")]
        public bool submitDisbursementOfDepartment(List<DisbursementDetailPayload> disbursementDetailPayloads, string depId, string empId, string passcode)
        {
            try
            {
                if (_departmentService.verifyPassCode(passcode, depId))
                {
                    _disbursementService.submitDisbursementOfDep(depId, disbursementDetailPayloads, empId);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}