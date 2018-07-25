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
    public class ReportingApiController: ApiController
    {
        private readonly IClassificationService _classificationService;

        public ReportingApiController(IClassificationService classificationService)
        {
            this._classificationService = classificationService;
        }

        [HttpPost]
        [Route("api/reports/reorder")]
        public ReorderCostReportPayload test(ReorderCostReportPayload payload)
        {
            return payload;
        }
    }
}