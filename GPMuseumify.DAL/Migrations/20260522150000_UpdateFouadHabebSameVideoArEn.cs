using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GPMuseumify.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFouadHabebSameVideoArEn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
UPDATE Statues SET
    VideoUrl = N'https://drive.google.com/file/d/148Y0NpOM0986DLpWI82QHkW8HVeE_KQs/preview',
    VideoUrlEn = N'https://drive.google.com/file/d/148Y0NpOM0986DLpWI82QHkW8HVeE_KQs/preview',
    UpdatedAt = SYSUTCDATETIME()
WHERE Id = 'a1000007-0000-0000-0000-000000000007';
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
