using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    public partial class GetPerson_StoredProcedure_One : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp_GetAllPersons = @"CREATE PROCEDURE [dbo].[GetAllPersons]
               AS BEGIN
                SELECT PersonID,PersonName,Email,DateOfBirth,Geneder,CountryID,Address,ReceivedNewsLetter FROM [dbo].[Persons] END
            ";
            migrationBuilder.Sql(sp_GetAllPersons);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sp_GetAllPersons = @"DROP PROCEDURE [dbo].[GetAllPersons]
            ";
            migrationBuilder.Sql(sp_GetAllPersons);
        }
    }
}
