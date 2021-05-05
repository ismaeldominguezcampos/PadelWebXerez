using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace PadelWebXerez.Models
{
    public class Usuario : IdentityUser
    {
        #region propiedades
       
        public string DNI { get; set; }
        
        public string Nombre { get; set; }
       
        public string Apellidos { get; set; }
        
        public string Codigo { get; set; }
        
        public string Direccion { get; set; }
        
        public string CodPostal { get; set; }

        public string Telefono { get; set; }

        public DateTime? FechaNacimiento { get; set; }

        public DateTime FechaIngreso { get; set; }

        public bool Activo { get; set; }

        #endregion

        #region Propiedades no mapeadas

        public string FullName { get; set; }
        public string EmailUser { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string PasswordNueva { get; set; }

        // TODO: Eliminar y mapear para una posterior migración. Una vez realizada borrar
        public bool IsDelete { get; set; }

        public string NombreCompleto
        {
            get
            {
                return this.Nombre + " " + this.Apellidos;
            }
        }

        public string Iniciales
        {
            get
            {
                //Extraer las 2 letras para el Icono
                string[] nombre = this.NombreCompleto.Split(' ');
                string inicial = "";
                if (nombre.Length > 0)
                {
                    inicial += nombre[0].Length > 0 ? nombre[0].Substring(0, 1) : "";
                }
                if (nombre.Length > 1 && nombre[1].Length > 0)
                {
                    inicial += nombre[1].Substring(0, 1);
                }
                else
                {
                    inicial += nombre[0].Length > 1 ? nombre[0].Substring(1, 1) : "";
                }
                return inicial;
            }
        }

        public bool IsSystemRoot
        {
            get
            {
                if (this.Codigo == null)
                    return false;
                else
                    return this.Codigo.StartsWith("@#@#");
            } 
        }

        #endregion

        #region Valores por defecto

        public Usuario()
        {
            this.FechaIngreso = DateTime.Now;
        }

        #endregion

        #region Auditoria

        public string Key
        {
            get
            {
                return this.Id;
            }
        }

        public string KeyMaestra
        {
            get
            {
                return "";
            }
        }

        #endregion

        #region override

        public override string ToString()
        {
            string str = "Código: " + string.Format("{0}", this.Codigo);
            str += "';'Login: " + string.Format("{0}", this.UserName);
            str += "';'Nombre: " + string.Format("{0}", this.Nombre);
            str += "';'Apellidos: " + string.Format("{0}", this.Apellidos);
            str += "';'DNI: " + string.Format("{0}", this.DNI);
            str += "';'Teléfono: " + string.Format("{0}", this.Telefono);
            str += "';'Fecha de Nacimiento: " + string.Format("{0}", this.FechaNacimiento);
            str += "';'Fecha de Ingreso: " + string.Format("{0}", this.FechaIngreso);
            str += "';'Email: " + string.Format("{0}", this.Email);
            str += "';'Código Postal: " + string.Format("{0}", this.CodPostal);
            str += "';'Dirección: " + string.Format("{0}", this.Direccion);
            str += "';'Password: " + string.Format("{0}", this.PasswordHash);

            return str;
        }

        #endregion
    }
}
