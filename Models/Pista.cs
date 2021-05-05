using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace PadelWebXerez.Models
{
    public class Pista : BaseEntity
    {
        #region propiedades

        public int Numero { get; set; }

        public decimal PrecioPorHora { get; set; }

        public bool Disponible { get; set; }
        
        public int HorarioId { get; set; }

        #endregion

        #region propiedades de navegacion
           public Horario Horario { get; set; }
        #endregion

        #region Valores por defecto

        public Pista()
        {
        }

        #endregion

        #region override

        public override string ToString()
        {
            string str = "Código: " + string.Format("{0}", this.Numero);
            str += "';Precio por hora: " + string.Format("{0}", this.PrecioPorHora);
            str += "';'Nombre: " + string.Format("{0}", this.Disponible);

            return str;
        }

        #endregion
    }
}
