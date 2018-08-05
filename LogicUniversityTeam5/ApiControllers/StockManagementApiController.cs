using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using LogicUniversityTeam5.Models;
using Microsoft.AspNet.Identity;
using ServiceLayer;
using ServiceLayer.DataAccess;

namespace LogicUniversityTeam5
{
    // Author: Jack
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
                _stockManagementService.addStockVoucher(payload.ItemID, payload.ActualCount, payload.VoucherRaiserID, payload.Reason);
                return true;
            }
            catch (Exception) {
                return false;
            }
        }

        [HttpGet]
        [Route("api/store/stockcount/{catId}")]
        public List<StockVoucherPayload> getStockCounts(int catId)
            => StockVoucherPayload.ConvertEntityToPayload(_stockManagementService.getStockCountItemsByCategory(catId), _stockManagementService);

        [HttpPost]
        [Route("api/store/stockcount/submit/{empId}")]
        public bool submitStockCount(List<StockVoucherPayload> stockVoucherPayloads, string empId)
        {
            try
            {
                _stockManagementService.submitStockCountItems(stockVoucherPayloads, empId);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [HttpGet]
        [Route("api/store/vouchers/retrieve/{isManager}")]
        public List<StockVoucherPayload> retrieveOutstandingStockVouchers(bool isManager)
            => StockVoucherPayload.ConvertEntityToPayload(_stockManagementService.getOpenVouchers(isManager));

        [HttpPost]
        [Route("api/store/vouchers/submit/{empId}")]
        public bool submitStockVouchers(List<StockVoucherPayload> payload, string empId)
        {
            try
            {
                _stockManagementService.submitVouchers(payload, empId);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}