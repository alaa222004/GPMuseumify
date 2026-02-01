using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GPMuseumify.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddUserFavoriteNews22 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserFavoriteNews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ItemType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFavoriteNews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserFavoriteNews_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserFavoriteNews_UserId_ItemId_ItemType",
                table: "UserFavoriteNews",
                columns: new[] { "UserId", "ItemId", "ItemType" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserFavoriteNews");
        }
    }
}
