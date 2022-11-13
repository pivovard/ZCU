﻿// <auto-generated />
using System;
using Bank.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Bank.Migrations
{
    [DbContext(typeof(BankContext))]
    [Migration("20190107231107_uzmetovaznesere")]
    partial class uzmetovaznesere
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.0-rtm-35687")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Bank.Models.Payment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Amount");

                    b.Property<int?>("Constant");

                    b.Property<DateTime>("Date");

                    b.Property<long>("DestAccount");

                    b.Property<long?>("DestAccountPrefix");

                    b.Property<string>("DestBank")
                        .IsRequired();

                    b.Property<long>("FromAccount");

                    b.Property<string>("Message");

                    b.Property<int?>("Specific");

                    b.Property<int>("UserId");

                    b.Property<int?>("Variable");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Payment");
                });

            modelBuilder.Entity("Bank.Models.Standing", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Account");

                    b.Property<int>("Bank");

                    b.Property<int>("Constant");

                    b.Property<DateTime>("EndDate");

                    b.Property<int>("Frequency");

                    b.Property<string>("Message");

                    b.Property<int>("Specific");

                    b.Property<DateTime>("StartDate");

                    b.Property<int>("Variable");

                    b.HasKey("Id");

                    b.ToTable("Standing");
                });

            modelBuilder.Entity("Bank.Models.Template", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Amount");

                    b.Property<int?>("Constant");

                    b.Property<long>("DestAccount");

                    b.Property<long?>("DestAccountPrefix");

                    b.Property<int>("DestBank");

                    b.Property<string>("Message");

                    b.Property<string>("Name");

                    b.Property<int?>("Specific");

                    b.Property<int>("UserId");

                    b.Property<int?>("Variable");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Template");
                });

            modelBuilder.Entity("Bank.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("AccountNumber");

                    b.Property<string>("Adress");

                    b.Property<int>("BirthNumber");

                    b.Property<long?>("CardNumber");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("Login")
                        .IsRequired();

                    b.Property<double?>("Money");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int?>("Phone");

                    b.Property<string>("Pin")
                        .IsRequired();

                    b.Property<int>("Role");

                    b.HasKey("Id");

                    b.HasIndex("Login")
                        .IsUnique();

                    b.ToTable("User");
                });

            modelBuilder.Entity("Bank.Models.Payment", b =>
                {
                    b.HasOne("Bank.Models.User")
                        .WithMany("payments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Bank.Models.Template", b =>
                {
                    b.HasOne("Bank.Models.User")
                        .WithMany("templates")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
