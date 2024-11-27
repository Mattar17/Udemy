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
    }
}
