using CacheManager.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Linq;

namespace CacheManager.Repository
{
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        protected string ConnectionString { private set; get; }

        /// <summary>
        /// 表名称
        /// </summary>
        protected string TableName { private set; get; }

        public BaseRepository(string connectionString, string tableName)
        {
            ConnectionString = connectionString;
            TableName = tableName;
        }

        internal MySqlConnection Connection
        {
            get
            {
                return new MySqlConnection(ConnectionString);
            }
        }


        public IEnumerable<T> FindAll()
        {
            using (MySqlConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<T>($"Select * from {TableName}");
            }
        }

        public T FindById(int id)
        {
            using (MySqlConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<T>($"Select * from {TableName} where Id={id}").SingleOrDefault();
            }
        }

        public void Remove(int id)
        {
            using (MySqlConnection dbConnection = Connection)
            {
                dbConnection.Open();
                dbConnection.Execute($"Delete from {TableName} where Id={id}");
            }
        }

        public virtual void Add(T item)
        {
            throw new NotImplementedException();
        }

        public virtual void Update(T item)
        {
            throw new NotImplementedException();
        }
    }
}
