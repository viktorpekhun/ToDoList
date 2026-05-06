using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDoList.Domain.Entities;

namespace ToDoList.Infrastructure.Persistence.Configurations
{
    public class ToDoItemConfiguration : IEntityTypeConfiguration<ToDoItem>
    {
        public void Configure(EntityTypeBuilder<ToDoItem> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Description)
                .IsRequired(false)
                .HasMaxLength(500);

            builder.Property(e => e.CreatedAt)
                .IsRequired();

            builder.Property(e => e.UpdatedAt)
                .IsRequired();

            builder.Property(e => e.DueDate)
                .IsRequired(false);

            builder.Property(e => e.Status)
                .IsRequired()
                .HasColumnType("tinyint");

            builder.HasIndex(e => e.Title);
            builder.HasIndex(e => e.UpdatedAt);
            builder.HasIndex(e => e.DueDate);
            builder.HasIndex(e => e.Status);
        }
    }
}
