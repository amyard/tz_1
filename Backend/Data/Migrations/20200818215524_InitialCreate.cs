using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TransactionMDs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TransactionId = table.Column<int>(nullable: false),
                    Status = table.Column<string>(nullable: false),
                    Type = table.Column<string>(nullable: false),
                    ClientName = table.Column<string>(nullable: false),
                    Price = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionMDs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransactionMDs");
        }
    }
}
