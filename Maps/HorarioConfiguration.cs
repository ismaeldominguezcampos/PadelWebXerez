using System;
using PadelWebXerez.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PadelWebXerez
{
    internal class HorarioConfiguration : EntityTypeConfiguration<Horario>
    {
        public override void Map(EntityTypeBuilder<Horario> builder)
        {
            builder.ToTable("Horarios", schema: "PadelWebXerez");

            builder.Property(e => e.HoraApertura).HasColumnType("datetime");

            builder.Property(e => e.HoraCierre).HasColumnType("datetime");

            builder.Property(e => e.DiaSemana);

            builder.Property(e => e.Timestamp).IsConcurrencyToken();

            builder.HasKey(e => e.Id);
        }
    }
}
