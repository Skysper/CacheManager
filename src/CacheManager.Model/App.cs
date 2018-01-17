using System;
using System.Collections.Generic;
using System.Text;

namespace CacheManager.Model
{
    public class App:BaseEntity
    {
        public App() { }
        public App(int id, string name, string description, string identity, int type, string connectionString)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.Identity = identity;
            this.Type = type;
            this.ConnectionString = connectionString;
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// 简介
        /// </summary>
        public string Description { set; get; }
        /// <summary>
        /// 标识
        /// </summary>
        public string Identity { set; get; }
        /// <summary>
        /// 类型
        /// </summary>
        public int Type { set; get; }
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString { set; get; }

    }
}
