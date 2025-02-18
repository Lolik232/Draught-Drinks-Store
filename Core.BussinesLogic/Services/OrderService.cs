using Core.BussinesLogic.Abstractions.Entities.Order;
using Core.BussinesLogic.Abstractions.Interfaces;
using Core.BussinesLogic.Abstractions.Primitives;
using DAL.Abstractions.Entities;
using DAL.Abstractions.Interfaces.Payment;
using DAL.Abstractions.Interfaces.Repositories;
using System.Text.RegularExpressions;

namespace Core.BussinesLogic.Services
{
    public class OrderService : IOrderService
    {

        private readonly IOrderRepository _orderRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;
        private readonly IPaymentService _paymentService;

        public OrderService(
            IOrderRepository orderRepository,
            IPaymentRepository paymentRepository,
            IUserRepository userRepository,
            IProductRepository productRepository,
            IPaymentService paymentService)
        {
            _orderRepository = orderRepository;
            _paymentRepository = paymentRepository;
            _userRepository = userRepository;
            _productRepository = productRepository;
            _paymentService = paymentService;
        }

        public async Task<Result<Order>> GetOrderAsync(int number)
        {
            try
            {
                var order = _orderRepository.Get(number);
                return Result.Sucsess(order);
            }
            catch (Exception)
            {
                return Error.NullValue;
            }
        }

        public async Task<Result<ICollection<Order>>> GetOrdersForUserAsync(int userId)
        {
            try
            {
                var user = await _userRepository.GetAsync(userId, new() { nameof(User.ContactData) });
                var orders = await _orderRepository.GetAllAsync(order => order.ContactDataId == user.ContactDataId);

                return Result.Sucsess(orders);
            }
            catch (Exception)
            {
                return Error.NullValue;
            }
        }

        public async Task<Result<OrderCreated>> MakeOrderAsync(int userId, CreateOrder order, string returnUrl)
        {
            try
            {
                var user = await _userRepository.GetAsync(userId, new() { nameof(User.ContactData) });

                var productsInOrder =
                    order.ProductsInOrder.Select(or => new ProductsInOrder { Count = or.Count, ProductId = or.ProductId }).ToList();
                var ids = productsInOrder.Select(p => p.ProductId);

                var products =
                      _productRepository.GetAll(0,
                      int.MaxValue,
                      pr => ids.Contains(pr.Id),
                      nameof(Product.Id),
                     new() { nameof(Product.ProductsInStocks)
                 });


                foreach (var productInOrder in productsInOrder)
                {
                    productInOrder.Product = products.First(pr => pr.Id == productInOrder.ProductId);
                    productInOrder.PriceInOrder = productInOrder.Product.Price;
                }

                var all = productsInOrder.TrueForAll(pr => pr.Product.ProductsInStocks.Count >= pr.Count);

                if (!all)
                    return Error.NullValue;

                var sum = productsInOrder.Sum(pr => pr.Product.Price * pr.Count);

                var repositoryOrder = new Order
                {
                    ContactDataId = user.ContactDataId,
                    ContactData = user.ContactData,
                    Date = DateTime.UtcNow,
                    ProductsInOrders = productsInOrder,
                    Status = "in pr",
                    Payment = new Payment
                    {
                        Sum = sum,
                        Status = "in pr",
                        PaymentInExternalService = new PaymentInExternalService
                        {
                            ExternalPaymentId = "id",
                            ExternalService = "yoo",
                        }
                    }
                };

                var orderId = _orderRepository.CreateOrder(repositoryOrder);

                var paymentUrl = await _paymentService.CreatePayment((int)orderId, returnUrl, new Amount
                {
                    Currency = "RUB",
                    Value = sum.ToString().Replace(',', '.'),
                }, "Оплата пива");

                return Result.Sucsess(new OrderCreated((int)orderId, repositoryOrder.Status, repositoryOrder.Date, sum.ToString() + " RUB", paymentUrl));
            }
            catch (Exception)
            {
                return Error.NullValue;
            }
        }
    }
}
