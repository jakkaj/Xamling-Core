using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Portable.Contract.Repos;
using XamlingCore.Portable.Model.Response;
using XamlingCore.Portable.Model.Security;

namespace XamlingCore.Portable.Data.Security
{
    public class SecurityRepo : ISecurityRepo
    {
        public async Task<XResult<XSecurityContext>> GetContextByTargetId(Guid targetId)
        {
            return null;
        }

        public async Task<XResult<XSecurityContext>> GetContextById(Guid contextId)
        {
            return null;
        }

        public async Task<XResult<bool>> SetContext(XSecurityContext context)
        {
            return null;
        }

        public async Task<XResult<bool>> DeleteContext(Guid contextId)
        {
            throw new NotImplementedException();
        }
    }
}
