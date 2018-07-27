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
        private readonly IOrderService _orderService = new OrderService();

        [HttpGet]
        [Route("api/orders/suppliers")]
        public List<SupplierPayload> getSuppliers()
            => SupplierPayload.ConvertEntityToPayload(_orderService.getSuppliers());

        [HttpGet]
        [Route("api/orders/orderids")]
        public List<OrderPayload> getOutstandingOrderIds()
            => OrderPayload.ConvertEntityToPayload(_orderService.getOutstandingOrders());
    }
}