using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Udemy.Domain.Contracts;
using Udemy.Domain.Models;
using Udemy.Presentation.DTOs;
using Udemy.Repository.Specification;

namespace Udemy.Presentation.Controllers
{
    [Route("api/reviews")]
    [ApiController]
    [Authorize]
    public class ReviewsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReviewsController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        [HttpPost("{course_id}")]
        public async Task<ActionResult<ReviewDTO>> CreateReview(string course_id , [FromForm]string ReviewContent)
        {
            var review = new CourseReview()
            {
                Review_Content = ReviewContent ,
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                CourseId = course_id ,
                CreationDate = DateTime.Now,
                IsApproved = false
            };

            await _unitOfWork.ReviewRepository.Add(review);

            var mappedReview = _mapper.Map<ReviewDTO>(review);

            return Ok(mappedReview);

        }

        [HttpGet("course_review/{course_id}")]
        public async Task<ActionResult<IEnumerable<ReviewDTO>>> CourseReviews(string course_id)
        {
            var specification = new GetCourseByIdSpecification(course_id);
            var course = await _unitOfWork.CourseRepository.GetByIdAsync(specification);

            if (course == null)
                return BadRequest("Course not Found");
            
            var courseReviews = course.CourseReviews?.ToList() ?? new List<CourseReview>();
            var MappedReviews = _mapper.Map<ICollection<ReviewDTO>>(courseReviews);
            
            return Ok(MappedReviews);
        }

    }
}
