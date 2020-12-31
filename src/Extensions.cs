using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doublel.EntityAccessor
{
    internal static class Extensions
    {
        internal static void ExcludeInheritedProperties(this ModelBuilder modelBuilder, Type typeToExcludePropertiesFrom)
        {
            var propertyNames = typeToExcludePropertiesFrom.GetProperties()
                        .Select(p => p.Name)
                        .ToList();
            var entityTypes = modelBuilder.Model.GetEntityTypes()
                .Where(t => typeToExcludePropertiesFrom.IsAssignableFrom(t.ClrType));
            foreach (var entityType in entityTypes)
            {
                var entityTypeBuilder = modelBuilder.Entity(entityType.ClrType);
                foreach (var propertyName in propertyNames)
                    entityTypeBuilder.Ignore(propertyName);
            }
        }
    }
}
