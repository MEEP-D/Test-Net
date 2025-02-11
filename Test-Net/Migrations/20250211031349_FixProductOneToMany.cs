using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Test_Net.Migrations
{
    /// <inheritdoc />
    public partial class FixProductOneToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductTypes_ProductTypeId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductProductTypes",
                table: "ProductProductTypes");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ProductProductTypes",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductProductTypes",
                table: "ProductProductTypes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProductProductTypes_ProductId",
                table: "ProductProductTypes",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductTypes_ProductTypeId",
                table: "Products",
                column: "ProductTypeId",
                principalTable: "ProductTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductTypes_ProductTypeId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductProductTypes",
                table: "ProductProductTypes");

            migrationBuilder.DropIndex(
                name: "IX_ProductProductTypes_ProductId",
                table: "ProductProductTypes");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ProductProductTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductProductTypes",
                table: "ProductProductTypes",
                columns: new[] { "ProductId", "ProductTypeId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductTypes_ProductTypeId",
                table: "Products",
                column: "ProductTypeId",
                principalTable: "ProductTypes",
                principalColumn: "Id");
        }
    }
}
