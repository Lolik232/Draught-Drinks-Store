using Core.BussinesLogic.Abstractions.Entities.Order;
using Core.BussinesLogic.Abstractions.Interfaces;
using DAL.Abstractions.Entities;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Order.V1;
using System.Security.Claims;

namespace Draught_Drinks_Store_GRPC.Services.v1
{
    public class OrderService : Order.V1.Order.OrderBase
    {
        private readonly IOrderService _orderService;

        public OrderService(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize]
        public override Task<OrderStatusResponce> GetOrderStatus(OrderStatusRequest request, ServerCallContext context)
        {
            return base.GetOrderStatus(request, context);
        }

        [Authorize]
        public override async Task<OrderStatusResponce> Place(PlaceRequest request, ServerCallContext context)
        {
            var userID = Convert.ToInt32(context.GetHttpContext().User.FindFirst(ClaimTypes.Name)!.Value);

            var order = new CreateOrder(request.Products.Select(p => new ProductCount(p.Id, p.Count)).ToList());

            var makeOrderResult = await _orderService.MakeOrderAsync(userID, order, "");

            if (makeOrderResult.IsFailture)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "что-то не так"));
            }

            var responce = new OrderStatusResponce()
            {
                Date = makeOrderResult.Value.Date.ToString(),
                Status = makeOrderResult.Value.Status,
                Sum = makeOrderResult.Value.Sum,
                Number = makeOrderResult.Value.Number,
                PaymentLink = makeOrderResult.Value.Link
            };

            return responce;
        }
    }
}
