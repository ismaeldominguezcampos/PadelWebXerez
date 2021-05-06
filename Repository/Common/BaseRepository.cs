using System;
using PadelWebXerez.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore.Storage;
using Syncfusion.EJ2.Base;
using System.ComponentModel;
using System.Linq;
using Syncfusion.EJ2.Linq;
using System.Collections;
using EFCore.BulkExtensions;
using PadelWebXerez;

namespace iTunaFishLib.Logic
{
    public class BaseRepository<T> : IDisposable, IRepository<T> where T: BaseEntity, new()
    {
        protected readonly DbSet<T> _table;
        private readonly PadelWebXerezContext _db;
        protected PadelWebXerezContext Context => _db;

        private readonly string _currentUserKey;

        //Propiedades
        public string Tabla { get; set; }
        public string TablaMaestra { get; set; }
        public string KeyMaestra { get; set; }
        public bool UseAuditoria { get; set; }

        public BaseRepository(PadelWebXerezContext context, string currentUserKey)
        {
            _db = context;
            _table = _db.Set<T>();
            _currentUserKey = currentUserKey;
            UseAuditoria = false;
        }

        public string CurrentUserKey
        {
            get
            {
                return _currentUserKey;
            }
        }

        public IEnumerable<T> filtrado(DataManagerRequest dm, IEnumerable<T> DataSource, out int count)
        {
            DataOperations operation = new DataOperations();

           
            if (dm.Search != null && dm.Search.Count > 0)
            {
                DataSource = operation.PerformSearching(DataSource, dm.Search);
            }
            if (dm.Sorted != null && dm.Sorted.Count > 0) //Sorting
            {
                foreach (Sort sort in dm.Sorted)
                {
                    IQueryable<T> DataSourceQueryable = DataSource.AsQueryable<T>();

                    string field = sort.Name + (sort.Direction.ToLower().Equals("ascending") ? "" : " desc");
                    DataSource = DataSourceQueryable.OrderBy(field);
                }

               // DataSource = operation.PerformSorting(DataSource, dm.Sorted);
            }
            if (dm.Where != null && dm.Where.Count > 0)
            {   

                DataSource = operation.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator);
               
            }

            count = 0;

