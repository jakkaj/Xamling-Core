using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Portable.Contract.Repos;
using XamlingCore.Portable.Model.Response;
using XamlingCore.Portable.Model.Security;

namespace XamlingCore.Tests.NET.Security
{
    public class MockMemorySecurityRepo : ISecurityRepo
    {
        List<XSecurityContext> _contexts = new List<XSecurityContext>();

        public async Task<bool> Save()
        {
            return false;
        }

        public async Task<bool> Load()
        {
            return false;
        }

        public async Task<XResult<XSecurityContext>> GetContextByName(string contextName)
        {
            var existing = _contexts.FirstOrDefault(_ => _.Name == contextName);

            return new XResult<XSecurityContext>(existing);
        }

        public async Task<XResult<List<XSecurityContext>>> GetContextsByTargetId(Guid targetId)
        {
            var existing = _contexts.Where(_ => _.Targets.Contains(targetId));
            
            return new XResult<List<XSecurityContext>>(existing.ToList());
        }

        public async Task<XResult<XSecurityContext>> GetParentContext(Guid contextId)
        {
            var existing = _contexts.FirstOrDefault(_ => _.Children.Contains(contextId));

            if (existing == null)
            {
                return XResult<XSecurityContext>.GetNoRecord();
            }

            return new XResult<XSecurityContext>(existing);
        }

        public async Task<XResult<XSecurityContext>> GetContextById(Guid contextId)
        {
            var existing = _contexts.FirstOrDefault(_ => _.Id == contextId);

            if (existing == null)
            {
                return XResult<XSecurityContext>.GetNoRecord();
            }

            return new XResult<XSecurityContext>(existing);
        }

        public async Task<XResult<bool>> SetContext(XSecurityContext context)
        {
            var existing = _contexts.FirstOrDefault(_ => _.Id == context.Id);

            if (existing != null)
            {
                _contexts.Remove(existing);
            }

            _contexts.Add(context);

            return new XResult<bool>(true);
        }

        public Task<XResult<bool>> DeleteContext(Guid contextId)
        {
            throw new NotImplementedException();
        }
    }
}
