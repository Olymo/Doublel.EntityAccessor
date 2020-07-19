using System;
using System.Collections.Generic;
using System.Text;

namespace Doublel.EntityAccessor
{
    public abstract class Entity
    {
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string DeletedBy { get; set; }
        public string LastModifiedBy { get; set; }
        public string AddedBy { get; set; }
        public int Id { get; set; }
    }
}
