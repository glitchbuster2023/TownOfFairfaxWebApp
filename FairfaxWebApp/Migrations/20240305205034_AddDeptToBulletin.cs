﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FairfaxWebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddDeptToBulletin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "Bulletins",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Department",
                table: "Bulletins");
        }
    }
}
