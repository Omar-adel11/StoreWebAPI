using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Data.Configurations
{
    public class DeliveryMethodConfiguration : IEntityTypeConfiguration<DeliveryMethod>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {
            builder.Property(dm => dm.Price).HasColumnType("decimal(18,2)");
            builder.Property(dm => dm.ShortName).HasColumnType("varchar").HasMaxLength(128);
            builder.Property(dm => dm.Description).HasColumnType("varchar").HasMaxLength(256);
            builder.Property(dm => dm.DeliveryTime).HasColumnType("varchar").HasMaxLength(128);
        }
    }
}
