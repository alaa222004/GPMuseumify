using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GPMuseumify.DAL.Migrations
{
    /// <inheritdoc />
    public partial class SeedMoreStatuesAndMuseums : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var baseTime = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var placeholderVideo = "https://example.com/placeholder";

            // تماثيل إضافية
            migrationBuilder.InsertData(
                table: "Statues",
                columns: new[] { "Id", "Name", "NameAr", "Description", "DescriptionAr", "HistoricalPeriod", "Location", "Museum", "VideoUrl", "ThumbnailUrl", "CreatedAt", "UpdatedAt", "IsActive" },
                values: new object[,]
                {
                    { new Guid("55555555-5555-5555-5555-555555550001"), "Tutankhamun", "توت عنخ آمون", "Pharaoh of the 18th dynasty, famous golden mask.", "فرعون الأسرة الثامنة عشرة، قناعه الذهبي شهير.", "New Kingdom", "Egypt", "Egyptian Museum, Cairo", placeholderVideo, "/images/statues/tutankhamun.jpg", baseTime, baseTime, true },
                    { new Guid("55555555-5555-5555-5555-555555550002"), "Pharaoh Bust", "تمثال نصفي فرعوني", "Ancient Egyptian pharaonic bust.", "تمثال نصفي فرعوني من مصر القديمة.", "New Kingdom", "Egypt", "Egyptian Museum", placeholderVideo, "/images/statues/pharaoh-bust.jpg", baseTime, baseTime, true },
                    { new Guid("55555555-5555-5555-5555-555555550003"), "Pharaoh Bust II", "تمثال نصفي فرعوني ٢", "Stone bust of an Egyptian pharaoh.", "تمثال حجري لفرعون مصري.", "New Kingdom", "Egypt", "Egyptian Museum", placeholderVideo, "/images/statues/pharaoh-bust-2.jpg", baseTime, baseTime, true },
                    { new Guid("55555555-5555-5555-5555-555555550004"), "Pharaoh Head", "رأس تمثال فرعوني", "Head of a pharaonic statue with ceremonial beard.", "رأس تمثال فرعوني باللحية الاحتفالية.", "New Kingdom", "Egypt", "Egyptian Museum", placeholderVideo, "/images/statues/pharaoh-head.jpg", baseTime, baseTime, true },
                    { new Guid("55555555-5555-5555-5555-555555550005"), "Pharaoh Standing", "تمثال فرعون واقف", "Full-length standing pharaoh statue.", "تمثال فرعون واقف بالكامل.", "New Kingdom", "Egypt", "Egyptian Museum", placeholderVideo, "/images/statues/pharaoh-standing.jpg", baseTime, baseTime, true },
                    { new Guid("55555555-5555-5555-5555-555555550006"), "Ramses Standing", "رمسيس واقف", "Standing statue of Ramses II.", "تمثال رمسيس الثاني واقفاً.", "New Kingdom", "Egypt", "Grand Egyptian Museum", placeholderVideo, "/images/statues/ramses-standing.jpg", baseTime, baseTime, true },
                    { new Guid("55555555-5555-5555-5555-555555550007"), "Pharaoh in Museum", "تمثال فرعون في المتحف", "Pharaoh statue displayed in a museum hall.", "تمثال فرعون في قاعة متحف.", "New Kingdom", "Egypt", "Grand Egyptian Museum", placeholderVideo, "/images/statues/pharaoh-museum.jpg", baseTime, baseTime, true },
                    { new Guid("55555555-5555-5555-5555-555555550008"), "Abu Simbel", "أبو سمبل", "Great Temple of Ramesses II at Abu Simbel.", "معبد رمسيس الثاني الكبير في أبو سمبل.", "New Kingdom", "Abu Simbel, Egypt", "Abu Simbel Temples", placeholderVideo, "/images/statues/abu-simbel.jpg", baseTime, baseTime, true },
                    { new Guid("55555555-5555-5555-5555-555555550009"), "Nefertiti Stylized", "نفرتيتي بأسلوب حديث", "Modern stylized representation of Nefertiti.", "تمثيل حديث لأميرة نفرتيتي.", "New Kingdom", "Egypt", "Neues Museum", placeholderVideo, "/images/statues/nefertiti-stylized.jpg", baseTime, baseTime, true },
                    { new Guid("55555555-5555-5555-5555-55555555000a"), "Nefertiti Collage", "كولاج نفرتيتي", "Collage of Nefertiti bust from discovery to display.", "كولاج لتمثال نفرتيتي من الاكتشاف إلى العرض.", "New Kingdom", "Egypt", "Neues Museum, Berlin", placeholderVideo, "/images/statues/nefertiti-collage.jpg", baseTime, baseTime, true },
                    { new Guid("55555555-5555-5555-5555-55555555000b"), "Nefertiti Detail", "نفرتيتي تفاصيل", "Detailed view of the Nefertiti bust.", "نظرة تفصيلية لتمثال نفرتيتي.", "New Kingdom", "Egypt", "Neues Museum, Berlin", placeholderVideo, "/images/statues/nefertiti-detail.jpg", baseTime, baseTime, true }
                });

            // متاحف / معارض إضافية
            migrationBuilder.InsertData(
                table: "Museums",
                columns: new[] { "Id", "Name", "NameAr", "Description", "Location", "ImageUrl", "CreatedAt", "IsActive" },
                values: new object[,]
                {
                    { new Guid("66666666-6666-6666-6666-666666660001"), "Grand Egyptian Museum", "المتحف المصري الكبير", "The Grand Egyptian Museum, Giza.", "الجيزة، مصر", "/images/museums/grand-egyptian-museum.jpg", baseTime, true },
                    { new Guid("66666666-6666-6666-6666-666666660002"), "Egyptian Temple", "معبد مصري", "Ancient Egyptian temple interior with columns.", "مصر", "/images/museums/egyptian-temple.jpg", baseTime, true },
                    { new Guid("66666666-6666-6666-6666-666666660003"), "Mummies Exhibit", "معرض المومياوات", "Exhibition of ancient Egyptian mummies.", "مصر", "/images/museums/mummies.jpg", baseTime, true },
                    { new Guid("66666666-6666-6666-6666-666666660004"), "Mummy Close-up", "مومياء لقطة قريبة", "Close-up of a preserved ancient Egyptian mummy.", "مصر", "/images/museums/mummy-closeup.jpg", baseTime, true },
                    { new Guid("66666666-6666-6666-6666-666666660005"), "Statue Collection", "مجموعة تماثيل", "Museum exhibit of classical statues and busts.", "متحف", "/images/museums/statue-collection.jpg", baseTime, true }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var statueIds = new[]
            {
                new Guid("55555555-5555-5555-5555-555555550001"),
                new Guid("55555555-5555-5555-5555-555555550002"),
                new Guid("55555555-5555-5555-5555-555555550003"),
                new Guid("55555555-5555-5555-5555-555555550004"),
                new Guid("55555555-5555-5555-5555-555555550005"),
                new Guid("55555555-5555-5555-5555-555555550006"),
                new Guid("55555555-5555-5555-5555-555555550007"),
                new Guid("55555555-5555-5555-5555-555555550008"),
                new Guid("55555555-5555-5555-5555-555555550009"),
                new Guid("55555555-5555-5555-5555-55555555000a"),
                new Guid("55555555-5555-5555-5555-55555555000b")
            };
            var museumIds = new[]
            {
                new Guid("66666666-6666-6666-6666-666666660001"),
                new Guid("66666666-6666-6666-6666-666666660002"),
                new Guid("66666666-6666-6666-6666-666666660003"),
                new Guid("66666666-6666-6666-6666-666666660004"),
                new Guid("66666666-6666-6666-6666-666666660005")
            };

            foreach (var id in statueIds)
                migrationBuilder.DeleteData(table: "Statues", keyColumn: "Id", keyValue: id);
            foreach (var id in museumIds)
                migrationBuilder.DeleteData(table: "Museums", keyColumn: "Id", keyValue: id);
        }
    }
}
