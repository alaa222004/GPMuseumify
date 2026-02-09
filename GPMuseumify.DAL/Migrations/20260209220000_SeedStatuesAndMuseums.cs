using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GPMuseumify.DAL.Migrations
{
    /// <inheritdoc />
    public partial class SeedStatuesAndMuseums : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var baseTime = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            migrationBuilder.InsertData(
                table: "Statues",
                columns: new[] { "Id", "Name", "NameAr", "Description", "DescriptionAr", "HistoricalPeriod", "Location", "Museum", "VideoUrl", "ThumbnailUrl", "CreatedAt", "UpdatedAt", "IsActive" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Akhenaten", "أخناتون", "Pharaoh of the 18th dynasty, known for religious revolution.", "فرعون الأسرة الثامنة عشرة، معروف بالثورة الدينية.", "New Kingdom", "Egypt", "Egyptian Museum", "https://example.com/placeholder", "/images/statues/akhenaten.jpg", baseTime, baseTime, true },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "Nefertiti", "نفرتيتي", "Queen of Egypt, wife of Akhenaten, famous bust.", "ملكة مصر، زوجة أخناتون، تمثال نصفي شهير.", "New Kingdom", "Egypt", "Neues Museum, Berlin", "https://example.com/placeholder", "/images/statues/nefertiti.jpg", baseTime, baseTime, true },
                    { new Guid("33333333-3333-3333-3333-333333333333"), "Ramses II", "رمسيس الثاني", "One of the greatest pharaohs of ancient Egypt.", "أحد أعظم فراعنة مصر القديمة.", "New Kingdom", "Egypt", "Grand Egyptian Museum", "https://example.com/placeholder", "/images/statues/ramses-ii.jpg", baseTime, baseTime, true }
                });

            migrationBuilder.InsertData(
                table: "Museums",
                columns: new[] { "Id", "Name", "NameAr", "Description", "Location", "ImageUrl", "CreatedAt", "IsActive" },
                values: new object[] { new Guid("44444444-4444-4444-4444-444444444444"), "Giza Pyramids", "أهرامات الجيزة", "The Great Pyramids of Giza, ancient wonder.", "Giza, Egypt", "/images/museums/pyramids.jpg", baseTime, true });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Statues",
                keyColumn: "Id",
                keyValues: new object[]
                {
                    new Guid("11111111-1111-1111-1111-111111111111"),
                    new Guid("22222222-2222-2222-2222-222222222222"),
                    new Guid("33333333-3333-3333-3333-333333333333")
                });

            migrationBuilder.DeleteData(
                table: "Museums",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"));
        }
    }
}
