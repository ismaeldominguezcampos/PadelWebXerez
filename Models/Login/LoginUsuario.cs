using System;
namespace PadelWebXerez.Models
{
    public class LoginUsuario : BaseEntity
    {
        #region campos de la tabla

        public string UserId { get; set; }

        public DateTime IniLogin { get; set; }

        public DateTime? FinLogin { get; set; }

        public string IpMachine { get; set; }

        #endregion

        #region propiedades de Navegación

        public Usuario Usuario { get; set; }

        #endregion
    }
}
