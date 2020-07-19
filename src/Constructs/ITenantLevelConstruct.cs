using System;
using System.Collections.Generic;
using System.Text;

namespace Doublel.EntityAccessor.Constructs
{
    public interface ITenantLevelConstruct
    {
        int TenantId { get; set; }
    }
}
