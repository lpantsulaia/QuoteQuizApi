using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteQuizApi.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "PasswordHash", "PasswordSalt", "UserName" },
                values: new object[] { 1, new byte[] { 5, 12, 172, 226, 241, 30, 27, 186, 153, 63, 89, 132, 122, 211, 8, 145, 128, 46, 99, 86, 189, 115, 230, 220, 32, 58, 207, 122, 246, 204, 95, 25, 217, 35, 157, 27, 139, 32, 212, 65, 69, 240, 155, 161, 181, 10, 110, 187, 153, 192, 107, 12, 5, 154, 205, 82, 182, 31, 87, 99, 182, 114, 198, 35 }, new byte[] { 115, 64, 138, 115, 64, 53, 175, 14, 55, 147, 217, 150, 228, 153, 158, 103, 3, 44, 191, 7, 223, 218, 58, 208, 25, 129, 89, 35, 217, 192, 185, 251, 65, 246, 203, 105, 30, 100, 195, 212, 240, 170, 57, 251, 153, 54, 164, 180, 160, 196, 16, 5, 25, 150, 245, 234, 43, 182, 219, 174, 25, 64, 153, 62, 65, 2, 180, 5, 28, 105, 111, 172, 77, 232, 197, 202, 36, 82, 9, 130, 155, 232, 173, 190, 184, 162, 2, 161, 240, 175, 24, 135, 14, 19, 168, 134, 43, 67, 214, 40, 239, 33, 189, 51, 62, 99, 247, 228, 86, 135, 96, 44, 91, 148, 109, 65, 43, 88, 92, 17, 224, 195, 58, 75, 8, 210, 136, 113 }, "admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
