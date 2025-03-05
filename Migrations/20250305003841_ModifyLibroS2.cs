using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FernandoLibrary.Migrations
{
    /// <inheritdoc />
    public partial class ModifyLibroS2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Disponible",
                table: "Libros",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Disponible",
                table: "Libros");
        }
    }
}
