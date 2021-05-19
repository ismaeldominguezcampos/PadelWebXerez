using System;
using PadelWebXerez.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PadelWebXerez
{
    internal class CobroConfiguration : EntityTypeConfiguration<Cobro>
    {
        public override void Map(EntityTypeBuilder<Cobro> builder)
        {
            builder.ToTable("Cobros", schema: "PadelWebXerez");

            builder.Property(e => e.FormaPago).IsRequired().HasMaxLength(1);

            builder.Property(e => e.NumTarjeta).IsRequired().HasMaxLength(25);

            builder.Property(e => e.Timestamp).IsConcurrencyToken();

            builder.HasKey(e => e.Id);

            builder.HasOne(e => e.Reserva).WithMany()
                .HasForeignKey(e => e.ReservaId);
        }

    }
}