using System;
using PadelWebXerez.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PadelWebXerez
{
    internal class UsuarioConfiguration : EntityTypeConfiguration<Usuario>
    {
        public override void Map(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuarios", schema: "PadelWebXerez");

            builder.Property(e => e.UserName).HasMaxLength(256);

            builder.Property(e => e.NormalizedUserName).HasMaxLength(256);

            builder.Property(e => e.Email).HasMaxLength(256);

            builder.Property(e => e.NormalizedEmail).HasMaxLength(256);

            builder.Property(e => e.DNI).IsRequired().HasMaxLength(25);

            builder.Property(e => e.Nombre).IsRequired().HasMaxLength(256);

            builder.Property(e => e.Apellidos).HasMaxLength(256);

            builder.Property(e => e.Codigo).IsRequired().HasMaxLength(5);

            builder.Property(e => e.Direccion).HasMaxLength(256);

            builder.Property(e => e.CodPostal).HasMaxLength(25);

            builder.Property(e => e.Telefono).HasMaxLength(25);

            builder.Property(e => e.FechaNacimiento).HasColumnType("datetime");

            builder.Property(e => e.FechaIngreso).HasColumnType("datetime");

            builder.Property(e => e.ConcurrencyStamp).IsConcurrencyToken();

            builder.HasKey(e => e.Id);

            builder.HasIndex(e => e.NormalizedUserName).HasName("UserNameIndex").IsUnique();

            builder.HasIndex(e => e.NormalizedEmail).HasName("EmailIndex");

            // Each User can have many UserClaims
            builder.HasMany<UsuarioClaim>().WithOne().HasForeignKey(uc => uc.UserId).IsRequired();

            // Each User can have many UserLogins
            builder.HasMany<UsuarioLogin>().WithOne().HasForeignKey(ul => ul.UserId).IsRequired();

            // Each User can have many UserTokens
            builder.HasMany<UsuarioToken>().WithOne().HasForeignKey(ut => ut.UserId).IsRequired();

            // Each User can have many entries in the UserRole join table
            builder.HasMany<UsuarioRole>().WithOne().HasForeignKey(ur => ur.UserId).IsRequired();
        }
    }
}