            if (dm.RequiresCounts)
            {
                IQueryable<T> DataSourceKey = DataSource.AsQueryable<T>();
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


        public IEnumerable<L> filtradoListados<L>(DataManagerRequest dm, IQueryable<L> DataSource, out int count)
        {
            DataOperations operation = new DataOperations();

            IEnumerable groupedData = null;

            IEnumerable<L> agrupamiento = null;

            if (dm.Sorted != null && dm.Sorted.Count > 0) //Sorting
            {
                foreach (Sort sort in dm.Sorted)
                {
                    IQueryable<L> DataSourceQueryable = DataSource.AsQueryable<L>();

                    string field = sort.Name + (sort.Direction.ToLower().Equals("ascending") ? "" : " desc");
                    DataSource = DataSourceQueryable.OrderBy(field);
                }

                // DataSource = operation.PerformSorting(DataSource, dm.Sorted);
            }

            count = 0;
            //Indice para montar los predicados
            int i = 0;
            string condicionWhere = "";

            if (dm.Where != null && dm.Where.Count > 0)
            {
                dm.Where.ForEach(filtroWhere =>
                {
                    if (filtroWhere.predicates != null)
                    {
                        condicionWhere = "";

                        filtroWhere.predicates.ForEach(filtro =>
                        {
                           


                            if (filtro.value != null)
                            {
                                string[] field = filtro.Field.Split("___");
                                if(field.Count()>1)
                                {
                                    var tipoDato = field[2];



                                    if (condicionWhere.Equals(""))
                                    {
                                        if (tipoDato == "Entero")
                                        {
                                            condicionWhere = field[0] + "." + field[1] + GetOperador(filtro.Operator, filtro.value.ToString(), tipoDato) + Int32.Parse(filtro.value.ToString());
                                        }
                                        else if (tipoDato == "Cadena")
                                        {
                                            condicionWhere = field[0] + "." + field[1] + GetOperador(filtro.Operator, filtro.value.ToString(), tipoDato) + filtro.value.ToString();
                                        }
                                        else if (tipoDato == "Decimal")
                                        {
                                            condicionWhere = field[0] + "." + field[1] + GetOperador(filtro.Operator, filtro.value.ToString(), tipoDato) + Decimal.Parse(filtro.value.ToString());
                                        }
                                        else if (tipoDato == "Fecha")
                                        {

                                            var fecha = DateTime.Parse(filtro.value.ToString());

                                            condicionWhere = field[0] + "." + field[1] + ">=DateTime(" + fecha.Year + "," + fecha.Month + "," + fecha.Day + ") ";
                                        }


                                    }
                                    else
                                    {
                                        if (tipoDato == "Entero")
                                        {
                                            condicionWhere += " ||  " + field[0] + "." + field[1] + GetOperador(filtro.Operator, filtro.value.ToString(), tipoDato) + Int32.Parse(filtro.value.ToString());
                                        }
                                        else if (tipoDato == "Cadena")
                                        {
                                            condicionWhere += " || " + field[0] + "." + field[1] + GetOperador(filtro.Operator, filtro.value.ToString(), tipoDato) + filtro.value.ToString();
                                        }
                                        else if (tipoDato == "Decimal")
                                        {
                                            condicionWhere += " || " + field[0] + "." + field[1] + GetOperador(filtro.Operator, filtro.value.ToString(), tipoDato) + Decimal.Parse(filtro.value.ToString());
                                        }
                                        else if (tipoDato == "Fecha")
                                        {
                                            var fecha = DateTime.Parse(filtro.value.ToString());
                                            condicionWhere += " AND " + field[0] + "." + field[1] + "<=DateTime(" + fecha.Year + "," + fecha.Month + "," + fecha.Day + ") ";
                                        }



                                    }

                                    i++;
                                }
                               
                            }

                        });

                        if (condicionWhere != "")
                            DataSource = DataSource.Where(condicionWhere);
                    }

                });


            }
            if (dm.RequiresCounts && dm.IsLazyLoad == false)
            {
                IQueryable<L> DataSourceKey = DataSource.AsQueryable<L>();
                count = DataSourceKey.Count();


            }


            if (dm.Skip != 0 && dm.IsLazyLoad == false)
            {
                DataSource = operation.PerformSkip(DataSource, dm.Skip);   //Paging 
            }
            if (dm.Take != 0 && dm.IsLazyLoad == false)
            {
                DataSource = operation.PerformTake(DataSource, dm.Take);
            }


            
            return DataSource.ToList();
        }

        public IEnumerable agruparResultado<L> (DataManagerRequest dm, IEnumerable<L> DataSource, out int count)
        {
            DataOperations operation = new DataOperations();
            count = 0;
            System.Collections.IEnumerable groupedData = null;

            if (dm.IsLazyLoad)
            {
                groupedData = operation.PerformGrouping<L>(DataSource, dm); // Lazy load grouping

                if (dm.OnDemandGroupInfo != null && dm.Group.Count() == dm.OnDemandGroupInfo.Level)
                {
                    count = groupedData.Cast<L>().Count();
                }
                else
                {
                    count = groupedData.Cast<Group>().Count();
                }
                groupedData = operation.PerformSkip(groupedData, dm.OnDemandGroupInfo == null ? dm.Skip : dm.OnDemandGroupInfo.Skip);
                groupedData = operation.PerformTake(groupedData, dm.OnDemandGroupInfo == null ? dm.Take : dm.OnDemandGroupInfo.Take);
            }
            return groupedData;
        }

   


        private string GetOperador(string operador, string valor, string tipoDato)
        {


            if (operador == "equal")
            {
                if (tipoDato == "Entero" || tipoDato == "Decimal")
                    return "=";
                else if (tipoDato == "Fecha")
                    return ">=";
                else
                    return " LIKE ";
            }
            else if (operador == "notequal")
            {
                return "!=";
            }
            else if (operador == "greaterthan")
            {
                return ">";
            }
            else if (operador == "greaterthanorequal")
            {
                return ">=";
            }
            else if (operador == "lessthan")
            {
                return "<";
            }
            else if (operador == "lessthanorequal")
            {
                return "<=";
            }
            else if (operador == "startswith")
            {
                return "LIKE " + valor + "%";
            }
            else if (operador == "endswith")
            {
                return "LIKE %" + valor;
            }
            else if (operador == "contains")
            {
                return "LIKE %" + valor + "%";
            }
            else
            {
                return "";
            }
        }




        public bool ExisteId(int id)
        {
            var query = from tabla in _table
                        where tabla.Id == id
                        select tabla.Id;
            return query.Take(1) != null;
        }

        public bool ExisteId(int? id)
        {
            var query = from tabla in _table
                        where tabla.Id == id
                        select tabla.Id;
            return query.Take(1) != null;
        }

        public void Dispose()
        {
            _db?.Dispose();
        }

        public int Add(T entity)
        {
            _table.Add(entity);
            int result = SaveChanges();
            if (result > 0 && UseAuditoria)
            {
                //Grabar auditoria
                new AuditoriaRepository(Context, _currentUserKey).Add(new Auditoria()
                {
                    Tipo = EnumTipoAuditoria.Insercción.ToString(),
                    Tabla = Tabla,
                    TablaMaestra = TablaMaestra,
                    Key = entity.Id.ToString(),
                    KeyMaestra = entity.GetPropValue<string>(KeyMaestra),
                    Datos = "",
                    UsuarioId = _currentUserKey
                });
            }
            return result;
        }

        public int Add(IList<T> entities)
        {
            _table.AddRange(entities);
            int result = SaveChanges();
            if (result > 0 && UseAuditoria)
            {
                List<Auditoria> auditorias = new List<Auditoria>();
                entities.ForEach(entity =>
                {

                    auditorias.Add(new Auditoria
                    {
                        //Grabar auditoria
                        Tipo = EnumTipoAuditoria.Insercción.ToString(),
                        Tabla = Tabla,
                        TablaMaestra = TablaMaestra,
                        Key = entity.Id.ToString(),
                        KeyMaestra = entity.GetPropValue<string>(KeyMaestra),
                        Datos = "",
                        UsuarioId = _currentUserKey
                    });
                    
                });

                _ = new AuditoriaRepository(Context, _currentUserKey).Add(auditorias);
               
          
            }
            return result;
        }

        public bool AddBulk(IList<T> entities)
        {

            var bulkConfig = new BulkConfig
            {
                CalculateStats = true,
                PreserveInsertOrder = true,
                SetOutputIdentity = true
            };

            Context.BulkInsert(entities, bulkConfig);

            return true;
        }

        public int Update(T entity)
        {
            string cambios = "";
            if (UseAuditoria)
            {
                T objOlder = GetOneNoTracking(entity.Id);
                cambios = Validaciones.GetModificaciones(objOlder.ToString(), entity.ToString());
            }
            _table.Update(entity);
            int result = SaveChanges();
            if (result > 0 && UseAuditoria && cambios.Length > 0)
            {
                //Grabar auditoria
                new AuditoriaRepository(Context, _currentUserKey).Add(new Auditoria()
                {
                    Tipo = EnumTipoAuditoria.Edicción.ToString(),
                    Tabla = Tabla,
                    TablaMaestra = TablaMaestra,
                    Key = entity.Id.ToString(),
                    KeyMaestra = entity.GetPropValue<string>(KeyMaestra),
                    Datos = cambios,
                    UsuarioId = _currentUserKey
                });
            }
            return result;
        }

        public int Update(IList<T> entities)
        {
            _table.UpdateRange(entities);

            return SaveChanges();
        }

        public int Delete(int id, byte[] timeStamp)
        {
            T entity = GetOne(id);
            if (entity.Timestamp != timeStamp)
            {
                return 0;
            }
            else
            {
                Delete(entity);
                //_db.Entry(new T() { Id = id, Timestamp = timeStamp }).State = EntityState.Deleted;      
                return SaveChanges();
            }
        }

        public int Delete(T entity)
        {
            _db.Entry(entity).State = EntityState.Deleted;
            return SaveChanges();
        }

        //public int Delete(List<T> entities)
        //{
        //    //_db.Entry(entities).State = EntityState.Deleted;
        //    _db.RemoveRange(entities);
        //    //_db.Remove(entities);
        //    return SaveChanges();
        //}
     
      public void Delete(List<T> entities)
       {
           entities.ForEach(entity =>  Delete(entity));
       }


        public T GetOne(int? id,out Error error)
        {

            var result = _table.Find(id);
            error = new Error();

            if (result is null)
            {
                ErrorItem item = new ErrorItem()
                {
                    CodError = Resources.Strings.codigo_error_no_existe,
                    Description = String.Format(Resources.Strings.error_codigo_no_existe, "")
                };
                error.Errores.Add(item);
            }

            return result;
        }


        public T GetOne(int? id) => _table.Find(id);

        public T GetOneNoTracking(int id ) => _table.Where(l => l.Id == id).AsNoTracking().FirstOrDefault();

        public List<T> GetSome(Expression<Func<T, bool>> where) => _table.Where(where).ToList();

        public virtual List<T> GetAll() => _table.ToList();

        public List<T> GetAll<TSortField>(Expression<Func<T, TSortField>> orderBy, bool ascending) =>
            (ascending ? _table.OrderBy(orderBy) : _table.OrderByDescending(orderBy)).ToList();

        public List<T> ExecuteQuery(string sql) => _table.FromSqlRaw(sql).ToList();

        public List<T> ExecuteQuery(string sql, object[] sqlParametersObjects) =>
            _table.FromSqlRaw(sql, sqlParametersObjects).ToList();

        internal int SaveChanges()
        {
            try
            {
                return _db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw;
            }
            catch (RetryLimitExceededException ex)
            {
                throw;
            }
            catch (DbUpdateException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }



    }
}
