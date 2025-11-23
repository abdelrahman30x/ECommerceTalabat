using ECommerceG02.Domian.Models.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Presistence.Configurations.OrderConfig
{
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder.Property(O => O.Subtotal)
                   .HasColumnType("decimal(10,3)");

            builder.Property(O => O.UserEmail)
                   .HasColumnType("varchar")
                   .HasMaxLength(100);

            builder.Property(O => O.OrderDate)
                   .HasColumnType("datetimeoffset");

            builder.Property(O => O.OrderStatus)
                   .HasConversion<string>()
                   .HasColumnType("varchar")
                   .HasMaxLength(50);

            builder.HasOne(O => O.DeliveryMethod)
                   .WithMany()
                   .HasForeignKey(O => O.DeliveryMethodId);

            builder.OwnsOne(O => O.ShipToAddress);
        }

    }

}
