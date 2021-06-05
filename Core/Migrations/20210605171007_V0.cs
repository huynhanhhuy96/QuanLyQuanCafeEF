using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Core.Migrations
{
    public partial class V0 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    UserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValueSql: "(N'Admin')"),
                    PassWord = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false, defaultValueSql: "((0))"),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Account__C9F2845720CEE35B", x => x.UserName);
                });

            migrationBuilder.CreateTable(
                name: "FoodCategory",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValueSql: "(N'Chưa đặt tên')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodCategory", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "TableFood",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValueSql: "(N'Bàn chưa có tên')"),
                    status = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValueSql: "(N'Trống')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TableFood", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Food",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValueSql: "(N'Chưa đặt tên')"),
                    idCategory = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Food", x => x.id);
                    table.ForeignKey(
                        name: "FK__Food__price__31EC6D26",
                        column: x => x.idCategory,
                        principalTable: "FoodCategory",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Bill",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateCheckIn = table.Column<DateTime>(type: "date", nullable: false, defaultValueSql: "(getdate())"),
                    DateCheckOut = table.Column<DateTime>(type: "date", nullable: true),
                    idTable = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    discount = table.Column<int>(type: "int", nullable: true),
                    totalPrice = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bill", x => x.id);
                    table.ForeignKey(
                        name: "FK__Bill__status__36B12243",
                        column: x => x.idTable,
                        principalTable: "TableFood",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BillInfo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idBill = table.Column<int>(type: "int", nullable: false),
                    idFood = table.Column<int>(type: "int", nullable: false),
                    count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillInfo", x => x.id);
                    table.ForeignKey(
                        name: "FK__BillInfo__count__3A81B327",
                        column: x => x.idBill,
                        principalTable: "Bill",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__BillInfo__idFood__3B75D760",
                        column: x => x.idFood,
                        principalTable: "Food",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bill_idTable",
                table: "Bill",
                column: "idTable");

            migrationBuilder.CreateIndex(
                name: "IX_BillInfo_idBill",
                table: "BillInfo",
                column: "idBill");

            migrationBuilder.CreateIndex(
                name: "IX_BillInfo_idFood",
                table: "BillInfo",
                column: "idFood");

            migrationBuilder.CreateIndex(
                name: "IX_Food_idCategory",
                table: "Food",
                column: "idCategory");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "BillInfo");

            migrationBuilder.DropTable(
                name: "Bill");

            migrationBuilder.DropTable(
                name: "Food");

            migrationBuilder.DropTable(
                name: "TableFood");

            migrationBuilder.DropTable(
                name: "FoodCategory");
        }
    }
}
