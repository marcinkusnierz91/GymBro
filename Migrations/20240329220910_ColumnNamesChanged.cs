using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymBro.Migrations
{
    /// <inheritdoc />
    public partial class ColumnNamesChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Obciążenie",
                table: "ExercisesSeries",
                newName: "Weight");

            migrationBuilder.RenameColumn(
                name: "Numer_Serii",
                table: "ExercisesSeries",
                newName: "SeriesNumber");

            migrationBuilder.RenameColumn(
                name: "Ilość_Powtórzeń",
                table: "ExercisesSeries",
                newName: "Repetitions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Weight",
                table: "ExercisesSeries",
                newName: "Obciążenie");

            migrationBuilder.RenameColumn(
                name: "SeriesNumber",
                table: "ExercisesSeries",
                newName: "Numer_Serii");

            migrationBuilder.RenameColumn(
                name: "Repetitions",
                table: "ExercisesSeries",
                newName: "Ilość_Powtórzeń");
        }
    }
}
