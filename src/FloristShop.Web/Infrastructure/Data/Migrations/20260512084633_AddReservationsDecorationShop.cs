using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FloristShop.Web.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddReservationsDecorationShop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "bouquet_templates",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    base_price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    category = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    image_paths = table.Column<List<string>>(type: "jsonb", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_bouquet_templates", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "decoration_inquiries",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    inquiry_number = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    event_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    event_date = table.Column<DateOnly>(type: "date", nullable: false),
                    location = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    budget_estimate = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    customer_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    customer_email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    customer_phone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    admin_note = table.Column<string>(type: "text", nullable: true),
                    final_price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_decoration_inquiries", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_number = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    customer_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    customer_email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    customer_phone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    delivery_method = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    delivery_address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    delivery_date = table.Column<DateOnly>(type: "date", nullable: false),
                    customer_note = table.Column<string>(type: "text", nullable: true),
                    admin_note = table.Column<string>(type: "text", nullable: true),
                    subtotal = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    delivery_fee = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    total_price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    payment_status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_orders", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rental_reservations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    reservation_number = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    end_date = table.Column<DateOnly>(type: "date", nullable: false),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    customer_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    customer_email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    customer_phone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    customer_note = table.Column<string>(type: "text", nullable: true),
                    admin_note = table.Column<string>(type: "text", nullable: true),
                    total_price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_rental_reservations", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "order_items",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    bouquet_template_id = table.Column<Guid>(type: "uuid", nullable: false),
                    bouquet_name_snapshot = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    unit_price_snapshot = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_order_items", x => x.id);
                    table.ForeignKey(
                        name: "fk_order_items_bouquet_templates_bouquet_template_id",
                        column: x => x.bouquet_template_id,
                        principalTable: "bouquet_templates",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_order_items_orders_order_id",
                        column: x => x.order_id,
                        principalTable: "orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "rental_reservation_items",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    rental_reservation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    rental_item_id = table.Column<Guid>(type: "uuid", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    unit_price_snapshot = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_rental_reservation_items", x => x.id);
                    table.ForeignKey(
                        name: "fk_rental_reservation_items_rental_items_rental_item_id",
                        column: x => x.rental_item_id,
                        principalTable: "rental_items",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_rental_reservation_items_rental_reservations_rental_reserva",
                        column: x => x.rental_reservation_id,
                        principalTable: "rental_reservations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_bouquet_templates_category_is_active",
                table: "bouquet_templates",
                columns: new[] { "category", "is_active" });

            migrationBuilder.CreateIndex(
                name: "ix_decoration_inquiries_event_date",
                table: "decoration_inquiries",
                column: "event_date");

            migrationBuilder.CreateIndex(
                name: "ix_decoration_inquiries_inquiry_number",
                table: "decoration_inquiries",
                column: "inquiry_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_decoration_inquiries_status",
                table: "decoration_inquiries",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "ix_order_items_bouquet_template_id",
                table: "order_items",
                column: "bouquet_template_id");

            migrationBuilder.CreateIndex(
                name: "ix_order_items_order_id",
                table: "order_items",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "ix_orders_delivery_date",
                table: "orders",
                column: "delivery_date");

            migrationBuilder.CreateIndex(
                name: "ix_orders_order_number",
                table: "orders",
                column: "order_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_orders_payment_status",
                table: "orders",
                column: "payment_status");

            migrationBuilder.CreateIndex(
                name: "ix_orders_status",
                table: "orders",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "ix_rental_reservation_items_rental_item_id",
                table: "rental_reservation_items",
                column: "rental_item_id");

            migrationBuilder.CreateIndex(
                name: "ix_rental_reservation_items_rental_reservation_id",
                table: "rental_reservation_items",
                column: "rental_reservation_id");

            migrationBuilder.CreateIndex(
                name: "ix_rental_reservations_reservation_number",
                table: "rental_reservations",
                column: "reservation_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_rental_reservations_start_date",
                table: "rental_reservations",
                column: "start_date");

            migrationBuilder.CreateIndex(
                name: "ix_rental_reservations_status",
                table: "rental_reservations",
                column: "status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "decoration_inquiries");

            migrationBuilder.DropTable(
                name: "order_items");

            migrationBuilder.DropTable(
                name: "rental_reservation_items");

            migrationBuilder.DropTable(
                name: "bouquet_templates");

            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "rental_reservations");
        }
    }
}
