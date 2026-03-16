using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoteManagemenSystemServer.Migrations
{
    /// <inheritdoc />
    public partial class migNoteTableUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Notes");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Notes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "DeletedAt" },
                values: new object[] { new DateTime(2026, 3, 4, 10, 50, 15, 24, DateTimeKind.Local).AddTicks(7464), null });

            migrationBuilder.UpdateData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "DeletedAt" },
                values: new object[] { new DateTime(2026, 3, 6, 10, 50, 15, 24, DateTimeKind.Local).AddTicks(7471), null });

            migrationBuilder.UpdateData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "DeletedAt" },
                values: new object[] { new DateTime(2026, 3, 9, 10, 50, 15, 24, DateTimeKind.Local).AddTicks(7473), null });

            migrationBuilder.UpdateData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "DeletedAt" },
                values: new object[] { new DateTime(2026, 3, 11, 10, 50, 15, 24, DateTimeKind.Local).AddTicks(7475), null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2fe49a93-787b-41da-be82-e059b50391e1", "AQAAAAIAAYagAAAAEF8iYxUC1YXiyISNnPBP/OwAgb9KXL7rOSkse0dJ0A32oxNKy87z7IXEkDj0NhyF7w==", "0be26f04-8570-4ecf-ab18-6853525a53eb" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Notes");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Notes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "IsDeleted" },
                values: new object[] { new DateTime(2026, 3, 3, 20, 56, 39, 408, DateTimeKind.Local).AddTicks(191), false });

            migrationBuilder.UpdateData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "IsDeleted" },
                values: new object[] { new DateTime(2026, 3, 5, 20, 56, 39, 408, DateTimeKind.Local).AddTicks(207), false });

            migrationBuilder.UpdateData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "IsDeleted" },
                values: new object[] { new DateTime(2026, 3, 8, 20, 56, 39, 408, DateTimeKind.Local).AddTicks(210), false });

            migrationBuilder.UpdateData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "IsDeleted" },
                values: new object[] { new DateTime(2026, 3, 10, 20, 56, 39, 408, DateTimeKind.Local).AddTicks(213), false });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "57113856-2794-4d58-b64e-6339c201d4a6", "AQAAAAIAAYagAAAAEInvqcGQxQ0j/nk1bg/INskpsWVmr8BvBpW7BHPkQzxCyozmeVSL8bDrKFqxzfWLxg==", "c7663f57-3456-4775-89fd-d7df5676c6d2" });
        }
    }
}
