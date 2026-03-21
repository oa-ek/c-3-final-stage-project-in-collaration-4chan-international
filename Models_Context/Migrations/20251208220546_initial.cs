using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models_Context.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    CharacterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CharactName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Level = table.Column<int>(type: "int", nullable: true),
                    Vigor = table.Column<int>(type: "int", nullable: true),
                    Endurance = table.Column<int>(type: "int", nullable: true),
                    Strenght = table.Column<int>(type: "int", nullable: true),
                    Dexterity = table.Column<int>(type: "int", nullable: true),
                    Intelligence = table.Column<int>(type: "int", nullable: true),
                    Faith = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.CharacterId);
                });

            migrationBuilder.CreateTable(
                name: "InfluenceTypes",
                columns: table => new
                {
                    TypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfluenceTypes", x => x.TypeId);
                });

            migrationBuilder.CreateTable(
                name: "Slots",
                columns: table => new
                {
                    SlotId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Slots", x => x.SlotId);
                });

            migrationBuilder.CreateTable(
                name: "Sets",
                columns: table => new
                {
                    SetId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CharacterId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sets", x => x.SetId);
                    table.ForeignKey(
                        name: "FK_Sets_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "CharacterId");
                });

            migrationBuilder.CreateTable(
                name: "ArmorTypes",
                columns: table => new
                {
                    TypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SlotId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArmorTypes", x => x.TypeId);
                    table.ForeignKey(
                        name: "FK_ArmorTypes_Slots_SlotId",
                        column: x => x.SlotId,
                        principalTable: "Slots",
                        principalColumn: "SlotId");
                });

            migrationBuilder.CreateTable(
                name: "WeaponTypes",
                columns: table => new
                {
                    TypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SlotId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeaponTypes", x => x.TypeId);
                    table.ForeignKey(
                        name: "FK_WeaponTypes_Slots_SlotId",
                        column: x => x.SlotId,
                        principalTable: "Slots",
                        principalColumn: "SlotId");
                });

            migrationBuilder.CreateTable(
                name: "Armors",
                columns: table => new
                {
                    ArmorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeId = table.Column<int>(type: "int", nullable: true),
                    ArmorTypeId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Weight = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Poise = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Skin = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Armors", x => x.ArmorId);
                    table.ForeignKey(
                        name: "FK_Armors_ArmorTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "ArmorTypes",
                        principalColumn: "TypeId");
                });

            migrationBuilder.CreateTable(
                name: "Weapons",
                columns: table => new
                {
                    WeaponId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeId = table.Column<int>(type: "int", nullable: true),
                    WeaponTypeId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Damage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Weight = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ReqStrenght = table.Column<int>(type: "int", nullable: true),
                    ReqDexterity = table.Column<int>(type: "int", nullable: true),
                    ReqIntelligence = table.Column<int>(type: "int", nullable: true),
                    ReqFaith = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weapons", x => x.WeaponId);
                    table.ForeignKey(
                        name: "FK_Weapons_WeaponTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "WeaponTypes",
                        principalColumn: "TypeId");
                });

            migrationBuilder.CreateTable(
                name: "ArmorInfluences",
                columns: table => new
                {
                    InfluenceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeId = table.Column<int>(type: "int", nullable: true),
                    InfluenceType = table.Column<int>(type: "int", nullable: true),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ArmorId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArmorInfluences", x => x.InfluenceId);
                    table.ForeignKey(
                        name: "FK_ArmorInfluences_Armors_ArmorId",
                        column: x => x.ArmorId,
                        principalTable: "Armors",
                        principalColumn: "ArmorId");
                    table.ForeignKey(
                        name: "FK_ArmorInfluences_InfluenceTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "InfluenceTypes",
                        principalColumn: "TypeId");
                });

            migrationBuilder.CreateTable(
                name: "ArmorSet",
                columns: table => new
                {
                    ArmorsArmorId = table.Column<int>(type: "int", nullable: false),
                    SetsSetId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArmorSet", x => new { x.ArmorsArmorId, x.SetsSetId });
                    table.ForeignKey(
                        name: "FK_ArmorSet_Armors_ArmorsArmorId",
                        column: x => x.ArmorsArmorId,
                        principalTable: "Armors",
                        principalColumn: "ArmorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArmorSet_Sets_SetsSetId",
                        column: x => x.SetsSetId,
                        principalTable: "Sets",
                        principalColumn: "SetId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SetWeapon",
                columns: table => new
                {
                    SetsSetId = table.Column<int>(type: "int", nullable: false),
                    WeaponsWeaponId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SetWeapon", x => new { x.SetsSetId, x.WeaponsWeaponId });
                    table.ForeignKey(
                        name: "FK_SetWeapon_Sets_SetsSetId",
                        column: x => x.SetsSetId,
                        principalTable: "Sets",
                        principalColumn: "SetId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SetWeapon_Weapons_WeaponsWeaponId",
                        column: x => x.WeaponsWeaponId,
                        principalTable: "Weapons",
                        principalColumn: "WeaponId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WeaponIfnluences",
                columns: table => new
                {
                    InfluenceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeId = table.Column<int>(type: "int", nullable: true),
                    InfluenceTypeId = table.Column<int>(type: "int", nullable: true),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WeaponId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeaponIfnluences", x => x.InfluenceId);
                    table.ForeignKey(
                        name: "FK_WeaponIfnluences_InfluenceTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "InfluenceTypes",
                        principalColumn: "TypeId");
                    table.ForeignKey(
                        name: "FK_WeaponIfnluences_Weapons_WeaponId",
                        column: x => x.WeaponId,
                        principalTable: "Weapons",
                        principalColumn: "WeaponId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArmorInfluences_ArmorId",
                table: "ArmorInfluences",
                column: "ArmorId");

            migrationBuilder.CreateIndex(
                name: "IX_ArmorInfluences_TypeId",
                table: "ArmorInfluences",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Armors_TypeId",
                table: "Armors",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ArmorSet_SetsSetId",
                table: "ArmorSet",
                column: "SetsSetId");

            migrationBuilder.CreateIndex(
                name: "IX_ArmorTypes_SlotId",
                table: "ArmorTypes",
                column: "SlotId",
                unique: true,
                filter: "[SlotId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Sets_CharacterId",
                table: "Sets",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_SetWeapon_WeaponsWeaponId",
                table: "SetWeapon",
                column: "WeaponsWeaponId");

            migrationBuilder.CreateIndex(
                name: "IX_WeaponIfnluences_TypeId",
                table: "WeaponIfnluences",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_WeaponIfnluences_WeaponId",
                table: "WeaponIfnluences",
                column: "WeaponId");

            migrationBuilder.CreateIndex(
                name: "IX_Weapons_TypeId",
                table: "Weapons",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_WeaponTypes_SlotId",
                table: "WeaponTypes",
                column: "SlotId",
                unique: true,
                filter: "[SlotId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArmorInfluences");

            migrationBuilder.DropTable(
                name: "ArmorSet");

            migrationBuilder.DropTable(
                name: "SetWeapon");

            migrationBuilder.DropTable(
                name: "WeaponIfnluences");

            migrationBuilder.DropTable(
                name: "Armors");

            migrationBuilder.DropTable(
                name: "Sets");

            migrationBuilder.DropTable(
                name: "InfluenceTypes");

            migrationBuilder.DropTable(
                name: "Weapons");

            migrationBuilder.DropTable(
                name: "ArmorTypes");

            migrationBuilder.DropTable(
                name: "Characters");

            migrationBuilder.DropTable(
                name: "WeaponTypes");

            migrationBuilder.DropTable(
                name: "Slots");
        }
    }
}
