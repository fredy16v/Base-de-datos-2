using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiAutores.Migrations
{
    /// <inheritdoc />
    public partial class AddTableReviewAndRespuesta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "valoracion",
                schema: "transacctional",
                table: "books",
                type: "float",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "reviews",
                schema: "transacctional",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    book_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    valoracion = table.Column<double>(type: "float", nullable: false),
                    comentario = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    usuario_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    fecha_publicacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    conmentable = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reviews", x => x.id);
                    table.ForeignKey(
                        name: "FK_reviews_books_book_id",
                        column: x => x.book_id,
                        principalSchema: "transacctional",
                        principalTable: "books",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_reviews_users_UserId",
                        column: x => x.UserId,
                        principalSchema: "security",
                        principalTable: "users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "respuestas",
                schema: "transacctional",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    review_id = table.Column<int>(type: "int", nullable: false),
                    comentario = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    usuario_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    fecha_publicacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    respuesta_padre = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_respuestas", x => x.id);
                    table.ForeignKey(
                        name: "FK_respuestas_respuestas_respuesta_padre",
                        column: x => x.respuesta_padre,
                        principalSchema: "transacctional",
                        principalTable: "respuestas",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_respuestas_reviews_review_id",
                        column: x => x.review_id,
                        principalSchema: "transacctional",
                        principalTable: "reviews",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_respuestas_users_UserId",
                        column: x => x.UserId,
                        principalSchema: "security",
                        principalTable: "users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_respuestas_respuesta_padre",
                schema: "transacctional",
                table: "respuestas",
                column: "respuesta_padre");

            migrationBuilder.CreateIndex(
                name: "IX_respuestas_review_id",
                schema: "transacctional",
                table: "respuestas",
                column: "review_id");

            migrationBuilder.CreateIndex(
                name: "IX_respuestas_UserId",
                schema: "transacctional",
                table: "respuestas",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_reviews_book_id",
                schema: "transacctional",
                table: "reviews",
                column: "book_id");

            migrationBuilder.CreateIndex(
                name: "IX_reviews_UserId",
                schema: "transacctional",
                table: "reviews",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "respuestas",
                schema: "transacctional");

            migrationBuilder.DropTable(
                name: "reviews",
                schema: "transacctional");

            migrationBuilder.DropColumn(
                name: "valoracion",
                schema: "transacctional",
                table: "books");
        }
    }
}
