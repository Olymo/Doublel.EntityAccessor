using System;
using System.Collections.Generic;
using System.Text;

namespace Doublel.EntityAccessor.Tests
{
    public class TestUserInfo : IUserInfo
    {
        public string Identity { get; set; }

        public int? UserId { get; set; }

        public int? TenantId { get; set; }

        public bool IgnoreUserFilter { get; set; }

        public static TestUserInfo Instance => new TestUserInfo
        {
            Identity = "Test Identity",
            IgnoreUserFilter = false,
            TenantId = 1,
            UserId = 11
        };
    }
}
