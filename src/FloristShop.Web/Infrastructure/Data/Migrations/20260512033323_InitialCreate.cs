using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FloristShop.Web.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "rental_items",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    category = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    price_per_rental = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    total_quantity = table.Column<int>(type: "integer", nullable: false),
                    image_paths = table.Column<List<string>>(type: "jsonb", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_rental_items", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_rental_items_category_is_active",
                table: "rental_items",
                columns: new[] { "category", "is_active" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "rental_items");
        }
    }
}
