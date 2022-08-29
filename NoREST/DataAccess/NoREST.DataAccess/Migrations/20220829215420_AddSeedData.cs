using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoREST.DataAccess.Migrations
{
    public partial class AddSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedOn", "IdentityProviderId", "IsAdmin", "IsSystemUser", "LastModifiedOn", "UserName" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "5qg4one8ve2o7024st8dks8ejq", true, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "NoRESTApi" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedOn", "IdentityProviderId", "IsAdmin", "IsSystemUser", "LastModifiedOn", "UserName" },
                values: new object[] { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "b9c45965-95d5-4d45-a580-bafeba254ad0", true, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "chris.rune" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
