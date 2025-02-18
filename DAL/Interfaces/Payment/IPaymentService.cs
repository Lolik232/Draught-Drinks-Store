using System.Text.Json.Serialization;
using DAL.Abstractions.Entities;

namespace DAL.Abstractions.Interfaces.Payment
{

    public class PaymentInvalidArgumentsException : ArgumentException { };
    public class PaymentHttpError : OperationCanceledException { };

    public class Amount
    {
        [JsonPropertyName("value")]
        public string Value { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }
    }

    public interface IPaymentService
    {
        Task< string> CreatePayment(
            int orderNumber,
             string returnUrl,
             Amount amount,
             string description);
    }
}
