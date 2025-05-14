using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SamsungV1.Data.Migrations
{
    /// <inheritdoc />
    public partial class tblDetallesVenta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DetallesVenta",
                columns: table => new
                {
                    Id_detalle = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_venta = table.Column<int>(type: "int", nullable: false),
                    Id_producto = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Precio_unitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetallesVenta", x => x.Id_detalle);
                    table.ForeignKey(
                        name: "FK_DetallesVenta_Producto_Id_producto",
                        column: x => x.Id_producto,
                        principalTable: "Producto",
                        principalColumn: "id_producto",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetallesVenta_Ventas_Id_venta",
                        column: x => x.Id_venta,
                        principalTable: "Ventas",
                        principalColumn: "Id_venta",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DetallesVenta_Id_producto",
                table: "DetallesVenta",
                column: "Id_producto");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesVenta_Id_venta",
                table: "DetallesVenta",
                column: "Id_venta");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetallesVenta");
        }
    }
}
