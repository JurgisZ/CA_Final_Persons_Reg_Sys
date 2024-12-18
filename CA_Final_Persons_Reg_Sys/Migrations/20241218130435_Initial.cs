using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CA_Final_Persons_Reg_Sys.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserPersonalData",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PersonalCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ProfilePicture = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CityName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    StreetName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    HouseNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ApartmentNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPersonalData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(127)", maxLength: 127, nullable: false),
                    UserPersonalDataId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_UserPersonalData_UserPersonalDataId",
                        column: x => x.UserPersonalDataId,
                        principalTable: "UserPersonalData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "UserPersonalData",
                columns: new[] { "Id", "ApartmentNumber", "CityName", "Email", "HouseNumber", "LastName", "Name", "PersonalCode", "PhoneNumber", "ProfilePicture", "StreetName" },
                values: new object[,]
                {
                    { 1L, "1", "Vilnius", "email@email.com", "1", "Jurgeliauskas", "Jurgis", "38901011234", "+37065123123", "LocalPathURL", "Programavimo g." },
                    { 2L, "2", "Kaunas", "antanas@email.eu", "5", "Antanauskas", "Antanas", "3702024321", "0037065321222", "LocalPathURL", "Laisves al." },
                    { 3L, "200", "London", "martyna@email.co.uk", "16", "Paparte", "Martyna", "49103047474", "004706532122211", "LocalPathURL", "Leicester str." }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "PasswordHash", "PasswordSalt", "Role", "UserName", "UserPersonalDataId" },
                values: new object[,]
                {
                    { 1L, new byte[] { 49, 68, 65, 52, 65, 50, 55, 50, 66, 67, 48, 48, 67, 57, 55, 67, 57, 67, 67, 65, 65, 70, 57, 65, 70, 68, 53, 48, 51, 49, 54, 50, 52, 55, 69, 54, 49, 51, 49, 54, 70, 48, 65, 70, 57, 53, 49, 69, 66, 65, 65, 69, 69, 65, 70, 65, 69, 66, 52, 52, 53, 53, 56, 70, 54, 68, 52, 57, 66, 57, 54, 53, 52, 54, 53, 65, 66, 66, 48, 51, 70, 67, 57, 69, 52, 53, 48, 67, 54, 69, 54, 57, 56, 66, 68, 55, 70, 54, 57, 49, 66, 50, 55, 69, 69, 50, 65, 56, 48, 48, 50, 55, 67, 68, 68, 56, 66, 54, 55, 52, 66, 53, 50, 69, 53, 67, 68, 52 }, new byte[] { 33, 79, 37, 67, 42, 101, 70, 75, 111, 87, 38, 107 }, "User", "jurginas", 1L },
                    { 2L, new byte[] { 67, 55, 52, 66, 66, 51, 49, 70, 66, 56, 65, 65, 66, 49, 51, 52, 50, 56, 49, 52, 69, 69, 70, 69, 49, 55, 55, 55, 49, 48, 69, 65, 55, 51, 67, 48, 54, 49, 51, 49, 69, 52, 48, 57, 67, 56, 57, 57, 65, 65, 54, 69, 55, 57, 48, 55, 69, 65, 68, 65, 56, 55, 56, 56, 48, 53, 52, 68, 48, 65, 66, 52, 69, 69, 67, 55, 66, 52, 49, 49, 54, 69, 69, 56, 49, 56, 65, 52, 56, 49, 68, 66, 51, 48, 50, 54, 51, 67, 56, 67, 50, 57, 67, 67, 49, 48, 57, 66, 67, 56, 55, 49, 57, 51, 54, 67, 51, 52, 70, 50, 65, 50, 56, 49, 53, 54, 53, 56 }, new byte[] { 111, 69, 37, 103, 76, 109, 55, 101, 108, 80, 107, 75 }, "User", "antoska", 2L },
                    { 3L, new byte[] { 70, 56, 66, 49, 65, 69, 54, 65, 56, 50, 49, 54, 53, 57, 68, 50, 57, 49, 53, 55, 53, 65, 56, 48, 48, 56, 70, 51, 68, 48, 50, 49, 53, 49, 55, 54, 65, 70, 57, 49, 49, 52, 55, 54, 54, 52, 54, 53, 52, 70, 66, 48, 53, 65, 68, 51, 65, 53, 69, 56, 65, 69, 56, 54, 53, 66, 51, 54, 48, 49, 66, 52, 49, 67, 50, 53, 68, 68, 66, 51, 50, 66, 52, 68, 55, 49, 69, 48, 50, 49, 50, 51, 48, 55, 50, 53, 65, 70, 66, 67, 66, 70, 65, 50, 54, 50, 70, 70, 67, 57, 65, 66, 48, 51, 52, 51, 52, 67, 49, 54, 67, 50, 65, 57, 50, 49, 66, 68 }, new byte[] { 65, 67, 88, 68, 102, 104, 86, 109, 100, 43, 65, 100 }, "User", "marmar", 3L }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserPersonalDataId",
                table: "Users",
                column: "UserPersonalDataId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "UserPersonalData");
        }
    }
}
