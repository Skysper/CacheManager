using System;
using CacheManager.Model;
using Dapper;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Text;

namespace CacheManager.Repository
{

    public class CacheKeyRepository : BaseRepository<CacheKey>
    {

        public CacheKeyRepository(string connectionString) : base(connectionString, "CacheKey")
        {

        }



        public override void Add(CacheKey item)
        {
            using (MySqlConnection dbConnection = Connection)
            {
                dbConnection.Open();
                dbConnection.Execute(string.Format("INSERT INTO {0} (Name,Description,AppId) VALUES(@Name,@Description,@AppId)", TableName), item);
            }
        }



        public override void Update(CacheKey item)
        {
            using (MySqlConnection dbConnection = Connection)
            {
                dbConnection.Open();
                dbConnection.Execute(string.Format("Update {0} set Name=@Name,Description=@Description,AppId=@AppId where Id=@Id", TableName), item);
            }
        }

        public List<Model.CacheKey> FindByIds(List<int> ids)
        {
            if (ids.Count == 0)
            {
                return null;
            }

            string idStr = string.Join(",", ids);
            List<Model.CacheKey> list = new List<CacheKey>();

            using (MySqlConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<Model.CacheKey>($"Select * from {TableName} where Id in ({idStr})").ToList();
            }
        }

        public List<Model.CacheKey> FindByPage(int appId, string key, int pageIndex, int pageSize)
        {
            using (MySqlConnection dbConnection = Connection)
            {
                dbConnection.Open();
                StringBuilder str = new StringBuilder($"Select * from {TableName}");

                bool hasCondition = false;
                if (appId > 0)
                {
                    str.Append(" where AppId = ");
                    str.Append(appId);
                    hasCondition = true;
                }

                if (!string.IsNullOrEmpty(key))
                {
                    if (!hasCondition)
                    {
                        str.Append(" where ");
                    }
                    else
                    {
                        str.Append(" and ");
                    }
                    str.Append($" Name like '%{key}%' ");
                }

                str.Append($" order by id desc limit {(pageIndex - 1) * pageSize},{pageSize}");

                return dbConnection.Query<Model.CacheKey>(str.ToString()).ToList();
            }
        }


    }
}