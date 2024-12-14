using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Udemy.Domain.Contracts;
using Udemy.Domain.Models;
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
        private readonly IUnitOfWork _unitOfWork;

        public PaymentController(IPaymentService stripePaymentService,IUnitOfWork unitOfWork)
        {
            _stripePaymentService = stripePaymentService;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("purchase_course")]
        public async Task<ActionResult> PurchaseCourse(PaymentRequest request)
        {
           try
           {
                var paymentIntent = _stripePaymentService.CreatePaymentIntent(request.amount , request.currency);
                var confirmedIntent = await _stripePaymentService.VerifyPayment(paymentIntent.Id , "pm_card_visa");
               
                if (confirmedIntent.Status == "succeeded")
                {
                    
                    string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                    var studentCourse = new Student_Course
                    {
                        UserId = userId ,
                        CourseId = request.CourseId
                    };

                    await _unitOfWork.StudentCourseRepo.Add(studentCourse);
                    await _unitOfWork.CompleteAsync();

                    return Ok("Payment succeeded and student enrolled");
                }
                else
                {
                    return BadRequest($"Payment failed with status: {confirmedIntent.Status}");
                }
            }
            catch (Exception ex)
            {
               
                return StatusCode(500 , $"An error occurred: {ex.Message}");
            }
        }

    }
}

