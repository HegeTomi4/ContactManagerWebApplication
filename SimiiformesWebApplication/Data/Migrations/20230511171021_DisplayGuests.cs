using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimiiformesWebApplication.Data.Migrations
{
    public partial class DisplayGuests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventPersonConnection_Events_EventId",
                table: "EventPersonConnection");

            migrationBuilder.DropForeignKey(
                name: "FK_EventPersonConnection_Person_PersonId",
                table: "EventPersonConnection");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventPersonConnection",
                table: "EventPersonConnection");

            migrationBuilder.RenameTable(
                name: "EventPersonConnection",
                newName: "Connections");

            migrationBuilder.RenameIndex(
                name: "IX_EventPersonConnection_PersonId",
                table: "Connections",
                newName: "IX_Connections_PersonId");

            migrationBuilder.RenameIndex(
                name: "IX_EventPersonConnection_EventId",
                table: "Connections",
                newName: "IX_Connections_EventId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Connections",
                table: "Connections",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Connections_Events_EventId",
                table: "Connections",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Connections_Person_PersonId",
                table: "Connections",
                column: "PersonId",
                principalTable: "Person",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Connections_Events_EventId",
                table: "Connections");

            migrationBuilder.DropForeignKey(
                name: "FK_Connections_Person_PersonId",
                table: "Connections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Connections",
                table: "Connections");

            migrationBuilder.RenameTable(
                name: "Connections",
                newName: "EventPersonConnection");

            migrationBuilder.RenameIndex(
                name: "IX_Connections_PersonId",
                table: "EventPersonConnection",
                newName: "IX_EventPersonConnection_PersonId");

            migrationBuilder.RenameIndex(
                name: "IX_Connections_EventId",
                table: "EventPersonConnection",
                newName: "IX_EventPersonConnection_EventId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventPersonConnection",
                table: "EventPersonConnection",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EventPersonConnection_Events_EventId",
                table: "EventPersonConnection",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventPersonConnection_Person_PersonId",
                table: "EventPersonConnection",
                column: "PersonId",
                principalTable: "Person",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
