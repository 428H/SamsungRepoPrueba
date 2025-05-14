using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SamsungV1.Data.Migrations
{
    /// <inheritdoc />
    public partial class tblfactura : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetallesVenta_Producto_Id_producto",
                table: "DetallesVenta");

            migrationBuilder.CreateTable(
                name: "Facturas",
                columns: table => new
                {
                    Id_factura = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Numero = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Id_venta = table.Column<int>(type: "int", nullable: false),
                    Id_cliente = table.Column<int>(type: "int", nullable: false),
                    Fecha_emision = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Id_usuario = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facturas", x => x.Id_factura);
                    table.ForeignKey(
                        name: "FK_Facturas_AspNetUsers_Id_usuario",
                        column: x => x.Id_usuario,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Facturas_Cliente_Id_cliente",
                        column: x => x.Id_cliente,
                        principalTable: "Cliente",
                        principalColumn: "Id_cliente");
                    table.ForeignKey(
                        name: "FK_Facturas_Ventas_Id_venta",
                        column: x => x.Id_venta,
                        principalTable: "Ventas",
                        principalColumn: "Id_venta");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_Id_cliente",
                table: "Facturas",
                column: "Id_cliente");

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_Id_usuario",
                table: "Facturas",
                column: "Id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_Id_venta",
                table: "Facturas",
                column: "Id_venta",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DetallesVenta_Producto_Id_producto",
                table: "DetallesVenta",
                column: "Id_producto",
                principalTable: "Producto",
                principalColumn: "id_producto");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetallesVenta_Producto_Id_producto",
                table: "DetallesVenta");

            migrationBuilder.DropTable(
                name: "Facturas");

            migrationBuilder.AddForeignKey(
                name: "FK_DetallesVenta_Producto_Id_producto",
                table: "DetallesVenta",
                column: "Id_producto",
                principalTable: "Producto",
                principalColumn: "id_producto",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
