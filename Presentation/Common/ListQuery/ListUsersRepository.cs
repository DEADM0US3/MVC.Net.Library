
using System.Linq.Expressions;
using FernandoLibrary.Persistance;
using FernandoLibrary.Persistance.Entities;
using Microsoft.EntityFrameworkCore;

namespace vortexUserConfig.UsersConfig.Presentation.Common.ListQuery;

public class ListUsersRepository
{
    private readonly LibraryDbContext _context;
    
    public ListUsersRepository(LibraryDbContext context)
    {
        _context = context;
    }

    public async Task<List<Usuario>> GetAll(LibraryDbContext context)
    {
        return await _context.Usuarios.ToListAsync(); 
    }
    
    public async Task<List<Usuario>> FindBySpecification(Expression<Func<Usuario, bool>> spec)
    {
        IQueryable<Usuario> query = _context.Usuarios.Where(spec);

        var users = await query.ToListAsync();
        
        return users;
    }
    
}