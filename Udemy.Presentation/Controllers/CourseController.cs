using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Udemy.Domain.Contracts;
using Udemy.Domain.Models;
using Udemy.Presentation.DTOs;

namespace Udemy.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CourseController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("Courses")]
        public async Task<ActionResult<IEnumerable<Course>>> GetAllCourses()
        {
            var Courses = await _unitOfWork.CourseRepository.GetAllAsync();
            
            if (Courses.Count() > 0) 
             return Ok(Courses);

            return NotFound();
        }

        [HttpPost("CreateCourse")]
        public async Task<ActionResult<CourseDTO>> CreateCourse(CourseDTO course)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var newCourse = _mapper.Map<Course>(course);
            _unitOfWork.CourseRepository.Add(newCourse);

            var Result = await _unitOfWork.CompleteAsync();

            if (Result>0)
                return Ok(course);

            else return BadRequest("Course Wasn't Added");
        }
    }
}
