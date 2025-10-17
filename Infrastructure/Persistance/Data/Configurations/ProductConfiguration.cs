using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Name).HasColumnType("nvarchar").HasMaxLength(256);
            builder.Property(p => p.Description).HasColumnType("nvarchar").HasMaxLength(512);
            builder.Property(p => p.Price).HasColumnType("decimal(18,2)");
            builder.Property(p => p.PictureUrl).HasColumnType("nvarchar").HasMaxLength(256);

            builder.HasOne(p => p.Brand)
                    .WithMany()
                    .HasForeignKey(p => p.BrandId)
                    .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(p => p.Type)
                  .WithMany()
                  .HasForeignKey(p => p.TypeId)
                  .OnDelete(DeleteBehavior.NoAction);


        }
    }
}
