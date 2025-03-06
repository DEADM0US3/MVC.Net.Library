using System.Linq.Expressions;
using FernandoLibrary.Persistance;
using FernandoLibrary.Persistance.Entities;
using Microsoft.EntityFrameworkCore;

namespace vortexUserConfig.UsersConfig.Presentation.Common.ValidatePermissions;

public class OneRoleRepository : Roles
{
    //Data for the constructor
    private readonly LibraryDbContext _context;
 
    //
    // Roles class is a heredit this atributes from the UserEntity
    //
    // public int Id = int.Newint();
    // public string Name = "";
    // public List<int> PermissionId = [];
    // public List<OnePermissionRepository> Permissions = [];
    // public int CreatedBy;
    // public DateTime CreatedAt = DateTime.Now;
    // public int? UpdatedBy = null;
    // public DateTime? UpdatedAt = null;
    // public bool IsDisable = false;
    // public int? DeletedBy = null;
    // public bool IsDeleted = false;

    //Constructor para que se use con permisos
    public OneRoleRepository(LibraryDbContext context)
    {
        _context = context;
    }

    //Private method for the init of the permission when it is first created
    private void InitRole(int createdBy, string name, List<Permissions> permissions)
    {
        this.CreatedBy = createdBy;
        this.Name = name;
        this.Permissions = permissions;
    }

    //Private method for the init of the permission when it is pull from the database
    private void InitRole(
        int id, string name, List<Permissions> permissions,
        int createdBy, DateTime createdAt, int? updatedBy,
        DateTime? updatedAt, bool isDisable, int? deletedBy, bool isDeleted)
    {
        this.Id = id;
        this.Name = name;
        this.Permissions = permissions;
        this.CreatedBy = createdBy;
        this.CreatedAt = createdAt;
        this.UpdatedBy = updatedBy;
        this.UpdatedAt = updatedAt;
        this.IsDisable = isDisable;
        this.DeletedBy = deletedBy;
        this.IsDisable = isDeleted;
    }
    
    private async Task SaveAsync()
    {
        _context.Roles.Update(this);
        await _context.SaveChangesAsync();
    }
    
    private void SetUpdateBy(int updatedBy)
    {
        this.UpdatedBy = updatedBy;
        this.UpdatedAt = DateTime.Now;
    }

    public async Task<OneRoleRepository> Create(int createdBy, string name, List<Permissions> permissions)
    {
        InitRole(createdBy, name, permissions);
        _context.Roles.Add(this);
        await _context.SaveChangesAsync();
        return this;
    }

    public async Task<OneRoleRepository> GetById(int id)
    {
        var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == id);

        if (role == null)
        {
            return new OneRoleRepository(_context) ;
        }

        InitRole(
            role.Id, role.Name, role.Permissions,
            role.CreatedBy, role.CreatedAt, role.UpdatedBy,
            role.UpdatedAt, role.IsDisable, role.DeletedBy, role.IsDeleted
        );

        return this;
    }
    
    public async Task<OneRoleRepository> FindBySpecification(Expression<Func<Roles, bool>> spec)
    {
        IQueryable<Roles> query = _context.Roles.Where(spec);

        var role = await query.FirstOrDefaultAsync();
        
        if( role is not null )
        {        
            InitRole(
                role.Id, role.Name, role.Permissions,
                role.CreatedBy, role.CreatedAt, role.UpdatedBy,
                role.UpdatedAt, role.IsDisable, role.DeletedBy, role.IsDeleted
            );
        } 
        
        return this;
    }

    
    public async Task<OneRoleRepository> ChangeName(int updatedBy, string name)
    {
        this.Name = name;
        SetUpdateBy(updatedBy);
        await SaveAsync();
        return this;
    }
    
    public async Task<OneRoleRepository> ChangePermissions(int updatedBy, List<int> permissionsId)
    {
        this.PermissionId = permissionsId;
        SetUpdateBy(updatedBy);
        await SaveAsync();
        return this;
    } 
    
    public async Task<OneRoleRepository> Disable(int updatedBy)
    {
        this.IsDisable = true;
        SetUpdateBy(updatedBy);
        await SaveAsync();
        return this;
    }
    
    public async Task<bool> SoftDelete(int id, int deletedBy)
    {
        this.DeletedBy = deletedBy;
        this.IsDeleted = true;
        await SaveAsync();
        return true;
        
    } 
    
    public async Task<bool> HardDelete(int id)
    {
        _context.Roles.Remove(this);
        await _context.SaveChangesAsync();
        return true;
        
    }



}

