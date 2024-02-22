using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Rdpzsd.Models.Migrations
{
    public partial class V100 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "admissionreason",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    oldcode = table.Column<string>(type: "text", nullable: true),
                    shortname = table.Column<string>(type: "text", nullable: true),
                    shortnamealt = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    admissionreasonstudenttype = table.Column<int>(type: "integer", nullable: true),
                    countryunion = table.Column<int>(type: "integer", nullable: true),
                    version = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    namealt = table.Column<string>(type: "text", nullable: true),
                    alias = table.Column<string>(type: "text", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: false),
                    vieworder = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_admissionreason", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "country",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    eucountry = table.Column<bool>(type: "boolean", nullable: false),
                    eeacountry = table.Column<bool>(type: "boolean", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    namealt = table.Column<string>(type: "text", nullable: true),
                    alias = table.Column<string>(type: "text", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: false),
                    vieworder = table.Column<int>(type: "integer", nullable: true),
                    code = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_country", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "district",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code2 = table.Column<string>(type: "text", nullable: true),
                    secondlevelregioncode = table.Column<string>(type: "text", nullable: true),
                    mainsettlementcode = table.Column<string>(type: "text", nullable: true),
                    version = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    namealt = table.Column<string>(type: "text", nullable: true),
                    alias = table.Column<string>(type: "text", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: false),
                    vieworder = table.Column<int>(type: "integer", nullable: true),
                    code = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_district", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "educationalform",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    datauniexternalid = table.Column<int>(type: "integer", nullable: true),
                    version = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    namealt = table.Column<string>(type: "text", nullable: true),
                    alias = table.Column<string>(type: "text", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: false),
                    vieworder = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_educationalform", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "educationalqualification",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    datauniexternalid = table.Column<int>(type: "integer", nullable: true),
                    version = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    namealt = table.Column<string>(type: "text", nullable: true),
                    alias = table.Column<string>(type: "text", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: false),
                    vieworder = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_educationalqualification", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "educationfeetype",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    oldcode = table.Column<string>(type: "text", nullable: true),
                    version = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    namealt = table.Column<string>(type: "text", nullable: true),
                    alias = table.Column<string>(type: "text", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: false),
                    vieworder = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_educationfeetype", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "language",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    version = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    namealt = table.Column<string>(type: "text", nullable: true),
                    alias = table.Column<string>(type: "text", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: false),
                    vieworder = table.Column<int>(type: "integer", nullable: true),
                    code = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_language", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "nationalstatisticalinstitute",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    oldcode = table.Column<string>(type: "text", nullable: true),
                    fordoctors = table.Column<bool>(type: "boolean", nullable: true),
                    version = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    namealt = table.Column<string>(type: "text", nullable: true),
                    alias = table.Column<string>(type: "text", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: false),
                    vieworder = table.Column<int>(type: "integer", nullable: true),
                    code = table.Column<string>(type: "text", nullable: true),
                    rootid = table.Column<int>(type: "integer", nullable: false),
                    parentid = table.Column<int>(type: "integer", nullable: true),
                    level = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nationalstatisticalinstitute", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "period",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    year = table.Column<int>(type: "integer", nullable: false),
                    semester = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    namealt = table.Column<string>(type: "text", nullable: true),
                    alias = table.Column<string>(type: "text", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: false),
                    vieworder = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_period", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "researcharea",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    codenumber = table.Column<string>(type: "text", nullable: true),
                    version = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    namealt = table.Column<string>(type: "text", nullable: true),
                    alias = table.Column<string>(type: "text", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: false),
                    vieworder = table.Column<int>(type: "integer", nullable: true),
                    code = table.Column<string>(type: "text", nullable: true),
                    rootid = table.Column<int>(type: "integer", nullable: false),
                    parentid = table.Column<int>(type: "integer", nullable: true),
                    level = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_researcharea", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "schemaversions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    version = table.Column<string>(type: "text", nullable: true),
                    updatedon = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    systemname = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_schemaversions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "studentstatus",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    version = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    namealt = table.Column<string>(type: "text", nullable: true),
                    alias = table.Column<string>(type: "text", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: false),
                    vieworder = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_studentstatus", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "admissionreasonhistory",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userid = table.Column<int>(type: "integer", nullable: false),
                    userfullname = table.Column<string>(type: "text", nullable: true),
                    admissionreasonid = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    namealt = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    shortname = table.Column<string>(type: "text", nullable: true),
                    shortnamealt = table.Column<string>(type: "text", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: false),
                    changedate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_admissionreasonhistory", x => x.id);
                    table.ForeignKey(
                        name: "FK_admissionreasonhistory_admissionreason_admissionreasonid",
                        column: x => x.admissionreasonid,
                        principalTable: "admissionreason",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "admissionreasoncitizenship",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    admissionreasonid = table.Column<int>(type: "integer", nullable: false),
                    excludecountry = table.Column<bool>(type: "boolean", nullable: true),
                    countryid = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_admissionreasoncitizenship", x => x.id);
                    table.ForeignKey(
                        name: "FK_admissionreasoncitizenship_admissionreason_admissionreasonid",
                        column: x => x.admissionreasonid,
                        principalTable: "admissionreason",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_admissionreasoncitizenship_country_countryid",
                        column: x => x.countryid,
                        principalTable: "country",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "municipality",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    districtid = table.Column<int>(type: "integer", nullable: false),
                    code2 = table.Column<string>(type: "text", nullable: true),
                    mainsettlementcode = table.Column<string>(type: "text", nullable: true),
                    category = table.Column<string>(type: "text", nullable: true),
                    version = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    namealt = table.Column<string>(type: "text", nullable: true),
                    alias = table.Column<string>(type: "text", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: false),
                    vieworder = table.Column<int>(type: "integer", nullable: true),
                    code = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_municipality", x => x.id);
                    table.ForeignKey(
                        name: "FK_municipality_district_districtid",
                        column: x => x.districtid,
                        principalTable: "district",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "admissionreasoneducationfee",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    admissionreasonid = table.Column<int>(type: "integer", nullable: false),
                    educationfeetypeid = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_admissionreasoneducationfee", x => x.id);
                    table.ForeignKey(
                        name: "FK_admissionreasoneducationfee_admissionreason_admissionreason~",
                        column: x => x.admissionreasonid,
                        principalTable: "admissionreason",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_admissionreasoneducationfee_educationfeetype_educationfeety~",
                        column: x => x.educationfeetypeid,
                        principalTable: "educationfeetype",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "speciality",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    researchareaid = table.Column<int>(type: "integer", nullable: true),
                    educationalqualificationid = table.Column<int>(type: "integer", nullable: false),
                    isregulated = table.Column<bool>(type: "boolean", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    namealt = table.Column<string>(type: "text", nullable: true),
                    alias = table.Column<string>(type: "text", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: false),
                    vieworder = table.Column<int>(type: "integer", nullable: true),
                    code = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_speciality", x => x.id);
                    table.ForeignKey(
                        name: "FK_speciality_educationalqualification_educationalqualificatio~",
                        column: x => x.educationalqualificationid,
                        principalTable: "educationalqualification",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_speciality_researcharea_researchareaid",
                        column: x => x.researchareaid,
                        principalTable: "researcharea",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "studentevent",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    studentstatusid = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    namealt = table.Column<string>(type: "text", nullable: true),
                    alias = table.Column<string>(type: "text", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: false),
                    vieworder = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_studentevent", x => x.id);
                    table.ForeignKey(
                        name: "FK_studentevent_studentstatus_studentstatusid",
                        column: x => x.studentstatusid,
                        principalTable: "studentstatus",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "admissionreasoneducationfeehistory",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    admissionreasonid = table.Column<int>(type: "integer", nullable: false),
                    educationfeetypeid = table.Column<int>(type: "integer", nullable: false),
                    educationfeetypename = table.Column<string>(type: "text", nullable: true),
                    admissionreasonhistoryid = table.Column<int>(type: "integer", nullable: true),
                    version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_admissionreasoneducationfeehistory", x => x.id);
                    table.ForeignKey(
                        name: "FK_admissionreasoneducationfeehistory_admissionreasonhistory_a~",
                        column: x => x.admissionreasonhistoryid,
                        principalTable: "admissionreasonhistory",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "settlement",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    municipalityid = table.Column<int>(type: "integer", nullable: false),
                    districtid = table.Column<int>(type: "integer", nullable: false),
                    municipalitycode = table.Column<string>(type: "text", nullable: true),
                    districtcode = table.Column<string>(type: "text", nullable: true),
                    municipalitycode2 = table.Column<string>(type: "text", nullable: true),
                    districtcode2 = table.Column<string>(type: "text", nullable: true),
                    typename = table.Column<string>(type: "text", nullable: true),
                    settlementname = table.Column<string>(type: "text", nullable: true),
                    typecode = table.Column<string>(type: "text", nullable: true),
                    mayoraltycode = table.Column<string>(type: "text", nullable: true),
                    category = table.Column<string>(type: "text", nullable: true),
                    altitude = table.Column<string>(type: "text", nullable: true),
                    isdistrict = table.Column<bool>(type: "boolean", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    namealt = table.Column<string>(type: "text", nullable: true),
                    alias = table.Column<string>(type: "text", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: false),
                    vieworder = table.Column<int>(type: "integer", nullable: true),
                    code = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_settlement", x => x.id);
                    table.ForeignKey(
                        name: "FK_settlement_district_districtid",
                        column: x => x.districtid,
                        principalTable: "district",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_settlement_municipality_municipalityid",
                        column: x => x.municipalityid,
                        principalTable: "municipality",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "studenteventqualification",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    studenteventid = table.Column<int>(type: "integer", nullable: false),
                    educationalqualificationid = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_studenteventqualification", x => x.id);
                    table.ForeignKey(
                        name: "FK_studenteventqualification_educationalqualification_educatio~",
                        column: x => x.educationalqualificationid,
                        principalTable: "educationalqualification",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_studenteventqualification_studentevent_studenteventid",
                        column: x => x.studenteventid,
                        principalTable: "studentevent",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "institution",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    lotnumber = table.Column<int>(type: "integer", nullable: false),
                    state = table.Column<int>(type: "integer", nullable: true),
                    uic = table.Column<string>(type: "text", nullable: true),
                    shortname = table.Column<string>(type: "text", nullable: true),
                    shortnamealt = table.Column<string>(type: "text", nullable: true),
                    organizationtype = table.Column<int>(type: "integer", nullable: true),
                    ownershiptype = table.Column<int>(type: "integer", nullable: true),
                    settlementid = table.Column<int>(type: "integer", nullable: true),
                    municipalityid = table.Column<int>(type: "integer", nullable: true),
                    districtid = table.Column<int>(type: "integer", nullable: true),
                    isresearchuniversity = table.Column<bool>(type: "boolean", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    namealt = table.Column<string>(type: "text", nullable: true),
                    alias = table.Column<string>(type: "text", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: false),
                    vieworder = table.Column<int>(type: "integer", nullable: true),
                    code = table.Column<string>(type: "text", nullable: true),
                    rootid = table.Column<int>(type: "integer", nullable: false),
                    parentid = table.Column<int>(type: "integer", nullable: true),
                    level = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_institution", x => x.id);
                    table.ForeignKey(
                        name: "FK_institution_district_districtid",
                        column: x => x.districtid,
                        principalTable: "district",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_institution_institution_parentid",
                        column: x => x.parentid,
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_institution_institution_rootid",
                        column: x => x.rootid,
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_institution_municipality_municipalityid",
                        column: x => x.municipalityid,
                        principalTable: "municipality",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_institution_settlement_settlementid",
                        column: x => x.settlementid,
                        principalTable: "settlement",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "school",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    state = table.Column<int>(type: "integer", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    ownershiptype = table.Column<int>(type: "integer", nullable: false),
                    settlementid = table.Column<int>(type: "integer", nullable: false),
                    municipalityid = table.Column<int>(type: "integer", nullable: false),
                    districtid = table.Column<int>(type: "integer", nullable: false),
                    migrationid = table.Column<int>(type: "integer", nullable: true),
                    parentid = table.Column<int>(type: "integer", nullable: true),
                    version = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    namealt = table.Column<string>(type: "text", nullable: true),
                    alias = table.Column<string>(type: "text", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: false),
                    vieworder = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_school", x => x.id);
                    table.ForeignKey(
                        name: "FK_school_district_districtid",
                        column: x => x.districtid,
                        principalTable: "district",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_school_municipality_municipalityid",
                        column: x => x.municipalityid,
                        principalTable: "municipality",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_school_school_parentid",
                        column: x => x.parentid,
                        principalTable: "school",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_school_settlement_settlementid",
                        column: x => x.settlementid,
                        principalTable: "settlement",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "institutionspeciality",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    institutionid = table.Column<int>(type: "integer", nullable: false),
                    specialityid = table.Column<int>(type: "integer", nullable: false),
                    educationalformid = table.Column<int>(type: "integer", nullable: true),
                    nsiregionid = table.Column<int>(type: "integer", nullable: true),
                    nationalstatisticalinstituteid = table.Column<int>(type: "integer", nullable: true),
                    duration = table.Column<decimal>(type: "numeric", nullable: true),
                    isaccredited = table.Column<bool>(type: "boolean", nullable: false),
                    isforcadets = table.Column<bool>(type: "boolean", nullable: false),
                    isactive = table.Column<bool>(type: "boolean", nullable: false),
                    isjointspeciality = table.Column<bool>(type: "boolean", nullable: true),
                    version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_institutionspeciality", x => x.id);
                    table.ForeignKey(
                        name: "FK_institutionspeciality_educationalform_educationalformid",
                        column: x => x.educationalformid,
                        principalTable: "educationalform",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_institutionspeciality_institution_institutionid",
                        column: x => x.institutionid,
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_institutionspeciality_nationalstatisticalinstitute_national~",
                        column: x => x.nationalstatisticalinstituteid,
                        principalTable: "nationalstatisticalinstitute",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_institutionspeciality_nationalstatisticalinstitute_nsiregio~",
                        column: x => x.nsiregionid,
                        principalTable: "nationalstatisticalinstitute",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_institutionspeciality_speciality_specialityid",
                        column: x => x.specialityid,
                        principalTable: "speciality",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "personimport",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    version = table.Column<int>(type: "integer", nullable: false),
                    state = table.Column<int>(type: "integer", nullable: false),
                    userid = table.Column<int>(type: "integer", nullable: false),
                    userfullname = table.Column<string>(type: "text", nullable: true),
                    useremail = table.Column<string>(type: "text", nullable: true),
                    institutionid = table.Column<int>(type: "integer", nullable: false),
                    subordinateid = table.Column<int>(type: "integer", nullable: true),
                    createdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    finishdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    entitiesacceptedcount = table.Column<int>(type: "integer", nullable: true),
                    entitiescount = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personimport", x => x.id);
                    table.ForeignKey(
                        name: "FK_personimport_institution_institutionid",
                        column: x => x.institutionid,
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personimport_institution_subordinateid",
                        column: x => x.subordinateid,
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "personlot",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    uan = table.Column<string>(type: "text", nullable: true),
                    state = table.Column<int>(type: "integer", nullable: false),
                    createuserid = table.Column<int>(type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    createinstitutionid = table.Column<int>(type: "integer", nullable: true),
                    createsubordinateid = table.Column<int>(type: "integer", nullable: true),
                    version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personlot", x => x.id);
                    table.ForeignKey(
                        name: "FK_personlot_institution_createinstitutionid",
                        column: x => x.createinstitutionid,
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personlot_institution_createsubordinateid",
                        column: x => x.createsubordinateid,
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "specialityimport",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    version = table.Column<int>(type: "integer", nullable: false),
                    state = table.Column<int>(type: "integer", nullable: false),
                    userid = table.Column<int>(type: "integer", nullable: false),
                    userfullname = table.Column<string>(type: "text", nullable: true),
                    useremail = table.Column<string>(type: "text", nullable: true),
                    institutionid = table.Column<int>(type: "integer", nullable: false),
                    subordinateid = table.Column<int>(type: "integer", nullable: true),
                    createdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    finishdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    entitiesacceptedcount = table.Column<int>(type: "integer", nullable: true),
                    entitiescount = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_specialityimport", x => x.id);
                    table.ForeignKey(
                        name: "FK_specialityimport_institution_institutionid",
                        column: x => x.institutionid,
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_specialityimport_institution_subordinateid",
                        column: x => x.subordinateid,
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "institutionspecialityjointspeciality",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    institutionspecialityid = table.Column<int>(type: "integer", nullable: false),
                    location = table.Column<int>(type: "integer", nullable: false),
                    institutionid = table.Column<int>(type: "integer", nullable: true),
                    institutionbyparentid = table.Column<int>(type: "integer", nullable: true),
                    foreigninstitutionname = table.Column<string>(type: "text", nullable: true),
                    foreigninstitutionbyparentname = table.Column<string>(type: "text", nullable: true),
                    version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_institutionspecialityjointspeciality", x => x.id);
                    table.ForeignKey(
                        name: "FK_institutionspecialityjointspeciality_institution_instituti~1",
                        column: x => x.institutionid,
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_institutionspecialityjointspeciality_institution_institutio~",
                        column: x => x.institutionbyparentid,
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_institutionspecialityjointspeciality_institutionspeciality_~",
                        column: x => x.institutionspecialityid,
                        principalTable: "institutionspeciality",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "institutionspecialitylanguage",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    institutionspecialityid = table.Column<int>(type: "integer", nullable: false),
                    languageid = table.Column<int>(type: "integer", nullable: true),
                    version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_institutionspecialitylanguage", x => x.id);
                    table.ForeignKey(
                        name: "FK_institutionspecialitylanguage_institutionspeciality_institu~",
                        column: x => x.institutionspecialityid,
                        principalTable: "institutionspeciality",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_institutionspecialitylanguage_language_languageid",
                        column: x => x.languageid,
                        principalTable: "language",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "personimporterrorfile",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    key = table.Column<Guid>(type: "uuid", nullable: false),
                    hash = table.Column<string>(type: "text", nullable: true),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    mimetype = table.Column<string>(type: "text", nullable: true),
                    dbid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personimporterrorfile", x => x.id);
                    table.ForeignKey(
                        name: "FK_personimporterrorfile_personimport_id",
                        column: x => x.id,
                        principalTable: "personimport",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "personimportfile",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    key = table.Column<Guid>(type: "uuid", nullable: false),
                    hash = table.Column<string>(type: "text", nullable: true),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    mimetype = table.Column<string>(type: "text", nullable: true),
                    dbid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personimportfile", x => x.id);
                    table.ForeignKey(
                        name: "FK_personimportfile_personimport_id",
                        column: x => x.id,
                        principalTable: "personimport",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "personimporthistory",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    version = table.Column<int>(type: "integer", nullable: false),
                    rdpzsdimportid = table.Column<int>(type: "integer", nullable: false),
                    state = table.Column<int>(type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personimporthistory", x => x.id);
                    table.ForeignKey(
                        name: "FK_personimporthistory_personimport_rdpzsdimportid",
                        column: x => x.rdpzsdimportid,
                        principalTable: "personimport",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "personimportuan",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    personimportid = table.Column<int>(type: "integer", nullable: false),
                    uan = table.Column<string>(type: "text", nullable: true),
                    version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personimportuan", x => x.id);
                    table.ForeignKey(
                        name: "FK_personimportuan_personimport_personimportid",
                        column: x => x.personimportid,
                        principalTable: "personimport",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "personbasic",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    state = table.Column<int>(type: "integer", nullable: false),
                    firstname = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    middlename = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    lastname = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    othernames = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    fullname = table.Column<string>(type: "text", nullable: true),
                    firstnamealt = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    middlenamealt = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    lastnamealt = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    othernamesalt = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    fullnamealt = table.Column<string>(type: "text", nullable: true),
                    uin = table.Column<string>(type: "character(10)", fixedLength: true, maxLength: 10, nullable: true),
                    foreignernumber = table.Column<string>(type: "character(10)", fixedLength: true, maxLength: 10, nullable: true),
                    idnnumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    phonenumber = table.Column<string>(type: "character varying(18)", maxLength: 18, nullable: true),
                    postcode = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: true),
                    gender = table.Column<int>(type: "integer", nullable: false),
                    birthdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    birthcountryid = table.Column<int>(type: "integer", nullable: true),
                    birthdistrictid = table.Column<int>(type: "integer", nullable: true),
                    birthmunicipalityid = table.Column<int>(type: "integer", nullable: true),
                    birthsettlementid = table.Column<int>(type: "integer", nullable: true),
                    foreignerbirthsettlement = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    citizenshipid = table.Column<int>(type: "integer", nullable: false),
                    secondcitizenshipid = table.Column<int>(type: "integer", nullable: true),
                    residencecountryid = table.Column<int>(type: "integer", nullable: true),
                    residencedistrictid = table.Column<int>(type: "integer", nullable: true),
                    residencemunicipalityid = table.Column<int>(type: "integer", nullable: true),
                    residencesettlementid = table.Column<int>(type: "integer", nullable: true),
                    residenceaddress = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personbasic", x => x.id);
                    table.ForeignKey(
                        name: "FK_personbasic_country_birthcountryid",
                        column: x => x.birthcountryid,
                        principalTable: "country",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personbasic_country_citizenshipid",
                        column: x => x.citizenshipid,
                        principalTable: "country",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personbasic_country_residencecountryid",
                        column: x => x.residencecountryid,
                        principalTable: "country",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personbasic_country_secondcitizenshipid",
                        column: x => x.secondcitizenshipid,
                        principalTable: "country",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personbasic_district_birthdistrictid",
                        column: x => x.birthdistrictid,
                        principalTable: "district",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personbasic_district_residencedistrictid",
                        column: x => x.residencedistrictid,
                        principalTable: "district",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personbasic_municipality_birthmunicipalityid",
                        column: x => x.birthmunicipalityid,
                        principalTable: "municipality",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personbasic_municipality_residencemunicipalityid",
                        column: x => x.residencemunicipalityid,
                        principalTable: "municipality",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personbasic_personlot_id",
                        column: x => x.id,
                        principalTable: "personlot",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personbasic_settlement_birthsettlementid",
                        column: x => x.birthsettlementid,
                        principalTable: "settlement",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personbasic_settlement_residencesettlementid",
                        column: x => x.residencesettlementid,
                        principalTable: "settlement",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "persondiplomacopy",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    lotid = table.Column<int>(type: "integer", nullable: false),
                    docname = table.Column<string>(type: "text", nullable: true),
                    schoolmun = table.Column<string>(type: "text", nullable: true),
                    schoolobl = table.Column<string>(type: "text", nullable: true),
                    schooltown = table.Column<string>(type: "text", nullable: true),
                    dtorigregdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    dtprotdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    dtregdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    intcurrver = table.Column<int>(type: "integer", nullable: true),
                    intcurryear = table.Column<int>(type: "integer", nullable: false),
                    intdocid = table.Column<int>(type: "integer", nullable: true),
                    intdoctemplid = table.Column<double>(type: "double precision", nullable: true),
                    intid = table.Column<double>(type: "double precision", nullable: true),
                    intidtype = table.Column<int>(type: "integer", nullable: true),
                    intmeanmark = table.Column<decimal>(type: "numeric", nullable: true),
                    intschoolid = table.Column<int>(type: "integer", nullable: true),
                    intstudentid = table.Column<long>(type: "bigint", nullable: false),
                    intyeargraduated = table.Column<int>(type: "integer", nullable: true),
                    vcidnumbertext = table.Column<string>(type: "text", nullable: true),
                    vcorigprnno = table.Column<string>(type: "text", nullable: true),
                    vcorigprnser = table.Column<string>(type: "text", nullable: true),
                    vcorigregno1 = table.Column<string>(type: "text", nullable: true),
                    vcorigregno2 = table.Column<string>(type: "text", nullable: true),
                    vcorigschoolname = table.Column<string>(type: "text", nullable: true),
                    vcprnno = table.Column<string>(type: "text", nullable: true),
                    vcprnser = table.Column<string>(type: "text", nullable: true),
                    vcprotno = table.Column<string>(type: "text", nullable: true),
                    vcregno1 = table.Column<string>(type: "text", nullable: true),
                    vcregno2 = table.Column<string>(type: "text", nullable: true),
                    vcschoolname = table.Column<string>(type: "text", nullable: true),
                    vcstudname1 = table.Column<string>(type: "text", nullable: true),
                    vcstudname2 = table.Column<string>(type: "text", nullable: true),
                    vcstudname3 = table.Column<string>(type: "text", nullable: true),
                    version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_persondiplomacopy", x => x.id);
                    table.ForeignKey(
                        name: "FK_persondiplomacopy_personlot_lotid",
                        column: x => x.lotid,
                        principalTable: "personlot",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "personlotaction",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    lotid = table.Column<int>(type: "integer", nullable: false),
                    userid = table.Column<int>(type: "integer", nullable: false),
                    userfullname = table.Column<string>(type: "text", nullable: true),
                    actiondate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    institutionid = table.Column<int>(type: "integer", nullable: true),
                    subordinateid = table.Column<int>(type: "integer", nullable: true),
                    actiontype = table.Column<int>(type: "integer", nullable: false),
                    note = table.Column<string>(type: "text", nullable: true),
                    version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personlotaction", x => x.id);
                    table.ForeignKey(
                        name: "FK_personlotaction_institution_institutionid",
                        column: x => x.institutionid,
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personlotaction_institution_subordinateid",
                        column: x => x.subordinateid,
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personlotaction_personlot_lotid",
                        column: x => x.lotid,
                        principalTable: "personlot",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "personlotidnumber",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    migrationidnumber = table.Column<double>(type: "double precision", nullable: true),
                    migrationuniid = table.Column<int>(type: "integer", nullable: true),
                    personlotid = table.Column<int>(type: "integer", nullable: false),
                    institutionlotid = table.Column<int>(type: "integer", nullable: true),
                    identifiertypeaction = table.Column<int>(type: "integer", nullable: true),
                    applicationid = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personlotidnumber", x => x.id);
                    table.ForeignKey(
                        name: "FK_personlotidnumber_personlot_personlotid",
                        column: x => x.personlotid,
                        principalTable: "personlot",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "personsecondary",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    fromrso = table.Column<bool>(type: "boolean", nullable: false),
                    rsointid = table.Column<double>(type: "double precision", nullable: true),
                    version = table.Column<int>(type: "integer", nullable: false),
                    state = table.Column<int>(type: "integer", nullable: false),
                    graduationyear = table.Column<int>(type: "integer", nullable: false),
                    countryid = table.Column<int>(type: "integer", nullable: false),
                    schoolid = table.Column<int>(type: "integer", nullable: true),
                    foreignschoolname = table.Column<string>(type: "character varying(180)", maxLength: 180, nullable: true),
                    profession = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    diplomanumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    diplomadate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    recognitionnumber = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    recognitiondate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    missingschoolfromregister = table.Column<bool>(type: "boolean", nullable: false),
                    missingschoolname = table.Column<string>(type: "character varying(180)", maxLength: 180, nullable: true),
                    missingschoolsettlementid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personsecondary", x => x.id);
                    table.ForeignKey(
                        name: "FK_personsecondary_country_countryid",
                        column: x => x.countryid,
                        principalTable: "country",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personsecondary_personlot_id",
                        column: x => x.id,
                        principalTable: "personlot",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personsecondary_school_schoolid",
                        column: x => x.schoolid,
                        principalTable: "school",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personsecondary_settlement_missingschoolsettlementid",
                        column: x => x.missingschoolsettlementid,
                        principalTable: "settlement",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "personstudent",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    facultynumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    lotid = table.Column<int>(type: "integer", nullable: false),
                    stickerstate = table.Column<int>(type: "integer", nullable: false),
                    stickeryear = table.Column<int>(type: "integer", nullable: true),
                    version = table.Column<int>(type: "integer", nullable: false),
                    state = table.Column<int>(type: "integer", nullable: false),
                    institutionid = table.Column<int>(type: "integer", nullable: false),
                    subordinateid = table.Column<int>(type: "integer", nullable: true),
                    institutionspecialityid = table.Column<int>(type: "integer", nullable: false),
                    studentstatusid = table.Column<int>(type: "integer", nullable: false),
                    studenteventid = table.Column<int>(type: "integer", nullable: false),
                    admissionreasonid = table.Column<int>(type: "integer", nullable: false),
                    petype = table.Column<int>(type: "integer", nullable: false),
                    pehighschooltype = table.Column<int>(type: "integer", nullable: true),
                    pediplomanumber = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    pediplomadate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    peresearchareaid = table.Column<int>(type: "integer", nullable: true),
                    peeducationalqualificationid = table.Column<int>(type: "integer", nullable: true),
                    peacquiredforeigneducationalqualificationid = table.Column<int>(type: "integer", nullable: true),
                    pepartid = table.Column<int>(type: "integer", nullable: true),
                    peinstitutionid = table.Column<int>(type: "integer", nullable: true),
                    pesubordinateid = table.Column<int>(type: "integer", nullable: true),
                    peinstitutionspecialityid = table.Column<int>(type: "integer", nullable: true),
                    pespecialitymissinginregister = table.Column<bool>(type: "boolean", nullable: true),
                    peinstitutionname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    pesubordinatename = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    pespecialityname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    pecountryid = table.Column<int>(type: "integer", nullable: true),
                    perecognizedspeciality = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    peacquiredspeciality = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    perecognitionnumber = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    perecognitiondate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personstudent", x => x.id);
                    table.ForeignKey(
                        name: "FK_personstudent_admissionreason_admissionreasonid",
                        column: x => x.admissionreasonid,
                        principalTable: "admissionreason",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personstudent_country_pecountryid",
                        column: x => x.pecountryid,
                        principalTable: "country",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personstudent_educationalqualification_peacquiredforeignedu~",
                        column: x => x.peacquiredforeigneducationalqualificationid,
                        principalTable: "educationalqualification",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personstudent_educationalqualification_peeducationalqualifi~",
                        column: x => x.peeducationalqualificationid,
                        principalTable: "educationalqualification",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personstudent_institution_institutionid",
                        column: x => x.institutionid,
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personstudent_institution_peinstitutionid",
                        column: x => x.peinstitutionid,
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personstudent_institution_pesubordinateid",
                        column: x => x.pesubordinateid,
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personstudent_institution_subordinateid",
                        column: x => x.subordinateid,
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personstudent_institutionspeciality_institutionspecialityid",
                        column: x => x.institutionspecialityid,
                        principalTable: "institutionspeciality",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personstudent_institutionspeciality_peinstitutionspeciality~",
                        column: x => x.peinstitutionspecialityid,
                        principalTable: "institutionspeciality",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personstudent_personlot_lotid",
                        column: x => x.lotid,
                        principalTable: "personlot",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personstudent_personstudent_pepartid",
                        column: x => x.pepartid,
                        principalTable: "personstudent",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personstudent_researcharea_peresearchareaid",
                        column: x => x.peresearchareaid,
                        principalTable: "researcharea",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personstudent_studentevent_studenteventid",
                        column: x => x.studenteventid,
                        principalTable: "studentevent",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personstudent_studentstatus_studentstatusid",
                        column: x => x.studentstatusid,
                        principalTable: "studentstatus",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "specialityimporterrorfile",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    key = table.Column<Guid>(type: "uuid", nullable: false),
                    hash = table.Column<string>(type: "text", nullable: true),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    mimetype = table.Column<string>(type: "text", nullable: true),
                    dbid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_specialityimporterrorfile", x => x.id);
                    table.ForeignKey(
                        name: "FK_specialityimporterrorfile_specialityimport_id",
                        column: x => x.id,
                        principalTable: "specialityimport",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "specialityimportfile",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    key = table.Column<Guid>(type: "uuid", nullable: false),
                    hash = table.Column<string>(type: "text", nullable: true),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    mimetype = table.Column<string>(type: "text", nullable: true),
                    dbid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_specialityimportfile", x => x.id);
                    table.ForeignKey(
                        name: "FK_specialityimportfile_specialityimport_id",
                        column: x => x.id,
                        principalTable: "specialityimport",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "specialityimporthistory",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    version = table.Column<int>(type: "integer", nullable: false),
                    rdpzsdimportid = table.Column<int>(type: "integer", nullable: false),
                    state = table.Column<int>(type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_specialityimporthistory", x => x.id);
                    table.ForeignKey(
                        name: "FK_specialityimporthistory_specialityimport_rdpzsdimportid",
                        column: x => x.rdpzsdimportid,
                        principalTable: "specialityimport",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "personimporthistoryerrorfile",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    key = table.Column<Guid>(type: "uuid", nullable: false),
                    hash = table.Column<string>(type: "text", nullable: true),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    mimetype = table.Column<string>(type: "text", nullable: true),
                    dbid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personimporthistoryerrorfile", x => x.id);
                    table.ForeignKey(
                        name: "FK_personimporthistoryerrorfile_personimporthistory_id",
                        column: x => x.id,
                        principalTable: "personimporthistory",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "personimporthistoryfile",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    key = table.Column<Guid>(type: "uuid", nullable: false),
                    hash = table.Column<string>(type: "text", nullable: true),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    mimetype = table.Column<string>(type: "text", nullable: true),
                    dbid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personimporthistoryfile", x => x.id);
                    table.ForeignKey(
                        name: "FK_personimporthistoryfile_personimporthistory_id",
                        column: x => x.id,
                        principalTable: "personimporthistory",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "passportcopy",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    key = table.Column<Guid>(type: "uuid", nullable: false),
                    hash = table.Column<string>(type: "text", nullable: true),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    mimetype = table.Column<string>(type: "text", nullable: true),
                    dbid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_passportcopy", x => x.id);
                    table.ForeignKey(
                        name: "FK_passportcopy_personbasic_id",
                        column: x => x.id,
                        principalTable: "personbasic",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "personbasichistory",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    partid = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    state = table.Column<int>(type: "integer", nullable: false),
                    firstname = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    middlename = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    lastname = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    othernames = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    fullname = table.Column<string>(type: "text", nullable: true),
                    firstnamealt = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    middlenamealt = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    lastnamealt = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    othernamesalt = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    fullnamealt = table.Column<string>(type: "text", nullable: true),
                    uin = table.Column<string>(type: "character(10)", fixedLength: true, maxLength: 10, nullable: true),
                    foreignernumber = table.Column<string>(type: "character(10)", fixedLength: true, maxLength: 10, nullable: true),
                    idnnumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    phonenumber = table.Column<string>(type: "character varying(18)", maxLength: 18, nullable: true),
                    postcode = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: true),
                    gender = table.Column<int>(type: "integer", nullable: false),
                    birthdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    birthcountryid = table.Column<int>(type: "integer", nullable: true),
                    birthdistrictid = table.Column<int>(type: "integer", nullable: true),
                    birthmunicipalityid = table.Column<int>(type: "integer", nullable: true),
                    birthsettlementid = table.Column<int>(type: "integer", nullable: true),
                    foreignerbirthsettlement = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    citizenshipid = table.Column<int>(type: "integer", nullable: false),
                    secondcitizenshipid = table.Column<int>(type: "integer", nullable: true),
                    residencecountryid = table.Column<int>(type: "integer", nullable: true),
                    residencedistrictid = table.Column<int>(type: "integer", nullable: true),
                    residencemunicipalityid = table.Column<int>(type: "integer", nullable: true),
                    residencesettlementid = table.Column<int>(type: "integer", nullable: true),
                    residenceaddress = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personbasichistory", x => x.id);
                    table.ForeignKey(
                        name: "FK_personbasichistory_country_birthcountryid",
                        column: x => x.birthcountryid,
                        principalTable: "country",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personbasichistory_country_citizenshipid",
                        column: x => x.citizenshipid,
                        principalTable: "country",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personbasichistory_country_residencecountryid",
                        column: x => x.residencecountryid,
                        principalTable: "country",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personbasichistory_country_secondcitizenshipid",
                        column: x => x.secondcitizenshipid,
                        principalTable: "country",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personbasichistory_district_birthdistrictid",
                        column: x => x.birthdistrictid,
                        principalTable: "district",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personbasichistory_district_residencedistrictid",
                        column: x => x.residencedistrictid,
                        principalTable: "district",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personbasichistory_municipality_birthmunicipalityid",
                        column: x => x.birthmunicipalityid,
                        principalTable: "municipality",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personbasichistory_municipality_residencemunicipalityid",
                        column: x => x.residencemunicipalityid,
                        principalTable: "municipality",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personbasichistory_personbasic_partid",
                        column: x => x.partid,
                        principalTable: "personbasic",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personbasichistory_settlement_birthsettlementid",
                        column: x => x.birthsettlementid,
                        principalTable: "settlement",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personbasichistory_settlement_residencesettlementid",
                        column: x => x.residencesettlementid,
                        principalTable: "settlement",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "personbasicinfo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    userid = table.Column<int>(type: "integer", nullable: false),
                    userfullname = table.Column<string>(type: "text", nullable: true),
                    actiondate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    institutionid = table.Column<int>(type: "integer", nullable: true),
                    subordinateid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personbasicinfo", x => x.id);
                    table.ForeignKey(
                        name: "FK_personbasicinfo_institution_institutionid",
                        column: x => x.institutionid,
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personbasicinfo_institution_subordinateid",
                        column: x => x.subordinateid,
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personbasicinfo_personbasic_id",
                        column: x => x.id,
                        principalTable: "personbasic",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "personimage",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    key = table.Column<Guid>(type: "uuid", nullable: false),
                    hash = table.Column<string>(type: "text", nullable: true),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    mimetype = table.Column<string>(type: "text", nullable: true),
                    dbid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personimage", x => x.id);
                    table.ForeignKey(
                        name: "FK_personimage_personbasic_id",
                        column: x => x.id,
                        principalTable: "personbasic",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "personsecondaryhistory",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    partid = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    state = table.Column<int>(type: "integer", nullable: false),
                    graduationyear = table.Column<int>(type: "integer", nullable: false),
                    countryid = table.Column<int>(type: "integer", nullable: false),
                    schoolid = table.Column<int>(type: "integer", nullable: true),
                    foreignschoolname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    profession = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    diplomanumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    diplomadate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    recognitionnumber = table.Column<string>(type: "text", nullable: true),
                    recognitiondate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    missingschoolfromregister = table.Column<bool>(type: "boolean", nullable: false),
                    missingschoolname = table.Column<string>(type: "text", nullable: true),
                    missingschoolsettlementid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personsecondaryhistory", x => x.id);
                    table.ForeignKey(
                        name: "FK_personsecondaryhistory_country_countryid",
                        column: x => x.countryid,
                        principalTable: "country",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personsecondaryhistory_personsecondary_partid",
                        column: x => x.partid,
                        principalTable: "personsecondary",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personsecondaryhistory_school_schoolid",
                        column: x => x.schoolid,
                        principalTable: "school",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personsecondaryhistory_settlement_missingschoolsettlementid",
                        column: x => x.missingschoolsettlementid,
                        principalTable: "settlement",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "personsecondaryinfo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    userid = table.Column<int>(type: "integer", nullable: false),
                    userfullname = table.Column<string>(type: "text", nullable: true),
                    actiondate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    institutionid = table.Column<int>(type: "integer", nullable: true),
                    subordinateid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personsecondaryinfo", x => x.id);
                    table.ForeignKey(
                        name: "FK_personsecondaryinfo_institution_institutionid",
                        column: x => x.institutionid,
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personsecondaryinfo_institution_subordinateid",
                        column: x => x.subordinateid,
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personsecondaryinfo_personsecondary_id",
                        column: x => x.id,
                        principalTable: "personsecondary",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "personsecondaryrecognitiondocument",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    key = table.Column<Guid>(type: "uuid", nullable: false),
                    hash = table.Column<string>(type: "text", nullable: true),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    mimetype = table.Column<string>(type: "text", nullable: true),
                    dbid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personsecondaryrecognitiondocument", x => x.id);
                    table.ForeignKey(
                        name: "FK_personsecondaryrecognitiondocument_personsecondary_id",
                        column: x => x.id,
                        principalTable: "personsecondary",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "persondoctoral",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    startdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    enddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    lotid = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    state = table.Column<int>(type: "integer", nullable: false),
                    institutionid = table.Column<int>(type: "integer", nullable: false),
                    subordinateid = table.Column<int>(type: "integer", nullable: true),
                    institutionspecialityid = table.Column<int>(type: "integer", nullable: false),
                    studentstatusid = table.Column<int>(type: "integer", nullable: false),
                    studenteventid = table.Column<int>(type: "integer", nullable: false),
                    admissionreasonid = table.Column<int>(type: "integer", nullable: false),
                    petype = table.Column<int>(type: "integer", nullable: false),
                    pehighschooltype = table.Column<int>(type: "integer", nullable: true),
                    pediplomanumber = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
                    pediplomadate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    peresearchareaid = table.Column<int>(type: "integer", nullable: true),
                    peeducationalqualificationid = table.Column<int>(type: "integer", nullable: true),
                    peacquiredforeigneducationalqualificationid = table.Column<int>(type: "integer", nullable: true),
                    pepartid = table.Column<int>(type: "integer", nullable: true),
                    peinstitutionid = table.Column<int>(type: "integer", nullable: true),
                    pesubordinateid = table.Column<int>(type: "integer", nullable: true),
                    peinstitutionspecialityid = table.Column<int>(type: "integer", nullable: true),
                    pespecialitymissinginregister = table.Column<bool>(type: "boolean", nullable: true),
                    peinstitutionname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    pesubordinatename = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    pespecialityname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    pecountryid = table.Column<int>(type: "integer", nullable: true),
                    perecognizedspeciality = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    peacquiredspeciality = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    perecognitionnumber = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    perecognitiondate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_persondoctoral", x => x.id);
                    table.ForeignKey(
                        name: "FK_persondoctoral_admissionreason_admissionreasonid",
                        column: x => x.admissionreasonid,
                        principalTable: "admissionreason",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_persondoctoral_country_pecountryid",
                        column: x => x.pecountryid,
                        principalTable: "country",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_persondoctoral_educationalqualification_peacquiredforeigned~",
                        column: x => x.peacquiredforeigneducationalqualificationid,
                        principalTable: "educationalqualification",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_persondoctoral_educationalqualification_peeducationalqualif~",
                        column: x => x.peeducationalqualificationid,
                        principalTable: "educationalqualification",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_persondoctoral_institution_institutionid",
                        column: x => x.institutionid,
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_persondoctoral_institution_peinstitutionid",
                        column: x => x.peinstitutionid,
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_persondoctoral_institution_pesubordinateid",
                        column: x => x.pesubordinateid,
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_persondoctoral_institution_subordinateid",
                        column: x => x.subordinateid,
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_persondoctoral_institutionspeciality_institutionspecialityid",
                        column: x => x.institutionspecialityid,
                        principalTable: "institutionspeciality",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_persondoctoral_institutionspeciality_peinstitutionspecialit~",
                        column: x => x.peinstitutionspecialityid,
                        principalTable: "institutionspeciality",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_persondoctoral_personlot_lotid",
                        column: x => x.lotid,
                        principalTable: "personlot",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_persondoctoral_personstudent_pepartid",
                        column: x => x.pepartid,
                        principalTable: "personstudent",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_persondoctoral_researcharea_peresearchareaid",
                        column: x => x.peresearchareaid,
                        principalTable: "researcharea",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_persondoctoral_studentevent_studenteventid",
                        column: x => x.studenteventid,
                        principalTable: "studentevent",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_persondoctoral_studentstatus_studentstatusid",
                        column: x => x.studentstatusid,
                        principalTable: "studentstatus",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "personstudentdiploma",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    diplomanumber = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    registrationdiplomanumber = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
                    diplomadate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    isvalid = table.Column<bool>(type: "boolean", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personstudentdiploma", x => x.id);
                    table.ForeignKey(
                        name: "FK_personstudentdiploma_personstudent_id",
                        column: x => x.id,
                        principalTable: "personstudent",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "personstudentduplicatediploma",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    partid = table.Column<int>(type: "integer", nullable: false),
                    duplicatestickerstate = table.Column<int>(type: "integer", nullable: false),
                    duplicatestickeryear = table.Column<int>(type: "integer", nullable: false),
                    duplicatediplomanumber = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
                    duplicateregistrationdiplomanumber = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
                    duplicatediplomadate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    isvalid = table.Column<bool>(type: "boolean", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personstudentduplicatediploma", x => x.id);
                    table.ForeignKey(
                        name: "FK_personstudentduplicatediploma_personstudent_partid",
                        column: x => x.partid,
                        principalTable: "personstudent",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "personstudenthistory",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    partid = table.Column<int>(type: "integer", nullable: false),
                    facultynumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    stickerstate = table.Column<int>(type: "integer", nullable: false),
                    stickeryear = table.Column<int>(type: "integer", nullable: true),
                    version = table.Column<int>(type: "integer", nullable: false),
                    state = table.Column<int>(type: "integer", nullable: false),
                    institutionid = table.Column<int>(type: "integer", nullable: false),
                    subordinateid = table.Column<int>(type: "integer", nullable: true),
                    institutionspecialityid = table.Column<int>(type: "integer", nullable: false),
                    studentstatusid = table.Column<int>(type: "integer", nullable: false),
                    studenteventid = table.Column<int>(type: "integer", nullable: false),
                    admissionreasonid = table.Column<int>(type: "integer", nullable: false),
                    petype = table.Column<int>(type: "integer", nullable: false),
                    pehighschooltype = table.Column<int>(type: "integer", nullable: true),
                    pediplomanumber = table.Column<string>(type: "text", nullable: true),
                    pediplomadate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    peresearchareaid = table.Column<int>(type: "integer", nullable: true),
                    peeducationalqualificationid = table.Column<int>(type: "integer", nullable: true),
                    peacquiredforeigneducationalqualificationid = table.Column<int>(type: "integer", nullable: true),
                    pepartid = table.Column<int>(type: "integer", nullable: true),
                    peinstitutionid = table.Column<int>(type: "integer", nullable: true),
                    pesubordinateid = table.Column<int>(type: "integer", nullable: true),
                    peinstitutionspecialityid = table.Column<int>(type: "integer", nullable: true),
                    pespecialitymissinginregister = table.Column<bool>(type: "boolean", nullable: true),
                    peinstitutionname = table.Column<string>(type: "text", nullable: true),
                    pesubordinatename = table.Column<string>(type: "text", nullable: true),
                    pespecialityname = table.Column<string>(type: "text", nullable: true),
                    pecountryid = table.Column<int>(type: "integer", nullable: true),
                    perecognizedspeciality = table.Column<string>(type: "text", nullable: true),
                    peacquiredspeciality = table.Column<string>(type: "text", nullable: true),
                    perecognitionnumber = table.Column<string>(type: "text", nullable: true),
                    perecognitiondate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personstudenthistory", x => x.id);
                    table.ForeignKey(
                        name: "FK_personstudenthistory_admissionreason_admissionreasonid",
                        column: x => x.admissionreasonid,
                        principalTable: "admissionreason",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personstudenthistory_country_pecountryid",
                        column: x => x.pecountryid,
                        principalTable: "country",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personstudenthistory_educationalqualification_peacquiredfor~",
                        column: x => x.peacquiredforeigneducationalqualificationid,
                        principalTable: "educationalqualification",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personstudenthistory_educationalqualification_peeducational~",
                        column: x => x.peeducationalqualificationid,
                        principalTable: "educationalqualification",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personstudenthistory_institution_institutionid",
                        column: x => x.institutionid,
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personstudenthistory_institution_peinstitutionid",
                        column: x => x.peinstitutionid,
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personstudenthistory_institution_pesubordinateid",
                        column: x => x.pesubordinateid,
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personstudenthistory_institution_subordinateid",
                        column: x => x.subordinateid,
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personstudenthistory_institutionspeciality_institutionspeci~",
                        column: x => x.institutionspecialityid,
                        principalTable: "institutionspeciality",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personstudenthistory_institutionspeciality_peinstitutionspe~",
                        column: x => x.peinstitutionspecialityid,
                        principalTable: "institutionspeciality",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personstudenthistory_personstudent_partid",
                        column: x => x.partid,
                        principalTable: "personstudent",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personstudenthistory_researcharea_peresearchareaid",
                        column: x => x.peresearchareaid,
                        principalTable: "researcharea",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personstudenthistory_studentevent_studenteventid",
                        column: x => x.studenteventid,
                        principalTable: "studentevent",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personstudenthistory_studentstatus_studentstatusid",
                        column: x => x.studentstatusid,
                        principalTable: "studentstatus",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "personstudentinfo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    userid = table.Column<int>(type: "integer", nullable: false),
                    userfullname = table.Column<string>(type: "text", nullable: true),
                    actiondate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    institutionid = table.Column<int>(type: "integer", nullable: true),
                    subordinateid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personstudentinfo", x => x.id);
                    table.ForeignKey(
                        name: "FK_personstudentinfo_institution_institutionid",
                        column: x => x.institutionid,
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personstudentinfo_institution_subordinateid",
                        column: x => x.subordinateid,
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personstudentinfo_personstudent_id",
                        column: x => x.id,
                        principalTable: "personstudent",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "personstudentperecognitiondocument",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    key = table.Column<Guid>(type: "uuid", nullable: false),
                    hash = table.Column<string>(type: "text", nullable: true),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    mimetype = table.Column<string>(type: "text", nullable: true),
                    dbid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personstudentperecognitiondocument", x => x.id);
                    table.ForeignKey(
                        name: "FK_personstudentperecognitiondocument_personstudent_id",
                        column: x => x.id,
                        principalTable: "personstudent",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "personstudentprotocol",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    partid = table.Column<int>(type: "integer", nullable: false),
                    protocoltype = table.Column<int>(type: "integer", nullable: false),
                    protocolnumber = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    protocoldate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personstudentprotocol", x => x.id);
                    table.ForeignKey(
                        name: "FK_personstudentprotocol_personstudent_partid",
                        column: x => x.partid,
                        principalTable: "personstudent",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "personstudentsemester",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    periodid = table.Column<int>(type: "integer", nullable: false),
                    course = table.Column<int>(type: "integer", nullable: false),
                    studentsemester = table.Column<int>(type: "integer", nullable: false),
                    secondfromtwoyearsplan = table.Column<bool>(type: "boolean", nullable: false),
                    semesterrelocatednumber = table.Column<string>(type: "text", nullable: true),
                    semesterrelocateddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    individualplancourse = table.Column<int>(type: "integer", nullable: true),
                    individualplansemester = table.Column<int>(type: "integer", nullable: true),
                    semesterinstitutionid = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    partid = table.Column<int>(type: "integer", nullable: false),
                    studentstatusid = table.Column<int>(type: "integer", nullable: false),
                    studenteventid = table.Column<int>(type: "integer", nullable: false),
                    educationfeetypeid = table.Column<int>(type: "integer", nullable: true),
                    relocatedfrompartid = table.Column<int>(type: "integer", nullable: true),
                    note = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    hasscholarship = table.Column<bool>(type: "boolean", nullable: false),
                    usehostel = table.Column<bool>(type: "boolean", nullable: false),
                    useholidaybase = table.Column<bool>(type: "boolean", nullable: false),
                    participatedintprograms = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personstudentsemester", x => x.id);
                    table.ForeignKey(
                        name: "FK_personstudentsemester_educationfeetype_educationfeetypeid",
                        column: x => x.educationfeetypeid,
                        principalTable: "educationfeetype",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personstudentsemester_institution_semesterinstitutionid",
                        column: x => x.semesterinstitutionid,
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personstudentsemester_period_periodid",
                        column: x => x.periodid,
                        principalTable: "period",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personstudentsemester_personstudent_partid",
                        column: x => x.partid,
                        principalTable: "personstudent",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personstudentsemester_personstudent_relocatedfrompartid",
                        column: x => x.relocatedfrompartid,
                        principalTable: "personstudent",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personstudentsemester_studentevent_studenteventid",
                        column: x => x.studenteventid,
                        principalTable: "studentevent",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personstudentsemester_studentstatus_studentstatusid",
                        column: x => x.studentstatusid,
                        principalTable: "studentstatus",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "personstudentstickernote",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    partid = table.Column<int>(type: "integer", nullable: false),
                    userid = table.Column<int>(type: "integer", nullable: false),
                    userfullname = table.Column<string>(type: "text", nullable: true),
                    actiondate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    institutionid = table.Column<int>(type: "integer", nullable: true),
                    subordinateid = table.Column<int>(type: "integer", nullable: true),
                    note = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personstudentstickernote", x => x.id);
                    table.ForeignKey(
                        name: "FK_personstudentstickernote_institution_institutionid",
                        column: x => x.institutionid,
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personstudentstickernote_institution_subordinateid",
                        column: x => x.subordinateid,
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personstudentstickernote_personstudent_partid",
                        column: x => x.partid,
                        principalTable: "personstudent",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "specialityimporthistoryerrorfile",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    key = table.Column<Guid>(type: "uuid", nullable: false),
                    hash = table.Column<string>(type: "text", nullable: true),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    mimetype = table.Column<string>(type: "text", nullable: true),
                    dbid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_specialityimporthistoryerrorfile", x => x.id);
                    table.ForeignKey(
                        name: "FK_specialityimporthistoryerrorfile_specialityimporthistory_id",
                        column: x => x.id,
                        principalTable: "specialityimporthistory",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "specialityimporthistoryfile",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    key = table.Column<Guid>(type: "uuid", nullable: false),
                    hash = table.Column<string>(type: "text", nullable: true),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    mimetype = table.Column<string>(type: "text", nullable: true),
                    dbid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_specialityimporthistoryfile", x => x.id);
                    table.ForeignKey(
                        name: "FK_specialityimporthistoryfile_specialityimporthistory_id",
                        column: x => x.id,
                        principalTable: "specialityimporthistory",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "passportcopyhistory",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    key = table.Column<Guid>(type: "uuid", nullable: false),
                    hash = table.Column<string>(type: "text", nullable: true),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    mimetype = table.Column<string>(type: "text", nullable: true),
                    dbid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_passportcopyhistory", x => x.id);
                    table.ForeignKey(
                        name: "FK_passportcopyhistory_personbasichistory_id",
                        column: x => x.id,
                        principalTable: "personbasichistory",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "personbasichistoryinfo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    userid = table.Column<int>(type: "integer", nullable: false),
                    userfullname = table.Column<string>(type: "text", nullable: true),
                    actiondate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    institutionid = table.Column<int>(type: "integer", nullable: true),
                    subordinateid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personbasichistoryinfo", x => x.id);
                    table.ForeignKey(
                        name: "FK_personbasichistoryinfo_institution_institutionid",
                        column: x => x.institutionid,
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personbasichistoryinfo_institution_subordinateid",
                        column: x => x.subordinateid,
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personbasichistoryinfo_personbasichistory_id",
                        column: x => x.id,
                        principalTable: "personbasichistory",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "personimagehistory",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    key = table.Column<Guid>(type: "uuid", nullable: false),
                    hash = table.Column<string>(type: "text", nullable: true),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    mimetype = table.Column<string>(type: "text", nullable: true),
                    dbid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personimagehistory", x => x.id);
                    table.ForeignKey(
                        name: "FK_personimagehistory_personbasichistory_id",
                        column: x => x.id,
                        principalTable: "personbasichistory",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "personsecondaryhistoryinfo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    userid = table.Column<int>(type: "integer", nullable: false),
                    userfullname = table.Column<string>(type: "text", nullable: true),
                    actiondate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    institutionid = table.Column<int>(type: "integer", nullable: true),
                    subordinateid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personsecondaryhistoryinfo", x => x.id);
                    table.ForeignKey(
                        name: "FK_personsecondaryhistoryinfo_institution_institutionid",
                        column: x => x.institutionid,
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personsecondaryhistoryinfo_institution_subordinateid",
                        column: x => x.subordinateid,
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personsecondaryhistoryinfo_personsecondaryhistory_id",
                        column: x => x.id,
                        principalTable: "personsecondaryhistory",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "personsecondaryrecognitiondocumenthistory",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    key = table.Column<Guid>(type: "uuid", nullable: false),
                    hash = table.Column<string>(type: "text", nullable: true),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    mimetype = table.Column<string>(type: "text", nullable: true),
                    dbid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personsecondaryrecognitiondocumenthistory", x => x.id);
                    table.ForeignKey(
                        name: "FK_personsecondaryrecognitiondocumenthistory_personsecondaryhi~",
                        column: x => x.id,
                        principalTable: "personsecondaryhistory",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "persondoctoralhistory",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    startdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    enddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    partid = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    state = table.Column<int>(type: "integer", nullable: false),
                    institutionid = table.Column<int>(type: "integer", nullable: false),
                    subordinateid = table.Column<int>(type: "integer", nullable: true),
                    institutionspecialityid = table.Column<int>(type: "integer", nullable: false),
                    studentstatusid = table.Column<int>(type: "integer", nullable: false),
                    studenteventid = table.Column<int>(type: "integer", nullable: false),
                    admissionreasonid = table.Column<int>(type: "integer", nullable: false),
                    petype = table.Column<int>(type: "integer", nullable: false),
                    pehighschooltype = table.Column<int>(type: "integer", nullable: true),
                    pediplomanumber = table.Column<string>(type: "text", nullable: true),
                    pediplomadate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    peresearchareaid = table.Column<int>(type: "integer", nullable: true),
                    peeducationalqualificationid = table.Column<int>(type: "integer", nullable: true),
                    peacquiredforeigneducationalqualificationid = table.Column<int>(type: "integer", nullable: true),
                    pepartid = table.Column<int>(type: "integer", nullable: true),
                    peinstitutionid = table.Column<int>(type: "integer", nullable: true),
                    pesubordinateid = table.Column<int>(type: "integer", nullable: true),
                    peinstitutionspecialityid = table.Column<int>(type: "integer", nullable: true),
                    pespecialitymissinginregister = table.Column<bool>(type: "boolean", nullable: true),
                    peinstitutionname = table.Column<string>(type: "text", nullable: true),
                    pesubordinatename = table.Column<string>(type: "text", nullable: true),
                    pespecialityname = table.Column<string>(type: "text", nullable: true),
                    pecountryid = table.Column<int>(type: "integer", nullable: true),
                    perecognizedspeciality = table.Column<string>(type: "text", nullable: true),
                    peacquiredspeciality = table.Column<string>(type: "text", nullable: true),
                    perecognitionnumber = table.Column<string>(type: "text", nullable: true),
                    perecognitiondate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_persondoctoralhistory", x => x.id);
                    table.ForeignKey(
                        name: "FK_persondoctoralhistory_admissionreason_admissionreasonid",
                        column: x => x.admissionreasonid,
                        principalTable: "admissionreason",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_persondoctoralhistory_country_pecountryid",
                        column: x => x.pecountryid,
                        principalTable: "country",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_persondoctoralhistory_educationalqualification_peacquiredfo~",
                        column: x => x.peacquiredforeigneducationalqualificationid,
                        principalTable: "educationalqualification",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_persondoctoralhistory_educationalqualification_peeducationa~",
                        column: x => x.peeducationalqualificationid,
                        principalTable: "educationalqualification",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_persondoctoralhistory_institution_institutionid",
                        column: x => x.institutionid,
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_persondoctoralhistory_institution_peinstitutionid",
                        column: x => x.peinstitutionid,
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_persondoctoralhistory_institution_pesubordinateid",
                        column: x => x.pesubordinateid,
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_persondoctoralhistory_institution_subordinateid",
                        column: x => x.subordinateid,
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_persondoctoralhistory_institutionspeciality_institutionspec~",
                        column: x => x.institutionspecialityid,
                        principalTable: "institutionspeciality",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_persondoctoralhistory_institutionspeciality_peinstitutionsp~",
                        column: x => x.peinstitutionspecialityid,
                        principalTable: "institutionspeciality",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_persondoctoralhistory_persondoctoral_partid",
                        column: x => x.partid,
                        principalTable: "persondoctoral",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_persondoctoralhistory_researcharea_peresearchareaid",
                        column: x => x.peresearchareaid,
                        principalTable: "researcharea",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_persondoctoralhistory_studentevent_studenteventid",
                        column: x => x.studenteventid,
                        principalTable: "studentevent",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_persondoctoralhistory_studentstatus_studentstatusid",
                        column: x => x.studentstatusid,
                        principalTable: "studentstatus",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "persondoctoralinfo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    userid = table.Column<int>(type: "integer", nullable: false),
                    userfullname = table.Column<string>(type: "text", nullable: true),
                    actiondate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    institutionid = table.Column<int>(type: "integer", nullable: true),
                    subordinateid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_persondoctoralinfo", x => x.id);
                    table.ForeignKey(
                        name: "FK_persondoctoralinfo_institution_institutionid",
                        column: x => x.institutionid,
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_persondoctoralinfo_institution_subordinateid",
                        column: x => x.subordinateid,
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_persondoctoralinfo_persondoctoral_id",
                        column: x => x.id,
                        principalTable: "persondoctoral",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "persondoctoralperecognitiondocument",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    key = table.Column<Guid>(type: "uuid", nullable: false),
                    hash = table.Column<string>(type: "text", nullable: true),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    mimetype = table.Column<string>(type: "text", nullable: true),
                    dbid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_persondoctoralperecognitiondocument", x => x.id);
                    table.ForeignKey(
                        name: "FK_persondoctoralperecognitiondocument_persondoctoral_id",
                        column: x => x.id,
                        principalTable: "persondoctoral",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "persondoctoralsemester",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    protocoldate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    protocolnumber = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    yeartype = table.Column<int>(type: "integer", nullable: false),
                    attestationtype = table.Column<int>(type: "integer", nullable: true),
                    semesterrelocatednumber = table.Column<string>(type: "text", nullable: true),
                    semesterrelocateddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    partid = table.Column<int>(type: "integer", nullable: false),
                    studentstatusid = table.Column<int>(type: "integer", nullable: false),
                    studenteventid = table.Column<int>(type: "integer", nullable: false),
                    educationfeetypeid = table.Column<int>(type: "integer", nullable: true),
                    relocatedfrompartid = table.Column<int>(type: "integer", nullable: true),
                    note = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    hasscholarship = table.Column<bool>(type: "boolean", nullable: false),
                    usehostel = table.Column<bool>(type: "boolean", nullable: false),
                    useholidaybase = table.Column<bool>(type: "boolean", nullable: false),
                    participatedintprograms = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_persondoctoralsemester", x => x.id);
                    table.ForeignKey(
                        name: "FK_persondoctoralsemester_educationfeetype_educationfeetypeid",
                        column: x => x.educationfeetypeid,
                        principalTable: "educationfeetype",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_persondoctoralsemester_persondoctoral_partid",
                        column: x => x.partid,
                        principalTable: "persondoctoral",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_persondoctoralsemester_persondoctoral_relocatedfrompartid",
                        column: x => x.relocatedfrompartid,
                        principalTable: "persondoctoral",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_persondoctoralsemester_studentevent_studenteventid",
                        column: x => x.studenteventid,
                        principalTable: "studentevent",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_persondoctoralsemester_studentstatus_studentstatusid",
                        column: x => x.studentstatusid,
                        principalTable: "studentstatus",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "personstudentdiplomafile",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    key = table.Column<Guid>(type: "uuid", nullable: false),
                    hash = table.Column<string>(type: "text", nullable: true),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    mimetype = table.Column<string>(type: "text", nullable: true),
                    dbid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personstudentdiplomafile", x => x.id);
                    table.ForeignKey(
                        name: "FK_personstudentdiplomafile_personstudentdiploma_id",
                        column: x => x.id,
                        principalTable: "personstudentdiploma",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "personstudentduplicatediplomafile",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    key = table.Column<Guid>(type: "uuid", nullable: false),
                    hash = table.Column<string>(type: "text", nullable: true),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    mimetype = table.Column<string>(type: "text", nullable: true),
                    dbid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personstudentduplicatediplomafile", x => x.id);
                    table.ForeignKey(
                        name: "FK_personstudentduplicatediplomafile_personstudentduplicatedip~",
                        column: x => x.id,
                        principalTable: "personstudentduplicatediploma",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "personstudentdiplomahistory",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    diplomanumber = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
                    registrationdiplomanumber = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
                    diplomadate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    isvalid = table.Column<bool>(type: "boolean", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personstudentdiplomahistory", x => x.id);
                    table.ForeignKey(
                        name: "FK_personstudentdiplomahistory_personstudenthistory_id",
                        column: x => x.id,
                        principalTable: "personstudenthistory",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "personstudentduplicatediplomahistory",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    partid = table.Column<int>(type: "integer", nullable: false),
                    duplicatestickerstate = table.Column<int>(type: "integer", nullable: false),
                    duplicatestickeryear = table.Column<int>(type: "integer", nullable: false),
                    duplicatediplomanumber = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
                    duplicateregistrationdiplomanumber = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
                    duplicatediplomadate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    isvalid = table.Column<bool>(type: "boolean", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personstudentduplicatediplomahistory", x => x.id);
                    table.ForeignKey(
                        name: "FK_personstudentduplicatediplomahistory_personstudenthistory_p~",
                        column: x => x.partid,
                        principalTable: "personstudenthistory",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "personstudenthistoryinfo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    userid = table.Column<int>(type: "integer", nullable: false),
                    userfullname = table.Column<string>(type: "text", nullable: true),
                    actiondate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    institutionid = table.Column<int>(type: "integer", nullable: true),
                    subordinateid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personstudenthistoryinfo", x => x.id);
                    table.ForeignKey(
                        name: "FK_personstudenthistoryinfo_institution_institutionid",
                        column: x => x.institutionid,
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personstudenthistoryinfo_institution_subordinateid",
                        column: x => x.subordinateid,
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personstudenthistoryinfo_personstudenthistory_id",
                        column: x => x.id,
                        principalTable: "personstudenthistory",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "personstudentperecognitiondocumenthistory",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    key = table.Column<Guid>(type: "uuid", nullable: false),
                    hash = table.Column<string>(type: "text", nullable: true),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    mimetype = table.Column<string>(type: "text", nullable: true),
                    dbid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personstudentperecognitiondocumenthistory", x => x.id);
                    table.ForeignKey(
                        name: "FK_personstudentperecognitiondocumenthistory_personstudenthist~",
                        column: x => x.id,
                        principalTable: "personstudenthistory",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "personstudentprotocolhistory",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    partid = table.Column<int>(type: "integer", nullable: false),
                    protocoltype = table.Column<int>(type: "integer", nullable: false),
                    protocolnumber = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
                    protocoldate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personstudentprotocolhistory", x => x.id);
                    table.ForeignKey(
                        name: "FK_personstudentprotocolhistory_personstudenthistory_partid",
                        column: x => x.partid,
                        principalTable: "personstudenthistory",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "personstudentsemesterhistory",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    periodid = table.Column<int>(type: "integer", nullable: false),
                    course = table.Column<int>(type: "integer", nullable: false),
                    studentsemester = table.Column<int>(type: "integer", nullable: false),
                    secondfromtwoyearsplan = table.Column<bool>(type: "boolean", nullable: false),
                    semesterrelocatednumber = table.Column<string>(type: "text", nullable: true),
                    semesterrelocateddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    individualplancourse = table.Column<int>(type: "integer", nullable: true),
                    individualplansemester = table.Column<int>(type: "integer", nullable: true),
                    semesterinstitutionid = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    partid = table.Column<int>(type: "integer", nullable: false),
                    studentstatusid = table.Column<int>(type: "integer", nullable: false),
                    studenteventid = table.Column<int>(type: "integer", nullable: false),
                    educationfeetypeid = table.Column<int>(type: "integer", nullable: true),
                    relocatedfrompartid = table.Column<int>(type: "integer", nullable: true),
                    note = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    hasscholarship = table.Column<bool>(type: "boolean", nullable: false),
                    usehostel = table.Column<bool>(type: "boolean", nullable: false),
                    useholidaybase = table.Column<bool>(type: "boolean", nullable: false),
                    participatedintprograms = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personstudentsemesterhistory", x => x.id);
                    table.ForeignKey(
                        name: "FK_personstudentsemesterhistory_educationfeetype_educationfeet~",
                        column: x => x.educationfeetypeid,
                        principalTable: "educationfeetype",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personstudentsemesterhistory_institution_semesterinstitutio~",
                        column: x => x.semesterinstitutionid,
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personstudentsemesterhistory_period_periodid",
                        column: x => x.periodid,
                        principalTable: "period",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personstudentsemesterhistory_personstudent_relocatedfrompar~",
                        column: x => x.relocatedfrompartid,
                        principalTable: "personstudent",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personstudentsemesterhistory_personstudenthistory_partid",
                        column: x => x.partid,
                        principalTable: "personstudenthistory",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personstudentsemesterhistory_studentevent_studenteventid",
                        column: x => x.studenteventid,
                        principalTable: "studentevent",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personstudentsemesterhistory_studentstatus_studentstatusid",
                        column: x => x.studentstatusid,
                        principalTable: "studentstatus",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "personstudentsemesterrelocatedfile",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    key = table.Column<Guid>(type: "uuid", nullable: false),
                    hash = table.Column<string>(type: "text", nullable: true),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    mimetype = table.Column<string>(type: "text", nullable: true),
                    dbid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personstudentsemesterrelocatedfile", x => x.id);
                    table.ForeignKey(
                        name: "FK_personstudentsemesterrelocatedfile_personstudentsemester_id",
                        column: x => x.id,
                        principalTable: "personstudentsemester",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "persondoctoralhistoryinfo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    userid = table.Column<int>(type: "integer", nullable: false),
                    userfullname = table.Column<string>(type: "text", nullable: true),
                    actiondate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    institutionid = table.Column<int>(type: "integer", nullable: true),
                    subordinateid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_persondoctoralhistoryinfo", x => x.id);
                    table.ForeignKey(
                        name: "FK_persondoctoralhistoryinfo_institution_institutionid",
                        column: x => x.institutionid,
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_persondoctoralhistoryinfo_institution_subordinateid",
                        column: x => x.subordinateid,
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_persondoctoralhistoryinfo_persondoctoralhistory_id",
                        column: x => x.id,
                        principalTable: "persondoctoralhistory",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "persondoctoralperecognitiondocumenthistory",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    key = table.Column<Guid>(type: "uuid", nullable: false),
                    hash = table.Column<string>(type: "text", nullable: true),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    mimetype = table.Column<string>(type: "text", nullable: true),
                    dbid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_persondoctoralperecognitiondocumenthistory", x => x.id);
                    table.ForeignKey(
                        name: "FK_persondoctoralperecognitiondocumenthistory_persondoctoralhi~",
                        column: x => x.id,
                        principalTable: "persondoctoralhistory",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "persondoctoralsemesterhistory",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    protocoldate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    protocolnumber = table.Column<string>(type: "text", nullable: true),
                    yeartype = table.Column<int>(type: "integer", nullable: false),
                    attestationtype = table.Column<int>(type: "integer", nullable: true),
                    semesterrelocatednumber = table.Column<string>(type: "text", nullable: true),
                    semesterrelocateddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    partid = table.Column<int>(type: "integer", nullable: false),
                    studentstatusid = table.Column<int>(type: "integer", nullable: false),
                    studenteventid = table.Column<int>(type: "integer", nullable: false),
                    educationfeetypeid = table.Column<int>(type: "integer", nullable: true),
                    relocatedfrompartid = table.Column<int>(type: "integer", nullable: true),
                    note = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    hasscholarship = table.Column<bool>(type: "boolean", nullable: false),
                    usehostel = table.Column<bool>(type: "boolean", nullable: false),
                    useholidaybase = table.Column<bool>(type: "boolean", nullable: false),
                    participatedintprograms = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_persondoctoralsemesterhistory", x => x.id);
                    table.ForeignKey(
                        name: "FK_persondoctoralsemesterhistory_educationfeetype_educationfee~",
                        column: x => x.educationfeetypeid,
                        principalTable: "educationfeetype",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_persondoctoralsemesterhistory_persondoctoral_relocatedfromp~",
                        column: x => x.relocatedfrompartid,
                        principalTable: "persondoctoral",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_persondoctoralsemesterhistory_persondoctoralhistory_partid",
                        column: x => x.partid,
                        principalTable: "persondoctoralhistory",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_persondoctoralsemesterhistory_studentevent_studenteventid",
                        column: x => x.studenteventid,
                        principalTable: "studentevent",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_persondoctoralsemesterhistory_studentstatus_studentstatusid",
                        column: x => x.studentstatusid,
                        principalTable: "studentstatus",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "persondoctoralsemesterrelocatedfile",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    key = table.Column<Guid>(type: "uuid", nullable: false),
                    hash = table.Column<string>(type: "text", nullable: true),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    mimetype = table.Column<string>(type: "text", nullable: true),
                    dbid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_persondoctoralsemesterrelocatedfile", x => x.id);
                    table.ForeignKey(
                        name: "FK_persondoctoralsemesterrelocatedfile_persondoctoralsemester_~",
                        column: x => x.id,
                        principalTable: "persondoctoralsemester",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "personstudentdiplomafilehistory",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    key = table.Column<Guid>(type: "uuid", nullable: false),
                    hash = table.Column<string>(type: "text", nullable: true),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    mimetype = table.Column<string>(type: "text", nullable: true),
                    dbid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personstudentdiplomafilehistory", x => x.id);
                    table.ForeignKey(
                        name: "FK_personstudentdiplomafilehistory_personstudentdiplomahistory~",
                        column: x => x.id,
                        principalTable: "personstudentdiplomahistory",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "personstudentduplicatediplomafilehistory",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    key = table.Column<Guid>(type: "uuid", nullable: false),
                    hash = table.Column<string>(type: "text", nullable: true),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    mimetype = table.Column<string>(type: "text", nullable: true),
                    dbid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personstudentduplicatediplomafilehistory", x => x.id);
                    table.ForeignKey(
                        name: "FK_personstudentduplicatediplomafilehistory_personstudentdupli~",
                        column: x => x.id,
                        principalTable: "personstudentduplicatediplomahistory",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "personstudentsemesterrelocatedfilehistory",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    key = table.Column<Guid>(type: "uuid", nullable: false),
                    hash = table.Column<string>(type: "text", nullable: true),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    mimetype = table.Column<string>(type: "text", nullable: true),
                    dbid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personstudentsemesterrelocatedfilehistory", x => x.id);
                    table.ForeignKey(
                        name: "FK_personstudentsemesterrelocatedfilehistory_personstudentseme~",
                        column: x => x.id,
                        principalTable: "personstudentsemesterhistory",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "persondoctoralsemesterrelocatedfilehistory",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    key = table.Column<Guid>(type: "uuid", nullable: false),
                    hash = table.Column<string>(type: "text", nullable: true),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    mimetype = table.Column<string>(type: "text", nullable: true),
                    dbid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_persondoctoralsemesterrelocatedfilehistory", x => x.id);
                    table.ForeignKey(
                        name: "FK_persondoctoralsemesterrelocatedfilehistory_persondoctoralse~",
                        column: x => x.id,
                        principalTable: "persondoctoralsemesterhistory",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_admissionreasoncitizenship_admissionreasonid",
                table: "admissionreasoncitizenship",
                column: "admissionreasonid");

            migrationBuilder.CreateIndex(
                name: "IX_admissionreasoncitizenship_countryid",
                table: "admissionreasoncitizenship",
                column: "countryid");

            migrationBuilder.CreateIndex(
                name: "IX_admissionreasoneducationfee_admissionreasonid",
                table: "admissionreasoneducationfee",
                column: "admissionreasonid");

            migrationBuilder.CreateIndex(
                name: "IX_admissionreasoneducationfee_educationfeetypeid",
                table: "admissionreasoneducationfee",
                column: "educationfeetypeid");

            migrationBuilder.CreateIndex(
                name: "IX_admissionreasoneducationfeehistory_admissionreasonhistoryid",
                table: "admissionreasoneducationfeehistory",
                column: "admissionreasonhistoryid");

            migrationBuilder.CreateIndex(
                name: "IX_admissionreasonhistory_admissionreasonid",
                table: "admissionreasonhistory",
                column: "admissionreasonid");

            migrationBuilder.CreateIndex(
                name: "IX_institution_districtid",
                table: "institution",
                column: "districtid");

            migrationBuilder.CreateIndex(
                name: "IX_institution_municipalityid",
                table: "institution",
                column: "municipalityid");

            migrationBuilder.CreateIndex(
                name: "IX_institution_parentid",
                table: "institution",
                column: "parentid");

            migrationBuilder.CreateIndex(
                name: "IX_institution_rootid",
                table: "institution",
                column: "rootid");

            migrationBuilder.CreateIndex(
                name: "IX_institution_settlementid",
                table: "institution",
                column: "settlementid");

            migrationBuilder.CreateIndex(
                name: "IX_institutionspeciality_educationalformid",
                table: "institutionspeciality",
                column: "educationalformid");

            migrationBuilder.CreateIndex(
                name: "IX_institutionspeciality_institutionid",
                table: "institutionspeciality",
                column: "institutionid");

            migrationBuilder.CreateIndex(
                name: "IX_institutionspeciality_nationalstatisticalinstituteid",
                table: "institutionspeciality",
                column: "nationalstatisticalinstituteid");

            migrationBuilder.CreateIndex(
                name: "IX_institutionspeciality_nsiregionid",
                table: "institutionspeciality",
                column: "nsiregionid");

            migrationBuilder.CreateIndex(
                name: "IX_institutionspeciality_specialityid",
                table: "institutionspeciality",
                column: "specialityid");

            migrationBuilder.CreateIndex(
                name: "IX_institutionspecialityjointspeciality_institutionbyparentid",
                table: "institutionspecialityjointspeciality",
                column: "institutionbyparentid");

            migrationBuilder.CreateIndex(
                name: "IX_institutionspecialityjointspeciality_institutionid",
                table: "institutionspecialityjointspeciality",
                column: "institutionid");

            migrationBuilder.CreateIndex(
                name: "IX_institutionspecialityjointspeciality_institutionspecialityid",
                table: "institutionspecialityjointspeciality",
                column: "institutionspecialityid");

            migrationBuilder.CreateIndex(
                name: "IX_institutionspecialitylanguage_institutionspecialityid",
                table: "institutionspecialitylanguage",
                column: "institutionspecialityid");

            migrationBuilder.CreateIndex(
                name: "IX_institutionspecialitylanguage_languageid",
                table: "institutionspecialitylanguage",
                column: "languageid");

            migrationBuilder.CreateIndex(
                name: "IX_municipality_districtid",
                table: "municipality",
                column: "districtid");

            migrationBuilder.CreateIndex(
                name: "IX_personbasic_birthcountryid",
                table: "personbasic",
                column: "birthcountryid");

            migrationBuilder.CreateIndex(
                name: "IX_personbasic_birthdistrictid",
                table: "personbasic",
                column: "birthdistrictid");

            migrationBuilder.CreateIndex(
                name: "IX_personbasic_birthmunicipalityid",
                table: "personbasic",
                column: "birthmunicipalityid");

            migrationBuilder.CreateIndex(
                name: "IX_personbasic_birthsettlementid",
                table: "personbasic",
                column: "birthsettlementid");

            migrationBuilder.CreateIndex(
                name: "IX_personbasic_citizenshipid",
                table: "personbasic",
                column: "citizenshipid");

            migrationBuilder.CreateIndex(
                name: "IX_personbasic_email",
                table: "personbasic",
                column: "email",
                unique: true,
                filter: "(email) NOT ILIKE 'NoEmail'");

            migrationBuilder.CreateIndex(
                name: "IX_personbasic_foreignernumber",
                table: "personbasic",
                column: "foreignernumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_personbasic_residencecountryid",
                table: "personbasic",
                column: "residencecountryid");

            migrationBuilder.CreateIndex(
                name: "IX_personbasic_residencedistrictid",
                table: "personbasic",
                column: "residencedistrictid");

            migrationBuilder.CreateIndex(
                name: "IX_personbasic_residencemunicipalityid",
                table: "personbasic",
                column: "residencemunicipalityid");

            migrationBuilder.CreateIndex(
                name: "IX_personbasic_residencesettlementid",
                table: "personbasic",
                column: "residencesettlementid");

            migrationBuilder.CreateIndex(
                name: "IX_personbasic_secondcitizenshipid",
                table: "personbasic",
                column: "secondcitizenshipid");

            migrationBuilder.CreateIndex(
                name: "IX_personbasic_uin",
                table: "personbasic",
                column: "uin",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_personbasichistory_birthcountryid",
                table: "personbasichistory",
                column: "birthcountryid");

            migrationBuilder.CreateIndex(
                name: "IX_personbasichistory_birthdistrictid",
                table: "personbasichistory",
                column: "birthdistrictid");

            migrationBuilder.CreateIndex(
                name: "IX_personbasichistory_birthmunicipalityid",
                table: "personbasichistory",
                column: "birthmunicipalityid");

            migrationBuilder.CreateIndex(
                name: "IX_personbasichistory_birthsettlementid",
                table: "personbasichistory",
                column: "birthsettlementid");

            migrationBuilder.CreateIndex(
                name: "IX_personbasichistory_citizenshipid",
                table: "personbasichistory",
                column: "citizenshipid");

            migrationBuilder.CreateIndex(
                name: "IX_personbasichistory_partid",
                table: "personbasichistory",
                column: "partid");

            migrationBuilder.CreateIndex(
                name: "IX_personbasichistory_residencecountryid",
                table: "personbasichistory",
                column: "residencecountryid");

            migrationBuilder.CreateIndex(
                name: "IX_personbasichistory_residencedistrictid",
                table: "personbasichistory",
                column: "residencedistrictid");

            migrationBuilder.CreateIndex(
                name: "IX_personbasichistory_residencemunicipalityid",
                table: "personbasichistory",
                column: "residencemunicipalityid");

            migrationBuilder.CreateIndex(
                name: "IX_personbasichistory_residencesettlementid",
                table: "personbasichistory",
                column: "residencesettlementid");

            migrationBuilder.CreateIndex(
                name: "IX_personbasichistory_secondcitizenshipid",
                table: "personbasichistory",
                column: "secondcitizenshipid");

            migrationBuilder.CreateIndex(
                name: "IX_personbasichistoryinfo_institutionid",
                table: "personbasichistoryinfo",
                column: "institutionid");

            migrationBuilder.CreateIndex(
                name: "IX_personbasichistoryinfo_subordinateid",
                table: "personbasichistoryinfo",
                column: "subordinateid");

            migrationBuilder.CreateIndex(
                name: "IX_personbasicinfo_institutionid",
                table: "personbasicinfo",
                column: "institutionid");

            migrationBuilder.CreateIndex(
                name: "IX_personbasicinfo_subordinateid",
                table: "personbasicinfo",
                column: "subordinateid");

            migrationBuilder.CreateIndex(
                name: "IX_persondiplomacopy_lotid",
                table: "persondiplomacopy",
                column: "lotid");

            migrationBuilder.CreateIndex(
                name: "IX_persondoctoral_admissionreasonid",
                table: "persondoctoral",
                column: "admissionreasonid");

            migrationBuilder.CreateIndex(
                name: "IX_persondoctoral_institutionid",
                table: "persondoctoral",
                column: "institutionid");

            migrationBuilder.CreateIndex(
                name: "IX_persondoctoral_institutionspecialityid",
                table: "persondoctoral",
                column: "institutionspecialityid");

            migrationBuilder.CreateIndex(
                name: "IX_persondoctoral_lotid",
                table: "persondoctoral",
                column: "lotid");

            migrationBuilder.CreateIndex(
                name: "IX_persondoctoral_peacquiredforeigneducationalqualificationid",
                table: "persondoctoral",
                column: "peacquiredforeigneducationalqualificationid");

            migrationBuilder.CreateIndex(
                name: "IX_persondoctoral_pecountryid",
                table: "persondoctoral",
                column: "pecountryid");

            migrationBuilder.CreateIndex(
                name: "IX_persondoctoral_peeducationalqualificationid",
                table: "persondoctoral",
                column: "peeducationalqualificationid");

            migrationBuilder.CreateIndex(
                name: "IX_persondoctoral_peinstitutionid",
                table: "persondoctoral",
                column: "peinstitutionid");

            migrationBuilder.CreateIndex(
                name: "IX_persondoctoral_peinstitutionspecialityid",
                table: "persondoctoral",
                column: "peinstitutionspecialityid");

            migrationBuilder.CreateIndex(
                name: "IX_persondoctoral_pepartid",
                table: "persondoctoral",
                column: "pepartid");

            migrationBuilder.CreateIndex(
                name: "IX_persondoctoral_peresearchareaid",
                table: "persondoctoral",
                column: "peresearchareaid");

            migrationBuilder.CreateIndex(
                name: "IX_persondoctoral_pesubordinateid",
                table: "persondoctoral",
                column: "pesubordinateid");

            migrationBuilder.CreateIndex(
                name: "IX_persondoctoral_studenteventid",
                table: "persondoctoral",
                column: "studenteventid");

            migrationBuilder.CreateIndex(
                name: "IX_persondoctoral_studentstatusid",
                table: "persondoctoral",
                column: "studentstatusid");

            migrationBuilder.CreateIndex(
                name: "IX_persondoctoral_subordinateid",
                table: "persondoctoral",
                column: "subordinateid");

            migrationBuilder.CreateIndex(
                name: "IX_persondoctoralhistory_admissionreasonid",
                table: "persondoctoralhistory",
                column: "admissionreasonid");

            migrationBuilder.CreateIndex(
                name: "IX_persondoctoralhistory_institutionid",
                table: "persondoctoralhistory",
                column: "institutionid");

            migrationBuilder.CreateIndex(
                name: "IX_persondoctoralhistory_institutionspecialityid",
                table: "persondoctoralhistory",
                column: "institutionspecialityid");

            migrationBuilder.CreateIndex(
                name: "IX_persondoctoralhistory_partid",
                table: "persondoctoralhistory",
                column: "partid");

            migrationBuilder.CreateIndex(
                name: "IX_persondoctoralhistory_peacquiredforeigneducationalqualifica~",
                table: "persondoctoralhistory",
                column: "peacquiredforeigneducationalqualificationid");

            migrationBuilder.CreateIndex(
                name: "IX_persondoctoralhistory_pecountryid",
                table: "persondoctoralhistory",
                column: "pecountryid");

            migrationBuilder.CreateIndex(
                name: "IX_persondoctoralhistory_peeducationalqualificationid",
                table: "persondoctoralhistory",
                column: "peeducationalqualificationid");

            migrationBuilder.CreateIndex(
                name: "IX_persondoctoralhistory_peinstitutionid",
                table: "persondoctoralhistory",
                column: "peinstitutionid");

            migrationBuilder.CreateIndex(
                name: "IX_persondoctoralhistory_peinstitutionspecialityid",
                table: "persondoctoralhistory",
                column: "peinstitutionspecialityid");

            migrationBuilder.CreateIndex(
                name: "IX_persondoctoralhistory_peresearchareaid",
                table: "persondoctoralhistory",
                column: "peresearchareaid");

            migrationBuilder.CreateIndex(
                name: "IX_persondoctoralhistory_pesubordinateid",
                table: "persondoctoralhistory",
                column: "pesubordinateid");

            migrationBuilder.CreateIndex(
                name: "IX_persondoctoralhistory_studenteventid",
                table: "persondoctoralhistory",
                column: "studenteventid");

            migrationBuilder.CreateIndex(
                name: "IX_persondoctoralhistory_studentstatusid",
                table: "persondoctoralhistory",
                column: "studentstatusid");

            migrationBuilder.CreateIndex(
                name: "IX_persondoctoralhistory_subordinateid",
                table: "persondoctoralhistory",
                column: "subordinateid");

            migrationBuilder.CreateIndex(
                name: "IX_persondoctoralhistoryinfo_institutionid",
                table: "persondoctoralhistoryinfo",
                column: "institutionid");

            migrationBuilder.CreateIndex(
                name: "IX_persondoctoralhistoryinfo_subordinateid",
                table: "persondoctoralhistoryinfo",
                column: "subordinateid");

            migrationBuilder.CreateIndex(
                name: "IX_persondoctoralinfo_institutionid",
                table: "persondoctoralinfo",
                column: "institutionid");

            migrationBuilder.CreateIndex(
                name: "IX_persondoctoralinfo_subordinateid",
                table: "persondoctoralinfo",
                column: "subordinateid");

            migrationBuilder.CreateIndex(
                name: "IX_persondoctoralsemester_educationfeetypeid",
                table: "persondoctoralsemester",
                column: "educationfeetypeid");

            migrationBuilder.CreateIndex(
                name: "IX_persondoctoralsemester_partid",
                table: "persondoctoralsemester",
                column: "partid");

            migrationBuilder.CreateIndex(
                name: "IX_persondoctoralsemester_relocatedfrompartid",
                table: "persondoctoralsemester",
                column: "relocatedfrompartid");

            migrationBuilder.CreateIndex(
                name: "IX_persondoctoralsemester_studenteventid",
                table: "persondoctoralsemester",
                column: "studenteventid");

            migrationBuilder.CreateIndex(
                name: "IX_persondoctoralsemester_studentstatusid",
                table: "persondoctoralsemester",
                column: "studentstatusid");

            migrationBuilder.CreateIndex(
                name: "IX_persondoctoralsemesterhistory_educationfeetypeid",
                table: "persondoctoralsemesterhistory",
                column: "educationfeetypeid");

            migrationBuilder.CreateIndex(
                name: "IX_persondoctoralsemesterhistory_partid",
                table: "persondoctoralsemesterhistory",
                column: "partid");

            migrationBuilder.CreateIndex(
                name: "IX_persondoctoralsemesterhistory_relocatedfrompartid",
                table: "persondoctoralsemesterhistory",
                column: "relocatedfrompartid");

            migrationBuilder.CreateIndex(
                name: "IX_persondoctoralsemesterhistory_studenteventid",
                table: "persondoctoralsemesterhistory",
                column: "studenteventid");

            migrationBuilder.CreateIndex(
                name: "IX_persondoctoralsemesterhistory_studentstatusid",
                table: "persondoctoralsemesterhistory",
                column: "studentstatusid");

            migrationBuilder.CreateIndex(
                name: "IX_personimport_institutionid",
                table: "personimport",
                column: "institutionid");

            migrationBuilder.CreateIndex(
                name: "IX_personimport_subordinateid",
                table: "personimport",
                column: "subordinateid");

            migrationBuilder.CreateIndex(
                name: "IX_personimporthistory_rdpzsdimportid",
                table: "personimporthistory",
                column: "rdpzsdimportid");

            migrationBuilder.CreateIndex(
                name: "IX_personimportuan_personimportid",
                table: "personimportuan",
                column: "personimportid");

            migrationBuilder.CreateIndex(
                name: "IX_personlot_createinstitutionid",
                table: "personlot",
                column: "createinstitutionid");

            migrationBuilder.CreateIndex(
                name: "IX_personlot_createsubordinateid",
                table: "personlot",
                column: "createsubordinateid");

            migrationBuilder.CreateIndex(
                name: "IX_personlot_uan",
                table: "personlot",
                column: "uan",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_personlotaction_institutionid",
                table: "personlotaction",
                column: "institutionid");

            migrationBuilder.CreateIndex(
                name: "IX_personlotaction_lotid",
                table: "personlotaction",
                column: "lotid");

            migrationBuilder.CreateIndex(
                name: "IX_personlotaction_subordinateid",
                table: "personlotaction",
                column: "subordinateid");

            migrationBuilder.CreateIndex(
                name: "IX_personlotidnumber_personlotid",
                table: "personlotidnumber",
                column: "personlotid");

            migrationBuilder.CreateIndex(
                name: "IX_personsecondary_countryid",
                table: "personsecondary",
                column: "countryid");

            migrationBuilder.CreateIndex(
                name: "IX_personsecondary_missingschoolsettlementid",
                table: "personsecondary",
                column: "missingschoolsettlementid");

            migrationBuilder.CreateIndex(
                name: "IX_personsecondary_schoolid",
                table: "personsecondary",
                column: "schoolid");

            migrationBuilder.CreateIndex(
                name: "IX_personsecondaryhistory_countryid",
                table: "personsecondaryhistory",
                column: "countryid");

            migrationBuilder.CreateIndex(
                name: "IX_personsecondaryhistory_missingschoolsettlementid",
                table: "personsecondaryhistory",
                column: "missingschoolsettlementid");

            migrationBuilder.CreateIndex(
                name: "IX_personsecondaryhistory_partid",
                table: "personsecondaryhistory",
                column: "partid");

            migrationBuilder.CreateIndex(
                name: "IX_personsecondaryhistory_schoolid",
                table: "personsecondaryhistory",
                column: "schoolid");

            migrationBuilder.CreateIndex(
                name: "IX_personsecondaryhistoryinfo_institutionid",
                table: "personsecondaryhistoryinfo",
                column: "institutionid");

            migrationBuilder.CreateIndex(
                name: "IX_personsecondaryhistoryinfo_subordinateid",
                table: "personsecondaryhistoryinfo",
                column: "subordinateid");

            migrationBuilder.CreateIndex(
                name: "IX_personsecondaryinfo_institutionid",
                table: "personsecondaryinfo",
                column: "institutionid");

            migrationBuilder.CreateIndex(
                name: "IX_personsecondaryinfo_subordinateid",
                table: "personsecondaryinfo",
                column: "subordinateid");

            migrationBuilder.CreateIndex(
                name: "IX_personstudent_admissionreasonid",
                table: "personstudent",
                column: "admissionreasonid");

            migrationBuilder.CreateIndex(
                name: "IX_personstudent_institutionid",
                table: "personstudent",
                column: "institutionid");

            migrationBuilder.CreateIndex(
                name: "IX_personstudent_institutionspecialityid",
                table: "personstudent",
                column: "institutionspecialityid");

            migrationBuilder.CreateIndex(
                name: "IX_personstudent_lotid",
                table: "personstudent",
                column: "lotid");

            migrationBuilder.CreateIndex(
                name: "IX_personstudent_peacquiredforeigneducationalqualificationid",
                table: "personstudent",
                column: "peacquiredforeigneducationalqualificationid");

            migrationBuilder.CreateIndex(
                name: "IX_personstudent_pecountryid",
                table: "personstudent",
                column: "pecountryid");

            migrationBuilder.CreateIndex(
                name: "IX_personstudent_peeducationalqualificationid",
                table: "personstudent",
                column: "peeducationalqualificationid");

            migrationBuilder.CreateIndex(
                name: "IX_personstudent_peinstitutionid",
                table: "personstudent",
                column: "peinstitutionid");

            migrationBuilder.CreateIndex(
                name: "IX_personstudent_peinstitutionspecialityid",
                table: "personstudent",
                column: "peinstitutionspecialityid");

            migrationBuilder.CreateIndex(
                name: "IX_personstudent_pepartid",
                table: "personstudent",
                column: "pepartid");

            migrationBuilder.CreateIndex(
                name: "IX_personstudent_peresearchareaid",
                table: "personstudent",
                column: "peresearchareaid");

            migrationBuilder.CreateIndex(
                name: "IX_personstudent_pesubordinateid",
                table: "personstudent",
                column: "pesubordinateid");

            migrationBuilder.CreateIndex(
                name: "IX_personstudent_studenteventid",
                table: "personstudent",
                column: "studenteventid");

            migrationBuilder.CreateIndex(
                name: "IX_personstudent_studentstatusid",
                table: "personstudent",
                column: "studentstatusid");

            migrationBuilder.CreateIndex(
                name: "IX_personstudent_subordinateid",
                table: "personstudent",
                column: "subordinateid");

            migrationBuilder.CreateIndex(
                name: "IX_personstudentduplicatediploma_partid",
                table: "personstudentduplicatediploma",
                column: "partid");

            migrationBuilder.CreateIndex(
                name: "IX_personstudentduplicatediplomahistory_partid",
                table: "personstudentduplicatediplomahistory",
                column: "partid");

            migrationBuilder.CreateIndex(
                name: "IX_personstudenthistory_admissionreasonid",
                table: "personstudenthistory",
                column: "admissionreasonid");

            migrationBuilder.CreateIndex(
                name: "IX_personstudenthistory_institutionid",
                table: "personstudenthistory",
                column: "institutionid");

            migrationBuilder.CreateIndex(
                name: "IX_personstudenthistory_institutionspecialityid",
                table: "personstudenthistory",
                column: "institutionspecialityid");

            migrationBuilder.CreateIndex(
                name: "IX_personstudenthistory_partid",
                table: "personstudenthistory",
                column: "partid");

            migrationBuilder.CreateIndex(
                name: "IX_personstudenthistory_peacquiredforeigneducationalqualificat~",
                table: "personstudenthistory",
                column: "peacquiredforeigneducationalqualificationid");

            migrationBuilder.CreateIndex(
                name: "IX_personstudenthistory_pecountryid",
                table: "personstudenthistory",
                column: "pecountryid");

            migrationBuilder.CreateIndex(
                name: "IX_personstudenthistory_peeducationalqualificationid",
                table: "personstudenthistory",
                column: "peeducationalqualificationid");

            migrationBuilder.CreateIndex(
                name: "IX_personstudenthistory_peinstitutionid",
                table: "personstudenthistory",
                column: "peinstitutionid");

            migrationBuilder.CreateIndex(
                name: "IX_personstudenthistory_peinstitutionspecialityid",
                table: "personstudenthistory",
                column: "peinstitutionspecialityid");

            migrationBuilder.CreateIndex(
                name: "IX_personstudenthistory_peresearchareaid",
                table: "personstudenthistory",
                column: "peresearchareaid");

            migrationBuilder.CreateIndex(
                name: "IX_personstudenthistory_pesubordinateid",
                table: "personstudenthistory",
                column: "pesubordinateid");

            migrationBuilder.CreateIndex(
                name: "IX_personstudenthistory_studenteventid",
                table: "personstudenthistory",
                column: "studenteventid");

            migrationBuilder.CreateIndex(
                name: "IX_personstudenthistory_studentstatusid",
                table: "personstudenthistory",
                column: "studentstatusid");

            migrationBuilder.CreateIndex(
                name: "IX_personstudenthistory_subordinateid",
                table: "personstudenthistory",
                column: "subordinateid");

            migrationBuilder.CreateIndex(
                name: "IX_personstudenthistoryinfo_institutionid",
                table: "personstudenthistoryinfo",
                column: "institutionid");

            migrationBuilder.CreateIndex(
                name: "IX_personstudenthistoryinfo_subordinateid",
                table: "personstudenthistoryinfo",
                column: "subordinateid");

            migrationBuilder.CreateIndex(
                name: "IX_personstudentinfo_institutionid",
                table: "personstudentinfo",
                column: "institutionid");

            migrationBuilder.CreateIndex(
                name: "IX_personstudentinfo_subordinateid",
                table: "personstudentinfo",
                column: "subordinateid");

            migrationBuilder.CreateIndex(
                name: "IX_personstudentprotocol_partid",
                table: "personstudentprotocol",
                column: "partid");

            migrationBuilder.CreateIndex(
                name: "IX_personstudentprotocolhistory_partid",
                table: "personstudentprotocolhistory",
                column: "partid");

            migrationBuilder.CreateIndex(
                name: "IX_personstudentsemester_educationfeetypeid",
                table: "personstudentsemester",
                column: "educationfeetypeid");

            migrationBuilder.CreateIndex(
                name: "IX_personstudentsemester_partid",
                table: "personstudentsemester",
                column: "partid");

            migrationBuilder.CreateIndex(
                name: "IX_personstudentsemester_periodid",
                table: "personstudentsemester",
                column: "periodid");

            migrationBuilder.CreateIndex(
                name: "IX_personstudentsemester_relocatedfrompartid",
                table: "personstudentsemester",
                column: "relocatedfrompartid");

            migrationBuilder.CreateIndex(
                name: "IX_personstudentsemester_semesterinstitutionid",
                table: "personstudentsemester",
                column: "semesterinstitutionid");

            migrationBuilder.CreateIndex(
                name: "IX_personstudentsemester_studenteventid",
                table: "personstudentsemester",
                column: "studenteventid");

            migrationBuilder.CreateIndex(
                name: "IX_personstudentsemester_studentstatusid",
                table: "personstudentsemester",
                column: "studentstatusid");

            migrationBuilder.CreateIndex(
                name: "IX_personstudentsemesterhistory_educationfeetypeid",
                table: "personstudentsemesterhistory",
                column: "educationfeetypeid");

            migrationBuilder.CreateIndex(
                name: "IX_personstudentsemesterhistory_partid",
                table: "personstudentsemesterhistory",
                column: "partid");

            migrationBuilder.CreateIndex(
                name: "IX_personstudentsemesterhistory_periodid",
                table: "personstudentsemesterhistory",
                column: "periodid");

            migrationBuilder.CreateIndex(
                name: "IX_personstudentsemesterhistory_relocatedfrompartid",
                table: "personstudentsemesterhistory",
                column: "relocatedfrompartid");

            migrationBuilder.CreateIndex(
                name: "IX_personstudentsemesterhistory_semesterinstitutionid",
                table: "personstudentsemesterhistory",
                column: "semesterinstitutionid");

            migrationBuilder.CreateIndex(
                name: "IX_personstudentsemesterhistory_studenteventid",
                table: "personstudentsemesterhistory",
                column: "studenteventid");

            migrationBuilder.CreateIndex(
                name: "IX_personstudentsemesterhistory_studentstatusid",
                table: "personstudentsemesterhistory",
                column: "studentstatusid");

            migrationBuilder.CreateIndex(
                name: "IX_personstudentstickernote_institutionid",
                table: "personstudentstickernote",
                column: "institutionid");

            migrationBuilder.CreateIndex(
                name: "IX_personstudentstickernote_partid",
                table: "personstudentstickernote",
                column: "partid");

            migrationBuilder.CreateIndex(
                name: "IX_personstudentstickernote_subordinateid",
                table: "personstudentstickernote",
                column: "subordinateid");

            migrationBuilder.CreateIndex(
                name: "IX_school_districtid",
                table: "school",
                column: "districtid");

            migrationBuilder.CreateIndex(
                name: "IX_school_municipalityid",
                table: "school",
                column: "municipalityid");

            migrationBuilder.CreateIndex(
                name: "IX_school_parentid",
                table: "school",
                column: "parentid");

            migrationBuilder.CreateIndex(
                name: "IX_school_settlementid",
                table: "school",
                column: "settlementid");

            migrationBuilder.CreateIndex(
                name: "IX_settlement_districtid",
                table: "settlement",
                column: "districtid");

            migrationBuilder.CreateIndex(
                name: "IX_settlement_municipalityid",
                table: "settlement",
                column: "municipalityid");

            migrationBuilder.CreateIndex(
                name: "IX_speciality_educationalqualificationid",
                table: "speciality",
                column: "educationalqualificationid");

            migrationBuilder.CreateIndex(
                name: "IX_speciality_researchareaid",
                table: "speciality",
                column: "researchareaid");

            migrationBuilder.CreateIndex(
                name: "IX_specialityimport_institutionid",
                table: "specialityimport",
                column: "institutionid");

            migrationBuilder.CreateIndex(
                name: "IX_specialityimport_subordinateid",
                table: "specialityimport",
                column: "subordinateid");

            migrationBuilder.CreateIndex(
                name: "IX_specialityimporthistory_rdpzsdimportid",
                table: "specialityimporthistory",
                column: "rdpzsdimportid");

            migrationBuilder.CreateIndex(
                name: "IX_studentevent_studentstatusid",
                table: "studentevent",
                column: "studentstatusid");

            migrationBuilder.CreateIndex(
                name: "IX_studenteventqualification_educationalqualificationid",
                table: "studenteventqualification",
                column: "educationalqualificationid");

            migrationBuilder.CreateIndex(
                name: "IX_studenteventqualification_studenteventid",
                table: "studenteventqualification",
                column: "studenteventid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "admissionreasoncitizenship");

            migrationBuilder.DropTable(
                name: "admissionreasoneducationfee");

            migrationBuilder.DropTable(
                name: "admissionreasoneducationfeehistory");

            migrationBuilder.DropTable(
                name: "institutionspecialityjointspeciality");

            migrationBuilder.DropTable(
                name: "institutionspecialitylanguage");

            migrationBuilder.DropTable(
                name: "passportcopy");

            migrationBuilder.DropTable(
                name: "passportcopyhistory");

            migrationBuilder.DropTable(
                name: "personbasichistoryinfo");

            migrationBuilder.DropTable(
                name: "personbasicinfo");

            migrationBuilder.DropTable(
                name: "persondiplomacopy");

            migrationBuilder.DropTable(
                name: "persondoctoralhistoryinfo");

            migrationBuilder.DropTable(
                name: "persondoctoralinfo");

            migrationBuilder.DropTable(
                name: "persondoctoralperecognitiondocument");

            migrationBuilder.DropTable(
                name: "persondoctoralperecognitiondocumenthistory");

            migrationBuilder.DropTable(
                name: "persondoctoralsemesterrelocatedfile");

            migrationBuilder.DropTable(
                name: "persondoctoralsemesterrelocatedfilehistory");

            migrationBuilder.DropTable(
                name: "personimage");

            migrationBuilder.DropTable(
                name: "personimagehistory");

            migrationBuilder.DropTable(
                name: "personimporterrorfile");

            migrationBuilder.DropTable(
                name: "personimportfile");

            migrationBuilder.DropTable(
                name: "personimporthistoryerrorfile");

            migrationBuilder.DropTable(
                name: "personimporthistoryfile");

            migrationBuilder.DropTable(
                name: "personimportuan");

            migrationBuilder.DropTable(
                name: "personlotaction");

            migrationBuilder.DropTable(
                name: "personlotidnumber");

            migrationBuilder.DropTable(
                name: "personsecondaryhistoryinfo");

            migrationBuilder.DropTable(
                name: "personsecondaryinfo");

            migrationBuilder.DropTable(
                name: "personsecondaryrecognitiondocument");

            migrationBuilder.DropTable(
                name: "personsecondaryrecognitiondocumenthistory");

            migrationBuilder.DropTable(
                name: "personstudentdiplomafile");

            migrationBuilder.DropTable(
                name: "personstudentdiplomafilehistory");

            migrationBuilder.DropTable(
                name: "personstudentduplicatediplomafile");

            migrationBuilder.DropTable(
                name: "personstudentduplicatediplomafilehistory");

            migrationBuilder.DropTable(
                name: "personstudenthistoryinfo");

            migrationBuilder.DropTable(
                name: "personstudentinfo");

            migrationBuilder.DropTable(
                name: "personstudentperecognitiondocument");

            migrationBuilder.DropTable(
                name: "personstudentperecognitiondocumenthistory");

            migrationBuilder.DropTable(
                name: "personstudentprotocol");

            migrationBuilder.DropTable(
                name: "personstudentprotocolhistory");

            migrationBuilder.DropTable(
                name: "personstudentsemesterrelocatedfile");

            migrationBuilder.DropTable(
                name: "personstudentsemesterrelocatedfilehistory");

            migrationBuilder.DropTable(
                name: "personstudentstickernote");

            migrationBuilder.DropTable(
                name: "schemaversions");

            migrationBuilder.DropTable(
                name: "specialityimporterrorfile");

            migrationBuilder.DropTable(
                name: "specialityimportfile");

            migrationBuilder.DropTable(
                name: "specialityimporthistoryerrorfile");

            migrationBuilder.DropTable(
                name: "specialityimporthistoryfile");

            migrationBuilder.DropTable(
                name: "studenteventqualification");

            migrationBuilder.DropTable(
                name: "admissionreasonhistory");

            migrationBuilder.DropTable(
                name: "language");

            migrationBuilder.DropTable(
                name: "persondoctoralsemester");

            migrationBuilder.DropTable(
                name: "persondoctoralsemesterhistory");

            migrationBuilder.DropTable(
                name: "personbasichistory");

            migrationBuilder.DropTable(
                name: "personimporthistory");

            migrationBuilder.DropTable(
                name: "personsecondaryhistory");

            migrationBuilder.DropTable(
                name: "personstudentdiploma");

            migrationBuilder.DropTable(
                name: "personstudentdiplomahistory");

            migrationBuilder.DropTable(
                name: "personstudentduplicatediploma");

            migrationBuilder.DropTable(
                name: "personstudentduplicatediplomahistory");

            migrationBuilder.DropTable(
                name: "personstudentsemester");

            migrationBuilder.DropTable(
                name: "personstudentsemesterhistory");

            migrationBuilder.DropTable(
                name: "specialityimporthistory");

            migrationBuilder.DropTable(
                name: "persondoctoralhistory");

            migrationBuilder.DropTable(
                name: "personbasic");

            migrationBuilder.DropTable(
                name: "personimport");

            migrationBuilder.DropTable(
                name: "personsecondary");

            migrationBuilder.DropTable(
                name: "educationfeetype");

            migrationBuilder.DropTable(
                name: "period");

            migrationBuilder.DropTable(
                name: "personstudenthistory");

            migrationBuilder.DropTable(
                name: "specialityimport");

            migrationBuilder.DropTable(
                name: "persondoctoral");

            migrationBuilder.DropTable(
                name: "school");

            migrationBuilder.DropTable(
                name: "personstudent");

            migrationBuilder.DropTable(
                name: "admissionreason");

            migrationBuilder.DropTable(
                name: "country");

            migrationBuilder.DropTable(
                name: "institutionspeciality");

            migrationBuilder.DropTable(
                name: "personlot");

            migrationBuilder.DropTable(
                name: "studentevent");

            migrationBuilder.DropTable(
                name: "educationalform");

            migrationBuilder.DropTable(
                name: "nationalstatisticalinstitute");

            migrationBuilder.DropTable(
                name: "speciality");

            migrationBuilder.DropTable(
                name: "institution");

            migrationBuilder.DropTable(
                name: "studentstatus");

            migrationBuilder.DropTable(
                name: "educationalqualification");

            migrationBuilder.DropTable(
                name: "researcharea");

            migrationBuilder.DropTable(
                name: "settlement");

            migrationBuilder.DropTable(
                name: "municipality");

            migrationBuilder.DropTable(
                name: "district");
        }
    }
}
