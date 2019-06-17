using Microsoft.EntityFrameworkCore.Migrations;

namespace APITest.Migrations
{
    public partial class AddUserTodoCollection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FK_User_Todo",
                table: "TodoItems",
                newName: "userId");

            migrationBuilder.AlterColumn<string>(
                name: "userId",
                table: "TodoItems",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TodoItems_userId",
                table: "TodoItems",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoItems_Users_userId",
                table: "TodoItems",
                column: "userId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoItems_Users_userId",
                table: "TodoItems");

            migrationBuilder.DropIndex(
                name: "IX_TodoItems_userId",
                table: "TodoItems");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "TodoItems",
                newName: "FK_User_Todo");

            migrationBuilder.AlterColumn<string>(
                name: "FK_User_Todo",
                table: "TodoItems",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
