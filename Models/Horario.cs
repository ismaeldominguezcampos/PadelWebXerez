using System;
using System.Collections.Generic;
using iTunaFishLib.Models;
using Microsoft.AspNetCore.Identity;

namespace PadelWebXerez.Models
{
    public class Horario : BaseEntity
    {
        #region propiedades

        public DateTime HoraApertura { get; set; }

        public DateTime HoraCierre { get; set; }

        public DayOfWeek DiaSemana { get; set; }

        #endregion

        #region propiedades de navegacion

        public Pista Pista { get; set; }

        #endregion

        #region Valores por defecto

        #endregion

        #region override

        public override string ToString()
        {
            string str = "Código: " + string.Format("{0:N0}", this.HoraApertura);
            str += "';Precio por hora: " + string.Format("{0:N0}", this.HoraCierre);
            str += "';'Nombre: " + string.Format("{0}", this.DiaSemana);

            return str;
        }

        #endregion
    }
}
