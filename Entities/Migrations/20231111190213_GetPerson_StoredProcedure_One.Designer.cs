﻿// <auto-generated />
using System;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Entities.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20231111190213_GetPerson_StoredProcedure_One")]
    partial class GetPerson_StoredProcedure_One
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.24")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Entities.Country", b =>
                {
                    b.Property<Guid>("CountryID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CountryName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CountryID");

                    b.ToTable("Countries", (string)null);

                    b.HasData(
                        new
                        {
                            CountryID = new Guid("2b712f00-0bbf-4cca-80b3-0f8ebd03253d"),
                            CountryName = "USA"
                        });
                });

            modelBuilder.Entity("Entities.Person", b =>
                {
                    b.Property<Guid>("PersonID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<Guid?>("CountryID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Geneder")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("PersonName")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<bool>("ReceivedNewsLetter")
                        .HasColumnType("bit");

                    b.HasKey("PersonID");

                    b.ToTable("Persons", (string)null);

                    b.HasData(
                        new
                        {
                            PersonID = new Guid("e4cf4f6a-4813-4444-976e-b8e36ef591db"),
                            Address = "street address 4",
                            CountryID = new Guid("2b712f00-0bbf-4cca-80b3-0f8ebd03253d"),
                            DateOfBirth = new DateTime(1993, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "sandesh@gmail.com",
                            PersonName = "sandesh",
                            ReceivedNewsLetter = false
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
