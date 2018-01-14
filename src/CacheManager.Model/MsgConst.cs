using System;
using System.Collections.Generic;
using System.Text;

namespace CacheManager.Model
{
    public class MsgConst
    {
        public static JsonMsg OK = new JsonMsg() { Ok = 1, Msg = "" };

        public static JsonMsg Error = new JsonMsg() { Ok = 0, Msg = "" };

        public static JsonMsg GetOkMsg(string msg = "")
        {
            OK.Msg = msg;
            return OK;
        }

        public static JsonMsg GetErrorMsg(string msg = "")
        {
            Error.Msg = msg;
            return Error;
        }

    }
}
