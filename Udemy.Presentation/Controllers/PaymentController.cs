using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.Security.Claims;
using Udemy.Domain.Contracts;
using Udemy.Domain.Models;
using Udemy.Presentation.DTOs;
using Udemy.Services;

namespace Udemy.Presentation.Controllers
{
    [Route("api/payment")]
    [ApiController]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaymentController(IPaymentService stripePaymentService , IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("{courseId}/create_payment_intent")]
        public async Task<ActionResult> CreatePyamentIntent(string courseId , string currency)
        {
            try
            {
                var course = await _unitOfWork.CourseRepository.GetById(courseId);

                var options = new PaymentIntentCreateOptions
                {
                    Amount = 2000 , // Amount in cents
                    Currency = "usd" ,
                    PaymentMethodTypes = new List<string> { "card" } ,
                    Metadata = new Dictionary<string , string>
                    {
                         { "course_id", courseId }, 
                         { "student_id", User.FindFirstValue(ClaimTypes.NameIdentifier) } 
                    }
                };

                var service = new PaymentIntentService();
                var paymentIntent = service.Create(options);

                return Ok(paymentIntent.Id);
            }
            catch (Exception ex)
            {

                return StatusCode(500 , $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("confirm_payment_intent")]
        public async Task<ActionResult> ConfirmPaymentIntent(string PaymentIntentId)
        {

            try
            {
                var options = new PaymentIntentConfirmOptions()
                {
                    PaymentMethod = "pm_card_visa"
                };

                var service = new PaymentIntentService();
                service.Confirm(PaymentIntentId , options);

                return Ok("intent confirmed");
            }

            catch (Exception ex)
            {
                return StatusCode(500 , $"An error occurred: {ex.Message}");
            }
        }


    }
}

