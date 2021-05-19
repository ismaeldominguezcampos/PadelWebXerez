using System;
using PadelWebXerez.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PadelWebXerez
{
    internal class PistaConfiguration : EntityTypeConfiguration<Pista>
    {
        public override void Map(EntityTypeBuilder<Pista> builder)
        {
            builder.ToTable("Pistas", schema: "PadelWebXerez");

            builder.Property(e => e.Numero).IsRequired().HasMaxLength(1);

            builder.Property(e => e.PrecioPorHora).HasColumnType("decimal(18,3)");
            builder.Property(e => e.Disponible).HasMaxLength(256);
            builder.Property(e => e.PrecioPorHora).HasMaxLength(256);

            builder.Property(e => e.Timestamp).IsConcurrencyToken();

            builder.HasKey(e => e.Id);

            builder.HasOne(e => e.Horario).WithMany()
                .HasForeignKey(e => e.HorarioId);
        }
    }
}
