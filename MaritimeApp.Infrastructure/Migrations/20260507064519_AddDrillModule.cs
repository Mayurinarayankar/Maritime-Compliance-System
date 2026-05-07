using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaritimeApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDrillModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Drills",
                newName: "DrillType");

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "Drills",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ShipId",
                table: "Drills",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Participations_CrewId",
                table: "Participations",
                column: "CrewId");

            migrationBuilder.CreateIndex(
                name: "IX_Participations_DrillId",
                table: "Participations",
                column: "DrillId");

            migrationBuilder.CreateIndex(
                name: "IX_Drills_ShipId",
                table: "Drills",
                column: "ShipId");

            migrationBuilder.AddForeignKey(
                name: "FK_Drills_Ships_ShipId",
                table: "Drills",
                column: "ShipId",
                principalTable: "Ships",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Participations_Crews_CrewId",
                table: "Participations",
                column: "CrewId",
                principalTable: "Crews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Participations_Drills_DrillId",
                table: "Participations",
                column: "DrillId",
                principalTable: "Drills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drills_Ships_ShipId",
                table: "Drills");

            migrationBuilder.DropForeignKey(
                name: "FK_Participations_Crews_CrewId",
                table: "Participations");

            migrationBuilder.DropForeignKey(
                name: "FK_Participations_Drills_DrillId",
                table: "Participations");

            migrationBuilder.DropIndex(
                name: "IX_Participations_CrewId",
                table: "Participations");

            migrationBuilder.DropIndex(
                name: "IX_Participations_DrillId",
                table: "Participations");

            migrationBuilder.DropIndex(
                name: "IX_Drills_ShipId",
                table: "Drills");

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "Drills");

            migrationBuilder.DropColumn(
                name: "ShipId",
                table: "Drills");

            migrationBuilder.RenameColumn(
                name: "DrillType",
                table: "Drills",
                newName: "Type");
        }
    }
}
