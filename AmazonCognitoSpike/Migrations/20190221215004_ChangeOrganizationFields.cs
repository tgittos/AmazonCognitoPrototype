using Microsoft.EntityFrameworkCore.Migrations;

namespace AmazonCognitoSpike.Migrations
{
    public partial class ChangeOrganizationFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CognitoUserPoolId",
                table: "Organizations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CognitoUserPoolId",
                table: "Organizations");
        }
    }
}
