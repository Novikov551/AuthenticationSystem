using AuthenticationSystem.Domain;
using AuthenticationSystem.Domain.Core;
using Microsoft.EntityFrameworkCore;

namespace Stemy.Dairy.FactStash.Services;

public class SeederService
{
    private readonly IServiceScopeFactory _serviceScope;

    private IUnitOfWork _unitOfWork;

    private Guid _defaultUserRoleId = Guid.NewGuid();
    private Guid _defaultAdminRoleId = Guid.NewGuid();

    public SeederService(IServiceScopeFactory serviceScope)
    {
        _serviceScope = serviceScope;
    }

    public async Task SeedAsync()
    {
        using var scope = _serviceScope.CreateScope();
        _unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        await SeedDefaultRolesAsync();
    }

    private async Task SeedDefaultRolesAsync()
    {
        var userRole = await _unitOfWork.Repository<Role>().Query.Where(p => p.RoleType == RoleType.User).FirstOrDefaultAsync();
        var adminRole = await _unitOfWork.Repository<Role>().Query.Where(p => p.RoleType == RoleType.User).FirstOrDefaultAsync();

        if (userRole is null)
        {
            userRole = new Role()
            {
                Id = _defaultUserRoleId,
                RoleName = "User",
                RoleType = RoleType.User
            };

            _unitOfWork.Repository<Role>().Create(userRole);
        }
        else
        {
            _defaultUserRoleId = userRole.Id;
        }

        if (adminRole is null)
        {
            adminRole = new Role()
            {
                Id = _defaultAdminRoleId,
                RoleName = "Administrator",
                RoleType = RoleType.Admin
            };

            _unitOfWork.Repository<Role>().Create(adminRole);
        }
        else
        {
            _defaultAdminRoleId = userRole.Id;
        }

        await _unitOfWork.SaveAsync();
    }
}