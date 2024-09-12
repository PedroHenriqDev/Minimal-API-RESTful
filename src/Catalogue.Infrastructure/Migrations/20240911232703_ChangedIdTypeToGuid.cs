using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Catalogue.Infrastructure.Migrations;

/// <inheritdoc />
public partial class ChangedIdTypeToGuid : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Add CreatedAt column to Users table
        migrationBuilder.AddColumn<DateTime>(
            name: "CreatedAt",
            table: "Users",
            type: "timestamp with time zone",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        // Add new UUID columns
        migrationBuilder.AddColumn<Guid>(
            name: "NewCategoryId",
            table: "Products",
            type: "uuid",
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "NewId",
            table: "Products",
            type: "uuid",
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "NewId",
            table: "Categories",
            type: "uuid",
            nullable: true);

        // Populate the new columns with converted data
        migrationBuilder.Sql(
            @"UPDATE ""Products""
              SET ""NewCategoryId"" = uuid_generate_v4()
              WHERE ""CategoryId"" IS NOT NULL;");

        migrationBuilder.Sql(
            @"UPDATE ""Products""
              SET ""NewId"" = uuid_generate_v4();");

        migrationBuilder.Sql(
            @"UPDATE ""Categories""
              SET ""NewId"" = uuid_generate_v4();");

        // Drop the old columns
        migrationBuilder.DropColumn(
            name: "CategoryId",
            table: "Products");

        migrationBuilder.DropColumn(
            name: "Id",
            table: "Products");

        migrationBuilder.DropColumn(
            name: "Id",
            table: "Categories");

        // Rename new columns to original names
        migrationBuilder.RenameColumn(
            name: "NewCategoryId",
            table: "Products",
            newName: "CategoryId");

        migrationBuilder.RenameColumn(
            name: "NewId",
            table: "Products",
            newName: "Id");

        migrationBuilder.RenameColumn(
            name: "NewId",
            table: "Categories",
            newName: "Id");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // Revert changes made in the Up method
        migrationBuilder.AddColumn<int>(
            name: "CategoryId",
            table: "Products",
            type: "integer",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<int>(
            name: "Id",
            table: "Products",
            type: "integer",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<int>(
            name: "Id",
            table: "Categories",
            type: "integer",
            nullable: false,
            defaultValue: 0);

        // Populate the old columns with converted data
        migrationBuilder.Sql(
            @"UPDATE ""Products""
              SET ""CategoryId"" = (SELECT CAST(""NewCategoryId"" AS integer) FROM ""Products"");");

        migrationBuilder.Sql(
            @"UPDATE ""Products""
              SET ""Id"" = (SELECT CAST(""NewId"" AS integer) FROM ""Products"");");

        migrationBuilder.Sql(
            @"UPDATE ""Categories""
              SET ""Id"" = (SELECT CAST(""NewId"" AS integer) FROM ""Categories"");");

        // Drop the new columns
        migrationBuilder.DropColumn(
            name: "NewCategoryId",
            table: "Products");

        migrationBuilder.DropColumn(
            name: "NewId",
            table: "Products");

        migrationBuilder.DropColumn(
            name: "NewId",
            table: "Categories");
    }
}
