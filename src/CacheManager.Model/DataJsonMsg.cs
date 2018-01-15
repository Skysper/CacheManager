using System;
using System.Collections.Generic;
using System.Text;

namespace CacheManager.Model
{
    public class DataJsonMsg : JsonMsg
    {
        public DataJsonMsg() { }

        public DataJsonMsg(int ok, string msg):base(ok, msg) { }

        public DataJsonMsg(int ok, string msg, object data) : base(ok, msg) {
            this.Data = data;
        }

        public object Data { set; get; }
    }
}
