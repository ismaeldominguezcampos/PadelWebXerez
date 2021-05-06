using System;
using PadelWebXerez.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PadelWebXerez
{
    internal class LoginUsuarioConfiguration : EntityTypeConfiguration<LoginUsuario>
    {
        public override void Map(EntityTypeBuilder<LoginUsuario> builder)
        {
            builder.ToTable("LoginUsuarios", schema: "PadelWebXerez");

            builder.Property(e => e.UserId).HasMaxLength(450);

            builder.Property(e => e.IniLogin).HasColumnType("datetime");

            builder.Property(e => e.FinLogin).HasColumnType("datetime");

            builder.Property(e => e.IpMachine).HasMaxLength(100);

            builder.Property(e => e.Timestamp).IsConcurrencyToken();

            builder.HasKey(e => e.Id);

            builder.HasOne(e => e.Usuario).WithMany()
                .HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.ClientSetNull);

        }
    }
}
