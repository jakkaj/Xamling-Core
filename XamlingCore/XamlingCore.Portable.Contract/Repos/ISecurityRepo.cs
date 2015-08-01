using System;
using System.Threading.Tasks;
using XamlingCore.Portable.Model.Response;
using XamlingCore.Portable.Model.Security;

namespace XamlingCore.Portable.Contract.Repos
{
    public interface ISecurityRepo
    {
        Task<XResult<XSecurityContext>> GetContextByTargetId(Guid targetId);
        Task<XResult<XSecurityContext>> GetContextById(Guid contextId);
        Task<XResult<bool>> SetContext(XSecurityContext context);
        Task<XResult<bool>> DeleteContext(Guid contextId);
    }
}
