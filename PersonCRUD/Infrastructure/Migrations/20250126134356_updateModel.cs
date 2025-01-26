using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonCRUD.Migrations
{
    /// <inheritdoc />
    public partial class updateModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Individuals",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "Family",
                table: "Individuals",
                newName: "FirstName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Individuals",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Individuals",
                newName: "Family");
        }
    }
}
