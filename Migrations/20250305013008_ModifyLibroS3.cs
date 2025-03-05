using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FernandoLibrary.Migrations
{
    /// <inheritdoc />
    public partial class ModifyLibroS3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Disponible",
                table: "Libros",
                newName: "Cantidad");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Cantidad",
                table: "Libros",
                newName: "Disponible");
        }
    }
}
