using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WikiFCVS.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Protocolos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EditadoPorId = table.Column<Guid>(nullable: false),
                    EditadoPorEmail = table.Column<string>(type: "varchar(100)", nullable: true),
                    EditadoEm = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Protocolos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegistroUsuarios",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UsuarioId = table.Column<string>(type: "varchar(100)", nullable: true),
                    DataRegistro = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistroUsuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Temas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Temas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EdicoesTemas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Titulo = table.Column<string>(type: "varchar(100)", nullable: true),
                    TemaId = table.Column<int>(nullable: false),
                    EdicaoEfetuadaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EdicoesTemas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EdicoesTemas_Protocolos_EdicaoEfetuadaId",
                        column: x => x.EdicaoEfetuadaId,
                        principalTable: "Protocolos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EdicoesTemas_Temas_TemaId",
                        column: x => x.TemaId,
                        principalTable: "Temas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EdicoesArtigos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Titulo = table.Column<string>(type: "varchar(100)", nullable: true),
                    Conteudo = table.Column<string>(type: "text", nullable: false),
                    ArtigoId = table.Column<int>(nullable: false),
                    EdicaoEfetuadaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EdicoesArtigos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EdicoesArtigos_Protocolos_EdicaoEfetuadaId",
                        column: x => x.EdicaoEfetuadaId,
                        principalTable: "Protocolos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Artigos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TemaId = table.Column<int>(nullable: true),
                    EdicaoId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artigos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Artigos_EdicoesArtigos_EdicaoId",
                        column: x => x.EdicaoId,
                        principalTable: "EdicoesArtigos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Artigos_Temas_TemaId",
                        column: x => x.TemaId,
                        principalTable: "Temas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Artigos_EdicaoId",
                table: "Artigos",
                column: "EdicaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Artigos_TemaId",
                table: "Artigos",
                column: "TemaId");

            migrationBuilder.CreateIndex(
                name: "IX_EdicoesArtigos_ArtigoId",
                table: "EdicoesArtigos",
                column: "ArtigoId");

            migrationBuilder.CreateIndex(
                name: "IX_EdicoesArtigos_EdicaoEfetuadaId",
                table: "EdicoesArtigos",
                column: "EdicaoEfetuadaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EdicoesTemas_EdicaoEfetuadaId",
                table: "EdicoesTemas",
                column: "EdicaoEfetuadaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EdicoesTemas_TemaId",
                table: "EdicoesTemas",
                column: "TemaId");

            migrationBuilder.AddForeignKey(
                name: "FK_EdicoesArtigos_Artigos_ArtigoId",
                table: "EdicoesArtigos",
                column: "ArtigoId",
                principalTable: "Artigos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Artigos_EdicoesArtigos_EdicaoId",
                table: "Artigos");

            migrationBuilder.DropTable(
                name: "EdicoesTemas");

            migrationBuilder.DropTable(
                name: "RegistroUsuarios");

            migrationBuilder.DropTable(
                name: "EdicoesArtigos");

            migrationBuilder.DropTable(
                name: "Artigos");

            migrationBuilder.DropTable(
                name: "Protocolos");

            migrationBuilder.DropTable(
                name: "Temas");
        }
    }
}