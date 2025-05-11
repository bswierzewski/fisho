using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FishSpecies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FishSpecies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScoringCategoryOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoringCategoryOptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClerkUserId = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Competitions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    StartTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    EndTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LocationText = table.Column<string>(type: "text", nullable: true),
                    RulesText = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    OrganizerId = table.Column<int>(type: "integer", nullable: false),
                    ResultsToken = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    MainScoringCategoryId = table.Column<int>(type: "integer", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Competitions_ScoringCategoryOptions_MainScoringCategoryId",
                        column: x => x.MainScoringCategoryId,
                        principalTable: "ScoringCategoryOptions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Competitions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Fisheries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    LocationText = table.Column<string>(type: "text", nullable: true),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fisheries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fisheries_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CompetitionParticipants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CompetitionId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: true),
                    GuestName = table.Column<string>(type: "text", nullable: true),
                    GuestIdentifier = table.Column<string>(type: "text", nullable: true),
                    Role = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    AddedByOrganizer = table.Column<bool>(type: "boolean", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetitionParticipants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompetitionParticipants_Competitions_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "Competitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompetitionParticipants_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "SpecialCategoryOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CompetitionId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialCategoryOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpecialCategoryOptions_Competitions_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "Competitions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FishSpeciesFishery",
                columns: table => new
                {
                    DefinedSpeciesId = table.Column<int>(type: "integer", nullable: false),
                    FisheriesId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FishSpeciesFishery", x => new { x.DefinedSpeciesId, x.FisheriesId });
                    table.ForeignKey(
                        name: "FK_FishSpeciesFishery_FishSpecies_DefinedSpeciesId",
                        column: x => x.DefinedSpeciesId,
                        principalTable: "FishSpecies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FishSpeciesFishery_Fisheries_FisheriesId",
                        column: x => x.FisheriesId,
                        principalTable: "Fisheries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LogbookEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    SpeciesName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    LengthCm = table.Column<decimal>(type: "numeric(6,2)", nullable: true),
                    WeightKg = table.Column<decimal>(type: "numeric(7,3)", nullable: true),
                    PhotoUrl = table.Column<string>(type: "text", nullable: false),
                    CatchTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    FisheryId = table.Column<int>(type: "integer", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogbookEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LogbookEntries_Fisheries_FisheryId",
                        column: x => x.FisheryId,
                        principalTable: "Fisheries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_LogbookEntries_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompetitionFishCatches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CompetitionId = table.Column<int>(type: "integer", nullable: false),
                    ParticipantId = table.Column<int>(type: "integer", nullable: false),
                    JudgeId = table.Column<int>(type: "integer", nullable: false),
                    SpeciesName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    LengthCm = table.Column<decimal>(type: "numeric(6,2)", nullable: true),
                    WeightKg = table.Column<decimal>(type: "numeric(7,3)", nullable: true),
                    PhotoUrl = table.Column<string>(type: "text", nullable: false),
                    CatchTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetitionFishCatches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompetitionFishCatches_CompetitionParticipants_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "CompetitionParticipants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompetitionFishCatches_Competitions_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "Competitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompetitionFishCatches_Users_JudgeId",
                        column: x => x.JudgeId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionFishCatches_CompetitionId",
                table: "CompetitionFishCatches",
                column: "CompetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionFishCatches_JudgeId",
                table: "CompetitionFishCatches",
                column: "JudgeId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionFishCatches_ParticipantId",
                table: "CompetitionFishCatches",
                column: "ParticipantId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionParticipants_CompetitionId_GuestIdentifier",
                table: "CompetitionParticipants",
                columns: new[] { "CompetitionId", "GuestIdentifier" },
                unique: true,
                filter: "\"GuestIdentifier\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionParticipants_CompetitionId_UserId",
                table: "CompetitionParticipants",
                columns: new[] { "CompetitionId", "UserId" },
                unique: true,
                filter: "\"UserId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionParticipants_UserId",
                table: "CompetitionParticipants",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Competitions_MainScoringCategoryId",
                table: "Competitions",
                column: "MainScoringCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Competitions_ResultsToken",
                table: "Competitions",
                column: "ResultsToken",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Competitions_UserId",
                table: "Competitions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Fisheries_UserId",
                table: "Fisheries",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FishSpecies_Name",
                table: "FishSpecies",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FishSpeciesFishery_FisheriesId",
                table: "FishSpeciesFishery",
                column: "FisheriesId");

            migrationBuilder.CreateIndex(
                name: "IX_LogbookEntries_FisheryId",
                table: "LogbookEntries",
                column: "FisheryId");

            migrationBuilder.CreateIndex(
                name: "IX_LogbookEntries_UserId",
                table: "LogbookEntries",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialCategoryOptions_CompetitionId",
                table: "SpecialCategoryOptions",
                column: "CompetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ClerkUserId",
                table: "Users",
                column: "ClerkUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true,
                filter: "\"Email\" IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompetitionFishCatches");

            migrationBuilder.DropTable(
                name: "FishSpeciesFishery");

            migrationBuilder.DropTable(
                name: "LogbookEntries");

            migrationBuilder.DropTable(
                name: "SpecialCategoryOptions");

            migrationBuilder.DropTable(
                name: "CompetitionParticipants");

            migrationBuilder.DropTable(
                name: "FishSpecies");

            migrationBuilder.DropTable(
                name: "Fisheries");

            migrationBuilder.DropTable(
                name: "Competitions");

            migrationBuilder.DropTable(
                name: "ScoringCategoryOptions");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
