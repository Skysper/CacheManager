using System;
using System.Collections.Generic;
using System.Text;
using CacheManager.Model;
using MySql.Data.MySqlClient;
using Dapper;
using System.Linq;

namespace CacheManager.Repository
{
    public class AppInfoRepository : BaseRepository<Model.AppInfo>
    {
        public AppInfoRepository(string connectionString) : base(connectionString, "AppInfo")
        {

        }

        public override void Add(AppInfo item)
        {
            using (MySqlConnection dbConnection = Connection)
            {
                dbConnection.Open();
                dbConnection.Execute(string.Format("INSERT INTO {0} (Name,Description,Identity,Type,ConnectionString) VALUES(@Name,@Description,@Identity,@Type,@ConnectionString)", TableName), item);
            }
        }

        public override void Update(AppInfo item)
        {
            using (MySqlConnection dbConnection = Connection)
            {
                dbConnection.Open();
                dbConnection.Execute(string.Format("Update {0} set Name=@Name,Description=@Description,Identity=@Identity,Type=@Type,ConnectionString=@ConnectionString where Id=@Id", TableName), item);
            }
        }

        public List<AppInfo> FindByPage(string key, int pageIndex, int pageSize)
        {
            using (MySqlConnection dbConnection = Connection)
            {
                StringBuilder str = new StringBuilder($"Select * from {TableName}");
                if (!string.IsNullOrEmpty(key))
                {
                    str.Append($" where Name like '%{key}%'");
                }
                int limit = (pageIndex - 1) * pageSize;
                str.Append($" order by id desc limit {limit},{pageSize} ");
                dbConnection.Open();
                return dbConnection.Query<AppInfo>(str.ToString()).ToList();
            }
        }
    }
}
