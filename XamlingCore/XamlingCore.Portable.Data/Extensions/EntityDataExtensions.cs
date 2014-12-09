using System;
using System.Threading.Tasks;
using Autofac;
using XamlingCore.Portable.Contract.Entities;
using XamlingCore.Portable.Data.Glue;
using XamlingCore.Portable.Model.Contract;
using XamlingCore.Portable.Workflow.Flow;

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

        static XWorkflowHub _getHub()
        {
            return _getContainer().Resolve<XWorkflowHub>();
        }

        public static async Task<XFlow> StartWorkflow<T>(this T entity, string flowId) where T : class, IEntity, new()
        {
            var flow = await _getHub().Start(flowId, entity.Id);
            return flow;
        }

        public static XFlow GetFlow<T>(this T entity, string flowId) where T : class, IEntity, new()
        {
            var flow =  _getHub().GetFlow(flowId);
            return flow;
        }

        public static async Task<bool> ResumeDisconnectedFlow<T>(this T entity, string flowId, bool result) where T : class, IEntity, new()
        {
            var flow =  GetFlow(entity, flowId);
            return await flow.ResumeDisconnected(entity.Id, result);
        }

        public static XFlowState GetWorkflowState<T>(this T entity, string flowId) where T : class, IEntity, new()
        {
            var flowState =  _getHub().GetFlowState(flowId, entity.Id);
            return flowState;
        }

        public static async Task<bool> IsInBucket<T>(this T entity, string bucket) where T : class, IEntity, new()
        {
            var manager = _getManagerFor(entity);
            return await manager.IsInBucket(bucket, entity);
        }

        public static async Task<T> MoveToBucket<T>(this T entity, string bucket) where T : class, IEntity, new()
        {
            var manager = _getManagerFor(entity);
            await manager.MoveToBucket(bucket, entity);
            return entity;
        }


        public static async Task<T> AddToBucket<T>(this T entity, string bucket) where T : class, IEntity, new()
        {
            var manager = _getManagerFor(entity);
            await manager.AddToBucket(bucket, entity);
            return entity;
        }

        public static async Task<T> RemoveFromBucket<T>(this T entity, string bucket) where T : class, IEntity, new()
        {
            var manager = _getManagerFor(entity);
            await manager.RemoveFromBucket(bucket, entity);
            return entity;
        }

        public static async Task<T> Set<T>(this T entity) where T : class, IEntity, new()
        {
            var manager = _getManagerFor(entity);
            return await manager.Set(entity);            
        }

        public static async Task<T> Delete<T>(this T entity) where T : class, IEntity, new()
        {
            var manager = _getManagerFor(entity);
            await manager.Delete(entity);
            return entity;
        }
    }
}
