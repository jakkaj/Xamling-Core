using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XamlingCore.Portable.Contract.Repos;
using XamlingCore.Portable.Contract.Services;
using XamlingCore.Portable.Model.Other;
using XamlingCore.Portable.Model.Response;
using XamlingCore.Portable.Model.Security;

namespace XamlingCore.Portable.Data.Security
{
    public class SecurityService : ISecurityService
    {
        private readonly ISecurityRepo _repo;

        public SecurityService(ISecurityRepo repo)
        {
            _repo = repo;
        }

        public async Task<XResult<XSecurityContext>> CreateContext(XSecurityContext parent, string name)
        {
            var context = new XSecurityContext
            {
                Id = Guid.NewGuid(),
                ParentId = parent.Id,
                Name = name,
                Members = new List<Guid>(), 
                Children = new List<Guid>()
            };

            parent.Children.Add(context.Id);

            var newContextResult = await SetContext(context);
            var parentUpdateResult = await SetContext(parent);

            if (!newContextResult)
            {
                return newContextResult.Copy<XSecurityContext>();
            }

            if (!parentUpdateResult)
            {
                return parentUpdateResult.Copy<XSecurityContext>();
            }

            return new XResult<XSecurityContext>(context);
        }

        public async Task<XResult<bool>> SetContext(XSecurityContext context)
        {
            return await _repo.SetContext(context);
        }

        public async Task<XResult<XSecurityContext>> GetAccess(Guid userId, Guid targetId, int securityTypes)
        {
            var context = await GetContextByTarget(targetId);

            if (!context)
            {
                return context.Copy<XSecurityContext>();
            }

            if ((context.Object.SecurityFlags & securityTypes) != 0)
            {
                return new XResult<XSecurityContext>();
            }

            var validatedChainResult = await _validateContextChain(context.Object, userId, securityTypes);

            if (!validatedChainResult)
            {
                return validatedChainResult.Copy<XSecurityContext>();
            }

            return new XResult<XSecurityContext>(context.Object, true, validatedChainResult.Message);
        }

        public async Task<XResult<bool>> AddMember(XSecurityContext context, Guid currentUserId, Guid memberId)
        {
            //first check the current user has permissions to edit security
            var canEditSecurityResult =
                await _validateContextChain(context, currentUserId, (int) XPermission.EditPermissions);

            if (!canEditSecurityResult)
            {
                return canEditSecurityResult;
            }

            var liveContext = await GetContextById(context.Id);

            if (!liveContext)
            {
                return liveContext.Copy<bool>();
            }

            if (!liveContext.Object.Members.Contains(memberId))
            {
                liveContext.Object.Members.Add(memberId);

                var setResult = await SetContext(liveContext.Object);

                if (!setResult)
                {
                    return setResult;
                }
            }

            context.Members.Add(currentUserId);

            return new XResult<bool>(true);
        }

        public async Task<XResult<bool>> RemoveMember(XSecurityContext context, Guid currentUserId, Guid memberId)
        {
            //first check the current user has permissions to edit security
            var canEditSecurityResult =
                await _validateContextChain(context, currentUserId, (int)XPermission.EditPermissions);

            if (!canEditSecurityResult)
            {
                return canEditSecurityResult;
            }

            var liveContext = await GetContextById(context.Id);

            if (!liveContext)
            {
                return liveContext.Copy<bool>();
            }

            if (liveContext.Object.Members.Contains(memberId))
            {
                liveContext.Object.Members.Remove(memberId);

                var setResult = await SetContext(liveContext.Object);

                if (!setResult)
                {
                    return setResult;
                }
            }

            context.Members.Add(currentUserId);

            return new XResult<bool>(true);
        }

        public async Task<XResult<XSecurityContext>> GetContextById(Guid contextId)
        {
            var context = await _repo.GetContextById(contextId);
            return context;
        }

        public async Task<XResult<XSecurityContext>> GetContextByTarget(Guid targetId)
        {
            var context = await _repo.GetContextByTargetId(targetId);
            return context;
        }

        public async Task<XResult<bool>> _validateContextChain(XSecurityContext context, Guid userId, int securityTypes)
        {
            if (context.Members.Contains(userId) && (context.SecurityFlags & securityTypes) != 0)
            {
                return new XResult<bool>(true, true, $"Authorised by ${context.Id}");
            }

            if (context.ParentId == Guid.Empty)
            {
                return
                    XResult<bool>.GetNotAuthorised(
                        $"Permission chain failed. Finished at context {context.Id} looking for permissions {securityTypes}");
            }

            var parent = await GetContextById(context.ParentId);

            if (parent == null)
            {
                return
                    XResult<bool>.GetNotAuthorised(
                        $"Could not find parent context ${context.ParentId} on context {context.Id}. looking for permisssions {securityTypes} ");
            }

            return await _validateContextChain(parent.Object, userId, securityTypes);
        }
    }
}
