using Doublel.EntityAccessor.Tests.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doublel.EntityAccessor.Tests.UserTests
{
    public class UserDataFixture : BaseFixture
    {
        protected override void SetInitialData()
        {
            _context.CreditCards.AddRange(
                Test<CreditCard>(),
                Test<CreditCard>(),
                Test<CreditCard>(),
                Test<CreditCard>(x => x.IsActive = false),
                Deleted<CreditCard>(),
                Test<CreditCard>(x => x.UserId = 10),
                Deleted<CreditCard>(x => x.UserId = 10)
           );
        }
    }
}
