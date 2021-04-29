using Microsoft.EntityFrameworkCore.Migrations;

namespace Rehberv2.Server.Migrations
{
    public partial class addRehberToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rehber",
                columns: table => new
                {
                    rehberID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    isim = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    soyisim = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    cepTel = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    isTel = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    email = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rehber", x => x.rehberID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rehber");
        }
    }
}
