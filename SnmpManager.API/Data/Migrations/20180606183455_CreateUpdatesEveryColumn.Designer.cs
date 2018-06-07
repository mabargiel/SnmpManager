﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SnmpManager.API.Models;

namespace SnmpManager.API.Migrations
{
    [DbContext(typeof(SnmpManagerContext))]
    [Migration("20180606183455_CreateUpdatesEveryColumn")]
    partial class CreateUpdatesEveryColumn
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.0-rtm-30799")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SnmpManager.API.Models.WatcherData", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("IpAddress");

                    b.Property<int>("Method");

                    b.Property<string>("Mib");

                    b.Property<int>("UpdatesEvery");

                    b.HasKey("Id");

                    b.ToTable("Watchers");
                });
#pragma warning restore 612, 618
        }
    }
}
