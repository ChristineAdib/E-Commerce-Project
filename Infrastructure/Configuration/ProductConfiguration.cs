using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Configuration
{
    public class ProductConfiguration: IEntityTypeConfiguration<Product>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.ProductName)
                .IsRequired()
                .HasMaxLength(200);
            builder.Property(p => p.Price)
                .HasColumnType("decimal(18,2)");
            builder.Property(p => p.Description)
                .HasMaxLength(1000);
            builder.Property(p => p.ImageUrl)
                .HasMaxLength(500);
            // Relationship
            builder.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
