using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PadelWebXerez.Models
{
    public class Reserva : BaseEntity
    {
        #region Campos de la tabla

        public bool Disponible { get; set; }

        public int UsuarioId { get; set; }

        public int PistaId { get; set; }

        public int HoraInicio { get; set; }

        public int HoraFin { get; set; }

        #endregion

        #region Propiedades de Navegacion

        public Cobro Cobro { get; set; }

        public Usuario Usuario { get; set; }

        public Horario Horario { get; set; }

        #endregion

        #region override

        public override string ToString()
        {
            string str = "Disponible: " + string.Format("{0}", this.Disponible);

            return str;
        }

        #endregion
    }
}
