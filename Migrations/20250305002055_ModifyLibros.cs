using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FernandoLibrary.Migrations
{
    /// <inheritdoc />
    public partial class ModifyLibros : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prestamos_Ejemplares_EjemplarId",
                table: "Prestamos");

            migrationBuilder.DropTable(
                name: "Ejemplares");

            migrationBuilder.RenameColumn(
                name: "EjemplarId",
                table: "Prestamos",
                newName: "LibroId");

            migrationBuilder.RenameIndex(
                name: "IX_Prestamos_EjemplarId",
                table: "Prestamos",
                newName: "IX_Prestamos_LibroId");

            migrationBuilder.AddForeignKey(
                name: "FK_Prestamos_Libros_LibroId",
                table: "Prestamos",
                column: "LibroId",
                principalTable: "Libros",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prestamos_Libros_LibroId",
                table: "Prestamos");

            migrationBuilder.RenameColumn(
                name: "LibroId",
                table: "Prestamos",
                newName: "EjemplarId");

            migrationBuilder.RenameIndex(
                name: "IX_Prestamos_LibroId",
                table: "Prestamos",
                newName: "IX_Prestamos_EjemplarId");

            migrationBuilder.CreateTable(
                name: "Ejemplares",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LibroId = table.Column<int>(type: "INTEGER", nullable: false),
                    Estado = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ejemplares", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ejemplares_Libros_LibroId",
                        column: x => x.LibroId,
                        principalTable: "Libros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ejemplares_LibroId",
                table: "Ejemplares",
                column: "LibroId");

            migrationBuilder.AddForeignKey(
                name: "FK_Prestamos_Ejemplares_EjemplarId",
                table: "Prestamos",
                column: "EjemplarId",
                principalTable: "Ejemplares",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
