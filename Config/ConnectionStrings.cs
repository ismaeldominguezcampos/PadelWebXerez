using System;
using Microsoft.Extensions.Configuration;
using PadelWebXerez.Models;
using Microsoft.EntityFrameworkCore;

namespace PadelWebXerez
{
    public class ConnectionStrings
    {
        public EnumServersBd TypeServerBD { get; set; } = EnumServersBd.SqlServer;
        public string Hostname { get; set; }
        public string Database { get; set; }
        public string User { get; set; }
        public string Password { get; set; }

        public ConnectionStrings(string strGestorBd)
        {
            // Obtener el gestor de base de datos a utilizar
            if (strGestorBd == null || strGestorBd.Length == 0)
            {
                TypeServerBD = EnumServersBd.SqlServer;
            }
            else
            {
                if (Enum.TryParse(strGestorBd, out EnumServersBd enumServersBd))
                    TypeServerBD = enumServersBd;
                else
                    TypeServerBD = EnumServersBd.SqlServer;

            }
        }

        public string GetConnectionString()
        {
            string strConnection;

            switch (TypeServerBD)
            {
                case EnumServersBd.SqlServer:
                    strConnection = @"Server=" + Hostname + ";Database=" + Database + ";User Id=" +
                        User + ";Password=" + Password;
                    break;
                default:
                    strConnection = @"Server=" + Hostname + ";Database=" + Database + ";User Id=" +
                        User + ";Password=" + Password +
                        ";Trusted_Connection=False;MultipleActiveResultSets=true";
                    break;
            }

            return strConnection;
        }


    }

}
