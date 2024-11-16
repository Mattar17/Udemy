﻿using AutoMapper;
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

namespace Udemy.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CourseController(IUnitOfWork unitOfWork , IMapper mapper)
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

        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDTO>> GetCourse(string id)
        {
            var course = await _unitOfWork.CourseRepository.GetByIdAsync(id);
            
            if (course == null)
                return NotFound();

            var MappedCourse = _mapper.Map<CourseDTO>(course);
            return Ok(MappedCourse);
        }

        [HttpPost("CreateCourse")]
        [Authorize(Roles = "Instructor,Admin")]
        public async Task<ActionResult<CourseDTO>> CreateCourse(CourseCreationDTO course , CourseLevel courseLevel)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            course.Level = courseLevel.ToString();

            var newCourse = _mapper.Map<Course>(course);
            newCourse.InstructorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _unitOfWork.CourseRepository.Add(newCourse);

            var Result = await _unitOfWork.CompleteAsync();

            var MappedCourse = _mapper.Map<CourseDTO>(newCourse);
            if (Result > 0)
                return Ok(MappedCourse);

            else return BadRequest("Course Wasn't Added");
        }

        [HttpDelete("DeleteCourse")]
        [Authorize(Roles = "Instructor,Admin")]
        public async Task<ActionResult> DeleteCourse(string Id)
        {
            var course = await _unitOfWork.CourseRepository.GetByIdAsync(Id);

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
        public async Task<ActionResult> UpdateCourse([FromBody]CourseCreationDTO model,string id)
        {
            var course = await _unitOfWork.CourseRepository.GetByIdAsync(id);
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

            course = _mapper.Map(model,course);
            
            var Result = await _unitOfWork.CompleteAsync();

            if (Result>0)
                return Ok("Coures Updated");

            else return BadRequest("Something gone wrong while updating");
        }



    }
}
