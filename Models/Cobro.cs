using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace PadelWebXerez.Models
{
    public class Cobro : BaseEntity
    {
        #region Campos de la tabla

        public int FormaPago { get; set; }

        public int NumTarjeta { get; set; }

        public int ReservaId { get; set; }

        #endregion

        #region Propiedades de navegacion

        public Reserva Reserva { get; set; }

        #endregion

        #region Valores por defecto

        public Cobro()
        {
        }

        #endregion

        #region override

        public override string ToString()
        {
            string str = "Forma de pago: " + string.Format("{0}", this.FormaPago);
            str += "Numero de tarjeta" + string.Format("{0}", this.NumTarjeta);

            return str;
        }

        #endregion
    }
}
