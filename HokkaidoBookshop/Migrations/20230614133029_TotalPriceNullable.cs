﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HokkaidoBookshop.Migrations
{
    /// <inheritdoc />
    public partial class TotalPriceNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Orders");
        }
    }
}
