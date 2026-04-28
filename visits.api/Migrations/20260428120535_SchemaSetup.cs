using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace visits.api.Migrations
{
    /// <inheritdoc />
    public partial class SchemaSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "institutions");

            migrationBuilder.EnsureSchema(
                name: "core");

            migrationBuilder.EnsureSchema(
                name: "documents");

            migrationBuilder.EnsureSchema(
                name: "policies");

            migrationBuilder.EnsureSchema(
                name: "residences");

            migrationBuilder.EnsureSchema(
                name: "users");

            migrationBuilder.EnsureSchema(
                name: "visits");

            migrationBuilder.RenameTable(
                name: "VisitTypePolicies",
                newName: "VisitTypePolicies",
                newSchema: "policies");

            migrationBuilder.RenameTable(
                name: "Visits",
                newName: "Visits",
                newSchema: "visits");

            migrationBuilder.RenameTable(
                name: "Visitors",
                newName: "Visitors",
                newSchema: "visits");

            migrationBuilder.RenameTable(
                name: "VisitorCodes",
                newName: "VisitorCodes",
                newSchema: "visits");

            migrationBuilder.RenameTable(
                name: "Students",
                newName: "Students",
                newSchema: "users");

            migrationBuilder.RenameTable(
                name: "StudentRooms",
                newName: "StudentRooms",
                newSchema: "residences");

            migrationBuilder.RenameTable(
                name: "Staff",
                newName: "Staff",
                newSchema: "users");

            migrationBuilder.RenameTable(
                name: "Rooms",
                newName: "Rooms",
                newSchema: "residences");

            migrationBuilder.RenameTable(
                name: "Residences",
                newName: "Residences",
                newSchema: "residences");

            migrationBuilder.RenameTable(
                name: "ResidenceAccessPolicies",
                newName: "ResidenceAccessPolicies",
                newSchema: "policies");

            migrationBuilder.RenameTable(
                name: "RefreshTokens",
                newName: "RefreshTokens",
                newSchema: "core");

            migrationBuilder.RenameTable(
                name: "Institutions",
                newName: "Institutions",
                newSchema: "institutions");

            migrationBuilder.RenameTable(
                name: "Images",
                newName: "Images",
                newSchema: "documents");

            migrationBuilder.RenameTable(
                name: "ClassificationValues",
                newName: "ClassificationValues",
                newSchema: "core");

            migrationBuilder.RenameTable(
                name: "Campuses",
                newName: "Campuses",
                newSchema: "institutions");

            migrationBuilder.RenameTable(
                name: "Addresses",
                newName: "Addresses",
                newSchema: "institutions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "VisitTypePolicies",
                schema: "policies",
                newName: "VisitTypePolicies");

            migrationBuilder.RenameTable(
                name: "Visits",
                schema: "visits",
                newName: "Visits");

            migrationBuilder.RenameTable(
                name: "Visitors",
                schema: "visits",
                newName: "Visitors");

            migrationBuilder.RenameTable(
                name: "VisitorCodes",
                schema: "visits",
                newName: "VisitorCodes");

            migrationBuilder.RenameTable(
                name: "Students",
                schema: "users",
                newName: "Students");

            migrationBuilder.RenameTable(
                name: "StudentRooms",
                schema: "residences",
                newName: "StudentRooms");

            migrationBuilder.RenameTable(
                name: "Staff",
                schema: "users",
                newName: "Staff");

            migrationBuilder.RenameTable(
                name: "Rooms",
                schema: "residences",
                newName: "Rooms");

            migrationBuilder.RenameTable(
                name: "Residences",
                schema: "residences",
                newName: "Residences");

            migrationBuilder.RenameTable(
                name: "ResidenceAccessPolicies",
                schema: "policies",
                newName: "ResidenceAccessPolicies");

            migrationBuilder.RenameTable(
                name: "RefreshTokens",
                schema: "core",
                newName: "RefreshTokens");

            migrationBuilder.RenameTable(
                name: "Institutions",
                schema: "institutions",
                newName: "Institutions");

            migrationBuilder.RenameTable(
                name: "Images",
                schema: "documents",
                newName: "Images");

            migrationBuilder.RenameTable(
                name: "ClassificationValues",
                schema: "core",
                newName: "ClassificationValues");

            migrationBuilder.RenameTable(
                name: "Campuses",
                schema: "institutions",
                newName: "Campuses");

            migrationBuilder.RenameTable(
                name: "Addresses",
                schema: "institutions",
                newName: "Addresses");
        }
    }
}
