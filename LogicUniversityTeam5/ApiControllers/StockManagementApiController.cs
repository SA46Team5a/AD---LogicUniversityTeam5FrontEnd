using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace LogicUniversityTeam5
{
    public class StockManagementApiController : ApiController
    {
        private readonly IStockManagementService _stockManagementService;

        public StockManagementApiController(IStockManagementService stockManagementService)
        {
            _stockManagementService = stockManagementService;
        }

        [HttpPost]
        [Route("api/store/vouchers/add")]
        public bool addStockVoucher(StockVoucherPayload payload)
        {
            try
            {
                _stockManagementService.addStockVoucher(payload.ItemID, payload.ActualCount, payload.EmployeeID, payload.Reason);
                return true;
            }
            catch (Exception) {
                return false;
            }
        }
    }
}