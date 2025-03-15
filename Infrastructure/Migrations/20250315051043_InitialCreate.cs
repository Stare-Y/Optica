using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "mica",
                columns: table => new
                {
                    id_mica = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tipo = table.Column<string>(type: "text", nullable: false),
                    fabricante = table.Column<string>(type: "text", nullable: false),
                    material = table.Column<string>(type: "text", nullable: false),
                    tratamiento = table.Column<string>(type: "text", nullable: false),
                    proposito = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mica", x => x.id_mica);
                });

            migrationBuilder.CreateTable(
                name: "usuario",
                columns: table => new
                {
                    id_usuario = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre_usuario = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    password = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    rol = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuario", x => x.id_usuario);
                });

            migrationBuilder.CreateTable(
                name: "mica_graduacion",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_mica = table.Column<int>(type: "integer", nullable: false),
                    graduacionesf = table.Column<float>(type: "real", nullable: false),
                    graduacioncil = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mica_graduacion", x => x.id);
                    table.ForeignKey(
                        name: "FK_mica_graduacion_mica_id_mica",
                        column: x => x.id_mica,
                        principalTable: "mica",
                        principalColumn: "id_mica",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lote",
                columns: table => new
                {
                    id_lote = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    referencia = table.Column<string>(type: "text", nullable: false),
                    fecha_entrada = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    proveedor = table.Column<string>(type: "text", nullable: false),
                    fecha_caducidad = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    id_usuario = table.Column<int>(type: "integer", nullable: false),
                    costo = table.Column<double>(type: "double precision", nullable: false),
                    existencias = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lote", x => x.id_lote);
                    table.ForeignKey(
                        name: "FK_lote_usuario_id_usuario",
                        column: x => x.id_usuario,
                        principalTable: "usuario",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "pedido",
                columns: table => new
                {
                    id_pedido = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fecha_salida = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    id_usuario = table.Column<int>(type: "integer", nullable: false),
                    razon_social = table.Column<string>(type: "text", nullable: false),
                    extra = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pedido", x => x.id_pedido);
                    table.ForeignKey(
                        name: "FK_pedido_usuario_id_usuario",
                        column: x => x.id_usuario,
                        principalTable: "usuario",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lote_mica",
                columns: table => new
                {
                    id_lote = table.Column<int>(type: "integer", nullable: false),
                    id_mica_graduacion = table.Column<int>(type: "integer", nullable: false),
                    cantidad = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lote_mica", x => new { x.id_mica_graduacion, x.id_lote });
                    table.ForeignKey(
                        name: "FK_lote_mica_lote_id_lote",
                        column: x => x.id_lote,
                        principalTable: "lote",
                        principalColumn: "id_lote",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_lote_mica_mica_graduacion_id_mica_graduacion",
                        column: x => x.id_mica_graduacion,
                        principalTable: "mica_graduacion",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "pedido_mica",
                columns: table => new
                {
                    id_pedido = table.Column<int>(type: "integer", nullable: false),
                    id_mica_graduacion = table.Column<int>(type: "integer", nullable: false),
                    id_lote_origen = table.Column<int>(type: "integer", nullable: false),
                    cantidad = table.Column<int>(type: "integer", nullable: false),
                    precio = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pedido_mica", x => new { x.id_mica_graduacion, x.id_pedido });
                    table.ForeignKey(
                        name: "FK_pedido_mica_lote_id_lote_origen",
                        column: x => x.id_lote_origen,
                        principalTable: "lote",
                        principalColumn: "id_lote",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_pedido_mica_mica_graduacion_id_mica_graduacion",
                        column: x => x.id_mica_graduacion,
                        principalTable: "mica_graduacion",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_pedido_mica_pedido_id_pedido",
                        column: x => x.id_pedido,
                        principalTable: "pedido",
                        principalColumn: "id_pedido",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_lote_id_usuario",
                table: "lote",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_lote_mica_id_lote",
                table: "lote_mica",
                column: "id_lote");

            migrationBuilder.CreateIndex(
                name: "IX_mica_graduacion_graduacionesf_graduacioncil_id_mica",
                table: "mica_graduacion",
                columns: new[] { "graduacionesf", "graduacioncil", "id_mica" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_mica_graduacion_id_mica",
                table: "mica_graduacion",
                column: "id_mica");

            migrationBuilder.CreateIndex(
                name: "IX_pedido_id_usuario",
                table: "pedido",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_pedido_mica_id_lote_origen",
                table: "pedido_mica",
                column: "id_lote_origen");

            migrationBuilder.CreateIndex(
                name: "IX_pedido_mica_id_pedido",
                table: "pedido_mica",
                column: "id_pedido");

            migrationBuilder.CreateIndex(
                name: "IX_usuario_nombre_usuario",
                table: "usuario",
                column: "nombre_usuario",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "lote_mica");

            migrationBuilder.DropTable(
                name: "pedido_mica");

            migrationBuilder.DropTable(
                name: "lote");

            migrationBuilder.DropTable(
                name: "mica_graduacion");

            migrationBuilder.DropTable(
                name: "pedido");

            migrationBuilder.DropTable(
                name: "mica");

            migrationBuilder.DropTable(
                name: "usuario");
        }
    }
}
