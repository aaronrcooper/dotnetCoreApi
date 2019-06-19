using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace APITest.Migrations
{
    public partial class Refactor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_People_Id",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_TodoItems_Users_userId",
                table: "TodoItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_People",
                table: "People");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TodoItems",
                table: "TodoItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Roles",
                table: "Roles");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "8110bff0-12a7-4cc7-906d-a9c052727e06");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "eaa559e4-090c-4706-a776-ffa16e7a2191");

            migrationBuilder.DeleteData(
                table: "People",
                keyColumn: "Id",
                keyValue: "eaa559e4-090c-4706-a776-ffa16e7a2191");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "a8cc16b7-aa6b-47d3-909a-06fdcae81619");

            migrationBuilder.AlterColumn<Guid>(
                name: "RoleId",
                table: "Users",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Users",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<Guid>(
                name: "userId",
                table: "TodoItems",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "TodoItems",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Roles",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "People",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.InsertData(
                table: "People",
                columns: new[] { "Id", "Address", "City", "Email", "FirstName", "LastName", "State", "Zipcode" },
                values: new object[] { new Guid("eaa559e4-090c-4706-a776-ffa16e7a2191"), "N/a", "Pittsburgh", "N/a", "Aaron", "Cooper", "PA", "N/a" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "UserRole" },
                values: new object[] { new Guid("a8cc16b7-aa6b-47d3-909a-06fdcae81619"), "Administrator" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "UserRole" },
                values: new object[] { new Guid("8110bff0-12a7-4cc7-906d-a9c052727e06"), "User" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "HashedPassword", "RoleId", "Salt", "Username" },
                values: new object[] { new Guid("eaa559e4-090c-4706-a776-ffa16e7a2191"), "Ov4B87zmh9j/dEG/y/BQlT3S8FA=", new Guid("a8cc16b7-aa6b-47d3-909a-06fdcae81619"), new byte[] { 92, 251, 117, 81, 232, 198, 132, 52, 28, 94, 233, 112, 135, 156, 117, 187 }, "Admin" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_People",
                table: "People",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Roles",
                table: "Roles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TodoItems",
                table: "TodoItems",
                column: "Id");
            
            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_People_Id",
                table: "Users",
                column: "Id",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TodoItems_Users_userId",
                table: "TodoItems",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <summary>
        /// THIS MIGRATION INTENTIONALLY LEFT BLANK
        /// </summary>
        /// <param name="migrationBuilder"></param>
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
