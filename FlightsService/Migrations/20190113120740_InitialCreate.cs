using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace FlightsService.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Flights",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ArrivalLocation = table.Column<string>(nullable: true),
                    AvailableSeats = table.Column<int>(nullable: false),
                    BasePrice = table.Column<int>(nullable: false),
                    Date = table.Column<string>(nullable: true),
                    DepartureLocation = table.Column<string>(nullable: true),
                    IsSpecialOffer = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flights", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Flights");
        }
    }
}
