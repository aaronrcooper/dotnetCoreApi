using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace APITest.Migrations
{
    public partial class AddRolesAndSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RoleId",
                table: "Users",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserRole = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "People",
                columns: new[] { "Id", "Address", "City", "Email", "FirstName", "LastName", "State", "Zipcode" },
                values: new object[] { "eaa559e4-090c-4706-a776-ffa16e7a2191", "N/a", "Pittsburgh", "N/a", "Aaron", "Cooper", "PA", "N/a" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "UserRole" },
                values: new object[] { "a8cc16b7-aa6b-47d3-909a-06fdcae81619", "Administrator" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "UserRole" },
                values: new object[] { "8110bff0-12a7-4cc7-906d-a9c052727e06", "User" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "HashedPassword", "RoleId", "Salt", "Username" },
                values: new object[] { "eaa559e4-090c-4706-a776-ffa16e7a2191", "Ov4B87zmh9j/dEG/y/BQlT3S8FA=", "a8cc16b7-aa6b-47d3-909a-06fdcae81619", new byte[] { 92, 251, 117, 81, 232, 198, 132, 52, 28, 94, 233, 112, 135, 156, 117, 187 }, "Admin" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Users_RoleId",
                table: "Users");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "eaa559e4-090c-4706-a776-ffa16e7a2191");

            migrationBuilder.DeleteData(
                table: "People",
                keyColumn: "Id",
                keyValue: "eaa559e4-090c-4706-a776-ffa16e7a2191");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Users");
        }
    }
}
