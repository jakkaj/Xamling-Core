﻿using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XamlingCore.Portable.Contract.Services;
using XamlingCore.Portable.Model.Security;
using XamlingCore.Tests.NET.Base;

namespace XamlingCore.Tests.NET.Security
{
    [TestClass]
    public class SecurityContextTests : TestBase
    {
        [TestMethod]
        public async Task TestSecurityContext()
        {
            var sec = Resolve<ISecurityService>();

            var companyAdmin = Guid.NewGuid();

            var companyId = Guid.NewGuid();
            var project1Id = Guid.NewGuid();
            var project2Id = Guid.NewGuid();

            var tripod1Id = Guid.NewGuid();
            var tripod2Id = Guid.NewGuid();

            var companyRoot = await _createSecurityChain(companyAdmin, companyId, project1Id, project2Id, tripod1Id, tripod2Id);

            var allProjectAdmin = Guid.NewGuid();
            var allProjectViewer = Guid.NewGuid();

            var project1Admin = Guid.NewGuid();
            var project2Admin = Guid.NewGuid();

            var project1Viewer = Guid.NewGuid();
            var project2Viewer = Guid.NewGuid();

            var tripod1Author = Guid.NewGuid();
            var tripod2Author = Guid.NewGuid();

            var tripod1Viewer = Guid.NewGuid();
            var tripod2Viewer = Guid.NewGuid();

            var reader = await sec.GetAccess(companyAdmin, tripod1Id, (int) XPermission.Read);
            Assert.IsTrue(reader);

            var projectAdminContext = await sec.GetContextByName("All Project Admin");
            var projectViewContext = await sec.GetContextByName("All Project View");

            Assert.IsNotNull(projectAdminContext);
            Assert.IsNotNull(projectViewContext);

            var addProjectAdminContextUser = await sec.AddMember(projectAdminContext.Object, companyAdmin, allProjectAdmin);
            var addProjectViewContextUser = await sec.AddMember(projectViewContext.Object, allProjectAdmin, allProjectViewer);

            Assert.IsTrue(addProjectAdminContextUser);
            Assert.IsTrue(addProjectViewContextUser);

            //attempt to alter things with a user that does not have edit permissions
            var addProjectAdminContextUserWhenNotAllowed = await sec.AddMember(projectAdminContext.Object, allProjectViewer, allProjectViewer);
            Assert.IsFalse(addProjectAdminContextUserWhenNotAllowed);

            var adminCanViewTripod = await sec.GetAccess(allProjectAdmin, tripod1Id, (int)XPermission.Read);
            var adminCanWriteTripod = await sec.GetAccess(allProjectAdmin, tripod1Id, (int)XPermission.Write);
            var adminCanEditPermissionsTripod = await sec.GetAccess(allProjectAdmin, tripod1Id, (int)XPermission.EditPermissions);

            Assert.IsTrue(adminCanViewTripod);
            Assert.IsTrue(adminCanWriteTripod);
            Assert.IsTrue(adminCanEditPermissionsTripod);

            var viewerCanViewTripod = await sec.GetAccess(allProjectViewer, tripod1Id, (int)XPermission.Read);
            var viewerCannotWriteTripod = await sec.GetAccess(allProjectViewer, tripod1Id, (int)XPermission.Write);
            var viewerCannotEditPermissionsTripod = await sec.GetAccess(allProjectViewer, tripod1Id, (int)XPermission.EditPermissions);

            Assert.IsTrue(viewerCanViewTripod);
            Assert.IsFalse(viewerCannotWriteTripod);
            Assert.IsFalse(viewerCannotEditPermissionsTripod);
        }

        async Task<XSecurityContext> _createSecurityChain(Guid companyAdmin, Guid companyRoot,
            Guid project1, Guid project2, Guid tripod1, Guid tripod2)
        {

            var root = await _createContext(companyRoot, null, "Root",
                (int)XPermission.Write | (int)XPermission.Read | (int)XPermission.EditPermissions, companyAdmin);


            var projectAdmin = await _createContext(new List<Guid> { project1, project2 }, root, "All Project Admin",
                (int)XPermission.Write | (int)XPermission.Read | (int)XPermission.EditPermissions);

            var projectView = await _createContext(new List<Guid> { project1, project2 }, projectAdmin, "All Project View",
                (int)XPermission.Read);

            var someProjectOnlyAdmin = await _createContext(project1, projectView, "Some Project Admin",
                (int)XPermission.Write | (int)XPermission.Read | (int)XPermission.EditPermissions);

            var someProjectOnlyView = await _createContext(project1, someProjectOnlyAdmin, "Some Project View",
                (int)XPermission.Read);

            var someProjectOnlyAdmin2 = await _createContext(project2, projectAdmin, "Some Project Admin",
                (int)XPermission.Write | (int)XPermission.Read | (int)XPermission.EditPermissions);

            var someProjectOnlyView2 = await _createContext(project2, someProjectOnlyAdmin2, "All Project View",
                (int)XPermission.Read);

            var tripodAuthor = await _createContext(tripod1, someProjectOnlyView, "Some Tripod Author",
                (int)XPermission.Write | (int)XPermission.Read);

            var tripodViewer = await _createContext(tripod1, tripodAuthor, "Some Tripod Viewer",
                (int)XPermission.Read);

            var tripodAuthor2 = await _createContext(tripod2, someProjectOnlyView2, "Some Tripod Author",
                (int)XPermission.Write | (int)XPermission.Read);

            var tripodViewer2 = await _createContext(tripod2, tripodAuthor2, "Some Tripod Viewer",
                (int)XPermission.Read);

            return root;

        }

        private async Task<XSecurityContext> _createContext(Guid targetId, XSecurityContext parent, string name, int permissions, Guid? owner = null)
        {
            return await _createContext(new List<Guid> { targetId }, parent, name, permissions, owner);
        }

        async Task<XSecurityContext> _createContext(List<Guid> targetIds, XSecurityContext parent, string name, int permissions, Guid? owner = null)
        {
            var sec = Resolve<ISecurityService>();
            var c = await sec.CreateContext(parent, name, permissions, owner, targetIds);
            return c.Object;
        }
    }
}
