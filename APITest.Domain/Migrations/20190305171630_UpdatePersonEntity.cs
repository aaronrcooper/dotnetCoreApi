using Microsoft.EntityFrameworkCore.Migrations;

namespace APITest.Migrations
{
    public partial class UpdatePersonEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "People",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.CreateIndex(
                name: "IX_People_Email",
                table: "People",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_People_Email",
                table: "People");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "People",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
