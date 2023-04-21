using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PromoWeb.Context.MigrationsPostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class ChangeImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Images_ImageName",
                table: "Images");

            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "Images",
                newName: "UniqueName");

            migrationBuilder.CreateIndex(
                name: "IX_Images_UniqueName",
                table: "Images",
                column: "UniqueName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Images_UniqueName",
                table: "Images");

            migrationBuilder.RenameColumn(
                name: "UniqueName",
                table: "Images",
                newName: "ImagePath");

            migrationBuilder.CreateIndex(
                name: "IX_Images_ImageName",
                table: "Images",
                column: "ImageName",
                unique: true);
        }
    }
}
