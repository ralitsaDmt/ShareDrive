﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using ShareDrive.Data;
using System;

namespace ShareDrive.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20171212205715_RemoveColumnAvailableSeats")]
    partial class RemoveColumnAvailableSeats
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<int>("RoleId");

                    b.HasKey("Id");

                    b.ToTable("RoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.ToTable("UserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("ProviderKey");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<int>("UserId");

                    b.HasKey("ProviderKey", "LoginProvider");

                    b.ToTable("UserLogin");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.Property<int>("RoleId");

                    b.Property<int>("UserId");

                    b.HasKey("RoleId", "UserId");

                    b.ToTable("UserRole");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId");

                    b.ToTable("UserTokens");
                });

            modelBuilder.Entity("ShareDrive.Models.ApplicationRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp");

                    b.Property<string>("Name");

                    b.Property<string>("NormalizedName");

                    b.HasKey("Id");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("ShareDrive.Models.ApplicationUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp");

                    b.Property<string>("Email");

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail");

                    b.Property<string>("NormalizedUserName");

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ShareDrive.Models.Car", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Brand");

                    b.Property<string>("CarModel");

                    b.Property<bool>("HasAirConditioner");

                    b.Property<byte[]>("Image");

                    b.Property<int>("OwnerId");

                    b.Property<int>("Year");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Cars");
                });

            modelBuilder.Entity("ShareDrive.Models.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("ShareDrive.Models.Drive", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CarId");

                    b.Property<DateTime>("DateTime");

                    b.Property<int>("DeclaredSeats");

                    b.Property<int?>("DriverId");

                    b.Property<int?>("FromId");

                    b.Property<string>("LocationToArrive");

                    b.Property<string>("LocationToPick");

                    b.Property<decimal>("Price");

                    b.Property<int?>("ToId");

                    b.HasKey("Id");

                    b.HasIndex("CarId");

                    b.HasIndex("DriverId");

                    b.HasIndex("FromId");

                    b.HasIndex("ToId");

                    b.ToTable("Drives");
                });

            modelBuilder.Entity("ShareDrive.Models.DrivesPassengers", b =>
                {
                    b.Property<int>("DriveId");

                    b.Property<int>("PassengerId");

                    b.HasKey("DriveId", "PassengerId");

                    b.HasIndex("PassengerId");

                    b.ToTable("DrivesPassengers");
                });

            modelBuilder.Entity("ShareDrive.Models.Car", b =>
                {
                    b.HasOne("ShareDrive.Models.ApplicationUser", "Owner")
                        .WithMany("Cars")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ShareDrive.Models.Drive", b =>
                {
                    b.HasOne("ShareDrive.Models.Car", "Car")
                        .WithMany("Drives")
                        .HasForeignKey("CarId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("ShareDrive.Models.ApplicationUser", "Driver")
                        .WithMany("Drives")
                        .HasForeignKey("DriverId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("ShareDrive.Models.City", "From")
                        .WithMany("DrivesFrom")
                        .HasForeignKey("FromId");

                    b.HasOne("ShareDrive.Models.City", "To")
                        .WithMany("DrivesTo")
                        .HasForeignKey("ToId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("ShareDrive.Models.DrivesPassengers", b =>
                {
                    b.HasOne("ShareDrive.Models.Drive", "Drive")
                        .WithMany("DrivesPassengers")
                        .HasForeignKey("DriveId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ShareDrive.Models.ApplicationUser", "Passenger")
                        .WithMany("DrivesPassengers")
                        .HasForeignKey("PassengerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
