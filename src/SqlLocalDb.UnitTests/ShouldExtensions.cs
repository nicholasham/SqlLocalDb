using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Should;

namespace SqlLocalDb.UnitTests
{
    public static class ShouldExtensions
    {
        public static void ShouldAllExist(this IEnumerable<FileInfo> files)
        {
            files.ForEach(info => info.Exists.ShouldBeTrue("info.Exists"));
        }

        public static void ShouldNotExist(this IEnumerable<FileInfo> files)
        {
            files.ForEach(info => info.Exists.ShouldBeFalse("info.Exists"));
        }

        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
            {
                action(item);
            }
        }
    }
}