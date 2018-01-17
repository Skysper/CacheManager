using System;
using System.Collections.Generic;
using System.Text;

namespace CacheManager.Model
{
    public class JsonMsg
    {
        public JsonMsg()
        {

        }

        public JsonMsg(int ok):this(ok,"")
        {
        }
        public JsonMsg(int ok, string msg)
        {
            this.Ok = ok;
            this.Msg = msg;
        }

        public int Ok { set; get; }

        public string Msg { set; get; }
    }
}
