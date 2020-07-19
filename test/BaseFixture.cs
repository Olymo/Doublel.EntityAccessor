using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Doublel.ReflexionExtensions;
using Doublel.EntityAccessor.Constructs;

namespace Doublel.EntityAccessor.Tests
{
    public abstract class BaseFixture : IDisposable
    {
        protected TestDbContext _context;
        public EntityAccessor Accessor { get; private set; }
        public BaseFixture()
        {
            SetCleanContextState();
        }

        private void SetCleanContextState()
        {
            _context = new TestDbContext();
            Accessor = new EntityAccessor(_context, TestUserInfo.Instance);
            SetInitialData();

            _context.SaveChanges();
        }

        protected abstract void SetInitialData();
        public void Dispose() => _context.Dispose();
        public void ResetData() => SetCleanContextState();

        public static TEntity Test<TEntity>(Action<TEntity> update = null) where TEntity : Entity, new()
        {
            var entity = new TEntity
            {
                AddedBy = "Fixture",
                CreatedAt = new DateTime(2020, 07, 18),
                IsActive = true,
            };

            if(entity is ITenantLevelConstruct c)
            {
                c.TenantId = 1;
            }

            if(entity is IUserLevelConstruct u)
            {
                u.UserId = 11;
            }

            update?.Invoke(entity);

            return entity;
        }

        public static TEntity Deleted<TEntity>(Action<TEntity> update = null) where TEntity : Entity, new()
        {
            var entity = Test<TEntity>(update);
            entity.DeletedAt = DateTime.Now;
            return entity;
        }
    }
}
