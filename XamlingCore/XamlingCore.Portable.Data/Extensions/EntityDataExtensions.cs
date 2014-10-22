using System;
using System.Threading.Tasks;
using Autofac;
using XamlingCore.Portable.Contract.Entities;
using XamlingCore.Portable.Data.Glue;
using XamlingCore.Portable.Model.Contract;

namespace XamlingCore.Portable.Data.Extensions
{
    public static class EntityDataExtensions
    {
        static ILifetimeScope _getContainer()
        {
            if (ContainerHost.Container == null)
            {
                throw new NullReferenceException("Container host must be set to current lifetime scope as part of glue. After build container, set ContainerHost.Container to that scope");
            }

            return ContainerHost.Container;
        }

        static IEntityManager<T> _getManagerFor<T>(T entity) where T : class, IEntity, new()
        {
            return _getContainer().Resolve<IEntityManager<T>>();
        }


        public static async Task<bool> IsInBucket<T>(this T entity, string bucket) where T : class, IEntity, new()
        {
            var manager = _getManagerFor(entity);
            return await manager.IsInBucket(bucket, entity);
        }


        public static async Task AddToBucket<T>(this T entity, string bucket) where T : class, IEntity, new()
        {
            var manager = _getManagerFor(entity);
            await manager.AddToBucket(bucket, entity);
        }

        public static async Task RemoveFromBucket<T>(this T entity, string bucket) where T : class, IEntity, new()
        {
            var manager = _getManagerFor(entity);
            await manager.RemoveFromBucket(bucket, entity);
        }

        public static async Task Set<T>(this T entity) where T : class, IEntity, new()
        {
            var manager = _getManagerFor(entity);
            await manager.Set(entity);
        }

        public static async Task Delete<T>(this T entity) where T : class, IEntity, new()
        {
            var manager = _getManagerFor(entity);
            await manager.Delete(entity);
        }
    }
}
