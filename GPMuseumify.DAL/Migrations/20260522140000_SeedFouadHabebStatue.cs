using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GPMuseumify.DAL.Migrations
{
    /// <inheritdoc />
    public partial class SeedFouadHabebStatue : Migration
    {
        private const string FouadVideo =
            "https://drive.google.com/file/d/148Y0NpOM0986DLpWI82QHkW8HVeE_KQs/preview";

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
IF NOT EXISTS (SELECT 1 FROM Statues WHERE Id = 'a1000007-0000-0000-0000-000000000007')
BEGIN
    INSERT INTO Statues (Id, Name, NameAr, Description, DescriptionAr, HistoricalPeriod, Location, Museum,
        VideoUrl, VideoUrlEn, ThumbnailUrl, CreatedAt, UpdatedAt, IsActive)
    VALUES ('a1000007-0000-0000-0000-000000000007', N'Fouad Habeb', N'فؤاد حبيب',
        N'Fouad Habeb.', N'فؤاد حبيب.', N'Modern Egypt', N'Egypt', N'Egyptian Museum',
        N'{FouadVideo}', N'{FouadVideo}', N'/images/statues/pharaoh-bust.jpg',
        SYSUTCDATETIME(), SYSUTCDATETIME(), 1);
END

IF NOT EXISTS (SELECT 1 FROM StatueImages WHERE Id = 'b1000008-0000-0000-0000-000000000008')
BEGIN
    INSERT INTO StatueImages (Id, StatueId, ImageUrl, ImageHash, IsPrimary, CreatedAt)
    VALUES ('b1000008-0000-0000-0000-000000000008', 'a1000007-0000-0000-0000-000000000007',
        N'/images/statues/pharaoh-bust.jpg', NULL, 1, SYSUTCDATETIME());
END
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
DELETE FROM StatueImages WHERE Id = 'b1000008-0000-0000-0000-000000000008';
DELETE FROM Statues WHERE Id = 'a1000007-0000-0000-0000-000000000007';
");
        }
    }
}
