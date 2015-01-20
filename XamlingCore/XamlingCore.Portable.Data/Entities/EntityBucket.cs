using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XamlingCore.Portable.Contract.Entities;
using XamlingCore.Portable.Contract.EventArgs;
using XamlingCore.Portable.Messages.Entities;
using XamlingCore.Portable.Messages.XamlingMessenger;
using XamlingCore.Portable.Model.Contract;
using XamlingCore.Portable.Util.Lock;

namespace XamlingCore.Portable.Data.Entities
{
    public class EntityBucket<T> : IEntityBucket<T> where T : class, IEntity, new()
    {
        private readonly IEntityCache _cache;

        private Dictionary<string, List<Guid>> _buckets = null;

        AsyncLock _lock = new AsyncLock();

        private const string BucketKey = "Buckets";

        public event EventHandler<BucketUpdatedEventArgs> BucketsUpdated;

        public EntityBucket(IEntityCache cache)
        {
            _cache = cache;
        }

        public async Task<List<Guid>> AllInBucket(string bucket)
        {
            await _init();

            using (var l = await _lock.LockAsync())
            {
                var b = _getBucket(bucket);
                return b;
            }
        }

        public async Task<bool> IsInBucket(string bucket, Guid guid)
        {
            await _init();

            using (var l = await _lock.LockAsync())
            {
                var b = _getBucket(bucket);
                return b.Contains(guid);
            }
        }

        public async Task MoveToBucket(string bucket, Guid guid)
        {
            await _init();

            using (var l = await _lock.LockAsync())
            {
                var doSave = false;

                foreach (var existingBucket in _buckets)
                {
                    if (existingBucket.Value.Contains(guid))
                    {
                        existingBucket.Value.Remove(guid);
                        doSave = true;
                    }    
                }

                var b = _getBucket(bucket);

                if (!b.Contains(guid))
                {
                    b.Add(guid);
                    doSave = true;
                }

                if (doSave)
                {
                    await _save();
                    _notifyUpdated(bucket, BucketUpdatedTypes.Add);    
                }
            }
        }

        public async Task AddToBucket(string bucket, Guid guid)
        {
            await _init();

            using (var l = await _lock.LockAsync())
            {
                var b = _getBucket(bucket);

                if (!b.Contains(guid))
                {
                    b.Add(guid);
                    await _save();
                    _notifyUpdated(bucket, BucketUpdatedTypes.Add);
                }
            }
        }

        public async Task RemoveFromBucket(string bucket, Guid guid)
        {
            await _init();

            using (var l = await _lock.LockAsync())
            {
                var b = _getBucket(bucket);
                if (b.Contains(guid))
                {
                    b.Remove(guid);
                    await _save();
                    _notifyUpdated(bucket, BucketUpdatedTypes.Remove);
                }
            }
        }

        public async Task ClearAll()
        {
            await _init();

            using (var l = await _lock.LockAsync())
            {
                _buckets.Clear();
                await _save();
                _notifyCleared();
            }
        }

        public async Task ClearBucket(string bucket)
        {
            await _init();

            using (var l = await _lock.LockAsync())
            {
                if (_buckets.ContainsKey(bucket))
                {
                    _buckets.Remove(bucket);
                    await _save();
                    _notifyUpdated(bucket, BucketUpdatedTypes.Clear);
                }
            }
        }

        void _notifyUpdated(string bucket, BucketUpdatedTypes updateType)
        {
            new BucketUpdatedMessage<T>(bucket, updateType).Send();
            
            if (BucketsUpdated != null)
            {
                BucketsUpdated(this, new BucketUpdatedEventArgs(bucket));
            }
        }

        void _notifyCleared()
        {
            new BucketUpdatedMessage<T>(BucketUpdatedTypes.Clear).Send();
        }

        List<Guid> _getBucket(string bucket)
        {
            if (!_buckets.ContainsKey(bucket))
            {
                _buckets.Add(bucket, new List<Guid>());
            }

            return _buckets[bucket];
        }

        async Task _save()
        {
            await _cache.SetEntity(_getThisBucketKey(), _buckets);
        }

        async Task _init()
        {
            if (_buckets != null)
            {
                return;
            }

            using (var l = await _lock.LockAsync())
            {
                var bucketKey = _getThisBucketKey();
                _buckets = await _cache.GetEntity<Dictionary<string, List<Guid>>>(bucketKey);
                if (_buckets == null)
                {
                    _buckets = new Dictionary<string, List<Guid>>();
                    await _save();
                }
            }
        }

        string _getThisBucketKey()
        {
            return string.Format("{0}_{1}", BucketKey, _getTypePath());
        }

        string _getTypePath()
        {
            var t = typeof(T);
            var args = t.GenericTypeArguments;

            string tName = t.Name;

            if (args != null)
            {
                foreach (var a in args)
                {
                    tName += "_" + a.Name;
                }
            }

            tName = tName.Replace("`", "-g-");

            return tName;
        }


    }
}
