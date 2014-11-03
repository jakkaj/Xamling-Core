using XamlingCore.Portable.Messages.XamlingMessenger;
using XamlingCore.Portable.Model.Contract;

namespace XamlingCore.Portable.Messages.Entities
{
    public class EntityUpdatedMessage<T> : XMessage where T : class, IEntity, new()
    {
        private readonly T _entity;

        public EntityUpdatedMessage(T entity)
        {
            _entity = entity;
        }

        public T Entity
        {
            get { return _entity; }
        }
    }
}
