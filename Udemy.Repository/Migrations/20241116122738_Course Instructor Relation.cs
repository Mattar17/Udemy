using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Udemy.Repository.Migrations
{
    /// <inheritdoc />
    public partial class CourseInstructorRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Student_Courses_Coureses_CourseId",
                table: "Student_Courses");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "Student_Courses");

            migrationBuilder.AddColumn<string>(
                name: "InstructorId",
                table: "Coureses",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Coureses_InstructorId",
                table: "Coureses",
                column: "InstructorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Coureses_AspNetUsers_InstructorId",
                table: "Coureses",
                column: "InstructorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Courses_Coureses_CourseId",
                table: "Student_Courses",
                column: "CourseId",
                principalTable: "Coureses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coureses_AspNetUsers_InstructorId",
                table: "Coureses");

            migrationBuilder.DropForeignKey(
                name: "FK_Student_Courses_Coureses_CourseId",
                table: "Student_Courses");

            migrationBuilder.DropIndex(
                name: "IX_Coureses_InstructorId",
                table: "Coureses");

            migrationBuilder.DropColumn(
                name: "InstructorId",
                table: "Coureses");

            migrationBuilder.AddColumn<string>(
                name: "StudentId",
                table: "Student_Courses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Courses_Coureses_CourseId",
                table: "Student_Courses",
                column: "CourseId",
                principalTable: "Coureses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
