using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamlingCore.Portable.DTO.Cache
{
    public class XCacheItem<T> where T: class, new()
    {
        public XCacheItem()
        {
            Id = Guid.NewGuid();
        }

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
