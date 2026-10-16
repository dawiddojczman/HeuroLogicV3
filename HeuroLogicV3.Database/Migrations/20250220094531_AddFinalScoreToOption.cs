using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HeuroLogicV3.Database.Migrations
{
    public partial class AddFinalScoreToOption : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "FinalScore",
                table: "Options",
                type: "double precision",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinalScore",
                table: "Options");
        }
    }
}
