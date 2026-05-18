using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GPMuseumify.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateScanStatueDriveVideos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // VideoUrl = Arabic, VideoUrlEn = English — /preview for in-app embed (same as Hatshepsut/Ramses)
            migrationBuilder.Sql(@"
UPDATE Statues SET
    VideoUrl = N'https://drive.google.com/file/d/147wkc2_jJS2huezdmqVsE8RaEzaA8xpm/preview',
    VideoUrlEn = N'https://drive.google.com/file/d/1B2PW2mRWFgV-fRKePDFQ8MQVcRGzWmfM/preview',
    UpdatedAt = SYSUTCDATETIME()
WHERE Id = 'a1000001-0000-0000-0000-000000000001';

UPDATE Statues SET
    VideoUrl = N'https://drive.google.com/file/d/1UoCFRg2K5NbFzJ3l3QzvNrbv5Xlv-tnA/preview',
    VideoUrlEn = N'https://drive.google.com/file/d/1IIFUb8vNkbPAkrZJA8xw92RpcLs7hj4x/preview',
    UpdatedAt = SYSUTCDATETIME()
WHERE Id = 'a1000002-0000-0000-0000-000000000002';

UPDATE Statues SET
    VideoUrl = N'https://drive.google.com/file/d/1ivQhnHzMHccNSn0OimM8QcMV0_U4nj1W/preview',
    VideoUrlEn = N'https://drive.google.com/file/d/1BQ8m8MmRqqE-0HacaGN-Lu94zS2c0zsQ/preview',
    UpdatedAt = SYSUTCDATETIME()
WHERE Id = 'a1000003-0000-0000-0000-000000000003';

UPDATE Statues SET
    VideoUrl = N'https://drive.google.com/file/d/1PlB1DLOfOOH1rI84WNNbJqn2NsI5NaSQ/preview',
    VideoUrlEn = N'https://drive.google.com/file/d/1JF-zpiUn9SkZI8NAF1nM1_cQnQmCElpf/preview',
    UpdatedAt = SYSUTCDATETIME()
WHERE Id = '11111111-1111-1111-1111-111111111111';

UPDATE Statues SET
    VideoUrl = N'https://drive.google.com/file/d/1qH9KD4lEHjKR1fjQpg0uLCk_bImf4IK9/preview',
    VideoUrlEn = N'https://drive.google.com/file/d/1J8NnRRUQhErY92juvhWpFtpeABgfCoqh/preview',
    UpdatedAt = SYSUTCDATETIME()
WHERE Id = 'a1000004-0000-0000-0000-000000000004';

UPDATE Statues SET
    VideoUrl = N'https://drive.google.com/file/d/1-kEc2bRJ4Pgvol4Pm3wcIOd_OCzrwZgb/preview',
    VideoUrlEn = N'https://drive.google.com/file/d/1pzVgguqeY3HR7lfNJsfwvosoSmPovUvO/preview',
    UpdatedAt = SYSUTCDATETIME()
WHERE Id = 'a1000005-0000-0000-0000-000000000005';

UPDATE Statues SET
    VideoUrl = N'https://drive.google.com/file/d/1famyQkwKW67Nq86lTHpgmSY_8GVSfizP/preview',
    VideoUrlEn = N'https://drive.google.com/file/d/1JhrxDMc4Gpl8a9db-iVGZ9124TnPxKxb/preview',
    UpdatedAt = SYSUTCDATETIME()
WHERE Id = 'a1000006-0000-0000-0000-000000000006';
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
