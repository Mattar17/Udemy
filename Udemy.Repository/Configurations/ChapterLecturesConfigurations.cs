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
    internal class ChapterLecturesConfigurations : IEntityTypeConfiguration<ChapterLecture>
    {
        public void Configure(EntityTypeBuilder<ChapterLecture> builder)
        {
            builder.HasOne(l => l.CourseChapter)
                .WithMany(c => c.Chapter_Lectures)
                .HasForeignKey(l => l.ChapterId);
        }
    }
}
