using EntityModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
namespace DBProvider
{
    public class DBProvider<T> where T : EntityBaseModel
    {
        protected DbSet<T> dbSet;
        protected DbContext DBC;
        /// <summary>
        /// Constractor for initilizing Database Context
        /// </summary>
        /// <param name="DBC"></param>
        public DBProvider(DbContext DBC)
        {
            this.DBC = DBC;
            this.dbSet = DBC.Set<T>();
        }
        //public void ReadyToUpdate(IList<dynamic> original,List<dynamic> updated)
        //{
        //    List<T> deletList = new List<T>();
        //    foreach (var item in original)
        //    {
        //        dynamic Temp = updated.Where(x => x.Id == item.Id);
        //        if (Temp != null)
        //        {
        //            DBC.Entry(item).CurrentValues.SetValues(Temp);
        //            updated.Remove(Temp);
        //        }
        //        else
        //        {
        //            deletList.Add(item);
        //        }
        //    }
        //    dbSet.RemoveRange(deletList);
        //    foreach (var item in updated)
        //    {
        //        original.Add(item);
        //    }
        //    DBC.SaveChanges();
        //}
        //public void up(T updatedLetter)
        //{
        //    Update(updatedLetter);
        //    //Get original letter from second dbcontext
        //    secondDBContext = new DatabaseContext();
        //    T OriginalLetter = secondDBContext.Set<T>().ToList().First();
        //    var props = typeof(T).GetProperties();
        //    foreach (var prop in props)
        //    {
        //        if (prop.PropertyType.GetInterface(typeof(ICollection<>).FullName) != null)
        //        {
        //            Type propertyType = prop.PropertyType;
        //            Console.WriteLine(propertyType.FullName);
        //            if (prop.Name == "sendLetterFile")
        //                Console.WriteLine(prop.Name);
        //            List<dynamic> deletList = new List<dynamic>();
        //            dynamic originalList = prop.GetValue(OriginalLetter, null);
        //            dynamic updatedList = prop.GetValue(updatedLetter, null);
        //            //Set State of all Database items to deleted.
        //            //foreach (var item in originalList)
        //            //{
        //            //    DBC.Entry(item).State = EntityState.Deleted;
        //            //}
        //            foreach (var item in updatedList)
        //            {
        //                if (item.Id > 0 || item.Id != null)
        //                {
        //                    foreach (var item2 in originalList)
        //                    {
        //                        if (item.Id == item2.Id)
        //                    }
        //                }
        //                else
        //                {
        //                    originalList.Add(item);
        //                }
        //            }
        //            //foreach (var item in originalList)
        //            //    dynamic Temp = updatedList.
        //            //    if (item.Id > 0 && item.Id != null)
        //            //        {
        //            //            dbcontext.Entry(item).CurrentValues.SetValues(upda);
        //            //            updated.Remove(Temp);
        //            //        }
        //            //        else
        //            //        {
        //            //            deletList.Add(item);
        //            //        }                        
        //            //foreach (var item in deletList)
        //            //{
        //            //    original.Remove(item);
        //            //}
        //            //DBC.Set<SendLetterFile>().RemoveRange(deletList);
        //            //foreach (var item in updated)
        //            //{
        //            //    original.Add(item);
        //            //}
        //            //DBC.SaveChanges();
        //        }
        //        //List<object> list = new List<object>();
        //        //prop.SetValue(list, Convert.ChangeType(UbdatedLetter, prop.PropertyType));
        //        //string entityName = prop.Name;
        //        //ObjectContext objContext = ((IObjectContextAdapter)dbcontext).ObjectContext;
        //        //MetadataWorkspace workspace = objContext.MetadataWorkspace;
        //        //Console.WriteLine(entityName + workspace.GetItems<EntityType>(DataSpace.CSpace).Any(e => e.BaseType == prop.BaseType));
        //    }
        //    secondDBContext.Dispose();
        //    DBC.SaveChanges();
        //}
        public void Add(T Entity)
        {
            dbSet.Add(Entity);
            //DBC.SaveChanges();
            // if (DBC.SaveChanges() == 0) throw new Exception(Errors.AddEntityException);
        }
        public void Add(List<T> Entities)
        {
            foreach (T Entity in Entities)
            {
                dbSet.Add(Entity);
            }
            //if (DBC.SaveChanges() != Entities.Count) throw new Exception(Errors.AddEntityException);
        }
        /// <summary>
        /// Read Specific Entity by Id with all it's relations.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public int Count
        {
            get
            {
                return dbSet.Count();
            }
        }
        public T Read(Guid Id)
        {
            //return DBC.Set<T>().Where(x => x.I
            return dbSet.Find(Id);
        }
        public List<T> Read(Expression<Func<T, bool>> filter = null)
        {
            DBC.Configuration.LazyLoadingEnabled = false;
            if (filter != null)
            {
                return dbSet.Where(filter).ToList();
            }
            else
            {
                return dbSet.ToList();
            }
        }
        /// <summary>
        /// Read with explicit Loding
        /// </summary>
        /// <param name="filter">A lambda Expreesion </param>
        /// <param name="orderBy"></param>
        /// <param name="includeProperties">Import propesrties you want to include to your loding(properties must be separeted by ",") </param>
        /// <returns></returns>
        public IEnumerable<T> Read(Expression<Func<T, bool>> filter = null,
                                            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                            string includeProperties = "")
        {
            DBC.Configuration.LazyLoadingEnabled = false;
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }
        public IEnumerable<T> Read(string filter,
                                    string includeProperties = "")
        {
            DBC.Configuration.LazyLoadingEnabled = true;
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (string.IsNullOrEmpty(includeProperties))
            {
                if (includeProperties.Contains(','))
                {
                    foreach (var includeProperty in includeProperties.Split
                        (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(includeProperty);
                    }
                }
                else
                {
                    query = query.Include(includeProperties);
                }
            }
            return query.ToList();
        }
        public IQueryable<T> Read<TResult>(IQueryable<T> query, int pageNumber, int pageSize, Expression<Func<T, TResult>> orderByModel, bool isAscendingOrder, out int rowsCount)
        {
            DBC.Configuration.LazyLoadingEnabled = false;
            if (pageSize <= 0) pageSize = 20;
            //Total result count
            rowsCount = query.Count();
            //If page number should be > 0 else set to first page
            if (rowsCount <= pageSize || pageNumber <= 0) pageNumber = 1;
            //Calculate nunber of rows to skip on pagesize
            int excludedRows = (pageNumber - 1) * pageSize;
            query = isAscendingOrder ? query.OrderBy(orderByModel) : query.OrderByDescending(orderByModel);
            //Skip the required rows for the current page and take the next records of pagesize count
            return query.Skip(excludedRows).Take(pageSize);
        }
        //public IList<T> Read(Expression<Func<T, bool>> orderByModel,Expression<Func<T, bool>> filter = null, int pageSize=0, bool isAscendingOrder=false)
        //{
        //    DBC.Configuration.LazyLoadingEnabled = false;
        //    IQueryable<T> query = dbSet;

        //    if (filter != null)
        //    {
        //        query = query.Where(filter);
        //    }
        //    if (pageSize <= 0) pageSize = 5;
        //    query = isAscendingOrder ? query.OrderBy(orderByModel) : query.OrderByDescending(orderByModel);
        //    //Skip the required rows for the current page and take the next records of pagesize count
        //    return query.Skip(pageSize).Take(pageSize).ToList();
        //}

        public void Remove(T Entity)
        {
            DBC.Entry(Entity).State = EntityState.Deleted;
        }
        public void RemoveRange(List<T> Entities)
        {
            dbSet.RemoveRange(Entities);
        }
        public void Update(T Entity)
        {
            DBC.Entry(Entity).State = EntityState.Modified;
            
        }
        public void Update(List<T> Entities)
        {
            foreach (T Entity in Entities)
            {
                DBC.Entry(Entity).State = EntityState.Modified;
            }
        }
        public void saveChanges()
        {
            DBC.SaveChanges();
            Dispose();

        }

        public void saveChangesWithoutDispose()
        {
            DBC.SaveChanges();
        }
        public void Dispose()
        {
            DBC?.Dispose();
        }
    }
}
