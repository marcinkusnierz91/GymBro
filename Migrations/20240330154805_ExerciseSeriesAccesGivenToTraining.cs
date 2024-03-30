using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymBro.Migrations
{
    /// <inheritdoc />
    public partial class ExerciseSeriesAccesGivenToTraining : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TrainingModelId",
                table: "ExercisesSeries",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExercisesSeries_TrainingModelId",
                table: "ExercisesSeries",
                column: "TrainingModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExercisesSeries_Trainings_TrainingModelId",
                table: "ExercisesSeries",
                column: "TrainingModelId",
                principalTable: "Trainings",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExercisesSeries_Trainings_TrainingModelId",
                table: "ExercisesSeries");

            migrationBuilder.DropIndex(
                name: "IX_ExercisesSeries_TrainingModelId",
                table: "ExercisesSeries");

            migrationBuilder.DropColumn(
                name: "TrainingModelId",
                table: "ExercisesSeries");
        }
    }
}
