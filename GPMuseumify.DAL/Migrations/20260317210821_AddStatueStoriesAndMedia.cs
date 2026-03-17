//using Microsoft.EntityFrameworkCore.Migrations;

//#nullable disable

//namespace GPMuseumify.DAL.Migrations
//{
//    /// <inheritdoc />
//    public partial class AddStatueStoriesAndMedia : Migration
//    {
//        /// <inheritdoc />
//        protected override void Up(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.AddColumn<string>(
//                name: "StoryUrl",
//                table: "Statues",
//                type: "nvarchar(500)",
//                maxLength: 500,
//                nullable: true);

//            migrationBuilder.AddColumn<string>(
//                name: "StoryUrlEn",
//                table: "Statues",
//                type: "nvarchar(500)",
//                maxLength: 500,
//                nullable: true);
//        }

//        /// <inheritdoc />
//        protected override void Down(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.DropColumn(
//                name: "StoryUrl",
//                table: "Statues");

//            migrationBuilder.DropColumn(
//                name: "StoryUrlEn",
//                table: "Statues");
//        }
//    }
//}
