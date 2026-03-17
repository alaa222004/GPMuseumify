using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GPMuseumify.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddStatueStoriesAndMedia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StoryUrl",
                table: "Statues",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StoryUrlEn",
                table: "Statues",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            // Hatshepsut seed (existing Id: 7777...)
            migrationBuilder.Sql(@"
UPDATE Statues
SET VideoUrl = N'https://drive.google.com/file/d/1_ScuRUtSb6T8cbZpOkMg6llaSeDsy_Sx/preview',
    VideoUrlEn = N'https://drive.google.com/file/d/1xBqmpWoQJgnfdJDGHHpLO2qh919sX2lE/preview',
    StoryUrl = N'https://drive.google.com/file/d/1h5KQFnc9ezFRvMU_F_NSz7gdXG3h7ZiW/preview',
    StoryUrlEn = N'https://drive.google.com/file/d/1cSsTkdQ3AcDCAU6xf-x5Xz2mhegLRKXO/preview',
    Description = N'Hatshepsut was one of the most powerful female pharaohs of ancient Egypt.',
    DescriptionAr = N'حتشبسوت كانت من أقوى ملكات مصر القديمة.'
WHERE Id = '77777777-7777-7777-7777-777777770001';
");

            // Ramses II seed (existing Id: 3333...)
            migrationBuilder.Sql(@"
UPDATE Statues
SET VideoUrl = N'https://drive.google.com/file/d/1BPpY-YR--uo1yjIW_QYTBMxKK9fA5gIu/preview',
    VideoUrlEn = N'https://drive.google.com/file/d/1fplqSWqgvXXSW4fAoSXxCpZUw7crLaU6/preview',
    Description = N'Ramses II was one of Egypt''s greatest pharaohs known for his military leadership.',
    DescriptionAr = N'رمسيس الثاني من أعظم فراعنة مصر وكان قائداً عسكرياً بارعاً.'
WHERE Id = '33333333-3333-3333-3333-333333333333';
");

            // Tutankhamun seed (existing Id: 5555...0001)
            migrationBuilder.Sql(@"
UPDATE Statues
SET VideoUrl = N'https://drive.google.com/file/d/1GjkXgzy7xFR9WRUA2QtnETAC_Lgv6IZ3/preview',
    VideoUrlEn = N'https://drive.google.com/file/d/1wrp4U2rVAFcUxP8UVXNh4GgGBeOJf_id/preview',
    Description = N'Tutankhamun is one of the most famous pharaohs due to the discovery of his tomb.',
    DescriptionAr = N'توت عنخ آمون من أشهر الفراعنة بسبب اكتشاف مقبرته كاملة.'
WHERE Id = '55555555-5555-5555-5555-555555550001';
");

            // Senusret I — insert if not exists (new Id)
            migrationBuilder.Sql(@"
IF NOT EXISTS (SELECT 1 FROM Statues WHERE Name = N'Senusret I')
BEGIN
    INSERT INTO Statues
        (Id, Name, NameAr, Description, DescriptionAr, HistoricalPeriod, Location, Museum,
         VideoUrl, VideoUrlEn, StoryUrl, StoryUrlEn, ThumbnailUrl, CreatedAt, UpdatedAt, IsActive)
    VALUES
        ('88888888-8888-8888-8888-888888880001',
         N'Senusret I',
         N'سنوسرت الأول',
         N'Senusret I was a powerful king of the Middle Kingdom who expanded Egypt''s borders.',
         N'سنوسرت الأول كان ملكاً قوياً في الدولة الوسطى ووسع حدود مصر.',
         N'Middle Kingdom',
         N'Egypt',
         N'Egyptian Museum',
         N'https://drive.google.com/file/d/1xU8emktFEDkI5hvd4YTlBmQ53zKOiNKr/preview',
         N'https://drive.google.com/file/d/1BOv1fhUVl75gmF8ASViaBB2twCoO0Nlm/preview',
         NULL,
         NULL,
         NULL,
         SYSUTCDATETIME(),
         SYSUTCDATETIME(),
         1);
END
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StoryUrl",
                table: "Statues");

            migrationBuilder.DropColumn(
                name: "StoryUrlEn",
                table: "Statues");

            migrationBuilder.Sql(@"
DELETE FROM Statues
WHERE Id = '88888888-8888-8888-8888-888888880001';
");
        }
    }
}

