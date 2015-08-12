using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XamlingCore.Portable.Model.Response;
using XamlingCore.Portable.Model.Security;

namespace XamlingCore.Portable.Contract.Repos
{
    public interface ISecurityRepo
    {
        Task<XResult<List<XSecurityContext>>> GetContextsByTargetId(Guid targetId);
        Task<XResult<XSecurityContext>> GetContextById(Guid contextId);
        Task<XResult<bool>> SetContext(XSecurityContext context);
        Task<XResult<bool>> DeleteContext(Guid contextId);
        Task<XResult<XSecurityContext>> GetContextByName(string contextName);
        Task<XResult<List<XSecurityContext>>> GetParentContexts(Guid targetId);
    }
}
