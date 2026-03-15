using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GPMuseumify.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddStatueVideoUrlEn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VideoUrlEn",
                table: "Statues",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            // حتشبسوت — الفيديو بالإنجليزي + جملة بالإنجليزي
            migrationBuilder.Sql(@"
                UPDATE Statues 
                SET VideoUrlEn = N'https://drive.google.com/file/d/1mzat796UwKwP7uos-yTF_eTwZ4f-79Vo/preview',
                    Description = N'Hatshepsut was one of the greatest pharaohs of ancient Egypt; she ruled as king and left an enduring architectural legacy at Deir el-Bahari.'
                WHERE Id = '77777777-7777-7777-7777-777777770001';
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VideoUrlEn",
                table: "Statues");
        }
    }
}
