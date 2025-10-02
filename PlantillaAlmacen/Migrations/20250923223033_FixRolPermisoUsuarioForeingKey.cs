using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlantillaAlmacen.Migrations
{
    /// <inheritdoc />
    public partial class FixRolPermisoUsuarioForeingKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RolPermiso_Permiso_PermisoIdPermiso",
                table: "RolPermiso");

            migrationBuilder.DropForeignKey(
                name: "FK_RolPermiso_Rol_RolIdRol",
                table: "RolPermiso");

            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioRol_Rol_RolIdRol",
                table: "UsuarioRol");

            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioRol_Usuario_UsuarioIdUsuario",
                table: "UsuarioRol");

            migrationBuilder.DropIndex(
                name: "IX_UsuarioRol_RolIdRol",
                table: "UsuarioRol");

            migrationBuilder.DropIndex(
                name: "IX_UsuarioRol_UsuarioIdUsuario",
                table: "UsuarioRol");

            migrationBuilder.DropIndex(
                name: "IX_RolPermiso_PermisoIdPermiso",
                table: "RolPermiso");

            migrationBuilder.DropIndex(
                name: "IX_RolPermiso_RolIdRol",
                table: "RolPermiso");

            migrationBuilder.DropIndex(
                name: "IX_Asignacion_IdArticulo",
                table: "Asignacion");

            migrationBuilder.DropColumn(
                name: "RolIdRol",
                table: "UsuarioRol");

            migrationBuilder.DropColumn(
                name: "UsuarioIdUsuario",
                table: "UsuarioRol");

            migrationBuilder.DropColumn(
                name: "PermisoIdPermiso",
                table: "RolPermiso");

            migrationBuilder.DropColumn(
                name: "RolIdRol",
                table: "RolPermiso");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioRol_IdRol",
                table: "UsuarioRol",
                column: "IdRol");

            migrationBuilder.CreateIndex(
                name: "IX_RolPermiso_IdPermiso",
                table: "RolPermiso",
                column: "IdPermiso");

            migrationBuilder.CreateIndex(
                name: "IX_Asignacion_IdArticulo_FechaDevolucion",
                table: "Asignacion",
                columns: new[] { "IdArticulo", "FechaDevolucion" },
                unique: true,
                filter: "[FechaDevolucion] IS NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_RolPermiso_Permiso_IdPermiso",
                table: "RolPermiso",
                column: "IdPermiso",
                principalTable: "Permiso",
                principalColumn: "IdPermiso",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RolPermiso_Rol_IdRol",
                table: "RolPermiso",
                column: "IdRol",
                principalTable: "Rol",
                principalColumn: "IdRol",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioRol_Rol_IdRol",
                table: "UsuarioRol",
                column: "IdRol",
                principalTable: "Rol",
                principalColumn: "IdRol",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioRol_Usuario_IdUsuario",
                table: "UsuarioRol",
                column: "IdUsuario",
                principalTable: "Usuario",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RolPermiso_Permiso_IdPermiso",
                table: "RolPermiso");

            migrationBuilder.DropForeignKey(
                name: "FK_RolPermiso_Rol_IdRol",
                table: "RolPermiso");

            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioRol_Rol_IdRol",
                table: "UsuarioRol");

            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioRol_Usuario_IdUsuario",
                table: "UsuarioRol");

            migrationBuilder.DropIndex(
                name: "IX_UsuarioRol_IdRol",
                table: "UsuarioRol");

            migrationBuilder.DropIndex(
                name: "IX_RolPermiso_IdPermiso",
                table: "RolPermiso");

            migrationBuilder.DropIndex(
                name: "IX_Asignacion_IdArticulo_FechaDevolucion",
                table: "Asignacion");

            migrationBuilder.AddColumn<int>(
                name: "RolIdRol",
                table: "UsuarioRol",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UsuarioIdUsuario",
                table: "UsuarioRol",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PermisoIdPermiso",
                table: "RolPermiso",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RolIdRol",
                table: "RolPermiso",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioRol_RolIdRol",
                table: "UsuarioRol",
                column: "RolIdRol");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioRol_UsuarioIdUsuario",
                table: "UsuarioRol",
                column: "UsuarioIdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_RolPermiso_PermisoIdPermiso",
                table: "RolPermiso",
                column: "PermisoIdPermiso");

            migrationBuilder.CreateIndex(
                name: "IX_RolPermiso_RolIdRol",
                table: "RolPermiso",
                column: "RolIdRol");

            migrationBuilder.CreateIndex(
                name: "IX_Asignacion_IdArticulo",
                table: "Asignacion",
                column: "IdArticulo");

            migrationBuilder.AddForeignKey(
                name: "FK_RolPermiso_Permiso_PermisoIdPermiso",
                table: "RolPermiso",
                column: "PermisoIdPermiso",
                principalTable: "Permiso",
                principalColumn: "IdPermiso",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RolPermiso_Rol_RolIdRol",
                table: "RolPermiso",
                column: "RolIdRol",
                principalTable: "Rol",
                principalColumn: "IdRol",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioRol_Rol_RolIdRol",
                table: "UsuarioRol",
                column: "RolIdRol",
                principalTable: "Rol",
                principalColumn: "IdRol",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioRol_Usuario_UsuarioIdUsuario",
                table: "UsuarioRol",
                column: "UsuarioIdUsuario",
                principalTable: "Usuario",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
