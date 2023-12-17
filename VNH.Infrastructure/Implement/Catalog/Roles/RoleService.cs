using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VNH.Application.DTOs.Catalog.Roles;
using VNH.Application.Interfaces.Catalog.Roles;
using VNH.Domain;

namespace VNH.Infrastructure.Implement.Catalog.Roles
{
    public class RoleService : IRoleService
    {
        /*private readonly RoleManager<Roles> _roleManager;

        public RoleService(RoleManager<Roles> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<List<RoleResponse>> GetAll()
        {
            var roles = await _roleManager.Roles
                .Select(x => new RoleResponse()
                {
                    Id = x.Id,
                    Name = x.Name,
                    NormalizedName = x.Normalizedname
                }).ToListAsync();

            return roles;
        }*/
    }
}
