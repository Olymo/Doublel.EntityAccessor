using Doublel.EntityAccessor.test.Entities;
using Doublel.EntityAccessor.Tests.Entities;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Text;

namespace Doublel.EntityAccessor.Tests.UserTests
{
    public class UserDataFixture : BaseFixture
    {
        protected override void SetInitialData()
        {
            _context.Roles.Add(Test<Role>(x => { x.Id = 1; x.Name = "UserDataFixture"; }));
            _context.Roles.Add(Test<Role>(x => { x.Id = 2; x.Name = "UserDataFixture2"; x.TenantId = 2; }));
            _context.SaveChanges();

            var role = _context.Roles.Find(1);
            _context.CreditCards.AddRange(
                Test<CreditCard>(x => x.Role = role),
                Test<CreditCard>(x => x.Role = role),
                Test<CreditCard>(x => x.Role = role),
                Test<CreditCard>(x => { x.IsActive = false; x.Role = role; }),
                Deleted<CreditCard>(),
                Test<CreditCard>(x => { x.UserId = 10; x.Role = null; x.Role = role; }),
                Deleted<CreditCard>(x => { x.UserId = 10; x.Role = null; })
           );
            _context.SaveChanges();
            _context.Payments.AddRange(
                Test<Payment>(x => { x.Amount = 100; x.Signature = "P1"; x.CreditCard = _context.CreditCards.First(x => x.UserId == 11); }),
                Test<Payment>(x => { x.Amount = 200; x.Signature = "P2"; x.CreditCard = _context.CreditCards.First(x => x.UserId == 11); }),
                Test<Payment>(x => { x.Amount = 200; x.Signature = "P3"; x.CreditCard = _context.CreditCards.First(x => x.UserId == 11); }),
                Deleted<Payment>(x => { x.Amount = 200; x.Signature = "P4"; x.CreditCard = _context.CreditCards.First(x => x.UserId == 11); }),
                Test<Payment>(x => { x.Amount = 200; x.Signature = "P5"; x.CreditCard = _context.CreditCards.First(x => x.UserId == 11); x.IsActive = false; }),
                Test<Payment>(x => { x.Amount = 100; x.Signature = "P6"; x.CreditCard = _context.CreditCards.First(x => x.UserId == 10); })
                );
        }
    }
}
