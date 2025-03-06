using System.Linq.Expressions;
using FernandoLibrary.Persistance;
using FernandoLibrary.Persistance.Entities;
using Microsoft.EntityFrameworkCore;

namespace vortexUserConfig.UsersConfig.Presentation.Common.ListQuery;

public class ListPermissionRepository
{
    private readonly LibraryDbContext _context;
    
    public ListPermissionRepository(LibraryDbContext context)
    {
        _context = context;
    }

    public async Task<List<Permissions>> GetAll(LibraryDbContext context)
    {
        return await _context.Permissions.ToListAsync(); 
    }
    
    public async Task<List<Permissions>> FindBySpecification(Expression<Func<Permissions, bool>> spec)
    {
        IQueryable<Permissions> query = _context.Permissions.Where(spec);

        var roles = await query.ToListAsync();
        
        return roles;
    }
}