using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XamlingCore.Portable.Model.Response;
using XamlingCore.Portable.Model.Security;

namespace XamlingCore.Portable.Contract.Services
{
    public interface ISecurityService
    {
        Task<XResult<XSecurityContext>> CreateContext(XSecurityContext parent, string name, int permissions, Guid? owner = null, List<Guid> targetIds = null);
        Task<XResult<bool>> SetContext(XSecurityContext context);
        Task<XResult<XSecurityContext>> GetAccess(Guid userId, Guid targetId, int securityTypes);
        Task<XResult<bool>> AddMember(XSecurityContext context, Guid currentUserId, Guid memberId);
        Task<XResult<bool>> RemoveMember(XSecurityContext context, Guid currentUserId, Guid memberId);
        Task<XResult<XSecurityContext>> GetContextById(Guid contextId);
        Task<XResult<List<XSecurityContext>>> GetContextByTarget(Guid targetId);
        Task<XResult<bool>> _validateContextChain(XSecurityContext context, Guid userId, int securityTypes);
        Task<XResult<XSecurityContext>> GetContextByName(string contextName);
        Task<XResult<XSecurityContext>> GetParentContext(Guid contextId);

        Task<XResult<XSecurityContext>> GetAccess(Guid userId, XSecurityContext context,
            int securityTypes);

        Task<XResult<XSecurityContext>> GetAccess(Guid userId, List<XSecurityContext> context, int securityTypes);
    }
}