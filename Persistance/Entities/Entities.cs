namespace FernandoLibrary.Persistance.Entities
{
    public class Usuario
    {
        //OneUserRepository personal data
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string LastName { get; set; }
        
        //OneUserRepository config 
        public string UserName { get; set; }
        public string Email { get; set; }
        
        //OneUserRepository password
        public string Password { get; set; }
        
        //OneUserRepository role
        //El usuario puede no tener un rol
        public int? RoleId { get; set; }
        public Roles? Role { get; set; }
        public List<Prestamo> Prestamos { get; set; } = new();
    }

    public class Autor
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<Libro> Libros { get; set; } = new();
    }

    public class Categoria
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<Libro> Libros { get; set; } = new();
    }

    public class Libro
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public int AutorId { get; set; }
        public Autor Autor { get; set; } = null;
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; } = null;
        public int Cantidad { get; set; }
        public List<Prestamo> Prestamos { get; set; } = new();
    }

    public class Prestamo
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        public int LibroId { get; set; }
        public Libro Libro { get; set; }
        public DateTime FechaPrestamo { get; set; }
        public DateTime? FechaDevolucion { get; set; }
    }

    public class Roles
    {
        //Paramaters
        public int Id { get; set; }
        public string Name { get; set; } = "";
        
        //Relations
        public List<int> PermissionId { get; set; } = [];
        public List<Permissions> Permissions { get; set; } = [];
        
        //Data for management
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int? UpdatedBy { get; set; } = null;
        public DateTime? UpdatedAt { get; set; } = null;
        public bool IsDisable { get; set; } = false;
        public int? DeletedBy { get; set; } = null;
        public bool IsDeleted { get; set; } = false;
    }

    public class Permissions
    {
        //Values for the class
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Description{ get; set; } = "";
        
        //Relations
        public List<Roles> Roles { get; set; } = [];

        
        //Data for management
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int? UpdatedBy { get; set; } = null;
        public DateTime? UpdatedAt { get; set; } = null;
        public bool IsDisable { get; set; } = false;
        public int? DeletedBy { get; set; } = null;
        public bool IsDeleted { get; set; } = false;
    }

    public class RolePermissions
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public Roles Roles { get; set; }
        public int PermissionId { get; set; }
        public Permissions Permissions { get; set; }
    }

    public class Sessions
    {
        public int Id { get; set; }
        public int UserId { get; set; }
    }
    
}