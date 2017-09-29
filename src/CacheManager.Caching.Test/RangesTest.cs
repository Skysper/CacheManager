using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CacheManager.Caching.Test
{
    public class RangesTest
    {
        [Fact]
        public void MinusTest()
        {
            var r = new RangeValue(10);
            var result = r - 1;

            Assert.Equal(9, result);
        }
    }

    public class CharRangesTest
    {
        private CharRanges Init()
        {
            CharRanges cr = new CharRanges();
            cr.From = 'A';
            cr.To = 'Z';
            return cr;
        }
        [Fact]
        public void CountTest()
        {
            var cr = Init();
            Assert.Equal(26, cr.Count());
        }

        [Fact]
        public void IndexTest()
        {
            var cr = Init();
            Assert.Equal<char>('B', cr.Index(1));
        }

        [Fact]
        public void OutBoundaryTest()
        {
            var cr = Init();
            Assert.True(cr.OutBoundary((char)('Z'+1)));
            Assert.False(cr.OutBoundary('B'));
        }

        [Fact]
        public void NextTest()
        {
            var cr = Init();
            Assert.Equal<char>('C', cr.Next('B'));
        }

    }
        
}
