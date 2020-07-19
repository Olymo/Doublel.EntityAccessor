using Doublel.EntityAccessor.test.Entities;
using Doublel.EntityAccessor.Tests;
using Doublel.EntityAccessor.Tests.Entities;
using Doublel.EntityAccessor.Tests.UserTests;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Doublel.EntityAccessor.test.UserTests
{
    public class MixedTests : FixtureTest<UserDataFixture>
    {
        public MixedTests(UserDataFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public void ReturnsCreditCards_BasedOnTenant_IAccessibleByTenant()
        {
            var info = TestUserInfo.Instance;
            info.UserId = null;
            info.TenantId = 1;
            Fixture.Accessor.UserInfo = info;

            var items = Fixture.Accessor.GetQuery<CreditCard>();
            items.Should().HaveCount(4);
        }

        [Fact]
        public void ReturnsPayments_BasedOnTenant_IAccessibleByTenant_MultiNavigation()
        {
            var info = TestUserInfo.Instance;
            info.UserId = null;
            info.TenantId = 1;
            Fixture.Accessor.UserInfo = info;

            var items = Fixture.Accessor.GetQuery<Payment>();
            items.Should().HaveCount(4);
        }
    }
}
