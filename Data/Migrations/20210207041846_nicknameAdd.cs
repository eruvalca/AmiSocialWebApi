using Microsoft.EntityFrameworkCore.Migrations;

namespace AmiSocialWebApi.Data.Migrations
{
    public partial class nicknameAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Members",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Members");
        }
    }
}
