using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SamsungV1.Data.Migrations
{
    /// <inheritdoc />
    public partial class tablaMoInventario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_inventarioActual_Producto_id_producto",
                table: "inventarioActual");

            migrationBuilder.DropPrimaryKey(
                name: "PK_inventarioActual",
                table: "inventarioActual");

            migrationBuilder.RenameTable(
                name: "inventarioActual",
                newName: "InventarioActual");

            migrationBuilder.RenameIndex(
                name: "IX_inventarioActual_id_producto",
                table: "InventarioActual",
                newName: "IX_InventarioActual_id_producto");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InventarioActual",
                table: "InventarioActual",
                column: "id_inventario");

            migrationBuilder.CreateTable(
                name: "MovimientoInventario",
                columns: table => new
                {
                    id_movimiento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_producto = table.Column<int>(type: "int", nullable: false),
                    cantidad = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    id_tipo_movimiento = table.Column<int>(type: "int", nullable: false),
                    fecha_movimiento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    motivo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    id_usuario = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovimientoInventario", x => x.id_movimiento);
                    table.ForeignKey(
                        name: "FK_MovimientoInventario_AspNetUsers_id_usuario",
                        column: x => x.id_usuario,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MovimientoInventario_Producto_id_producto",
                        column: x => x.id_producto,
                        principalTable: "Producto",
                        principalColumn: "id_producto");
                    table.ForeignKey(
                        name: "FK_MovimientoInventario_tipoMovimientoI_id_tipo_movimiento",
                        column: x => x.id_tipo_movimiento,
                        principalTable: "tipoMovimientoI",
                        principalColumn: "id_tipomovimiento");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MovimientoInventario_id_producto",
                table: "MovimientoInventario",
                column: "id_producto");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientoInventario_id_tipo_movimiento",
                table: "MovimientoInventario",
                column: "id_tipo_movimiento");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientoInventario_id_usuario",
                table: "MovimientoInventario",
                column: "id_usuario");

            migrationBuilder.AddForeignKey(
                name: "FK_InventarioActual_Producto_id_producto",
                table: "InventarioActual",
                column: "id_producto",
                principalTable: "Producto",
                principalColumn: "id_producto",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventarioActual_Producto_id_producto",
                table: "InventarioActual");

            migrationBuilder.DropTable(
                name: "MovimientoInventario");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InventarioActual",
                table: "InventarioActual");

            migrationBuilder.RenameTable(
                name: "InventarioActual",
                newName: "inventarioActual");

            migrationBuilder.RenameIndex(
                name: "IX_InventarioActual_id_producto",
                table: "inventarioActual",
                newName: "IX_inventarioActual_id_producto");

            migrationBuilder.AddPrimaryKey(
                name: "PK_inventarioActual",
                table: "inventarioActual",
                column: "id_inventario");

            migrationBuilder.AddForeignKey(
                name: "FK_inventarioActual_Producto_id_producto",
                table: "inventarioActual",
                column: "id_producto",
                principalTable: "Producto",
                principalColumn: "id_producto",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
