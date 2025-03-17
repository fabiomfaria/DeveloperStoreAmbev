﻿using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping
{
    public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
    {
        public void Configure(EntityTypeBuilder<SaleItem> builder)
        {
            builder.ToTable("SaleItems");

            builder.HasKey(i => i.Id);
            builder.Property(i => i.Quantity).IsRequired();
            builder.Property(i => i.UnitPrice).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(i => i.Discount).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(i => i.TotalAmount).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(i => i.IsCancelled).IsRequired();

            builder.HasOne(i => i.Product)
                .WithMany()
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}