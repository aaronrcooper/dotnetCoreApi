﻿// <auto-generated />
using System;
using APITest.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace APITest.Migrations
{
    [DbContext(typeof(TodoContext))]
    partial class TodoContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("APITest.Domain.Models.Person", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("State")
                        .IsRequired();

                    b.Property<string>("Zipcode")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("People");

                    b.HasData(
                        new
                        {
                            Id = new Guid("eaa559e4-090c-4706-a776-ffa16e7a2191"),
                            Address = "N/a",
                            City = "Pittsburgh",
                            Email = "N/a",
                            FirstName = "Aaron",
                            LastName = "Cooper",
                            State = "PA",
                            Zipcode = "N/a"
                        });
                });

            modelBuilder.Entity("APITest.Domain.Models.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("UserRole");

                    b.HasKey("Id");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = new Guid("a8cc16b7-aa6b-47d3-909a-06fdcae81619"),
                            UserRole = "Administrator"
                        },
                        new
                        {
                            Id = new Guid("8110bff0-12a7-4cc7-906d-a9c052727e06"),
                            UserRole = "User"
                        });
                });

            modelBuilder.Entity("APITest.Domain.Models.TodoItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsComplete");

                    b.Property<string>("Name");

                    b.Property<Guid>("userId");

                    b.HasKey("Id");

                    b.HasIndex("userId");

                    b.ToTable("TodoItems");
                });

            modelBuilder.Entity("APITest.Domain.Models.User", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<string>("HashedPassword")
                        .IsRequired();

                    b.Property<Guid>("RoleId");

                    b.Property<byte[]>("Salt")
                        .IsRequired()
                        .HasMaxLength(16);

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("eaa559e4-090c-4706-a776-ffa16e7a2191"),
                            HashedPassword = "Ov4B87zmh9j/dEG/y/BQlT3S8FA=",
                            RoleId = new Guid("a8cc16b7-aa6b-47d3-909a-06fdcae81619"),
                            Salt = new byte[] { 92, 251, 117, 81, 232, 198, 132, 52, 28, 94, 233, 112, 135, 156, 117, 187 },
                            Username = "Admin"
                        });
                });

            modelBuilder.Entity("APITest.Domain.Models.TodoItem", b =>
                {
                    b.HasOne("APITest.Domain.Models.User", "user")
                        .WithMany("TodoItems")
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("APITest.Domain.Models.User", b =>
                {
                    b.HasOne("APITest.Domain.Models.Person", "Person")
                        .WithOne("User")
                        .HasForeignKey("APITest.Domain.Models.User", "Id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("APITest.Domain.Models.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
