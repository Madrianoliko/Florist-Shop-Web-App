using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FloristShop.Web.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSequenceCounters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "sequence_counters",
                columns: table => new
                {
                    type = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    year = table.Column<int>(type: "integer", nullable: false),
                    last_value = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sequence_counters", x => new { x.type, x.year });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sequence_counters");
        }
    }
}
