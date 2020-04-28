using Microsoft.EntityFrameworkCore.Migrations;

namespace PurchaseMeNow.DataAccess.Migrations
{
    public partial class addLocationToOrderHeader : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "OrderHeaders",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderHeaders_LocationId",
                table: "OrderHeaders",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderHeaders_Locations_LocationId",
                table: "OrderHeaders",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderHeaders_Locations_LocationId",
                table: "OrderHeaders");

            migrationBuilder.DropIndex(
                name: "IX_OrderHeaders_LocationId",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "OrderHeaders");
        }
    }
}
