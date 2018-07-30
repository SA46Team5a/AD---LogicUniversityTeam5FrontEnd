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
    // Author: Jack,Bhat Pavana
    public class ReportingApiController: ApiController
    {
        private readonly IClassificationService _classificationService;
        private readonly IReportService _reportService;

        public ReportingApiController(IClassificationService classificationService, IReportService reportService)
        {
            _classificationService = classificationService;
            _reportService = reportService;
        }

        [HttpPost]
        [Route("api/reports/reorder")]
        public ReportResponsePayload test(ReorderRequestPayload payload)
        {
            ReportResponsePayload response = _reportService.generateReorderCostReport(payload);
            return response;
        }


        [HttpPost]
        [Route("api/reports/requisition")]
        public ReportResponsePayload test(RequisitionRequestPayload payload)
        {
            ReportResponsePayload response = _reportService.generateRequisitionCostReport(payload);
            return response;
        }

        [HttpPost]
        [Route("api/reports/requisitionitem")]
        public ReportResponsePayload test(RequisitionItemPayload payload)
        {
            ReportResponsePayload response = _reportService.generateRequisitionItemReport(payload);
            return response;
        }
    }
}