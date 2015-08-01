using System;

namespace XamlingCore.Portable.Model.Security
{
    [Flags]
    public enum XPermission
    {
        Read = 1,
        Write = 2, 
        EditPermissions = 4
    }
}
