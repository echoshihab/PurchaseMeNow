using Microsoft.EntityFrameworkCore.Migrations;

namespace PurchaseMeNow.DataAccess.Migrations
{
    public partial class remotveItemsFromOrderHeader : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SeenByAdmin",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "SeenByCoordinator",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "SeenByEmployee",
                table: "OrderHeaders");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SeenByAdmin",
                table: "OrderHeaders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SeenByCoordinator",
                table: "OrderHeaders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SeenByEmployee",
                table: "OrderHeaders",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
