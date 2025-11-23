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
    public class OrderltemConfigurations : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("OrderItems");
            builder.Property(D => D.Price).HasColumnType("decimal(8,2)");
            builder.OwnsOne(OI => OI.Product);

        }
    }
    
}
