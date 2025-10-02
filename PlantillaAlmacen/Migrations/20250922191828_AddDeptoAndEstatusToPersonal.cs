using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlantillaAlmacen.Migrations
{
    /// <inheritdoc />
    public partial class AddDeptoAndEstatusToPersonal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Departamento",
                table: "Personal");

            migrationBuilder.DropColumn(
                name: "Estatus",
                table: "Personal");

            migrationBuilder.CreateTable(
                name: "Departamento",
                columns: table => new
                {
                    IdDepartamento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departamento", x => x.IdDepartamento);
                });

            migrationBuilder.CreateTable(
                name: "EstatusPersonal",
                columns: table => new
                {
                    IdEstatusPersonal = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstatusPersonal", x => x.IdEstatusPersonal);
                });

            migrationBuilder.AddColumn<int>(
                name: "IdDepartamento",
                table: "Personal",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdEstatusPersonal",
                table: "Personal",
                type: "int",
                nullable: true);

            migrationBuilder.Sql(@"
        INSERT INTO Departamento (Nombre, Descripcion, Activo) VALUES ('Sin departamento', 'Temporal migración', 1);
        DECLARE @depId INT = SCOPE_IDENTITY();

        INSERT INTO EstatusPersonal (Nombre, Activo) VALUES ('Sin estatus', 1);
        DECLARE @estId INT = SCOPE_IDENTITY();

        UPDATE Personal SET IdDepartamento = @depId WHERE IdDepartamento IS NULL;
        UPDATE Personal SET IdEstatusPersonal = @estId WHERE IdEstatusPersonal IS NULL;
    ");

            migrationBuilder.AlterColumn<int>(
                name: "IdDepartamento",
                table: "Personal",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IdEstatusPersonal",
                table: "Personal",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Personal_IdDepartamento",
                table: "Personal",
                column: "IdDepartamento");

            migrationBuilder.CreateIndex(
                name: "IX_Personal_IdEstatusPersonal",
                table: "Personal",
                column: "IdEstatusPersonal");

            migrationBuilder.AddForeignKey(
                name: "FK_Personal_Departamento_IdDepartamento",
                table: "Personal",
                column: "IdDepartamento",
                principalTable: "Departamento",
                principalColumn: "IdDepartamento",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Personal_EstatusPersonal_IdEstatusPersonal",
                table: "Personal",
                column: "IdEstatusPersonal",
                principalTable: "EstatusPersonal",
                principalColumn: "IdEstatusPersonal",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Personal_Departamento_IdDepartamento",
                table: "Personal");

            migrationBuilder.DropForeignKey(
                name: "FK_Personal_EstatusPersonal_IdEstatusPersonal",
                table: "Personal");

            migrationBuilder.DropTable(
                name: "Departamento");

            migrationBuilder.DropTable(
                name: "EstatusPersonal");

            migrationBuilder.DropIndex(
                name: "IX_Personal_IdDepartamento",
                table: "Personal");

            migrationBuilder.DropIndex(
                name: "IX_Personal_IdEstatusPersonal",
                table: "Personal");

            migrationBuilder.DropColumn(
                name: "IdDepartamento",
                table: "Personal");

            migrationBuilder.DropColumn(
                name: "IdEstatusPersonal",
                table: "Personal");

            migrationBuilder.AddColumn<string>(
                name: "Departamento",
                table: "Personal",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Estatus",
                table: "Personal",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
