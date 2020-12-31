using Doublel.EntityAccessor.Constructs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;

namespace Doublel.EntityAccessor
{
    public class EntityAccessorContext : DbContext
    {
        protected EntityAccessorContext() { }
        public EntityAccessorContext(DbContextOptions options) 
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ExcludeInheritedProperties(typeof(IAccessibleByTenant));
            modelBuilder.ExcludeInheritedProperties(typeof(IAccessibleByUser));
            SetGlobalIsDeletedFilter(modelBuilder);
        }

        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is Entity e)
                {
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            e.UpdatedAt = DateTime.UtcNow;
                            break;
                        case EntityState.Added:
                            e.CreatedAt = DateTime.UtcNow;
                            break;
                    }
                }
            }
            return base.SaveChanges();
        }

        private void SetGlobalIsDeletedFilter(ModelBuilder modelBuilder)
        {
            var entityTypes = GetType().Assembly.DefinedTypes.Where(x => x.BaseType == typeof(Entity)).Select(x => x.AsType()).ToList();

            foreach (var type in entityTypes)
            {
                var method = SetGlobalQueryMethod.MakeGenericMethod(type);
                method.Invoke(this, new object[] { modelBuilder });
            }
        }

        private MethodInfo SetGlobalQueryMethod => GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance).Single(t => t.IsGenericMethod && t.Name == "SetGlobalFilters");

        /// <summary>
        /// DO NOT Delete this, its used via reflexion.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="modelBuilder"></param>
        public void SetGlobalFilters<T>(ModelBuilder modelBuilder)
            where T : Entity => modelBuilder.Entity<T>().HasQueryFilter(x => x.DeletedAt == null);
    }
}
