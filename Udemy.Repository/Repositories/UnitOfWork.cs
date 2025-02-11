﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Udemy.Domain.Contracts;
using Udemy.Repository.Context;

namespace Udemy.Repository.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;

        public IReviewRepository ReviewRepository { get; }
        public ICourseChapterRepository ChapterRepository { get; }
        public ILectureRepository LectureRepository { get; }
        public IStudentCourseRepo StudentCourseRepo { get; }
        public ICourseRepository CourseRepository { get; }
        public UnitOfWork(ApplicationDbContext dbContext,ICourseRepository courseRepository,ICourseChapterRepository chapterRepository,ILectureRepository lectureRepository,IStudentCourseRepo studentCourseRepo,IReviewRepository reviewRepository)
        {
            _dbContext = dbContext;
            CourseRepository = courseRepository;
            ChapterRepository = chapterRepository;
            LectureRepository = lectureRepository;
            StudentCourseRepo = studentCourseRepo;
            ReviewRepository = reviewRepository;
        }
        public async Task<int>  CompleteAsync()
            => await _dbContext.SaveChangesAsync();

        public void Dispose()
            => _dbContext.Dispose();
    }
}
