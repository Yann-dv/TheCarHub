using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace theCarHub.Migrations
{
    /// <inheritdoc />
    public partial class CarListRenamed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Watched",
                table: "UserCars",
                newName: "Listed");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Listed",
                table: "UserCars",
                newName: "Watched");
        }
    }
}
