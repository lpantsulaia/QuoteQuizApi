using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteQuizApi.Migrations
{
    public partial class Quotes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Quotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Contetnt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Quotes_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuoteAnswers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuoteId = table.Column<int>(type: "int", nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuoteAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuoteAnswers_Quotes_QuoteId",
                        column: x => x.QuoteId,
                        principalTable: "Quotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 6, 120, 119, 131, 202, 39, 85, 175, 203, 93, 128, 69, 92, 123, 110, 186, 166, 113, 46, 80, 59, 173, 171, 25, 227, 41, 237, 83, 250, 198, 40, 118, 130, 99, 149, 219, 102, 112, 131, 198, 217, 197, 221, 84, 247, 60, 235, 206, 80, 190, 160, 64, 184, 16, 51, 94, 40, 151, 83, 183, 32, 115, 18, 113 }, new byte[] { 55, 171, 12, 3, 14, 22, 164, 107, 30, 54, 122, 196, 24, 211, 143, 226, 2, 121, 7, 236, 201, 3, 102, 214, 168, 9, 167, 212, 119, 158, 129, 235, 184, 200, 157, 101, 199, 102, 18, 97, 90, 16, 197, 30, 49, 53, 68, 110, 208, 86, 55, 232, 28, 113, 7, 232, 116, 100, 100, 211, 41, 106, 16, 45, 147, 197, 114, 120, 133, 124, 221, 51, 11, 44, 56, 104, 57, 71, 75, 164, 106, 125, 89, 80, 202, 214, 195, 171, 169, 184, 58, 0, 48, 197, 13, 101, 110, 160, 199, 106, 62, 108, 17, 44, 36, 41, 58, 0, 208, 80, 26, 136, 183, 11, 116, 226, 114, 15, 232, 12, 153, 255, 155, 183, 61, 240, 68, 65 } });

            migrationBuilder.CreateIndex(
                name: "IX_QuoteAnswers_QuoteId",
                table: "QuoteAnswers",
                column: "QuoteId");

            migrationBuilder.CreateIndex(
                name: "IX_Quotes_CreatorId",
                table: "Quotes",
                column: "CreatorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuoteAnswers");

            migrationBuilder.DropTable(
                name: "Quotes");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 153, 30, 120, 147, 143, 78, 134, 34, 169, 181, 232, 21, 215, 178, 83, 214, 115, 67, 82, 92, 102, 234, 27, 133, 79, 102, 80, 34, 226, 9, 47, 238, 144, 115, 198, 199, 177, 44, 68, 158, 172, 81, 99, 64, 63, 8, 26, 193, 56, 61, 0, 159, 96, 5, 53, 102, 57, 39, 8, 207, 204, 84, 11, 46 }, new byte[] { 137, 71, 182, 146, 79, 243, 170, 47, 97, 154, 71, 208, 27, 219, 115, 212, 32, 58, 226, 58, 150, 34, 148, 214, 133, 163, 232, 108, 87, 245, 85, 60, 103, 98, 19, 133, 144, 11, 67, 95, 91, 88, 83, 106, 216, 184, 33, 92, 64, 0, 97, 72, 236, 89, 16, 172, 93, 36, 249, 218, 131, 32, 28, 147, 38, 166, 63, 158, 237, 236, 106, 91, 239, 41, 124, 139, 64, 126, 25, 23, 161, 34, 235, 115, 41, 195, 129, 148, 204, 236, 178, 41, 115, 24, 82, 127, 109, 216, 183, 105, 234, 221, 87, 191, 0, 5, 218, 4, 211, 203, 252, 109, 175, 108, 250, 149, 249, 147, 46, 212, 18, 88, 46, 209, 169, 171, 161, 89 } });
        }
    }
}
