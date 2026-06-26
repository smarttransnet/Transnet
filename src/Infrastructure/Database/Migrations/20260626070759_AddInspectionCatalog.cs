using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations;

    /// <inheritdoc />
    public partial class AddInspectionCatalog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "inspection_catalog_items",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    category = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    item_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    sort_order = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_inspection_catalog_items", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("2ca28cb1-6bc9-488d-a5ac-fb526cb7a164"), "RADIATOR & HOSE", "a). Coolent correct level & Radiator cap condition.", 1, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("f7c5de3d-f4f0-486b-b610-d7334aec5858"), "RADIATOR & HOSE", "b). No Cracks (or) splits", 2, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("644c44a8-b727-4139-bce5-c8e0d57189da"), "RADIATOR & HOSE", "c). Fan belt tension & belt adjustment pully condition.", 3, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("5833002c-008f-4513-826a-e6000449fcf4"), "RADIATOR & HOSE", "d). Radiator mounting bolts and Hoses condition", 4, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("c4725676-a082-464c-b5c0-dc311832714e"), "ENGINE SYSTEM", "a). Engine oil change and check", 1, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("76b92425-89e2-435f-a49e-8438a8ceb4d2"), "ENGINE SYSTEM", "b). Clean (or) Renew the air filter element.", 2, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("e98a10d3-7486-4565-8051-588cac56572b"), "ENGINE SYSTEM", "c). Renew engine lubricating oil filter's, if it's required", 3, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("7a3ed299-8135-4d09-ba36-2f28dd6a0fba"), "ENGINE SYSTEM", "d). Renew elements of fuel filter's, if it's required", 4, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("50eb7d76-8e86-49cb-bd1c-141319a1518c"), "ENGINE SYSTEM", "e). Renew water separator, if it's required", 5, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("af9fd060-9590-491a-8302-d825479f898b"), "ENGINE SYSTEM", "f). Engine RPM, Smoke and Oil leakage check", 6, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("6c74b364-2d10-49be-a5b5-5a8ea036cb48"), "AUTOELECTRICAL SYSTEM", "a). Head lights H/L Properly operative", 1, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("913052f6-b805-45a5-9b51-d6e879565839"), "AUTOELECTRICAL SYSTEM", "b). Parking Lights Properly operative", 2, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("9f5aaed3-e945-471d-bae6-613a13993486"), "AUTOELECTRICAL SYSTEM", "c). Signal Lights Properly operative", 3, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("cd39cdaa-d86c-4fce-9429-797a3769483b"), "AUTOELECTRICAL SYSTEM", "d). Wipper Properly operative", 4, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("9ae37bba-d75c-4cfd-a1ae-ab97b08776a6"), "AUTOELECTRICAL SYSTEM", "e). Horn Properly operative", 5, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("4c5fcd2c-3360-4018-9af1-1051a10fcb19"), "AUTOELECTRICAL SYSTEM", "f). Battery Properly charging", 6, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("34b2d129-da11-4643-a411-9f03ff03fc1d"), "AUTOELECTRICAL SYSTEM", "g). Brake lights Properly operative", 7, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("7ec01179-3d86-45dc-859a-42c1842963ec"), "AUTOELECTRICAL SYSTEM", "h). Reverse Horn Properly operative", 8, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("182d9f87-ffd9-4157-a50e-1a1ab627c383"), "AUTOELECTRICAL SYSTEM", "i). Door Winders Properly operative", 9, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("785eebc6-c9a3-4496-afaa-8b47e424b2ce"), "AUTOELECTRICAL SYSTEM", "j). Phone Charges Properly operative", 10, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("92a95e66-9c26-4c50-a402-4db7a2ef0171"), "POWER STEERING SYSTEM", "a). Steering Wheel No unusual slack (or) looseness.", 1, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("3eca73e7-45ab-4201-93d7-cb000547ed9a"), "POWER STEERING SYSTEM", "b). Power Steering fluid correct level", 2, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("1c0b8d03-577c-416a-9913-4fe2be8b116f"), "POWER STEERING SYSTEM", "c). Tie rod end's Ball joint's no slack", 3, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("8bfc239f-b66e-432d-81bf-971c44effd57"), "OIL LEVELS", "Engine oil level correct level", 1, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("36fc11b3-3fef-439a-ab60-c445a83873a3"), "OIL LEVELS", "Power steering oil level correct level", 2, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("8e3ece1f-29f6-4ad0-ae39-031ef5846825"), "OIL LEVELS", "Brake fluid level correct level", 3, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("55c6b304-b0b7-4ede-aa28-d59bb67b7554"), "OIL LEVELS", "Differentials oil level correct level", 4, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("66df643b-ad2a-46c9-93ed-970e9ada7306"), "OIL LEVELS", "Drive axle oil level correct level", 5, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("d9894455-5512-4808-b042-e97b5e96a64d"), "OIL LEVELS", "Gear oil correct level", 6, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("7d14e5b3-1fb8-42ca-a3af-05b10f11f559"), "EXHAUST SYSTEM", "Muffler & Exhust pipe snug on brackets", 1, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("5e3bd41c-0c82-424b-98d9-6ef92fecfd8f"), "EXHAUST SYSTEM", "No leak and free of debris", 2, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("5ed62abe-21c6-435a-957a-f096a91c3881"), "AIR BRAKE SYSTEM", "a). Air leakage check", 1, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("bcb89854-9afb-4b3a-ab88-7f9dcc763cdc"), "AIR BRAKE SYSTEM", "b). Water drain in Air tank", 2, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("b7800658-d756-4803-b899-fdca2bf3b7d0"), "AIR BRAKE SYSTEM", "c). Air dryer filter check", 3, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("7d285185-f70c-4e4a-864c-366f2380e72d"), "AIR BRAKE SYSTEM", "d). Oil leakage in Master Cylinder check", 4, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("188b7424-5ac6-498d-90eb-069062268a91"), "AIR BRAKE SYSTEM", "e). Brake pad and disc or liner and drum Check", 5, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("3ecb49a6-388f-40cb-a4a7-dfd7b0dc5ad1"), "GENERAL", "a). Air leakage in Brake booster check", 1, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("56270d49-7eaa-493b-83ed-15ad430e09d2"), "GENERAL", "b). Clutch pedal free ply check", 2, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("664b3afc-b9b5-4075-aab7-603cfd5e19f8"), "GENERAL", "c). Gear shifting check", 3, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("f7137bd7-9f4e-4d17-8c83-11a53e8ff734"), "GENERAL", "d). Tyre pressure and condition check", 4, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("38ad61e1-c0b3-4608-9944-296024bcbadd"), "GENERAL", "e). All greasing point greasing", 5, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("812bc882-2cdf-4f47-8d6d-1e73505830ae"), "GENERAL", "f). Tie Rod", 6, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("9076e7ec-c05e-44e1-b540-061666a65dd8"), "GENERAL", "g). King Pin", 7, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("80df6b9d-8982-4245-8851-07cc5f62eb03"), "GENERAL", "h). Bearing", 8, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("f0c03996-0a85-41e2-9dd6-ec070be34eaf"), "GENERAL", "i). Suspension and propeller shaft check", 9, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("db723aa0-370f-4d3b-9ea6-3894c22d759f"), "METER BOARDT", "AIR METER", 1, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("f0b95d07-ac45-407f-a628-8afe1a331241"), "METER BOARDT", "DIESAL METER", 2, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("5bbba37b-ccf6-4a83-ba94-4556a237141d"), "METER BOARDT", "RPM", 3, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("ebc4eee2-18d5-443f-b196-d9e0dcfdaa5d"), "METER BOARDT", "TEMPRETURE METER", 4, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("ed307a7c-8f26-445b-ae41-87cee9e05c43"), "METER BOARDT", "SPEEDO METER", 5, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("6d5eb9df-d24f-42ff-9c56-aac3430e552f"), "ROAD TEST", "BRAKES: No grabbing or pulling, pedal not spongy", 1, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("212c192d-8325-4526-b686-28d6a63021ca"), "ROAD TEST", "STEERINGS: No excessive wheel play /shimmy / pulling", 2, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("4ab0f845-1b84-425a-ae99-89e6239337e2"), "ROAD TEST", "ENGINE: Operational no unusual noise or vibration.", 3, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("b7c24ae5-355f-488a-852a-683ec9f73702"), "BODY TYPE", "Side mirrors", 1, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("26340d4d-cc70-4ddc-bce7-98d94e24aa72"), "BODY TYPE", "Side Doors", 2, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("85ec0667-4c53-4794-9248-f9a8dc2c7b17"), "BODY TYPE", "Buffers", 3, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("c371e1d9-8e41-4aac-9a87-618e6a9e7f42"), "BODY TYPE", "Door Locks", 4, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("c78bfbf2-3423-42aa-884e-d7e5466e7928"), "BODY TYPE", "Seats", 5, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("42b35c88-366a-4bdc-b1b9-5c8e9a7310ad"), "BODY TYPE", "Safety Belts", 6, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("80066877-19ff-40be-8728-3ef9591b634b"), "BODY TYPE", "Mudguards", 7, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("068b0c6d-9a03-4c48-8f7f-e1019c6eda76"), "BODY TYPE", "Cabin Side Boxes", 8, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("11bfac83-d5e1-4eb2-9a5a-a6108040bd98"), "BODY TYPE", "Lights", 9, true });
            migrationBuilder.InsertData(
                table: "inspection_catalog_items",
                schema: "public",
                columns: new[] { "id", "category", "item_name", "sort_order", "is_active" },
                values: new object[] { new Guid("9301b21a-c964-400a-9cbd-871b43ff51d6"), "BODY TYPE", "Tires", 10, true });


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "inspection_catalog_items",
                schema: "public");
        }
    }

