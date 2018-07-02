using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Life_Web.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.CreateTable(
                name: "GameStrategy",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CommentCount = table.Column<int>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    FromUrl = table.Column<string>(nullable: true),
                    GameName = table.Column<string>(nullable: true),
                    GameNodeID = table.Column<int>(nullable: false),
                    GameTitle = table.Column<string>(nullable: true),
                    ReadCount = table.Column<int>(nullable: true),
                    content = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameStrategy", x => x.Id);
                });
        }
    }
}
