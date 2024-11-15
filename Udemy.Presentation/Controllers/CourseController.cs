using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Udemy.Domain.Contracts;
using Udemy.Domain.Enums;
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

        public CourseController(IUnitOfWork unitOfWork,IMapper mapper)
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
        public async Task<ActionResult<CourseDTO>> CreateCourse(CourseCreationDTO course,CourseLevel courseLevel)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            course.Level = courseLevel.ToString();

            var newCourse = _mapper.Map<Course>(course);
            await _unitOfWork.CourseRepository.Add(newCourse);

            var Result = await _unitOfWork.CompleteAsync();

            if (Result>0)
                return Ok(course);

            else return BadRequest("Course Wasn't Added");
        }

        [HttpDelete("DeleteCourse")]
        public async Task<ActionResult> DeleteCourse(string Id)
        {
            var course = await _unitOfWork.CourseRepository.GetByIdAsync(Id);

            if (course is null)
                return NotFound();

            _unitOfWork.CourseRepository.Delete(course);
            var Result = await _unitOfWork.CompleteAsync();

            if (Result > 0)
                return Ok("Course Deleted");

            return BadRequest();
        }

    }
}
