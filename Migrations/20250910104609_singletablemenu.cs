using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TruYum.Migrations
{
    /// <inheritdoc />
    public partial class singletablemenu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Beverages");

            migrationBuilder.DropTable(
                name: "MainCourses");

            migrationBuilder.DropTable(
                name: "Snacks");

            migrationBuilder.DropTable(
                name: "Starters");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "MenuItems",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "MenuItems");

            migrationBuilder.CreateTable(
                name: "Beverages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    IsIced = table.Column<bool>(type: "bit", nullable: false),
                    VolumeMl = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Beverages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Beverages_MenuItems_Id",
                        column: x => x.Id,
                        principalTable: "MenuItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MainCourses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Cuisine = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Size = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainCourses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MainCourses_MenuItems_Id",
                        column: x => x.Id,
                        principalTable: "MenuItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Snacks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Baked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Snacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Snacks_MenuItems_Id",
                        column: x => x.Id,
                        principalTable: "MenuItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Starters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Serves = table.Column<int>(type: "int", nullable: false),
                    Spicy = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Starters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Starters_MenuItems_Id",
                        column: x => x.Id,
                        principalTable: "MenuItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }
    }
}
