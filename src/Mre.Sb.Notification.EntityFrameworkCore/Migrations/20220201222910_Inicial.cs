using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mre.Sb.Notification.Migrations
{
    public partial class Inicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Configuracion",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Valor = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    Proveedor = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    ProveedorClave = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuracion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Plantilla",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Contenido = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plantilla", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Configuracion_Nombre_Proveedor_ProveedorClave",
                table: "Configuracion",
                columns: new[] { "Nombre", "Proveedor", "ProveedorClave" },
                unique: true,
                filter: "[Proveedor] IS NOT NULL AND [ProveedorClave] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Configuracion");

            migrationBuilder.DropTable(
                name: "Plantilla");
        }
    }
}
