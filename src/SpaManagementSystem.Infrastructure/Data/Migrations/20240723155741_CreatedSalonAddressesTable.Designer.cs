﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SpaManagementSystem.Infrastructure.Data.Context;

#nullable disable

namespace SpaManagementSystem.Infrastructure.Data.Migrations
{
    [DbContext(typeof(SmsDbContext))]
    [Migration("20240723155741_CreatedSalonAddressesTable")]
    partial class CreatedSalonAddressesTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("SMS")
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("SpaManagementSystem.Domain.Entities.OpeningHours", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<TimeSpan>("ClosingTime")
                        .HasColumnType("interval");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("DayOfWeek")
                        .HasColumnType("integer");

                    b.Property<bool>("IsClosed")
                        .HasColumnType("boolean");

                    b.Property<TimeSpan>("OpeningTime")
                        .HasColumnType("interval");

                    b.Property<Guid>("SalonId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("SalonId");

                    b.ToTable("OpeningHours", "SMS");
                });

            modelBuilder.Entity("SpaManagementSystem.Domain.Entities.Salon", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Salons", "SMS");
                });

            modelBuilder.Entity("SpaManagementSystem.Domain.Entities.SalonAddress", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("BuildingNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Region")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("SalonId")
                        .HasColumnType("uuid");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("SalonId")
                        .IsUnique();

                    b.ToTable("SalonAddresses", "SMS");
                });

            modelBuilder.Entity("SpaManagementSystem.Domain.Entities.UserProfile", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateOnly>("DateOfBirth")
                        .HasColumnType("date");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Gender")
                        .HasColumnType("integer");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("UserProfiles", "SMS");
                });

            modelBuilder.Entity("SpaManagementSystem.Domain.Entities.OpeningHours", b =>
                {
                    b.HasOne("SpaManagementSystem.Domain.Entities.Salon", "Salon")
                        .WithMany("OpeningHours")
                        .HasForeignKey("SalonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Salon");
                });

            modelBuilder.Entity("SpaManagementSystem.Domain.Entities.SalonAddress", b =>
                {
                    b.HasOne("SpaManagementSystem.Domain.Entities.Salon", "Salon")
                        .WithOne("Address")
                        .HasForeignKey("SpaManagementSystem.Domain.Entities.SalonAddress", "SalonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Salon");
                });

            modelBuilder.Entity("SpaManagementSystem.Domain.Entities.Salon", b =>
                {
                    b.Navigation("Address")
                        .IsRequired();

                    b.Navigation("OpeningHours");
                });
#pragma warning restore 612, 618
        }
    }
}
