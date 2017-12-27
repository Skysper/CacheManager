﻿using System;
using System.Collections.Generic;

namespace CacheManager.Caching
{
    public interface ICache
    {
        string Query(string key);

        IEnumerable<string> Query(string[] keys);

        bool Clear(string key);

        string QueryWithType(string key, CacheKeyType type);

        CacheKeyType Type(string key);
    }
}
