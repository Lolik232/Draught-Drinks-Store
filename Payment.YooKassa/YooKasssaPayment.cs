using DAL.Abstractions.Interfaces.Payment;
using DAL.Abstractions.Interfaces.Repositories;
using System;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Payment.YooKassa
{
    public class YooKasssaPayment : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public YooKasssaPayment(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public class ConfirmationCreate
        {
            [JsonPropertyName("type")]
            public string Type { get; set; } = "redirect";

            [JsonPropertyName("return_url")]
            public string ReturnUrl { get; set; } = "";
        }
        public class Metadata
        {
            [JsonPropertyName("order_number")]
            public int OrderNumber { get; set; }

            [JsonPropertyName("payment_number")]

            public int PaymentNumber { get; set; }
        }
        class PaymentCreate
        {
            [JsonPropertyName("amount")]
            public Amount Amount { get; set; } = new Amount();

            [JsonPropertyName("description")]
            public string Description { get; set; } = "";

            [JsonPropertyName("confirmation")]
            public ConfirmationCreate Confirmation { get; set; } = new ConfirmationCreate();

            [JsonPropertyName("metadata")]
            public Metadata Metadata { get; set; } = new Metadata();
        }

        public class Confirmation
        {
            [JsonPropertyName("type")]
            public string Type { get; set; } = "";

            [JsonPropertyName("confirmation_url")]
            public string ConfirmationUrl { get; set; } = "";
        }

        public class PaymentMethod
        {
            [JsonPropertyName("type")]
            public string Type { get; set; } = "";

            [JsonPropertyName("id")]
            public string Id { get; set; } = "";

            [JsonPropertyName("saved")]
            public bool Saved { get; set; }
        }

        public class Recipient
        {
            [JsonPropertyName("account_id")]
            public string AccountId { get; set; } = "";

            [JsonPropertyName("gateway_id")]
            public string GatewayId { get; set; } = "";
        }

        public class PaymentResponce
        {
            [JsonPropertyName("id")]
            public string Id { get; set; } = "";

            [JsonPropertyName("status")]
            public string Status { get; set; } = "";

            [JsonPropertyName("paid")]
            public bool Paid { get; set; }

            [JsonPropertyName("amount")]
            public Amount Amount { get; set; } = new Amount();

            [JsonPropertyName("confirmation")]
            public Confirmation Confirmation { get; set; } = new Confirmation();

            [JsonPropertyName("created_at")]
            public DateTime CreatedAt { get; set; }

            [JsonPropertyName("description")]
            public string Description { get; set; } = "";

            [JsonPropertyName("metadata")]
            public Metadata Metadata { get; set; } = new Metadata();

            [JsonPropertyName("payment_method")]
            public PaymentMethod PaymentMethod { get; set; } = new PaymentMethod();

            [JsonPropertyName("recipient")]
            public Recipient Recipient { get; set; } = new Recipient();

            [JsonPropertyName("refundable")]
            public bool Refundable { get; set; }

            [JsonPropertyName("test")]
            public bool Test { get; set; }
        }


        public async Task<string> CreatePayment(
            int orderNumber, string returnUrl, Amount amount, string description)
        {

            var payment = new PaymentCreate
            {
                Amount = amount,
                Description = description,
                Confirmation = new ConfirmationCreate
                {
                    ReturnUrl = returnUrl,
                    Type = "redirect"
                },
                Metadata = new Metadata
                {
                    PaymentNumber = orderNumber,
                    OrderNumber = orderNumber,
                }
            };



            // TODO: send to yookassa api
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
               "Basic",
                "ODc0NzU0OnRlc3Rfck5OZlc4aU0tdWp4MkZnb2xqcFE5NzRIWmVCWDVNQXBqb3JVQTNzM1BVNA=="
                );
            Random random = new Random();

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";
            var idempotenceKey = new string(Enumerable.Repeat(chars, 10)
                 .Select(s => s[random.Next(s.Length)]).ToArray());

            client.DefaultRequestHeaders.Add("Idempotence-Key", idempotenceKey);

            try
            {
                var res = await client.PostAsJsonAsync("https://api.yookassa.ru/v3/payments", payment);

                var responce = await res.Content.ReadFromJsonAsync<PaymentResponce>();
                return responce.Confirmation.ConfirmationUrl;
            }
            catch (Exception)
            {
                throw new PaymentInvalidArgumentsException();
            }
        }
    }
}
