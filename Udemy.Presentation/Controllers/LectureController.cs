using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Udemy.Domain.Contracts;
using Udemy.Domain.Models;
using Udemy.Presentation.DTOs;
using Udemy.Presentation.Helpers;

namespace Udemy.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LectureController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LectureController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }



        #region Add Lecture to a Chapter
        [HttpPost("AddLecture")]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<ActionResult> AddLecture([FromForm] LectureCreationDTO lecture)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model isn't Valid");

            var Chapter = await _unitOfWork.ChapterRepository.GetByIntId(lecture.ChapterId);

            if (Chapter is null)
                return NotFound();

            var Course = await _unitOfWork.CourseRepository.GetById(Chapter.Course_id);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (Course.InstructorId != userId)
                return Unauthorized("You Can't Add to this course");

            var MediaUrl = await FileUpload.UploadFileAsync(lecture.MediaUrl);

            var MappedLecture = _mapper.Map<ChapterLecture>(lecture);
            MappedLecture.MediaUrl = MediaUrl;
            await _unitOfWork.LectureRepository.Add(MappedLecture);
            var Result = await _unitOfWork.CompleteAsync();

            if (Result > 0)
                return Ok("Lecture Was Added Successfully!");
            else
                return BadRequest("Lecture not added!");
        }
        #endregion
        #region GetLectureById
        [HttpGet("get-lecture")]
        public async Task<ActionResult<LectureDTO>> GetLecture(int lectureId)
        {
            var Lecture = await _unitOfWork.LectureRepository.GetByIntId(lectureId);
            var Chapter = await _unitOfWork.ChapterRepository.GetByIntId(Lecture.ChapterId);

            if (User.IsInRole("Student"))
            {
                var isEnrolled = _unitOfWork.StudentCourseRepo.IsEnrolled(User.FindFirstValue(ClaimTypes.NameIdentifier) , Chapter.Course_id);

                if (!isEnrolled)
                    return Unauthorized();
            }

            var MappedLecture = _mapper.Map<LectureDTO>(Lecture);
            return Ok(MappedLecture);
        }
        #endregion
        #region UpdateLecture
        [HttpPut("UpdateLecture")]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<ActionResult> UpdateLecture([FromForm] LectureCreationDTO lecture)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model isn't Valid");

            var Chapter = await _unitOfWork.ChapterRepository.GetByIntId(lecture.ChapterId);

            if (Chapter is null)
                return NotFound();

            var Course = await _unitOfWork.CourseRepository.GetById(Chapter.Course_id);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (Course.InstructorId != userId)
                return Unauthorized("You Can't Add to this course");


            var MappedLecture = _mapper.Map<ChapterLecture>(lecture);

            if (lecture.MediaUrl is not null)
            {
                var MediaUrl = await FileUpload.UploadFileAsync(lecture.MediaUrl);
                MappedLecture.MediaUrl = MediaUrl;
            }

            _unitOfWork.LectureRepository.Update(MappedLecture);
            var Result = await _unitOfWork.CompleteAsync();

            if (Result > 0)
                return Ok("Lecture Was Updated Successfully!");
            else
                return BadRequest("Lecture NOT Updated!");
        }
        #endregion
        #region DeleteLecture
        [HttpDelete("DeleteLecture")]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<ActionResult> DeleteLecture(int id)
        {
            var lecture = await _unitOfWork.LectureRepository.GetByIntId(id);
            if (lecture == null)
                return NotFound();

            var Chapter = await _unitOfWork.ChapterRepository.GetByIntId(lecture.ChapterId);

            if (Chapter is null)
                return NotFound();

            var Course = await _unitOfWork.CourseRepository.GetById(Chapter.Course_id);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (Course.InstructorId != userId)
                return Unauthorized("You Can't Add to this course");


            _unitOfWork.LectureRepository.Delete(lecture);
            var Result = await _unitOfWork.CompleteAsync();

            if (Result > 0)
                return Ok("Lecture Was Deleted Successfully!");
            else
                return BadRequest("Lecture NOT Deleted!");
        }
        #endregion
        

    }
}
