using System;
using System.Collections.Generic;
using System.Text;

namespace Doublel.EntityAccessor
{
    public interface IUserInfo
    {
        public string Identity { get; }
        public int? UserId { get; }
        public int? TenantId { get; }
        public bool IgnoreUserFilter { get; }
    }
}
