﻿// <auto-generated />
using System;
using GarageDoorsWeb;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GarageDoorsWeb.Migrations
{
    [DbContext(typeof(GarageDoorsContext))]
    [Migration("20240521112419_CreatedBy")]
    partial class CreatedBy
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.25")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("GarageDoorsWeb.Models.Door", b =>
                {
                    b.Property<int>("DoorID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DoorID"), 1L, 1);

                    b.Property<string>("DoorName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsLocked")
                        .HasColumnType("bit");

                    b.Property<bool>("IsOpen")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("datetime2");

                    b.HasKey("DoorID");

                    b.ToTable("Doors");
                });

            modelBuilder.Entity("GarageDoorsWeb.Models.Logs", b =>
                {
                    b.Property<int>("LogID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LogID"), 1L, 1);

                    b.Property<string>("Action")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("DoorID")
                        .HasColumnType("int");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("LogID");

                    b.HasIndex("DoorID");

                    b.HasIndex("UserID");

                    b.ToTable("Logs");
                });

            modelBuilder.Entity("GarageDoorsWeb.Models.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserID"), 1L, 1);

                    b.Property<int?>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("isAdmin")
                        .HasColumnType("bit");

                    b.Property<bool>("isBlocked")
                        .HasColumnType("bit");

                    b.Property<bool>("isOnline")
                        .HasColumnType("bit");

                    b.Property<bool>("isOwner")
                        .HasColumnType("bit");

                    b.HasKey("UserID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("GarageDoorsWeb.Models.UserDoor", b =>
                {
                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.Property<int>("DoorID")
                        .HasColumnType("int");

                    b.HasKey("UserID", "DoorID");

                    b.HasIndex("DoorID");

                    b.ToTable("UserDoors");
                });

            modelBuilder.Entity("GarageDoorsWeb.Models.Logs", b =>
                {
                    b.HasOne("GarageDoorsWeb.Models.Door", "Door")
                        .WithMany("Logs")
                        .HasForeignKey("DoorID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GarageDoorsWeb.Models.User", "User")
                        .WithMany("Logs")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Door");

                    b.Navigation("User");
                });

            modelBuilder.Entity("GarageDoorsWeb.Models.UserDoor", b =>
                {
                    b.HasOne("GarageDoorsWeb.Models.Door", "Door")
                        .WithMany("UserDoors")
                        .HasForeignKey("DoorID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GarageDoorsWeb.Models.User", "User")
                        .WithMany("UserDoors")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Door");

                    b.Navigation("User");
                });

            modelBuilder.Entity("GarageDoorsWeb.Models.Door", b =>
                {
                    b.Navigation("Logs");

                    b.Navigation("UserDoors");
                });

            modelBuilder.Entity("GarageDoorsWeb.Models.User", b =>
                {
                    b.Navigation("Logs");

                    b.Navigation("UserDoors");
                });
#pragma warning restore 612, 618
        }
    }
}