using Doublel.EntityAccessor.Tests.Entities;

namespace Doublel.EntityAccessor.Tests
{
    public class TenantDataFixture : BaseFixture
    {
        protected override void SetInitialData()
        {
            _context.Roles.AddRange(
                Test<Role>(r =>   r.Name = "Role 1"),
                Test<Role>(r => { r.Name = "Role 2"; r.IsActive = false; }),
                Test<Role>(r => { r.Name = "Role 3"; r.IsActive = false; }),
                Test<Role>(r => { r.Name = "Second Role"; r.TenantId = 2; }),
                Deleted<Role>(r => { r.Name = "Role 4"; r.TenantId = 2; })
            );
        }
    }
}
