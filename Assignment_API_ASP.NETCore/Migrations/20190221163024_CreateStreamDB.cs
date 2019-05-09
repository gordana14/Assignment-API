using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Assignment_API_ASP.NETCore.Migrations
{
    public partial class CreateStreamDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "ChannelInfo",
                schema: "dbo",
                columns: table => new
                {
                    ChannelID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelInfo", x => x.ChannelID);
                });

            migrationBuilder.CreateTable(
                name: "MessageInfo",
                schema: "dbo",
                columns: table => new
                {
                    MessageID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Text = table.Column<string>(type: "varchar(50)", nullable: true),
                    Inserted = table.Column<DateTime>(type: "datetime", nullable: true),
                    Validated = table.Column<DateTime>(type: "datetime", nullable: true),
                    ChannelID = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageInfo", x => x.MessageID);
                });

            migrationBuilder.CreateTable(
                name: "UserInfo",
                schema: "dbo",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(50)", nullable: true),
                    Activated = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInfo", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "UserChannels",
                columns: table => new
                {
                    UserID = table.Column<int>(nullable: false),
                    ChannelID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserChannels", x => new { x.UserID, x.ChannelID });
                    table.ForeignKey(
                        name: "FK_UserChannels_ChannelInfo_ChannelID",
                        column: x => x.ChannelID,
                        principalSchema: "dbo",
                        principalTable: "ChannelInfo",
                        principalColumn: "ChannelID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserChannels_UserInfo_UserID",
                        column: x => x.UserID,
                        principalSchema: "dbo",
                        principalTable: "UserInfo",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserChannels_ChannelID",
                table: "UserChannels",
                column: "ChannelID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserChannels");

            migrationBuilder.DropTable(
                name: "MessageInfo",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ChannelInfo",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "UserInfo",
                schema: "dbo");
        }
    }
}
