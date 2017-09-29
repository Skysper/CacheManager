using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CacheManager.Caching.Test
{
    public class RangeAnalysisTest
    {
        [Fact]
        public void TestFormat()
        {
            string key = "Noah:cms:{a-z}{0-99}";
            RangeAnalysis ra = new RangeAnalysis(key);
            string result = ra.Format();
            Assert.Equal("Noah:cms:{0}{1}", result);

            key = "{a-z}:{0-11}:{11-220}ssss";
            ra = new RangeAnalysis(key);
            Assert.Equal("{0}:{1}:{2}ssss", ra.Format());

            key = "{aa-zz}:{0-11}:{11-220}ssss";
            ra = new RangeAnalysis(key);
            Assert.Equal(":{0}:{1}ssss", ra.Format());
        }


    }
}
