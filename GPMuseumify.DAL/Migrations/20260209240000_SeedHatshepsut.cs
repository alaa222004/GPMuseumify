using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GPMuseumify.DAL.Migrations
{
    /// <inheritdoc />
    public partial class SeedHatshepsut : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var baseTime = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            // فيديو حتشبسوت من Drive — صيغة preview للتضمين عند الـ upload/scan
            var videoUrl = "https://drive.google.com/file/d/1h5KQFnc9ezFRvMU_F_NSz7gdXG3h7ZiW/preview";

            migrationBuilder.InsertData(
                table: "Statues",
                columns: new[] { "Id", "Name", "NameAr", "Description", "DescriptionAr", "HistoricalPeriod", "Location", "Museum", "VideoUrl", "ThumbnailUrl", "CreatedAt", "UpdatedAt", "IsActive" },
                values: new object[] { new Guid("77777777-7777-7777-7777-777777770001"), "Hatshepsut", "حتشبسوت", "Hatshepsut, one of the greatest pharaohs of ancient Egypt, ruled as king and left an enduring architectural legacy at Deir el-Bahari.", "حتشبسوت من أعظم ملكات مصر القديمة؛ حكمت كفرعون وتركت إرثاً معمارياً خالداً في الدير البحري.", "New Kingdom", "Egypt", "Deir el-Bahari, Luxor", videoUrl, null, baseTime, baseTime, true });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Statues",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777770001"));
        }
    }
}
