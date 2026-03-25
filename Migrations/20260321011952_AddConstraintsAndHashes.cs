using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace socialcrap.Migrations
{
    /// <inheritdoc />
    public partial class AddConstraintsAndHashes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Poops_UsuarioId",
                table: "Poops");

            migrationBuilder.DropIndex(
                name: "IX_Amizades_UsuarioId",
                table: "Amizades");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Usuarios",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Poops_UsuarioId_CrapId",
                table: "Poops",
                columns: new[] { "UsuarioId", "CrapId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Amizades_UsuarioId_AmigoId",
                table: "Amizades",
                columns: new[] { "UsuarioId", "AmigoId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Poops_UsuarioId_CrapId",
                table: "Poops");

            migrationBuilder.DropIndex(
                name: "IX_Amizades_UsuarioId_AmigoId",
                table: "Amizades");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Poops_UsuarioId",
                table: "Poops",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Amizades_UsuarioId",
                table: "Amizades",
                column: "UsuarioId");
        }
    }
}
