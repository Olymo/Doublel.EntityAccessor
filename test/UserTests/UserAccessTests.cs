using Doublel.EntityAccessor.test.Entities;
using Doublel.EntityAccessor.Tests.Entities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Doublel.EntityAccessor.Tests.UserTests
{
    public class UserAccessTests : FixtureTest<UserDataFixture>
    {
        public UserAccessTests(UserDataFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public void ReturnsOnlyActiveCurrentUserCreditCards()
        {
            Fixture.ResetData();
            Fixture.Accessor.GetQuery<CreditCard>().Should().HaveCount(3);
        }

        [Fact]
        public void ReturnsOnlyActiveItemsFilteredForUser()
        {
            Fixture.ResetData();
            Fixture.Accessor.All<CreditCard>().Should().HaveCount(4);
        }

        [Fact]
        public void ReturnsAllRecordsWhenIgnoreUserFilterTrue()
        {
            var userInfo = TestUserInfo.Instance;
            userInfo.IgnoreUserFilter = true;
            Fixture.Accessor.UserInfo = userInfo;
            Fixture.Accessor.All<CreditCard>().Should().HaveCount(5);
        }

        [Fact]
        public void ReturnsAllRecordsWhenUserIdIsNull()
        {
            var userInfo = TestUserInfo.Instance;
            userInfo.UserId = null;
            Fixture.Accessor.UserInfo = userInfo;
            Fixture.Accessor.All<CreditCard>().Should().HaveCount(5);
        }

        [Fact]
        public void ReturnsUserRelatedItems_WhenIAccessibleByUserInterfaceIsUsed()
        {
            var payments = Fixture.Accessor.All<Payment>().ToList();
            payments.Should().HaveCount(4);
            payments.TrueForAll(x => x.CreditCard.UserId == 11).Should().BeTrue();
        }

        [Fact]
        public void ReturnsOnlyActiveUserRelatedItems_WhenIAccessibleByUserInterfaceIsUsed()
        {
            var payments = Fixture.Accessor.GetQuery<Payment>().ToList();
            payments.Should().HaveCount(3);
            payments.TrueForAll(x => x.CreditCard.UserId == 11).Should().BeTrue();
        }

        [Fact]
        public void ReturnsAllItems_WhenIAccessibleByUserInterfaceIsUsed_IgnoreFilterSetToTrue()
        {
            var info = TestUserInfo.Instance;
            info.IgnoreUserFilter = true;
            Fixture.Accessor.UserInfo = info;
            var payments = Fixture.Accessor.All<Payment>().ToList();
            payments.Should().HaveCount(5);
            payments.TrueForAll(x => x.CreditCard.UserId == 11 || x.CreditCard.UserId == 10).Should().BeTrue();
        }
    }
}
