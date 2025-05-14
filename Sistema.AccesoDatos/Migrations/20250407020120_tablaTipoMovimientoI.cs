using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SamsungV1.Data.Migrations
{
    /// <inheritdoc />
    public partial class tablaTipoMovimientoI : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tipoMovimientoI",
                columns: table => new
                {
                    id_tipomovimiento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    afecta_stock = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    estado = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tipoMovimientoI", x => x.id_tipomovimiento);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tipoMovimientoI");
        }
    }
}
