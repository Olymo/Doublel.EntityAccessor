using Doublel.EntityAccessor.Constructs;
using Doublel.EntityAccessor.Tests.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doublel.EntityAccessor.test.Entities
{
    public class Payment : Entity, IAccessibleByUser, IAccessibleByTenant
    {
        public decimal Amount { get; set; }
        public string Signature { get; set; }
        public string UserIdProperty => "CreditCard.UserId";
        public virtual CreditCard CreditCard { get; set; }

        public string TenantIdProperty => "CreditCard.Role.TenantId";
    }
}
