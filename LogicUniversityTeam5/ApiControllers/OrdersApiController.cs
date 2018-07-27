using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace LogicUniversityTeam5
{
    public class OrdersApiController : ApiController
    {
        private readonly IOrderService _orderService;
        private readonly IStockManagementService _stockManagementService;

        public OrdersApiController(IOrderService orderService, IStockManagementService stockManagementService)
        {
            _orderService = orderService;
            _stockManagementService = stockManagementService;
        }

        [HttpGet]
        [Route("api/orders/suppliers")]
        public List<SupplierPayload> getSuppliers()
            => SupplierPayload.ConvertEntityToPayload(_orderService.getSuppliers());

        [HttpGet]
        [Route("api/orders/orderids")]
        public List<OrderPayload> getOutstandingOrderIds()
            => OrderPayload.ConvertEntityToPayload(_orderService.getOutstandingOrders());

        [HttpGet]
        [Route("api/orders/orderdetails/{orderId}/{supplierId}")]
        public List<OrderDetailsPayload> getOrderDetailsOfOrderIdAndSupplier(int orderId, string supplierId)
            => OrderDetailsPayload.ConvertEntityToPayload(_orderService.getOrderDetailsOfOrderIdAndSupplier(orderId, supplierId), _stockManagementService);

        [HttpGet]
        [Route("api/orders/addstock/{empId}/{orderSupplierDetailId}/{qty}")]
        public bool addStock(string empId, int orderSupplierDetailId, int qty)
        {
            try
            {
                _orderService.updateQtyRecievedOfOrderSupplierDetail(orderSupplierDetailId, qty, empId);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}