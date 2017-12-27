using System;
using System.Collections.Generic;
using System.Text;

namespace CacheManager.Caching
{
    /// <summary>
    /// 常用常量
    /// </summary>
    public class CommonConst
    {
        public static char[] REDIS_MULTI_VALUE_SPLIT_CHARS = { '\r', '\n' };

        public static String NEW_LINE = "\r\n";

        public static String INLINE_SPLIT_CHAR = "$$";
        
    }
}
