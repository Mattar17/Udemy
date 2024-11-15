using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Udemy.Domain.Contracts;
using Udemy.Domain.Models;
using Udemy.Presentation.DTOs;
using Udemy.Presentation.Helpers;
using Udemy.Repository.Context;

namespace Udemy.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _dbContext;

        public CourseController(IUnitOfWork unitOfWork,IMapper mapper,ApplicationDbContext dbContext)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("Courses")]
        public async Task<ActionResult<IEnumerable<CourseDTO>>> GetAllCourses()
        {
            var Courses = await _unitOfWork.CourseRepository.GetAllAsync();

            if (Courses.Count() == 0)
                return NotFound();

            var MappedCourses = _mapper.Map<IEnumerable<CourseDTO>>(Courses);

            return Ok(MappedCourses);
        }

        [HttpPost("CreateCourse")]
        public async Task<ActionResult<CourseDTO>> CreateCourse(CourseDTO course,IFormFile file)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var newCourse = _mapper.Map<Course>(course);
            await _unitOfWork.CourseRepository.Add(newCourse);

            var Result = await _unitOfWork.CompleteAsync();

            if (Result>0)
                return Ok(course);

            else return BadRequest("Course Wasn't Added");
        }

        [HttpDelete("DeleteCourse")]
        public async Task<ActionResult> DeleteCourse(Course course)
        {
            _unitOfWork.CourseRepository.Delete(course);
            var Result = await _unitOfWork.CompleteAsync();

            if (Result > 0)
                return Ok("Course Deleted");

            return BadRequest();
        }

    }
}
