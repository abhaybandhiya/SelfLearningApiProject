using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SelfLearningApiProject.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeedPasswords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Password", "Role", "Username" },
                values: new object[,]
                {
                    { 1, "$2a$12$l6fM6kpo4EMtIrkaRyGW3.TowOQtXcqCrtl0Hk5Ih95XEgshdTFQW", "Admin", "admin" },
                    { 2, "$2a$12$y9ivYvovbImDl8xzPpZVWeK9wGaqYX/qo0P2gUxW8y.TQaLEi3iRW", "User", "user" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
