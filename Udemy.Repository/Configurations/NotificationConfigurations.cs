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
    internal class NotificationConfigurations : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasOne(p => p.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(u => u.UserId);
        }
    }
}
