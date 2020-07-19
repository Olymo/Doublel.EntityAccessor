using System;
using System.Collections.Generic;
using System.Text;

namespace Doublel.EntityAccessor.Exceptions
{
    public class EntityAlreadyDeletedException : Exception
    {
        public EntityAlreadyDeletedException(Type t, object id) : base($"Entity of type {t.Name} with an id of {id} is already inactive.")
        {

        }
    }
}
