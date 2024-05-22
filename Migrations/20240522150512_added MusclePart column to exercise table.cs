using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymBro.Migrations
{
    /// <inheritdoc />
    public partial class addedMusclePartcolumntoexercisetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MusclePart",
                table: "Exercises",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MusclePart",
                table: "Exercises");
        }
    }
}
