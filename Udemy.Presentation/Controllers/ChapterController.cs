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
    [Route("api/[controller]")]
    [ApiController]
    public class ChapterController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ChapterController(IMapper mapper,IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }


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

            var reqChapter = await _unitOfWork.ChapterRepository.GetById(chapter.Id);

            if (chapter is null)
                return NotFound();

            var Course = await _unitOfWork.CourseRepository.GetById(chapter.Course_id);
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (UserId != Course.InstructorId)
                return Unauthorized("You can't Update this Chapter");

            var mappedChapter = _mapper.Map<CourseChapter>(chapter);
            _unitOfWork.ChapterRepository.Update(mappedChapter);
            var Result = await _unitOfWork.CompleteAsync();

            if (Result > 0)
                return Ok("Chapter is updated");

            else
                return
                    BadRequest("Chapter NOT updated");
        }

        #endregion
        #region DeleteChapter
        [HttpDelete("DeleteChapter")]
        [Authorize(Roles = "Instructor,Admin")]
        public async Task<ActionResult> DeleteChapter(int id)
        {

            var chapter = await _unitOfWork.ChapterRepository.GetById(id);

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

    }
}
