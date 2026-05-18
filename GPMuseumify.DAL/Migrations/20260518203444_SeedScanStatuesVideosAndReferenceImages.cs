using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GPMuseumify.DAL.Migrations
{
    /// <inheritdoc />
    public partial class SeedScanStatuesVideosAndReferenceImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
UPDATE Statues
SET VideoUrl = N'https://drive.google.com/file/d/1PlB1DLOfOOH1rI84WNNbJqn2NsI5NaSQ/preview',
    VideoUrlEn = N'https://drive.google.com/file/d/1JF-zpiUn9SkZI8NAF1nM1_cQnQmCElpf/preview',
    UpdatedAt = SYSUTCDATETIME()
WHERE Id = '11111111-1111-1111-1111-111111111111';
");

            migrationBuilder.Sql(@"
IF NOT EXISTS (SELECT 1 FROM Statues WHERE Id = 'a1000001-0000-0000-0000-000000000001')
BEGIN
    INSERT INTO Statues (Id, Name, NameAr, Description, DescriptionAr, HistoricalPeriod, Location, Museum,
        VideoUrl, VideoUrlEn, ThumbnailUrl, CreatedAt, UpdatedAt, IsActive)
    VALUES ('a1000001-0000-0000-0000-000000000001', N'Ahmose I', N'أحمس الأول',
        N'Founder of the 18th dynasty who expelled the Hyksos and reunified Egypt.',
        N'مؤسس الأسرة الثامنة عشرة الذي طرد الهكسوس وأعاد توحيد مصر.',
        N'New Kingdom', N'Egypt', N'Egyptian Museum',
        N'https://drive.google.com/file/d/147wkc2_jJS2huezdmqVsE8RaEzaA8xpm/preview',
        N'https://drive.google.com/file/d/1B2PW2mRWFgV-fRKePDFQ8MQVcRGzWmfM/preview',
        N'/images/statues/pharaoh-bust.jpg', SYSUTCDATETIME(), SYSUTCDATETIME(), 1);
END
");

            migrationBuilder.Sql(@"
IF NOT EXISTS (SELECT 1 FROM Statues WHERE Id = 'a1000002-0000-0000-0000-000000000002')
BEGIN
    INSERT INTO Statues (Id, Name, NameAr, Description, DescriptionAr, HistoricalPeriod, Location, Museum,
        VideoUrl, VideoUrlEn, ThumbnailUrl, CreatedAt, UpdatedAt, IsActive)
    VALUES ('a1000002-0000-0000-0000-000000000002', N'Seated Writer', N'الكاتب الجالس',
        N'Famous Middle Kingdom statue of a seated scribe at work.',
        N'تمثال شهير من الدولة الوسطى لكاتب جالس يؤدي عمله.',
        N'Middle Kingdom', N'Egypt', N'Egyptian Museum',
        N'https://drive.google.com/file/d/1UoCFRg2K5NbFzJ3l3QzvNrbv5Xlv-tnA/preview',
        N'https://drive.google.com/file/d/1IIFUb8vNkbPAkrZJA8xw92RpcLs7hj4x/preview',
        N'/images/statues/pharaoh-bust-2.jpg', SYSUTCDATETIME(), SYSUTCDATETIME(), 1);
END
");

            migrationBuilder.Sql(@"
IF NOT EXISTS (SELECT 1 FROM Statues WHERE Id = 'a1000003-0000-0000-0000-000000000003')
BEGIN
    INSERT INTO Statues (Id, Name, NameAr, Description, DescriptionAr, HistoricalPeriod, Location, Museum,
        VideoUrl, VideoUrlEn, ThumbnailUrl, CreatedAt, UpdatedAt, IsActive)
    VALUES ('a1000003-0000-0000-0000-000000000003', N'Thutmose III', N'تحتمس الثالث',
        N'One of Egypt''s greatest warrior pharaohs who expanded the empire.',
        N'من أعظم الفراعنة المحاربين الذين وسّعوا الإمبراطورية المصرية.',
        N'New Kingdom', N'Egypt', N'Egyptian Museum',
        N'https://drive.google.com/file/d/1ivQhnHzMHccNSn0OimM8QcMV0_U4nj1W/preview',
        N'https://drive.google.com/file/d/1BQ8m8MmRqqE-0HacaGN-Lu94zS2c0zsQ/preview',
        N'/images/statues/pharaoh-head.jpg', SYSUTCDATETIME(), SYSUTCDATETIME(), 1);
END
");

            migrationBuilder.Sql(@"
IF NOT EXISTS (SELECT 1 FROM Statues WHERE Id = 'a1000004-0000-0000-0000-000000000004')
BEGIN
    INSERT INTO Statues (Id, Name, NameAr, Description, DescriptionAr, HistoricalPeriod, Location, Museum,
        VideoUrl, VideoUrlEn, ThumbnailUrl, CreatedAt, UpdatedAt, IsActive)
    VALUES ('a1000004-0000-0000-0000-000000000004', N'Khafre', N'خفرع',
        N'Pharaoh of the 4th dynasty, builder of the second pyramid at Giza.',
        N'فرعون الأسرة الرابعة، باني الهرم الثاني في الجيزة.',
        N'Old Kingdom', N'Giza', N'Giza Plateau',
        N'https://drive.google.com/file/d/1qH9KD4lEHjKR1fjQpg0uLCk_bImf4IK9/preview',
        N'https://drive.google.com/file/d/1J8NnRRUQhErY92juvhWpFtpeABgfCoqh/preview',
        N'/images/statues/pharaoh-standing.jpg', SYSUTCDATETIME(), SYSUTCDATETIME(), 1);
END
");

            migrationBuilder.Sql(@"
IF NOT EXISTS (SELECT 1 FROM Statues WHERE Id = 'a1000005-0000-0000-0000-000000000005')
BEGIN
    INSERT INTO Statues (Id, Name, NameAr, Description, DescriptionAr, HistoricalPeriod, Location, Museum,
        VideoUrl, VideoUrlEn, ThumbnailUrl, CreatedAt, UpdatedAt, IsActive)
    VALUES ('a1000005-0000-0000-0000-000000000005', N'Djoser', N'زوسر',
        N'Pharaoh who commissioned the Step Pyramid at Saqqara.',
        N'الفرعون الذي أمر ببناء الهرم المدرج في سقارة.',
        N'Old Kingdom', N'Saqqara', N'Saqqara',
        N'https://drive.google.com/file/d/1-kEc2bRJ4Pgvol4Pm3wcIOd_OCzrwZgb/preview',
        N'https://drive.google.com/file/d/1pzVgguqeY3HR7lfNJsfwvosoSmPovUvO/preview',
        N'/images/statues/pharaoh-museum.jpg', SYSUTCDATETIME(), SYSUTCDATETIME(), 1);
END
");

            migrationBuilder.Sql(@"
IF NOT EXISTS (SELECT 1 FROM Statues WHERE Id = 'a1000006-0000-0000-0000-000000000006')
BEGIN
    INSERT INTO Statues (Id, Name, NameAr, Description, DescriptionAr, HistoricalPeriod, Location, Museum,
        VideoUrl, VideoUrlEn, ThumbnailUrl, CreatedAt, UpdatedAt, IsActive)
    VALUES ('a1000006-0000-0000-0000-000000000006', N'Senusret III', N'سنوسرت الثالث',
        N'Powerful Middle Kingdom pharaoh known for his realistic portrait statues.',
        N'فرعون قوي من الدولة الوسطى اشتهر بتماثيله الواقعية.',
        N'Middle Kingdom', N'Egypt', N'Egyptian Museum',
        N'https://drive.google.com/file/d/1famyQkwKW67Nq86lTHpgmSY_8GVSfizP/preview',
        N'https://drive.google.com/file/d/1JhrxDMc4Gpl8a9db-iVGZ9124TnPxKxb/preview',
        N'/images/statues/ramses-ii.jpg', SYSUTCDATETIME(), SYSUTCDATETIME(), 1);
END
");

            SeedReferenceImage(migrationBuilder, "b1000001-0000-0000-0000-000000000001", "11111111-1111-1111-1111-111111111111", "/images/statues/akhenaten.jpg");
            SeedReferenceImage(migrationBuilder, "b1000002-0000-0000-0000-000000000002", "a1000001-0000-0000-0000-000000000001", "/images/statues/pharaoh-bust.jpg");
            SeedReferenceImage(migrationBuilder, "b1000003-0000-0000-0000-000000000003", "a1000002-0000-0000-0000-000000000002", "/images/statues/pharaoh-bust-2.jpg");
            SeedReferenceImage(migrationBuilder, "b1000004-0000-0000-0000-000000000004", "a1000003-0000-0000-0000-000000000003", "/images/statues/pharaoh-head.jpg");
            SeedReferenceImage(migrationBuilder, "b1000005-0000-0000-0000-000000000005", "a1000004-0000-0000-0000-000000000004", "/images/statues/pharaoh-standing.jpg");
            SeedReferenceImage(migrationBuilder, "b1000006-0000-0000-0000-000000000006", "a1000005-0000-0000-0000-000000000005", "/images/statues/pharaoh-museum.jpg");
            SeedReferenceImage(migrationBuilder, "b1000007-0000-0000-0000-000000000007", "a1000006-0000-0000-0000-000000000006", "/images/statues/ramses-ii.jpg");
        }

        private static void SeedReferenceImage(MigrationBuilder migrationBuilder, string imageId, string statueId, string imageUrl)
        {
            migrationBuilder.Sql($@"
IF NOT EXISTS (SELECT 1 FROM StatueImages WHERE Id = '{imageId}')
BEGIN
    INSERT INTO StatueImages (Id, StatueId, ImageUrl, ImageHash, IsPrimary, CreatedAt)
    VALUES ('{imageId}', '{statueId}', N'{imageUrl}', NULL, 1, SYSUTCDATETIME());
END
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
DELETE FROM StatueImages WHERE Id IN (
    'b1000001-0000-0000-0000-000000000001',
    'b1000002-0000-0000-0000-000000000002',
    'b1000003-0000-0000-0000-000000000003',
    'b1000004-0000-0000-0000-000000000004',
    'b1000005-0000-0000-0000-000000000005',
    'b1000006-0000-0000-0000-000000000006',
    'b1000007-0000-0000-0000-000000000007'
);
");

        }
    }
}
