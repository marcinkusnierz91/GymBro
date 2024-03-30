using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymBro.Migrations
{
    /// <inheritdoc />
    public partial class tableschanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RepetitionsAmount",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "SeriesAmount",
                table: "Exercises");

            migrationBuilder.AddColumn<int>(
                name: "Ilość_Powtórzeń",
                table: "ExercisesSeries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Numer_Serii",
                table: "ExercisesSeries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Obciążenie",
                table: "ExercisesSeries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExerciseDate",
                table: "Exercises",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ilość_Powtórzeń",
                table: "ExercisesSeries");

            migrationBuilder.DropColumn(
                name: "Numer_Serii",
                table: "ExercisesSeries");

            migrationBuilder.DropColumn(
                name: "Obciążenie",
                table: "ExercisesSeries");

            migrationBuilder.DropColumn(
                name: "ExerciseDate",
                table: "Exercises");

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
    }
}
