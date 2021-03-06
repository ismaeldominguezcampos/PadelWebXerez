using System;
using Microsoft.EntityFrameworkCore;
using PadelWebXerez.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


namespace PadelWebXerez
{
    public class PadelWebXerezContext : IdentityDbContext<Usuario, Role, string,
        UsuarioClaim, UsuarioRole, UsuarioLogin, RoleClaim, UsuarioToken>
    {
        public PadelWebXerezContext(DbContextOptions<PadelWebXerezContext> options)
            : base(options)
        {
        }
        //Pista
        public DbSet<Pista> Pistas { get; set; }

        //Horarios
        public DbSet<Horario> Horarios { get; set; }
        //Usuario
        public DbSet<Usuario> Usuarios { get; set; }
        //Cobro
        public DbSet<Cobro> Cobros { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Usuario
            builder.AddConfiguration(new UsuarioConfiguration());

            //Pista
            builder.AddConfiguration(new PistaConfiguration());

            //Horario
            builder.AddConfiguration(new HorarioConfiguration()); 
            
            //Cobro
            builder.AddConfiguration(new CobroConfiguration());



        }

    }
}
