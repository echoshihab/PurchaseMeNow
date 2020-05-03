using Microsoft.EntityFrameworkCore.Migrations;

namespace PurchaseMeNow.DataAccess.Migrations
{
    public partial class addNotificationColumnsToOrderHeader : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SeenByAdmin",
                table: "OrderHeaders",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SeenByCoordinator",
                table: "OrderHeaders",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SeenByEmployee",
                table: "OrderHeaders",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
