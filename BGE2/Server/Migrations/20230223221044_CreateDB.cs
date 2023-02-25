using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BGE2.Server.Migrations
{
    /// <inheritdoc />
    public partial class CreateDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserRights",
                columns: table => new
                {
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Role = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRights", x => x.Email);
                });

            migrationBuilder.InsertData(
                table: "UserRights",
                columns: new[] { "Email", "Role" },
                values: new object[] { "joel.blomberg@iths.se", "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserRights");
        }
    }
}
