using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductInformation.Migrations
{
    public partial class AddDataToCategoryAndProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "ID", "Name" },
                values: new object[] { -3, "Auto" });

            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ID", "CategoryID", "Name" },
                values: new object[] { -6, -3, "Tires" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ID",
                keyValue: -6);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "ID",
                keyValue: -3);
        }
    }
}
