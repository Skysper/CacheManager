using System;
using Xunit;
using System.Collections.Generic;
using System.Linq;

namespace CacheManager.Caching.Test
{
    public class KeyManagerTest
    {
        [Fact]
        public void TestGetPosition()
        {
            List<Ranges> list = InitRanges();
            int c = KeyManager.GetKeysCount(list);
            Assert.Equal(260, c);
        }

        [Fact]
        public void TestGetPagedKeys()
        {
            List<Ranges> list = InitRanges();
            var keys = KeyManager.GetPagedKeys("Noah.CMS.{0}.{1}", list, 2, 8);
            Assert.Equal("Noah.CMS.Z.8", keys[0]);
            Assert.Equal("Noah.CMS.Y.5", keys[7]);
            Assert.Equal(8, keys.Count);

            keys = KeyManager.GetPagedKeys("Noah.CMS.{0}.{1}", list, 11, 25);
            Assert.Equal(10, keys.Count);

            keys = KeyManager.GetPagedKeys("Noah.CMS.{0}.{1}", list, 12, 25);
            Assert.Equal(0, keys.Count);
        }

        [Fact]
        public void KeysTest()
        {
            string keyFormat = "noah:cms:column:{0}{1}";
            List<Caching.Ranges> list = new List<Caching.Ranges>();
            Caching.Ranges r = new Caching.CharRanges();
            r.From = 'A';
            r.To = 'Z';
            list.Add(r);

            r = new Caching.NumberRanges();
            r.From = 10;
            r.To = 120;
            list.Add(r);

            var keys = Caching.KeyManager.GetPagedKeys(keyFormat, list, 2, 100);

            Assert.Equal("noah:cms:column:A110",keys.First());
        }

        private List<Ranges> InitRanges()
        {
            List<Ranges> list = new List<Ranges>();
            CharRanges r = new CharRanges();
            r.From = 'Z';
            r.To = 'A';
            list.Add(r);
            r = new CharRanges();
            r.From = '0';
            r.To = '9';
            list.Add(r);
            return list;
        }

    }
}
