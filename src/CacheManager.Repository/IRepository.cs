using System;
using System.Collections.Generic;
using CacheManager.Model;

namespace CacheManager.Repository
{    
    public interface IRepository<T> where T:BaseEntity
    {
        void Add(T item);
        void Remove(int id);
        void Update(T item);
        T FindById(int id);
        IEnumerable<T> FindAll();   
    }
}
