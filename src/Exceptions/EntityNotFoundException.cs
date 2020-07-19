using System;
using System.Collections.Generic;
using System.Text;

namespace Doublel.EntityAccessor.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(Type t, int id) : base($"Entity of type {t.Name} with an id of {id} was not found in the system.")
        {

        }
    }
}
