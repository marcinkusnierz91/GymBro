using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymBro.Migrations
{
    /// <inheritdoc />
    public partial class przerobionotabeleexercises : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RepetitionsAmount",
                table: "Exercises",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SeriesAmount",
                table: "Exercises",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RepetitionsAmount",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "SeriesAmount",
                table: "Exercises");
        }
    }
}
