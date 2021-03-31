//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;

//namespace ArchiveBusinessModel
//{
//    public interface ICrud<T>
//    {
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="Entity"></param>
//        /// <returns></returns>
//        void Add(T Entity);
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="Entities"></param>
//        /// <returns></returns>
//        void Add(List<T> Entities);
//        /// <summary>
//        /// Read T from Entity by Id
//        /// </summary>
//        /// <param name="Id"></param>
//        /// <returns></returns>
//        T Read(int Id);
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <returns></returns>
//        IQueryable<T> Read<T, TResult>(IQueryable<T> query,int pageNumber,int pageSize, Expression<Func<T,TResult>> orderByModel,bool isAscendingOrder,out int rowCount);
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <returns></returns>
//        Remove(T Entity);
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <returns></returns>
//        void Ubdate(List<T> Entities);
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="Entity"></param>
//        /// <returns></returns>
//        void Ubdate(T Entity);
//    }
//}
