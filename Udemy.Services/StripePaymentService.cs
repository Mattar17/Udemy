using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Udemy.Domain.Contracts;

namespace Udemy.Services
{
    public class StripePaymentService:IPaymentService
    {
        private readonly PaymentIntentService _paymentIntentService;
        public StripePaymentService()
        {
            _paymentIntentService = new PaymentIntentService();  
        }
        public PaymentIntent CreatePaymentIntent(decimal amount,string currency)    
        {
          var options = new PaymentIntentCreateOptions()
          {
              Amount = (long)(amount*100),
              Currency = currency,
              PaymentMethodTypes = new List<string> { "card" }
          };

            var Service = new PaymentIntentService();
            return Service.CreateAsync(options).Result;
        }

        public async Task<PaymentIntent> VerifyPayment(string paymentIntentId,string PaymentMethodId)
        {
            var options = new PaymentIntentConfirmOptions()
            {
                PaymentMethod = PaymentMethodId
            };

            var Service = new PaymentIntentService();
            return await Service.ConfirmAsync(paymentIntentId , options);

        }

        
    }
}
