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

        public CourseController(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        #region Course endpoints
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
            var PreviewPicUrl = await FileUpload.UploadFileAsync(course.PreviewPicture);

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
        #endregion



        #region Chapter endpoints
        #region Add Chapter


        [HttpPost("AddChapter")]
        [Authorize(Roles = "Instructor,Admin")]

        public async Task<ActionResult<ChapterDTO>> AddChapter(ChapterDTO chapter)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var InstructorId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            var specification = new GetCourseByIdSpecification(chapter.Course_id);
            var Course = await _unitOfWork.CourseRepository.GetByIdAsync(specification);

            if (InstructorId != Course.InstructorId)
            {
                return Unauthorized("You Can't Add to this course");
            }

            var mappedChapter = _mapper.Map<CourseChapter>(chapter);
            await _unitOfWork.ChapterRepository.Add(mappedChapter);

            var Result = await _unitOfWork.CompleteAsync();
            if (Result > 0)
                return Ok(chapter);

            return BadRequest("Chapter Was NOT Added");
        }
        #endregion
        #region GetChapterById
        [HttpGet("ChapterById")]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<ActionResult<ChapterDTO>> GetChapterById(int id)
        {
            var specification = new ChapterWithLecturesSpecification(id);
            var chapter = await _unitOfWork.ChapterRepository.GetByIdAsync(specification);

            if (chapter is null)
                return NotFound();

            var mappedChapter = _mapper.Map<ChapterDTO>(chapter);

            return Ok(chapter);

        }

        #endregion
        #region UpdateChapter
        [HttpPut("UpdateChapter")]
        [Authorize(Roles = "Instructor,Admin")]
        public async Task<ActionResult> UpdateChapter(ChapterDTO chapter)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var reqChapter = await _unitOfWork.ChapterRepository.GetByIntId(chapter.Id);

            if (chapter is null)
                return NotFound();

            var Course = await _unitOfWork.CourseRepository.GetById(chapter.Course_id);
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (UserId != Course.InstructorId)
                return Unauthorized("You can't Update this Chapter");

            var mappedChapter = _mapper.Map<CourseChapter>(chapter);
            _unitOfWork.ChapterRepository.Update(mappedChapter);
            var Result = await _unitOfWork.CompleteAsync();

            if (Result>0)
                return Ok("Chapter is updated");

            else return 
                    BadRequest("Chapter NOT updated");
        }

        #endregion
        #region DeleteChapter
        [HttpDelete("DeleteChapter")]
        [Authorize(Roles = "Instructor,Admin")]
        public async Task<ActionResult> DeleteChapter(int id)
        {

            var chapter = await _unitOfWork.ChapterRepository.GetByIntId(id);

            if (chapter is null)
                return NotFound();

            var Course = await _unitOfWork.CourseRepository.GetById(chapter.Course_id);
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (UserId != Course.InstructorId)
                return Unauthorized("You can't Update this Chapter");

            _unitOfWork.ChapterRepository.Delete(chapter);
            var Result = await _unitOfWork.CompleteAsync();

            if (Result > 0)
                return Ok("Chapter Deleted");

            else
                return
                    BadRequest("Chapter NOT Deleted");
        }
        #endregion
        #endregion



        #region Lecture endpoints
        #region Add Lecture to a Chapter
        [HttpPost("AddLecture")]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<ActionResult> AddLecture([FromForm]LectureCreationDTO lecture)
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
            if (lecture == null) return NotFound();

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
        #endregion
    }
}
