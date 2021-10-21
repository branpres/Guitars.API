﻿// <auto-generated />
using System;
using Application.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Application.Migrations
{
    [DbContext(typeof(GuitarsContext))]
    partial class GuitarsContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "6.0.0-preview.3.21201.2");

            modelBuilder.Entity("Domain.Models.Guitar", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime");

                    b.Property<int>("GuitarType")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Make")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<int>("MaxNumberOfStrings")
                        .HasColumnType("int");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.ToTable("Guitar");
                });

            modelBuilder.Entity("Domain.Models.GuitarString", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime");

                    b.Property<string>("Gauge")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("varchar(5)");

                    b.Property<int?>("GuitarId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("Number")
                        .HasMaxLength(2)
                        .HasColumnType("int");

                    b.Property<string>("Tuning")
                        .IsRequired()
                        .HasMaxLength(2)
                        .HasColumnType("varchar(2)");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.HasIndex("GuitarId");

                    b.ToTable("GuitarString");
                });

            modelBuilder.Entity("Domain.Models.GuitarString", b =>
                {
                    b.HasOne("Domain.Models.Guitar", null)
                        .WithMany("GuitarStrings")
                        .HasForeignKey("GuitarId");
                });

            modelBuilder.Entity("Domain.Models.Guitar", b =>
                {
                    b.Navigation("GuitarStrings");
                });
#pragma warning restore 612, 618
        }
    }
}
