using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations;

/// <inheritdoc />
public partial class AddClientIdAndLocationsToTrip : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<Guid>(
            name: "client_id",
            schema: "public",
            table: "trips",
            type: "uuid",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "destination",
            schema: "public",
            table: "trips",
            type: "character varying(200)",
            maxLength: 200,
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            name: "origin",
            schema: "public",
            table: "trips",
            type: "character varying(200)",
            maxLength: 200,
            nullable: false,
            defaultValue: "");

        migrationBuilder.CreateIndex(
            name: "ix_trips_client_id",
            schema: "public",
            table: "trips",
            column: "client_id");

        migrationBuilder.AddForeignKey(
            name: "fk_trips_clients_client_id",
            schema: "public",
            table: "trips",
            column: "client_id",
            principalSchema: "public",
            principalTable: "Clients",
            principalColumn: "id",
            onDelete: ReferentialAction.Restrict);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "fk_trips_clients_client_id",
            schema: "public",
            table: "trips");

        migrationBuilder.DropIndex(
            name: "ix_trips_client_id",
            schema: "public",
            table: "trips");

        migrationBuilder.DropColumn(
            name: "client_id",
            schema: "public",
            table: "trips");

        migrationBuilder.DropColumn(
            name: "destination",
            schema: "public",
            table: "trips");

        migrationBuilder.DropColumn(
            name: "origin",
            schema: "public",
            table: "trips");
    }
}
