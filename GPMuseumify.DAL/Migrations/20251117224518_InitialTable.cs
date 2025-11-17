using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GPMuseumify.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Museums",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Museums", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Statues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HistoricalPeriod = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Museum = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    VideoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ThumbnailUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsEmailVerified = table.Column<bool>(type: "bit", nullable: false),
                    EmailVerificationToken = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    EmailVerificationExpiry = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResetPasswordToken = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ResetPasswordExpiry = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StatueImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StatueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ImageHash = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatueImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StatueImages_Statues_StatueId",
                        column: x => x.StatueId,
                        principalTable: "Statues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TitleAr = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VideoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    StatueId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stories_Statues_StatueId",
                        column: x => x.StatueId,
                        principalTable: "Statues",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ImageRecognitionLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UploadedImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RecognizedStatueId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Confidence = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    RecognitionMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageRecognitionLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImageRecognitionLogs_Statues_RecognizedStatueId",
                        column: x => x.RecognizedStatueId,
                        principalTable: "Statues",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ImageRecognitionLogs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserFavorites",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StatueId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MuseumId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFavorites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserFavorites_Museums_MuseumId",
                        column: x => x.MuseumId,
                        principalTable: "Museums",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserFavorites_Statues_StatueId",
                        column: x => x.StatueId,
                        principalTable: "Statues",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserFavorites_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StatueId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MuseumId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SearchType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ViewedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserHistories_Museums_MuseumId",
                        column: x => x.MuseumId,
                        principalTable: "Museums",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserHistories_Statues_StatueId",
                        column: x => x.StatueId,
                        principalTable: "Statues",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserHistories_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImageRecognitionLogs_RecognizedStatueId",
                table: "ImageRecognitionLogs",
                column: "RecognizedStatueId");

            migrationBuilder.CreateIndex(
                name: "IX_ImageRecognitionLogs_UserId",
                table: "ImageRecognitionLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_StatueImages_StatueId",
                table: "StatueImages",
                column: "StatueId");

            migrationBuilder.CreateIndex(
                name: "IX_Statues_Name",
                table: "Statues",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Stories_StatueId",
                table: "Stories",
                column: "StatueId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFavorites_MuseumId",
                table: "UserFavorites",
                column: "MuseumId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFavorites_StatueId",
                table: "UserFavorites",
                column: "StatueId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFavorites_UserId_MuseumId",
                table: "UserFavorites",
                columns: new[] { "UserId", "MuseumId" },
                unique: true,
                filter: "[MuseumId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserFavorites_UserId_StatueId",
                table: "UserFavorites",
                columns: new[] { "UserId", "StatueId" },
                unique: true,
                filter: "[StatueId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserHistories_MuseumId",
                table: "UserHistories",
                column: "MuseumId");

            migrationBuilder.CreateIndex(
                name: "IX_UserHistories_StatueId",
                table: "UserHistories",
                column: "StatueId");

            migrationBuilder.CreateIndex(
                name: "IX_UserHistories_UserId",
                table: "UserHistories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Role",
                table: "Users",
                column: "Role");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImageRecognitionLogs");

            migrationBuilder.DropTable(
                name: "StatueImages");

            migrationBuilder.DropTable(
                name: "Stories");

            migrationBuilder.DropTable(
                name: "UserFavorites");

            migrationBuilder.DropTable(
                name: "UserHistories");

            migrationBuilder.DropTable(
                name: "Museums");

            migrationBuilder.DropTable(
                name: "Statues");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
