using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TvMazeMirror.Migrations
{
    /// <inheritdoc />
    public partial class ShowGenreFixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShowGenre_Shows_ShowId",
                table: "ShowGenre");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShowGenre",
                table: "ShowGenre");

            migrationBuilder.RenameTable(
                name: "ShowGenre",
                newName: "ShowGenres");

            migrationBuilder.RenameIndex(
                name: "IX_ShowGenre_ShowId",
                table: "ShowGenres",
                newName: "IX_ShowGenres_ShowId");

            migrationBuilder.AlterColumn<int>(
                name: "ShowId",
                table: "ShowGenres",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShowGenres",
                table: "ShowGenres",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShowGenres_Shows_ShowId",
                table: "ShowGenres",
                column: "ShowId",
                principalTable: "Shows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShowGenres_Shows_ShowId",
                table: "ShowGenres");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShowGenres",
                table: "ShowGenres");

            migrationBuilder.RenameTable(
                name: "ShowGenres",
                newName: "ShowGenre");

            migrationBuilder.RenameIndex(
                name: "IX_ShowGenres_ShowId",
                table: "ShowGenre",
                newName: "IX_ShowGenre_ShowId");

            migrationBuilder.AlterColumn<int>(
                name: "ShowId",
                table: "ShowGenre",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShowGenre",
                table: "ShowGenre",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShowGenre_Shows_ShowId",
                table: "ShowGenre",
                column: "ShowId",
                principalTable: "Shows",
                principalColumn: "Id");
        }
    }
}
