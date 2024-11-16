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
    internal class CourseConfigurations : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasOne(c => c.Category)
                .WithMany(cat => cat.courses)
                .HasForeignKey(c => c.CategoryId);

            builder.HasOne(i => i.Instructor)
                .WithMany(c => c.Instructor_Courses)
                .HasForeignKey(fk => fk.InstructorId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Property(c => c.Level)
                .HasConversion<string>();

            builder.Property(c => c.Price)
                .HasColumnType("decimal(18,2)");
        }
    }
}
