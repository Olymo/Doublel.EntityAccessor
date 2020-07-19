using Doublel.EntityAccessor.Constructs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doublel.EntityAccessor.Tests.Entities
{
    public class CreditCard : Entity, IUserLevelConstruct, IAccessibleByTenant
    {
        public int UserId { get; set; }
        public string CardNumber { get; set; } = "5584-4432-1123-4434";
        public virtual Role Role { get; set; }
        public string TenantIdProperty => "Role.TenantId";
    }
}
