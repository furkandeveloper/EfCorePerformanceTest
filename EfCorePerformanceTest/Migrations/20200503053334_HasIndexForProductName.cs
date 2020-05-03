using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EfCorePerformanceTest.Migrations
{
    public partial class HasIndexForProductName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Categories_CreateDate",
                table: "Categories",
                column: "CreateDate");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Name",
                table: "Products",
                column: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
