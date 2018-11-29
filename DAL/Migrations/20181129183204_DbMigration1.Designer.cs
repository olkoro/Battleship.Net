﻿// <auto-generated />
using System;
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DAL.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20181129183204_DbMigration1")]
    partial class DbMigration1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.0-preview3-35497");

            modelBuilder.Entity("DAL.GameBoard", b =>
                {
                    b.Property<int>("GameBoardId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("cols");

                    b.Property<int>("rows");

                    b.HasKey("GameBoardId");

                    b.ToTable("GameBoards");
                });

            modelBuilder.Entity("DAL.GameboardSquare", b =>
                {
                    b.Property<int>("GameboardSquareId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("GameBoardId");

                    b.Property<string>("Value");

                    b.Property<int>("x")
                        .HasColumnType("int");

                    b.Property<int>("y")
                        .HasColumnType("int");

                    b.HasKey("GameboardSquareId");

                    b.HasIndex("GameBoardId");

                    b.ToTable("GameboardSquares");
                });

            modelBuilder.Entity("DAL.Player", b =>
                {
                    b.Property<int>("PlayerId")
                        .ValueGeneratedOnAdd();

                    b.Property<short>("AI")
                        .HasColumnType("bit");

                    b.Property<string>("Name");

                    b.HasKey("PlayerId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("DAL.Rules", b =>
                {
                    b.Property<int>("RulesId")
                        .ValueGeneratedOnAdd();

                    b.Property<short>("CanTouch")
                        .HasColumnType("bit");

                    b.Property<int>("Columns")
                        .HasColumnType("int");

                    b.Property<int>("Rows")
                        .HasColumnType("int");

                    b.HasKey("RulesId");

                    b.ToTable("Ruleses");
                });

            modelBuilder.Entity("DAL.Save", b =>
                {
                    b.Property<int>("SaveId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("LastStateId");

                    b.Property<int>("Player1Id");

                    b.Property<int>("Player2Id");

                    b.Property<int>("RulesId");

                    b.Property<string>("TimeStamp");

                    b.HasKey("SaveId");

                    b.HasIndex("LastStateId");

                    b.HasIndex("Player1Id");

                    b.HasIndex("Player2Id");

                    b.HasIndex("RulesId");

                    b.ToTable("Saves");
                });

            modelBuilder.Entity("DAL.Ship", b =>
                {
                    b.Property<int>("ShipId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("GameBoardId");

                    b.Property<int>("Health")
                        .HasColumnType("int");

                    b.Property<int>("Length")
                        .HasColumnType("int");

                    b.Property<string>("Name");

                    b.HasKey("ShipId");

                    b.HasIndex("GameBoardId");

                    b.ToTable("Ships");
                });

            modelBuilder.Entity("DAL.ShipsLocation", b =>
                {
                    b.Property<int>("ShipsLocationId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ShipId");

                    b.Property<int>("x")
                        .HasColumnType("int");

                    b.Property<int>("y")
                        .HasColumnType("int");

                    b.HasKey("ShipsLocationId");

                    b.HasIndex("ShipId");

                    b.ToTable("ShipsLocations");
                });

            modelBuilder.Entity("DAL.State", b =>
                {
                    b.Property<int>("StateId")
                        .ValueGeneratedOnAdd();

                    b.Property<short>("P2Turn")
                        .HasColumnType("bit");

                    b.Property<int>("Player1GBId");

                    b.Property<int>("Player1MapId");

                    b.Property<int>("Player2GBId");

                    b.Property<int>("Player2MapId");

                    b.Property<int?>("SaveId");

                    b.Property<string>("TimeStamp");

                    b.HasKey("StateId");

                    b.HasIndex("Player1GBId");

                    b.HasIndex("Player1MapId");

                    b.HasIndex("Player2GBId");

                    b.HasIndex("Player2MapId");

                    b.HasIndex("SaveId");

                    b.ToTable("States");
                });

            modelBuilder.Entity("DAL.Test", b =>
                {
                    b.Property<int>("TestId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("HitOrMiss");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<string>("value");

                    b.HasKey("TestId");

                    b.ToTable("Tests");
                });

            modelBuilder.Entity("DAL.GameboardSquare", b =>
                {
                    b.HasOne("DAL.GameBoard", "GameBoard")
                        .WithMany("Squares")
                        .HasForeignKey("GameBoardId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DAL.Save", b =>
                {
                    b.HasOne("DAL.State", "LastState")
                        .WithMany()
                        .HasForeignKey("LastStateId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DAL.Player", "Player1")
                        .WithMany()
                        .HasForeignKey("Player1Id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DAL.Player", "Player2")
                        .WithMany()
                        .HasForeignKey("Player2Id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DAL.Rules", "Rules")
                        .WithMany()
                        .HasForeignKey("RulesId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DAL.Ship", b =>
                {
                    b.HasOne("DAL.GameBoard", "GameBoard")
                        .WithMany("Ships")
                        .HasForeignKey("GameBoardId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DAL.ShipsLocation", b =>
                {
                    b.HasOne("DAL.Ship", "Ship")
                        .WithMany("ShipsLocations")
                        .HasForeignKey("ShipId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DAL.State", b =>
                {
                    b.HasOne("DAL.GameBoard", "Player1GB")
                        .WithMany()
                        .HasForeignKey("Player1GBId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DAL.GameBoard", "Player1Map")
                        .WithMany()
                        .HasForeignKey("Player1MapId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DAL.GameBoard", "Player2GB")
                        .WithMany()
                        .HasForeignKey("Player2GBId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DAL.GameBoard", "Player2Map")
                        .WithMany()
                        .HasForeignKey("Player2MapId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DAL.Save")
                        .WithMany("States")
                        .HasForeignKey("SaveId");
                });
#pragma warning restore 612, 618
        }
    }
}