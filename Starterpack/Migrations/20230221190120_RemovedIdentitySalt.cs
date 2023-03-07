using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Starterpack.Migrations
{
    /// <inheritdoc />
    public partial class RemovedIdentitySalt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IdentitySalt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IdentitySalt",
                columns: table => new
                {
                    Email = table.Column<string>(type: "text", nullable: false),
                    Salt = table.Column<string>(type: "text", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentitySalt", x => x.Email);
                });
        }
    }
}
