using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlantillaAlmacen.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnPersonal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ArchivoLicencia",
                table: "Personal",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TieneLicenciaManejo",
                table: "Personal",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "VigenciaLicencia",
                table: "Personal",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArchivoLicencia",
                table: "Personal");

            migrationBuilder.DropColumn(
                name: "TieneLicenciaManejo",
                table: "Personal");

            migrationBuilder.DropColumn(
                name: "VigenciaLicencia",
                table: "Personal");
        }
    }
}
