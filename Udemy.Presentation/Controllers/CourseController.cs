using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Udemy.Domain.Contracts;
using Udemy.Domain.Enums;
using Udemy.Domain.Models;
using Udemy.Presentation.DTOs;
using Udemy.Presentation.Helpers;
using Udemy.Repository.Context;
using Udemy.Repository.Specification;

namespace Udemy.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly FileUpload _fileUpload;

        public CourseController(IUnitOfWork unitOfWork , IMapper mapper,FileUpload fileUpload)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileUpload = fileUpload;
        }

        #region Get All Courses + GetCourse By id


        [HttpGet("Courses")]
        public async Task<ActionResult<IEnumerable<CourseDTO>>> GetAllCourses()
        {
            var specification = new CourseIncludesSpecification();
            var Courses = await _unitOfWork.CourseRepository.GetAllAsync(specification);

            if (Courses.Count() == 0)
                return NotFound();

            var MappedCourses = _mapper.Map<IEnumerable<CourseDTO>>(Courses);

            return Ok(MappedCourses);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDTO>> GetCourse(string id)
        {
            var specification = new GetCourseByIdSpecification(id);
            var course = await _unitOfWork.CourseRepository.GetByIdAsync(specification);

            if (course == null)
                return NotFound();

            var MappedCourse = _mapper.Map<CourseDTO>(course);
            return Ok(MappedCourse);
        }
        #endregion
        #region Create Course


        [HttpPost("CreateCourse")]
        [Authorize(Roles = "Instructor,Admin")]
        public async Task<ActionResult<CourseDTO>> CreateCourse(CourseCreationDTO course , CourseLevel courseLevel)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            course.Level = courseLevel.ToString();
            var PreviewPicUrl = _fileUpload.UploadImage(course.PreviewPicture);

            var newCourse = _mapper.Map<Course>(course);
            newCourse.InstructorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            newCourse.PreviewPictureUrl = PreviewPicUrl;
            await _unitOfWork.CourseRepository.Add(newCourse);

            var Result = await _unitOfWork.CompleteAsync();

            var MappedCourse = _mapper.Map<CourseDTO>(newCourse);
            if (Result > 0)
                return Ok(MappedCourse);

            else
                return BadRequest("Course Wasn't Added");
        }
        #endregion
        #region Update-Delete Course


        [HttpDelete("DeleteCourse")]
        [Authorize(Roles = "Instructor,Admin")]
        public async Task<ActionResult> DeleteCourse(string Id)
        {
            var specification = new GetCourseByIdSpecification(Id);
            var course = await _unitOfWork.CourseRepository.GetByIdAsync(specification);

            if (course.InstructorId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return Unauthorized("You can't Delete this course");
            }

            if (course is null)
                return NotFound();

            _unitOfWork.CourseRepository.Delete(course);
            var Result = await _unitOfWork.CompleteAsync();

            if (Result > 0)
                return Ok("Course Deleted");

            return BadRequest();
        }

        [HttpPut("UpdateCourse")]
        [Authorize(Roles = "Instructor,Admin")]
        public async Task<ActionResult> UpdateCourse([FromBody] CourseCreationDTO model , string id)
        {
            var specification = new GetCourseByIdSpecification(id);
            var course = await _unitOfWork.CourseRepository.GetByIdAsync(specification);
            if (course is null)
                return NotFound();

            if (course.InstructorId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return Unauthorized("You can't Edit this course");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            course = _mapper.Map(model , course);

            var Result = await _unitOfWork.CompleteAsync();

            if (Result > 0)
                return Ok("Coures Updated");

            else
                return BadRequest("Something gone wrong while updating");
        }
        #endregion

    }
}
