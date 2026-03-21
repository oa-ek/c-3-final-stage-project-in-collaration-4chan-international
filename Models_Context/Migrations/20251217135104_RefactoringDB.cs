using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models_Context.Migrations
{
    /// <inheritdoc />
    public partial class RefactoringDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArmorInfluences_InfluenceTypes_TypeId",
                table: "ArmorInfluences");

            migrationBuilder.DropForeignKey(
                name: "FK_Armors_ArmorTypes_TypeId",
                table: "Armors");

            migrationBuilder.DropForeignKey(
                name: "FK_WeaponIfnluences_InfluenceTypes_TypeId",
                table: "WeaponIfnluences");

            migrationBuilder.DropForeignKey(
                name: "FK_Weapons_WeaponTypes_TypeId",
                table: "Weapons");

            migrationBuilder.DropIndex(
                name: "IX_Weapons_TypeId",
                table: "Weapons");

            migrationBuilder.DropIndex(
                name: "IX_WeaponIfnluences_TypeId",
                table: "WeaponIfnluences");

            migrationBuilder.DropIndex(
                name: "IX_Armors_TypeId",
                table: "Armors");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "Weapons");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "WeaponIfnluences");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "Armors");

            migrationBuilder.DropColumn(
                name: "InfluenceType",
                table: "ArmorInfluences");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "ArmorInfluences",
                newName: "InfluenceTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_ArmorInfluences_TypeId",
                table: "ArmorInfluences",
                newName: "IX_ArmorInfluences_InfluenceTypeId");

            migrationBuilder.AddColumn<string>(
                name: "IconPath",
                table: "Weapons",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Value",
                table: "WeaponIfnluences",
                type: "float",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HandsArmorId",
                table: "Characters",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HeadArmorId",
                table: "Characters",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LeftHandWeaponId",
                table: "Characters",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LegsArmorId",
                table: "Characters",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RightHandWeaponId",
                table: "Characters",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TorsoArmorId",
                table: "Characters",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IconPath",
                table: "Armors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CharacterBuilds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Vigor = table.Column<int>(type: "int", nullable: true),
                    Endurance = table.Column<int>(type: "int", nullable: true),
                    Strength = table.Column<int>(type: "int", nullable: true),
                    Dexterity = table.Column<int>(type: "int", nullable: true),
                    Intelligence = table.Column<int>(type: "int", nullable: true),
                    Faith = table.Column<int>(type: "int", nullable: true),
                    HeadId = table.Column<int>(type: "int", nullable: true),
                    ChestId = table.Column<int>(type: "int", nullable: true),
                    HandsId = table.Column<int>(type: "int", nullable: true),
                    LegsId = table.Column<int>(type: "int", nullable: true),
                    RightHandId = table.Column<int>(type: "int", nullable: true),
                    LeftHandId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterBuilds", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Weapons_WeaponTypeId",
                table: "Weapons",
                column: "WeaponTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_WeaponIfnluences_InfluenceTypeId",
                table: "WeaponIfnluences",
                column: "InfluenceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_HandsArmorId",
                table: "Characters",
                column: "HandsArmorId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_HeadArmorId",
                table: "Characters",
                column: "HeadArmorId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_LeftHandWeaponId",
                table: "Characters",
                column: "LeftHandWeaponId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_LegsArmorId",
                table: "Characters",
                column: "LegsArmorId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_RightHandWeaponId",
                table: "Characters",
                column: "RightHandWeaponId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_TorsoArmorId",
                table: "Characters",
                column: "TorsoArmorId");

            migrationBuilder.CreateIndex(
                name: "IX_Armors_ArmorTypeId",
                table: "Armors",
                column: "ArmorTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArmorInfluences_InfluenceTypes_InfluenceTypeId",
                table: "ArmorInfluences",
                column: "InfluenceTypeId",
                principalTable: "InfluenceTypes",
                principalColumn: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Armors_ArmorTypes_ArmorTypeId",
                table: "Armors",
                column: "ArmorTypeId",
                principalTable: "ArmorTypes",
                principalColumn: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Armors_HandsArmorId",
                table: "Characters",
                column: "HandsArmorId",
                principalTable: "Armors",
                principalColumn: "ArmorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Armors_HeadArmorId",
                table: "Characters",
                column: "HeadArmorId",
                principalTable: "Armors",
                principalColumn: "ArmorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Armors_LegsArmorId",
                table: "Characters",
                column: "LegsArmorId",
                principalTable: "Armors",
                principalColumn: "ArmorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Armors_TorsoArmorId",
                table: "Characters",
                column: "TorsoArmorId",
                principalTable: "Armors",
                principalColumn: "ArmorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Weapons_LeftHandWeaponId",
                table: "Characters",
                column: "LeftHandWeaponId",
                principalTable: "Weapons",
                principalColumn: "WeaponId");

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Weapons_RightHandWeaponId",
                table: "Characters",
                column: "RightHandWeaponId",
                principalTable: "Weapons",
                principalColumn: "WeaponId");

            migrationBuilder.AddForeignKey(
                name: "FK_WeaponIfnluences_InfluenceTypes_InfluenceTypeId",
                table: "WeaponIfnluences",
                column: "InfluenceTypeId",
                principalTable: "InfluenceTypes",
                principalColumn: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Weapons_WeaponTypes_WeaponTypeId",
                table: "Weapons",
                column: "WeaponTypeId",
                principalTable: "WeaponTypes",
                principalColumn: "TypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArmorInfluences_InfluenceTypes_InfluenceTypeId",
                table: "ArmorInfluences");

            migrationBuilder.DropForeignKey(
                name: "FK_Armors_ArmorTypes_ArmorTypeId",
                table: "Armors");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Armors_HandsArmorId",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Armors_HeadArmorId",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Armors_LegsArmorId",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Armors_TorsoArmorId",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Weapons_LeftHandWeaponId",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Weapons_RightHandWeaponId",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_WeaponIfnluences_InfluenceTypes_InfluenceTypeId",
                table: "WeaponIfnluences");

            migrationBuilder.DropForeignKey(
                name: "FK_Weapons_WeaponTypes_WeaponTypeId",
                table: "Weapons");

            migrationBuilder.DropTable(
                name: "CharacterBuilds");

            migrationBuilder.DropIndex(
                name: "IX_Weapons_WeaponTypeId",
                table: "Weapons");

            migrationBuilder.DropIndex(
                name: "IX_WeaponIfnluences_InfluenceTypeId",
                table: "WeaponIfnluences");

            migrationBuilder.DropIndex(
                name: "IX_Characters_HandsArmorId",
                table: "Characters");

            migrationBuilder.DropIndex(
                name: "IX_Characters_HeadArmorId",
                table: "Characters");

            migrationBuilder.DropIndex(
                name: "IX_Characters_LeftHandWeaponId",
                table: "Characters");

            migrationBuilder.DropIndex(
                name: "IX_Characters_LegsArmorId",
                table: "Characters");

            migrationBuilder.DropIndex(
                name: "IX_Characters_RightHandWeaponId",
                table: "Characters");

            migrationBuilder.DropIndex(
                name: "IX_Characters_TorsoArmorId",
                table: "Characters");

            migrationBuilder.DropIndex(
                name: "IX_Armors_ArmorTypeId",
                table: "Armors");

            migrationBuilder.DropColumn(
                name: "IconPath",
                table: "Weapons");

            migrationBuilder.DropColumn(
                name: "HandsArmorId",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "HeadArmorId",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "LeftHandWeaponId",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "LegsArmorId",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "RightHandWeaponId",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "TorsoArmorId",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "IconPath",
                table: "Armors");

            migrationBuilder.RenameColumn(
                name: "InfluenceTypeId",
                table: "ArmorInfluences",
                newName: "TypeId");

            migrationBuilder.RenameIndex(
                name: "IX_ArmorInfluences_InfluenceTypeId",
                table: "ArmorInfluences",
                newName: "IX_ArmorInfluences_TypeId");

            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "Weapons",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Value",
                table: "WeaponIfnluences",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "WeaponIfnluences",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "Armors",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InfluenceType",
                table: "ArmorInfluences",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Weapons_TypeId",
                table: "Weapons",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_WeaponIfnluences_TypeId",
                table: "WeaponIfnluences",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Armors_TypeId",
                table: "Armors",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArmorInfluences_InfluenceTypes_TypeId",
                table: "ArmorInfluences",
                column: "TypeId",
                principalTable: "InfluenceTypes",
                principalColumn: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Armors_ArmorTypes_TypeId",
                table: "Armors",
                column: "TypeId",
                principalTable: "ArmorTypes",
                principalColumn: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_WeaponIfnluences_InfluenceTypes_TypeId",
                table: "WeaponIfnluences",
                column: "TypeId",
                principalTable: "InfluenceTypes",
                principalColumn: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Weapons_WeaponTypes_TypeId",
                table: "Weapons",
                column: "TypeId",
                principalTable: "WeaponTypes",
                principalColumn: "TypeId");
        }
    }
}
