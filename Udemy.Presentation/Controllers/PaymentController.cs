using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Udemy.Domain.Contracts;
using Udemy.Presentation.DTOs;
using Udemy.Services;

namespace Udemy.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _stripePaymentService;

        public PaymentController(IPaymentService stripePaymentService)
        {
            _stripePaymentService = stripePaymentService;
        }

        [HttpPost("CreatePaymentIntent")]
        public async Task<ActionResult> CreatePaymentIntent(PaymentRequest request)
        {
            var PaymentIntent = _stripePaymentService.CreatePaymentIntent(request.amount , request.currency);
            return Ok(PaymentIntent.Id);
        }
    }
}
