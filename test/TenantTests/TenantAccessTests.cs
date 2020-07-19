using Doublel.EntityAccessor.Tests.Entities;
using FluentAssertions;
using FluentAssertions.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Doublel.EntityAccessor.Tests
{
    public class TenantAccessTests : FixtureTest<TenantDataFixture>
    {
        public TenantAccessTests(TenantDataFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public void ItemsSuccessfullyFilteredByTenant()
        {
            var tenantInfo = new TestUserInfo
            {
                Identity = "FakeItem",
                TenantId = 1
            };

            Fixture.Accessor.UserInfo = tenantInfo;

            var items = Fixture.Accessor.GetQuery<Role>(ActivityLevel.All).ToList();
            items.Should().HaveCount(3);
            var firstItem = items.First();
            firstItem.Name.Should().Be("Role 1");
            firstItem.AddedBy.Should().Be("Fixture");

            tenantInfo.TenantId = 2;

            var secondTenatnItems = Fixture.Accessor.GetQuery<Role>().ToList();
            secondTenatnItems.Should().HaveCount(1);
            var secondTenantFirstItem = secondTenatnItems.First();
            secondTenantFirstItem.Name.Should().Be("Second Role");
            secondTenantFirstItem.AddedBy.Should().Be("Fixture");
        }

        [Fact]
        public void ReturnsEmptyCollection_WhenNonExistingTenantIdIsSet()
        {
            var tenantInfo = new TestUserInfo
            {
                Identity = "FakeItem",
                TenantId = 3
            };

            Fixture.Accessor.UserInfo = tenantInfo;

            Fixture.Accessor.GetQuery<Role>().ToList().Should().BeEmpty();
        }
        
        [Fact]
        public void ActivityFilterWorksWithTenant()
        {
            var tenantInfo = new TestUserInfo
            {
                Identity = "FakeItem",
                TenantId = 1
            };

            Fixture.Accessor.UserInfo = tenantInfo;

            Fixture.Accessor.GetQuery<Role>().ToList().Should().HaveCount(1);
            var item = Fixture.Accessor.GetQuery<Role>().First();
            item.IsActive.Should().BeTrue();
            item.Name.Should().Be("Role 1");
        }


        [Fact]
        public void AccessAllData_WhenTenantIdIsNotSet()
        {
            var tenantInfo = new TestUserInfo
            {
                Identity = "Fake"
            };
            Fixture.Accessor.UserInfo = tenantInfo;
            var result = Fixture.Accessor.All<Role>().ToList();
            result.Should().HaveCount(4);
        }
    }
}
