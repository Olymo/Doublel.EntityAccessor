using System;
using System.Collections.Generic;
using System.Text;

namespace Doublel.EntityAccessor
{
    public interface IUserInfo
    {
        string Identity { get; }
        int? UserId { get; }
        int? TenantId { get; }
        bool IgnoreUserFilter { get; }
    }
}
