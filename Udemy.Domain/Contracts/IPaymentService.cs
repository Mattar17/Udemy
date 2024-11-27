using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Udemy.Domain.Contracts
{
    public interface IPaymentService
    {
        PaymentIntent CreatePaymentIntent(decimal amount,string currency);
    }
}
