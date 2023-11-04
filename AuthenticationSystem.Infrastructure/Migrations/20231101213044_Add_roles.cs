using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthenticationSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_roles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "birth_date",
                table: "user",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "phone_number",
                table: "user",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "role_id",
                table: "user",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    role_name = table.Column<string>(type: "text", nullable: false),
                    role_type = table.Column<byte[]>(type: "bytea", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modified_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    version = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_user_email_hash",
                table: "user",
                column: "email_hash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_role_id",
                table: "user",
                column: "role_id");

            migrationBuilder.AddForeignKey(
                name: "FK_user_role_role_id",
                table: "user",
                column: "role_id",
                principalTable: "role",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_role_role_id",
                table: "user");

            migrationBuilder.DropTable(
                name: "role");

            migrationBuilder.DropIndex(
                name: "IX_user_email_hash",
                table: "user");

            migrationBuilder.DropIndex(
                name: "IX_user_role_id",
                table: "user");

            migrationBuilder.DropColumn(
                name: "birth_date",
                table: "user");

            migrationBuilder.DropColumn(
                name: "phone_number",
                table: "user");

            migrationBuilder.DropColumn(
                name: "role_id",
                table: "user");
        }
    }
}
