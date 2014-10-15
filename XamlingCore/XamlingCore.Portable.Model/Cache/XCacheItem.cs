using System;

namespace XamlingCore.Portable.Model.Cache
{
    public class XCacheItem<T> where T: class, new()
    {
        public XCacheItem()
        {
            Id = Guid.NewGuid();
        }

        public string Key { get; set; }

        public Guid Id { get; set; }

        public XCacheItem(T item)
        {
            Item = item;
            DateStamp = DateTime.UtcNow;
        }

        public T Item { get; set; }
        public DateTime DateStamp { get; set; }
    }
}
