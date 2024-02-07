using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NodesOfTrees.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExceptionLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EventId = table.Column<string>(type: "text", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    QueryParameters = table.Column<string>(type: "text", nullable: false),
                    BodyParameters = table.Column<string>(type: "text", nullable: false),
                    StackTrace = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExceptionLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TreeNodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreeNodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TreeNodes_TreeNodes_ParentId",
                        column: x => x.ParentId,
                        principalTable: "TreeNodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "TreeNodes",
                columns: new[] { "Id", "Name", "ParentId" },
                values: new object[,]
                {
                    { new Guid("c496c40a-edff-4942-8672-abf4c791720f"), "RootTree", null },
                    { new Guid("8d7129ee-8dad-484a-ab37-89dba6aa3fdf"), "2 - childRoot", new Guid("c496c40a-edff-4942-8672-abf4c791720f") },
                    { new Guid("da63bcc6-ae55-492d-8b0f-6beae1e9f5dc"), "1 - childRoot", new Guid("c496c40a-edff-4942-8672-abf4c791720f") },
                    { new Guid("a446cbef-38c1-4e39-bc61-f30a50a10647"), "1 - child - 2 - childRoot", new Guid("8d7129ee-8dad-484a-ab37-89dba6aa3fdf") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TreeNodes_ParentId",
                table: "TreeNodes",
                column: "ParentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExceptionLogs");

            migrationBuilder.DropTable(
                name: "TreeNodes");
        }
    }
}
