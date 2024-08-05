using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBookingWebsite.Migrations
{
    /// <inheritdoc />
    public partial class AddCheckoutSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CheckoutSessionId",
                table: "Payments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CheckoutSessionId",
                table: "Payments");
        }
    }
}
