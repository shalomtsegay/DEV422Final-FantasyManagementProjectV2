﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TeamManagementServiceV2.Data;

#nullable disable

namespace TeamManagementServiceV2.Migrations
{
    [DbContext(typeof(TeamContext))]
    partial class TeamContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TeamManagementServiceV2.Model.Team", b =>
                {
                    b.Property<int>("TeamId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TeamId"));

                    b.PrimitiveCollection<string>("PlayerIds")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TeamName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TeamId");

                    b.ToTable("Teams");

                    b.HasData(
                        new
                        {
                            TeamId = 1,
                            PlayerIds = "[]",
                            TeamName = "Jaguars"
                        },
                        new
                        {
                            TeamId = 2,
                            PlayerIds = "[]",
                            TeamName = "Tigers"
                        },
                        new
                        {
                            TeamId = 3,
                            PlayerIds = "[]",
                            TeamName = "Panthers"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
