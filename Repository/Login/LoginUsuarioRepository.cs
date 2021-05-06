using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PadelWebXerez.Models;
using Syncfusion.EJ2.Base;

namespace PadelWebXerez
{
    public class LoginUsuarioRepository : BaseRepository<LoginUsuario>
    {
        
        public LoginUsuarioRepository(PadelWebXerezContext context, string currentUserKey) : base(context, currentUserKey)
        {
        }

        public List<LoginUsuario> GetAll(DataManagerRequest dm, out int count)
        {
            IEnumerable<LoginUsuario> DataSource = Context.LoginUsuarios.OrderBy(u => u.IniLogin);
            DataOperations operation = new DataOperations();
            if (dm.Search != null && dm.Search.Count > 0)
            {
                DataSource = operation.PerformSearching(DataSource, dm.Search);
            }
            if (dm.Sorted != null && dm.Sorted.Count > 0) //Sorting 
            {
                DataSource = operation.PerformSorting(DataSource, dm.Sorted);
            }
            if (dm.Where != null && dm.Where.Count > 0)
            {
                DataSource = operation.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator);
            }

            count = 0;
            if (dm.RequiresCounts)
            {
                IQueryable<LoginUsuario> DataSourceKey = DataSource.AsQueryable<LoginUsuario>();
                count = DataSourceKey.Count();

            }
            if (dm.Skip != 0)
            {
                DataSource = operation.PerformSkip(DataSource, dm.Skip);   //Paging 
            }
            if (dm.Take != 0)
            {
                DataSource = operation.PerformTake(DataSource, dm.Take);
            }

            return DataSource.ToList();
        }

        public List<LoginUsuario> GetAllByUser(string UsuarioId)
        {
            IEnumerable<LoginUsuario> DataSource = Context.LoginUsuarios
                .Where(u => u.UserId == UsuarioId).OrderByDescending(u => u.IniLogin);

            return DataSource.ToList();
        }

        public int Add(LoginUsuario loginUsuario, out Error error)
        {
            error = ValidateModelo(loginUsuario);

            //Si existen errores devolver no almacenar
            if (error.HasError())
            {
                return 0;
            }
            else
            {
                return Add(loginUsuario);
            }
        }

        public int Update(LoginUsuario loginUsuario, out Error error)
        {
            error = ValidateModelo(loginUsuario);

            //Si existen errores devolver no almacenar
            if (error.HasError())
            {
                return 0;
            }
            else
            {
                return Update(loginUsuario);
            }
        }

        public int Delete(LoginUsuario loginUsuario, out Error error)
        {
            //TODO: Comprobar si se puede eliminar
            error = new Error();

            return Delete(loginUsuario);

        }

        private Error ValidateModelo(LoginUsuario loginUsuario)
        {
            Error error = new Error();

            return error;
        }

        public int CerrarConexion (string usuarioId, out Error error)
        {
            error = new Error();
            List<LoginUsuario> loginUsuarios = Context.LoginUsuarios
                .Where(l => l.UserId == usuarioId && l.FinLogin == null).OrderByDescending(l => l.IniLogin).ToList();

            if (loginUsuarios.Count > 0)
            {
                //Actualizar el registro de login del usuario
                foreach (LoginUsuario loginUsuario in loginUsuarios)
                {
                    loginUsuario.FinLogin = DateTime.Now;
                    Update(loginUsuario, out error);
                }
            }

            return error.HasError() ? 0 : 1;
        }

        public bool PoseeConexiones (string usuarioId)
        {
            List<LoginUsuario> loginUsuarios = Context.LoginUsuarios
                .Where(l => l.UserId == usuarioId && l.FinLogin == null).ToList();

            return loginUsuarios.Count > 0;
        }

    }
}
