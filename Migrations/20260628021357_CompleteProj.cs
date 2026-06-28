using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ComprasVentas.Migrations
{
    /// <inheritdoc />
    public partial class CompleteProj : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categorias",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    descripcion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_categorias", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "cliente_proveedor",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tipo = table.Column<string>(type: "text", nullable: false),
                    razon_social = table.Column<string>(type: "text", nullable: false),
                    nro_identificacion = table.Column<string>(type: "text", nullable: false),
                    telefono = table.Column<string>(type: "text", nullable: false),
                    correo = table.Column<string>(type: "text", nullable: false),
                    estado = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cliente_proveedor", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "permisos",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    recurso = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    action = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_permisos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "personas",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombres = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    apellidos = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    fecha_nacimiento = table.Column<DateOnly>(type: "date", nullable: false),
                    genero = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    telefono = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    direccion = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    nacionalidad = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_personas", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    descripcion = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sucursales",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    direccion = table.Column<string>(type: "text", nullable: false),
                    telefono = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ciudad = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sucursales", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "productos",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "text", nullable: false),
                    descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    unidad_medida = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    marca = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    precio_venta_actual = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    imagen = table.Column<string>(type: "text", nullable: true),
                    estado = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    categoria_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_productos", x => x.id);
                    table.ForeignKey(
                        name: "fk_productos_categorias_categoria_id",
                        column: x => x.categoria_id,
                        principalTable: "categorias",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "documentos",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    descripcion_doc = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    referencia = table.Column<string>(type: "text", nullable: false),
                    persona_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_documentos", x => x.id);
                    table.ForeignKey(
                        name: "fk_documentos_personas_persona_id",
                        column: x => x.persona_id,
                        principalTable: "personas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "usuarios",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    password = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    persona_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_usuarios", x => x.id);
                    table.ForeignKey(
                        name: "fk_usuarios_personas_persona_id",
                        column: x => x.persona_id,
                        principalTable: "personas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "roles_permisos",
                columns: table => new
                {
                    permisos_id = table.Column<int>(type: "integer", nullable: false),
                    roles_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_roles_permisos", x => new { x.permisos_id, x.roles_id });
                    table.ForeignKey(
                        name: "fk_roles_permisos_permisos_permisos_id",
                        column: x => x.permisos_id,
                        principalTable: "permisos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_roles_permisos_roles_roles_id",
                        column: x => x.roles_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "almacenes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    codigo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    descripcion = table.Column<string>(type: "text", nullable: true),
                    direccion = table.Column<string>(type: "text", nullable: false),
                    telefono = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ciudad = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    sucursal_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_almacenes", x => x.id);
                    table.ForeignKey(
                        name: "fk_almacenes_sucursales_sucursal_id",
                        column: x => x.sucursal_id,
                        principalTable: "sucursales",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "notas",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    tipo_nota = table.Column<string>(type: "text", nullable: false),
                    descuento = table.Column<decimal>(type: "numeric", nullable: false),
                    impuestos = table.Column<decimal>(type: "numeric", nullable: false),
                    estado = table.Column<bool>(type: "boolean", nullable: false),
                    observaciones = table.Column<string>(type: "text", nullable: false),
                    usuario_id = table.Column<int>(type: "integer", nullable: false),
                    cliente_proveedor_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_notas", x => x.id);
                    table.ForeignKey(
                        name: "fk_notas_cliente_proveedor_cliente_proveedor_id",
                        column: x => x.cliente_proveedor_id,
                        principalTable: "cliente_proveedor",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_notas_usuarios_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "refresh_tokens",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    token = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", maxLength: 100, nullable: false),
                    expires = table.Column<DateTime>(type: "timestamp with time zone", maxLength: 100, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", maxLength: 100, nullable: false),
                    usuario_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_refresh_tokens", x => x.id);
                    table.ForeignKey(
                        name: "fk_refresh_tokens_usuarios_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sucursal_usuario",
                columns: table => new
                {
                    sucursales_id = table.Column<int>(type: "integer", nullable: false),
                    usuarios_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sucursal_usuario", x => new { x.sucursales_id, x.usuarios_id });
                    table.ForeignKey(
                        name: "fk_sucursal_usuario_sucursales_sucursales_id",
                        column: x => x.sucursales_id,
                        principalTable: "sucursales",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_sucursal_usuario_usuarios_usuarios_id",
                        column: x => x.usuarios_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "usuarios_roles",
                columns: table => new
                {
                    roles_id = table.Column<int>(type: "integer", nullable: false),
                    usuarios_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_usuarios_roles", x => new { x.roles_id, x.usuarios_id });
                    table.ForeignKey(
                        name: "fk_usuarios_roles_roles_roles_id",
                        column: x => x.roles_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_usuarios_roles_usuarios_usuarios_id",
                        column: x => x.usuarios_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "almacen_productos",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    cantidad_actual = table.Column<int>(type: "integer", nullable: false),
                    fecha_actualizacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    precio_venta_actual = table.Column<decimal>(type: "numeric", nullable: false),
                    estado = table.Column<bool>(type: "boolean", nullable: false),
                    producto_id = table.Column<int>(type: "integer", nullable: true),
                    almacen_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_almacen_productos", x => x.id);
                    table.ForeignKey(
                        name: "fk_almacen_productos_almacenes_almacen_id",
                        column: x => x.almacen_id,
                        principalTable: "almacenes",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_almacen_productos_productos_producto_id",
                        column: x => x.producto_id,
                        principalTable: "productos",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "movimientos",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    cantidad = table.Column<decimal>(type: "numeric", nullable: false),
                    tipo_movimiento = table.Column<string>(type: "text", nullable: false),
                    precio_unitario_compra = table.Column<decimal>(type: "numeric", nullable: false),
                    precio_unitario_venta = table.Column<decimal>(type: "numeric", nullable: false),
                    observaciones = table.Column<string>(type: "text", nullable: true),
                    producto_id = table.Column<int>(type: "integer", nullable: false),
                    almacen_id = table.Column<int>(type: "integer", nullable: false),
                    nota_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_movimientos", x => x.id);
                    table.ForeignKey(
                        name: "fk_movimientos_almacenes_almacen_id",
                        column: x => x.almacen_id,
                        principalTable: "almacenes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_movimientos_notas_nota_id",
                        column: x => x.nota_id,
                        principalTable: "notas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_movimientos_productos_producto_id",
                        column: x => x.producto_id,
                        principalTable: "productos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_almacen_productos_almacen_id",
                table: "almacen_productos",
                column: "almacen_id");

            migrationBuilder.CreateIndex(
                name: "ix_almacen_productos_producto_id",
                table: "almacen_productos",
                column: "producto_id");

            migrationBuilder.CreateIndex(
                name: "ix_almacenes_sucursal_id",
                table: "almacenes",
                column: "sucursal_id");

            migrationBuilder.CreateIndex(
                name: "ix_categorias_nombre",
                table: "categorias",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_documentos_persona_id",
                table: "documentos",
                column: "persona_id");

            migrationBuilder.CreateIndex(
                name: "ix_movimientos_almacen_id",
                table: "movimientos",
                column: "almacen_id");

            migrationBuilder.CreateIndex(
                name: "ix_movimientos_nota_id",
                table: "movimientos",
                column: "nota_id");

            migrationBuilder.CreateIndex(
                name: "ix_movimientos_producto_id",
                table: "movimientos",
                column: "producto_id");

            migrationBuilder.CreateIndex(
                name: "ix_notas_cliente_proveedor_id",
                table: "notas",
                column: "cliente_proveedor_id");

            migrationBuilder.CreateIndex(
                name: "ix_notas_usuario_id",
                table: "notas",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "ix_permisos_nombre",
                table: "permisos",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_productos_categoria_id",
                table: "productos",
                column: "categoria_id");

            migrationBuilder.CreateIndex(
                name: "ix_refresh_tokens_usuario_id",
                table: "refresh_tokens",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "ix_roles_nombre",
                table: "roles",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_roles_permisos_roles_id",
                table: "roles_permisos",
                column: "roles_id");

            migrationBuilder.CreateIndex(
                name: "ix_sucursal_usuario_usuarios_id",
                table: "sucursal_usuario",
                column: "usuarios_id");

            migrationBuilder.CreateIndex(
                name: "ix_usuarios_email",
                table: "usuarios",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_usuarios_nombre",
                table: "usuarios",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_usuarios_persona_id",
                table: "usuarios",
                column: "persona_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_usuarios_roles_usuarios_id",
                table: "usuarios_roles",
                column: "usuarios_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "almacen_productos");

            migrationBuilder.DropTable(
                name: "documentos");

            migrationBuilder.DropTable(
                name: "movimientos");

            migrationBuilder.DropTable(
                name: "refresh_tokens");

            migrationBuilder.DropTable(
                name: "roles_permisos");

            migrationBuilder.DropTable(
                name: "sucursal_usuario");

            migrationBuilder.DropTable(
                name: "usuarios_roles");

            migrationBuilder.DropTable(
                name: "almacenes");

            migrationBuilder.DropTable(
                name: "notas");

            migrationBuilder.DropTable(
                name: "productos");

            migrationBuilder.DropTable(
                name: "permisos");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "sucursales");

            migrationBuilder.DropTable(
                name: "cliente_proveedor");

            migrationBuilder.DropTable(
                name: "usuarios");

            migrationBuilder.DropTable(
                name: "categorias");

            migrationBuilder.DropTable(
                name: "personas");
        }
    }
}
