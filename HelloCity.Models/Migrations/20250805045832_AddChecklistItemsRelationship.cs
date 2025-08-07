using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelloCity.Models.Migrations
{
    /// <inheritdoc />
    public partial class AddChecklistItemsRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChecklistItem",
                columns: table => new
                {
                    ChecklistItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserOwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    IsComplete = table.Column<bool>(type: "boolean", nullable: false),
                    Importance = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChecklistItem", x => x.ChecklistItemId);
                    table.ForeignKey(
                        name: "FK_ChecklistItem_Users_UserOwnerId",
                        column: x => x.UserOwnerId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistItem_UserOwnerId",
                table: "ChecklistItem",
                column: "UserOwnerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChecklistItem");
        }
    }
}
