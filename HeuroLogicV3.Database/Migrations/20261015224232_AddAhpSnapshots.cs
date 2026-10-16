using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HeuroLogicV3.Database.Migrations
{
    public partial class AddAhpSnapshots : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Options",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Decisions",
                type: "character varying(160)",
                maxLength: 160,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "AlgorithmVersion",
                table: "Decisions",
                type: "character varying(60)",
                maxLength: 60,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CriteriaConsistencyRatio",
                table: "Decisions",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InputSnapshotJson",
                table: "Decisions",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Weight",
                table: "Criterias",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Criterias",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AlgorithmVersion",
                table: "Decisions");

            migrationBuilder.DropColumn(
                name: "CriteriaConsistencyRatio",
                table: "Decisions");

            migrationBuilder.DropColumn(
                name: "InputSnapshotJson",
                table: "Decisions");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Options",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Decisions",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(160)",
                oldMaxLength: 160);

            migrationBuilder.AlterColumn<int>(
                name: "Weight",
                table: "Criterias",
                type: "integer",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Criterias",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);
        }
    }
}
