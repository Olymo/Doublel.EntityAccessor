using Doublel.EntityAccessor.Constructs;
using Doublel.EntityAccessor.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Doublel.ReflexionExtensions;

namespace Doublel.EntityAccessor
{

    public class EntityAccessor
    {
        private readonly DbContext _context;
        private IUserInfo _userInfo;

        public IUserInfo UserInfo
        {
            set
            {
                if(value.Identity == null)
                {
                    throw new ArgumentNullException("IUserInfo must not have Identity property set to null.");
                }

                _userInfo = value;
            }
        }

        public EntityAccessor(DbContext context, IUserInfo actor)
        {
            _context = context;
            _userInfo = actor;
        }

        public virtual TEntity Find<TEntity>(int id, bool onlyActive = true)
            where TEntity : Entity
        {
            var query = GetQuery<TEntity>(onlyActive ? ActivityLevel.Active : ActivityLevel.All).Where(x => x.Id == id);

            return query.FirstOrDefault();
        }

        public virtual void Remove<TEntity>(int id)
            where TEntity : Entity
        {
            var itemToDelete = FindAndThrow<TEntity>(id);

            itemToDelete.DeletedAt = DateTime.UtcNow;
            itemToDelete.DeletedBy = _userInfo.Identity;
            itemToDelete.IsActive = false;
        }

        public TEntity FindAndThrow<TEntity>(int id)
            where TEntity : Entity
        {
            var item = Find<TEntity>(id, false);

            if (item == null)
            {
                throw new EntityNotFoundException(typeof(TEntity), id);
            }

            return item;
        }

        public virtual void Deactivate<TEntity>(int id)
            where TEntity : Entity
        {
            var itemToDeactivate = FindAndThrow<TEntity>(id);

            if (!itemToDeactivate.IsActive)
            {
                throw new EntityAlreadyDeletedException(typeof(TEntity), id);
            }

            itemToDeactivate.IsActive = false;
        }

        public virtual void Add<TEntity>(TEntity entity)
            where TEntity : Entity
        {
            SetTenantLevelProperties(entity);
            SetUserLevelProperties(entity);

            entity.CreatedAt = DateTime.UtcNow;
            entity.AddedBy = _userInfo.Identity;

            _context.Set<TEntity>().Add(entity);
        }

        public virtual void AddRange<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : Entity
        {
            var now = DateTime.UtcNow;

            foreach (var entity in entities)
            {
                SetTenantLevelProperties(entity);
                SetUserLevelProperties(entity);

                entity.CreatedAt = now;
                entity.AddedBy = _userInfo.Identity;
            }

            _context.Set<TEntity>().AddRange(entities);
        }

        private void SetTenantLevelProperties<TEntity>(TEntity entity)
            where TEntity : Entity
        {
            if (entity is ITenantLevelConstruct tenant && _userInfo.TenantId.HasValue)
            {
                tenant.TenantId = _userInfo.TenantId.GetValueOrDefault();
            }
        }

        private void SetUserLevelProperties<TEntity>(TEntity entity)
            where TEntity : Entity
        {
            if (entity is IUserLevelConstruct construct && _userInfo.UserId.HasValue)
            {
                construct.UserId = _userInfo.UserId.GetValueOrDefault();
            }
        }

        public virtual void Update<TEntity>(TEntity entity)
            where TEntity : Entity
        {
            SetTenantLevelProperties(entity);
            SetUserLevelProperties(entity);

            entity.UpdatedAt = DateTime.UtcNow;
            entity.LastModifiedBy = _userInfo.Identity;

            _context.Set<TEntity>().Update(entity);
        }

        public virtual void Commit() => _context.SaveChanges();

        public virtual IQueryable<TEntity> All<TEntity>()
            where TEntity : Entity
            => GetQuery<TEntity>(ActivityLevel.All);

        public virtual IQueryable<TEntity> GetQuery<TEntity>(ActivityLevel level = ActivityLevel.Active)
            where TEntity : Entity
        {
            var query = _context.Set<TEntity>().Where(x => !x.DeletedAt.HasValue);


            if (level != ActivityLevel.All)
            {
                query = level == ActivityLevel.Active ? query.Where(x => x.IsActive) : query.Where(x => !x.IsActive);
            }

            if (EntityShouldBeFilteredByUser<TEntity>())
            {
                query = FilterByUser(query);
                return query;
            }

            if (EntityShouldBeFilteredByTenant<TEntity>())
            {
                query = FilterByTenant(query);
            }

            return query;
        }

        private IQueryable<TEntity> FilterByUser<TEntity>(IQueryable<TEntity> query)
        {
            if(!_userInfo.UserId.HasValue)
            {
                return query;
            }

            if (typeof(TEntity).InheritsFrom<IUserLevelConstruct>())
            {
                query = query.Where($"UserId == @0", _userInfo.UserId.GetValueOrDefault());
            }
            else
            {
                if (typeof(TEntity).GetConstructor(Type.EmptyTypes) != null)
                {
                    var iidentifyableByUser = Activator.CreateInstance(typeof(TEntity)) as IAccessibleByUser;

                    query = query.Where($"{iidentifyableByUser.UserIdProperty} == @0", _userInfo.UserId.GetValueOrDefault());
                }
            }
            return query;
        }

        private IQueryable<TEntity> FilterByTenant<TEntity>(IQueryable<TEntity> query)
        {
            if(!_userInfo.TenantId.HasValue)
            {
                return query;
            }

            if (typeof(TEntity).InheritsFrom<ITenantLevelConstruct>())
            {
                query = query.Where($"TenantId == @0", _userInfo.TenantId);
            }
            else
            {
                if (typeof(TEntity).GetConstructor(Type.EmptyTypes) != null)
                {
                    var identifyableByTenantInstance = Activator.CreateInstance(typeof(TEntity)) as IAccessibleByTenant;
                    query = query.Where($"{identifyableByTenantInstance.TenantIdProperty} == @0", _userInfo.TenantId);
                }
            }

            return query;
        }

        private bool EntityShouldBeFilteredByTenant<TEntity>()
            => typeof(TEntity).InheritsFrom<IAccessibleByTenant>() || typeof(TEntity).InheritsFrom<ITenantLevelConstruct>();
        private bool EntityShouldBeFilteredByUser<TEntity>() => !_userInfo.IgnoreUserFilter && (typeof(TEntity).InheritsFrom<IAccessibleByUser>() || typeof(TEntity).InheritsFrom<IUserLevelConstruct>());

    }
}
