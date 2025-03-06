using System.Linq.Expressions;
using System.Net.Mail;
using FernandoLibrary.Persistance;
using FernandoLibrary.Persistance.Entities;
using FernandoLibrary.Presentation.Services.Authentication;
using Microsoft.EntityFrameworkCore;
using vortexUserConfig.UsersConfig.Presentation.Services.JwtConfig;

namespace FernandoLibrary.Presentation.Common;

public class OneUserRepository : Usuario
{
    // Variables for the constructor 
    private readonly JwtToken _jwtToken; 
    private readonly PasswordService _passwordService = new PasswordService();
    private readonly LibraryDbContext _userDbContext;

    //
    // OneUserRepository class is a heredit this atributes from the UserEntity
    //
    // public int Id = int.Newint();
    // public string Nombre;
    // public string LastName;
    // public string UserName;
    // public string Email;
    // public string Password;
    // public int? RoleId;
    // public Roles? Roles;

    //Made to create base user that contains the basic information
    public OneUserRepository(
        JwtToken jwtToken,
        LibraryDbContext userDbContext)
    {
        //Constructor
        _jwtToken = jwtToken;
        _userDbContext = userDbContext;
    }
    
    //Private method use for the init of a new user
    private void InitUser(
        string name, string lastName, string userName,
        string email, string password, int? roleId
    )
    {
        this.Nombre= name;
        this.LastName = lastName;
        this.UserName = userName;
        this.Email = email;
        this.Password = password;
        this.RoleId = roleId;
    }
    
    //Private method use for the init of a user get from the database
    private void InitUser(
        int id, string name, string lastName, string userName,
        string email, string password, int? roleId, Roles? role
    )
    {
        this.Id = id;
        this.Nombre = name;
        this.LastName = lastName;
        this.UserName = userName;
        this.Email = email;
        this.Password = password;
        this.RoleId = roleId;
        this.Role = role;
    }

    private async Task SaveAsync()
    {
        _userDbContext.Usuarios.Update(this);
        await _userDbContext.SaveChangesAsync();
    }

    //Method to create a new user
    public async Task<OneUserRepository> Create(
        string name, string lastName, string userName,
        string email, string password, int? roleId
    )
    {
        if (! (MailAddress.TryCreate(email, out var mailAddress)))
        {
            return null;    
        }

        //To create the user 
        InitUser(name, lastName, userName, email, password, roleId);
        
        //Save the user
        _userDbContext.Usuarios.Add(this);
        await _userDbContext.SaveChangesAsync();
        return this;
    }
    
    //Method to get a user by its id
    public async Task<string> GetById(int id)
    {
        var user =  await _userDbContext.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
        
        if(user == null)
        {
            return  "OneUserRepository not found";
        }
        
        InitUser(
            user.Id, user.Nombre, user.LastName, user.UserName,
            user.Email, user.Password, user.RoleId, user.Role!
        );
        
        return "OneUserRepository " + GetFullName() + " found and loaded ";
    }
    
    public async Task<OneUserRepository> FindBySpecification(Expression<Func<Usuario, bool>> spec)
    {
        IQueryable<Usuario> query = _userDbContext.Usuarios
            .Where(spec);

        var user = await query.FirstOrDefaultAsync();
        
        if(user is not null )
        {
            InitUser(user.Id, user.Nombre, user.LastName, 
                user.UserName, user.Email, user.Password,
                user.RoleId, user.Role!);
        } 
        
        return this;
    }

    //Method to get a JwtToken for the user
    public async Task<string> GenerateJwtToken()
    {


        return _jwtToken.GenerateToken(this);
    }

    //Compare passwords you give it a string and it gaves you a boolean
    public bool ComparePassword(string password)
    {
        string hashdedPassword = _passwordService.Hash(password); 
        
        if( this.Password == hashdedPassword)
        {
            return true;
        }

        return false;
    }

    //Change the user name 
    public async Task<bool> ChangeUserName(string name, string lastName)
    {
        if ( name.Trim() == string.Empty || lastName.Trim() == string.Empty)
        {
            return false;
        }
        
        this.Nombre = name;
        this.LastName = lastName;

        await SaveAsync();
        
        return true;
    }
    
    //Change the user email
    public async Task<bool> ChangeEmail(string email)
    {
        if (new MailAddress(email).Address != email)
        {
            return false;
        }
        
        this.Email = email;
        await SaveAsync();
        return true;
    }
    
    public async Task<bool> ChangePassword(string password)
    {
        if (password.Trim() == string.Empty)
        {
            return false;
        }
        
        this.Password = _passwordService.Hash(password);
        await SaveAsync();
        return true;
    }
    
    public async Task<bool> ChangeRole(int roleId)
    {
        this.RoleId = roleId;
        await SaveAsync();
        return true;
    }
    
    // Returns the user fullname
    public string GetFullName()
    {
        return $"{this.Nombre} {this.LastName}";
    }
    
    
    
}