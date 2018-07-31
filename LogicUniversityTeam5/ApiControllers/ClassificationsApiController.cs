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
    public class ClassificationsApiController : ApiController
    {
        private readonly IClassificationService _classificationService;
        public ClassificationsApiController() { }
        public ClassificationsApiController(IClassificationService classificationService)
        {
            this._classificationService = classificationService;
        }

        [HttpGet]
        [Route("api/classification/categories")]
        public List<CategoryPayload> categories()
        {
            List<CategoryPayload> payload = CategoryPayload.ConvertEntityToPayload(_classificationService.GetCategories());
            return payload;
        }

        [HttpGet]
        [Route("api/classification/collectionpoints")]
        public List<CollectionPointPayload> collectionPoints()
        {
            List<CollectionPointPayload> payload = CollectionPointPayload.ConvertEntityToPayload(_classificationService.GetCollectionPoints());
            return payload;
        }
    }
}