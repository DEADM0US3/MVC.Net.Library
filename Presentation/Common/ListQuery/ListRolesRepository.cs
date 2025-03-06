using System.Linq.Expressions;
using FernandoLibrary.Persistance;
using FernandoLibrary.Persistance.Entities;
using Microsoft.EntityFrameworkCore;

namespace vortexUserConfig.UsersConfig.Presentation.Common.ListQuery;

public class ListRolesRepository
{
    private readonly LibraryDbContext _context;
    
    public ListRolesRepository(LibraryDbContext context)
    {
        _context = context;
    }

    public async Task<List<Roles>> GetAll(LibraryDbContext context)
    {
        return await _context.Roles.ToListAsync(); 
    }
    
    public async Task<List<Roles>> FindBySpecification(Expression<Func<Roles, bool>> spec)
    {
        IQueryable<Roles> query = _context.Roles.Where(spec);

        var roles = await query.ToListAsync();
        
        return roles;
    }
}