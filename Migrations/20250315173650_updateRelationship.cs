using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TvMazeScaper.Migrations
{
    /// <inheritdoc />
    public partial class updateRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cast_Person_PersonId",
                table: "Cast");

            migrationBuilder.AddForeignKey(
                name: "FK_Cast_Person_PersonId",
                table: "Cast",
                column: "PersonId",
                principalTable: "Person",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cast_Person_PersonId",
                table: "Cast");

            migrationBuilder.AddForeignKey(
                name: "FK_Cast_Person_PersonId",
                table: "Cast",
                column: "PersonId",
                principalTable: "Person",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
