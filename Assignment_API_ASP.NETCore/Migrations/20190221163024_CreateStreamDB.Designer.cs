﻿// <auto-generated />
using System;
using Assignment_API_ASP.NETCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Assignment_API_ASP.NETCore.Migrations
{
    [DbContext(typeof(StreamContext))]
    [Migration("20190221163024_CreateStreamDB")]
    partial class CreateStreamDB
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Assignment_API_ASP.NETCore.Models.Channel", b =>
                {
                    b.Property<int>("ChannelID")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ChannelID")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnName("Name")
                        .HasColumnType("varchar(50)");

                    b.HasKey("ChannelID");

                    b.ToTable("ChannelInfo","dbo");
                });

            modelBuilder.Entity("Assignment_API_ASP.NETCore.Models.Message", b =>
                {
                    b.Property<int>("MessageID")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("MessageID")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("Inserted")
                        .HasColumnName("Inserted")
                        .HasColumnType("datetime");

                    b.Property<int>("ReferenceChannelID")
                        .HasColumnName("ChannelID")
                        .HasColumnType("int");

                    b.Property<int>("ReferenceUserID")
                        .HasColumnName("UserID")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .HasColumnName("Text")
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime?>("Validated")
                        .HasColumnName("Validated")
                        .HasColumnType("datetime");

                    b.HasKey("MessageID");

                    b.ToTable("MessageInfo","dbo");
                });

            modelBuilder.Entity("Assignment_API_ASP.NETCore.Models.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("UserID")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Activated")
                        .HasColumnName("Activated")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnName("Name")
                        .HasColumnType("varchar(50)");

                    b.HasKey("UserID");

                    b.ToTable("UserInfo","dbo");
                });

            modelBuilder.Entity("Assignment_API_ASP.NETCore.Models.UserChannel", b =>
                {
                    b.Property<int>("UserID");

                    b.Property<int>("ChannelID");

                    b.HasKey("UserID", "ChannelID");

                    b.HasIndex("ChannelID");

                    b.ToTable("UserChannels");
                });

            modelBuilder.Entity("Assignment_API_ASP.NETCore.Models.UserChannel", b =>
                {
                    b.HasOne("Assignment_API_ASP.NETCore.Models.Channel", "Channel")
                        .WithMany()
                        .HasForeignKey("ChannelID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Assignment_API_ASP.NETCore.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
