using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Udemy.Domain.Models;

namespace Udemy.Repository.Configurations
{
    internal class StudentCoursesConfigurations : IEntityTypeConfiguration<Student_Course>
    {
        public void Configure(EntityTypeBuilder<Student_Course> builder)
        {
            builder.HasKey(fk => new { fk.UserId , fk.CourseId });

            builder.HasOne(p => p.Course)
                .WithMany(p => p.Course_Students)
                .HasForeignKey(fk=>fk.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.User)
                .WithMany(p => p.student_Courses)
                .HasForeignKey(k=>k.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
