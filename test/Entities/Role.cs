using Doublel.EntityAccessor.Constructs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doublel.EntityAccessor.Tests.Entities
{
    public class Role : Entity, ITenantLevelConstruct
    {
        public int TenantId { get; set; }
        public string Name { get; set; }
    }
}
