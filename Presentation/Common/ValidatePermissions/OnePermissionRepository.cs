using System.Linq.Expressions;
using FernandoLibrary.Persistance;
using FernandoLibrary.Persistance.Entities;
using Microsoft.EntityFrameworkCore;

namespace FernandoLibrary.Presentation.Common.ValidatePermissions;

public class OnePermissionRepository : Permissions
{
    //Values for the constructor
    private readonly LibraryDbContext _context;

    //
    // OnePermissionRepository class is a heredit this atributes from the UserEntity
    //
    // public int Id = int.Newint();
    // public string Title = "";
    // public string Description = "";
    // public int CreatedBy;
    // public DateTime CreatedAt = DateTime.Now;
    // public int? UpdatedBy = null;
    // public DateTime? UpdatedAt = null;
    // public bool IsDisable = false;
    // public int? DeletedBy = null;
    // public bool IsDeleted = false;

    //Constructor
    public OnePermissionRepository(LibraryDbContext context)
    {
        _context = context;
    }
    
    //Private method for the init of the permission when it is first created
    private void InitPermission(int createdBy, string title, string description)
    {
        this.CreatedBy = createdBy;
        this.Title = title;
        this.Description = description;
    }

    //Private method for the init of the permission when it is pull from the database
    private void InitPermission(
        int id, string title, string description,
        int createdBy, DateTime createdAt , int? updatedBy ,
        DateTime? updatedAt , bool isDisable , int? deletedBy , bool isDeleted )
    {
        this.Id = id;
        this.Title = title;
        this.Description = description;
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
        _context.Permissions.Update(this);
        await _context.SaveChangesAsync();
    }
    
    private void SetUpdateBy(int updatedBy)
    {
        this.UpdatedBy = updatedBy;
        this.UpdatedAt = DateTime.Now;
    }
    
    public async Task<OnePermissionRepository> Create(int userId, string title, string description)
    {
        InitPermission(userId, title, description);
        
        _context.Permissions.Add(this);
        await _context.SaveChangesAsync();
        
        return this;
    }
    
    public async Task<OnePermissionRepository> GetById(int id)
    {
        var permission = await _context.Permissions.FirstOrDefaultAsync(p => p.Id == id);

        if (permission is not null)
        {
            InitPermission(
                permission.Id, permission.Title, permission.Description,
                permission. CreatedBy, permission.CreatedAt , permission.UpdatedBy,
                permission.UpdatedAt , permission.IsDisable , permission.DeletedBy , permission.IsDeleted 
                );
        }
        
        return this;
    }
    
    public async Task<OnePermissionRepository> FindBySpecification(Expression<Func<Permissions, bool>> spec)
    {
        IQueryable<Permissions> query = _context.Permissions.Where(spec);

        var permission = await query.FirstOrDefaultAsync();
        
        if( permission is not null )
        {        
            InitPermission(
                permission.Id, permission.Title, permission.Description,
                permission. CreatedBy, permission.CreatedAt , permission.UpdatedBy,
                permission.UpdatedAt , permission.IsDisable , permission.DeletedBy , permission.IsDeleted 
            );
        } 
        
        return this;
    }

    
    public async Task<OnePermissionRepository> ChangeTitle(int id, int updatedBy, string title, string description)
    {
        this.Title = title;
        SetUpdateBy(updatedBy);
        await SaveAsync();
        
        return this;
    }
    
    public async Task<OnePermissionRepository> ChangeDescription(int id, int updatedBy, string title, string description)
    {
        
        this.Description = description;
        SetUpdateBy(updatedBy);
        await SaveAsync();
        
        return this;
    }
    
    public async Task<bool> Disable(int id, int updatedBy)
    {
        
        this.IsDisable = true;
        SetUpdateBy(updatedBy);
        await SaveAsync();
        
        return true;
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
        _context.Permissions.Remove(this);
        await _context.SaveChangesAsync();
        return true;
    }
    
}