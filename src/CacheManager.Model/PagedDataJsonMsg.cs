using System;
using System.Collections.Generic;
using System.Text;

namespace CacheManager.Model
{
    public class PagedDataJsonMsg : DataJsonMsg
    {
        public PagedDataJsonMsg() { }

        public PagedDataJsonMsg(int ok, string msg) : base(ok, msg) { }

        public PagedDataJsonMsg(int ok, string msg, object data, int count) : base(ok, msg, data)
        {
            this.Count = count;
        }

        public int Count { set; get; }
    }
}
