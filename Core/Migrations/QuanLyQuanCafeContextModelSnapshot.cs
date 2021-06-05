﻿// <auto-generated />
using System;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Core.Migrations
{
    [DbContext(typeof(QuanLyQuanCafeContext))]
    partial class QuanLyQuanCafeContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.6")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Core.Models.Account", b =>
                {
                    b.Property<string>("UserName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasDefaultValueSql("(N'Admin')");

                    b.Property<string>("PassWord")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)")
                        .HasDefaultValueSql("((0))");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("UserName")
                        .HasName("PK__Account__C9F2845720CEE35B");

                    b.ToTable("Account");
                });

            modelBuilder.Entity("Core.Models.Bill", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateCheckIn")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("date")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<DateTime?>("DateCheckOut")
                        .HasColumnType("date");

                    b.Property<int?>("Discount")
                        .HasColumnType("int")
                        .HasColumnName("discount");

                    b.Property<int>("IdTable")
                        .HasColumnType("int")
                        .HasColumnName("idTable");

                    b.Property<int>("Status")
                        .HasColumnType("int")
                        .HasColumnName("status");

                    b.Property<double?>("TotalPrice")
                        .HasColumnType("float")
                        .HasColumnName("totalPrice");

                    b.HasKey("Id");

                    b.HasIndex("IdTable");

                    b.ToTable("Bill");
                });

            modelBuilder.Entity("Core.Models.BillInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Count")
                        .HasColumnType("int")
                        .HasColumnName("count");

                    b.Property<int>("IdBill")
                        .HasColumnType("int")
                        .HasColumnName("idBill");

                    b.Property<int>("IdFood")
                        .HasColumnType("int")
                        .HasColumnName("idFood");

                    b.HasKey("Id");

                    b.HasIndex("IdBill");

                    b.HasIndex("IdFood");

                    b.ToTable("BillInfo");
                });

            modelBuilder.Entity("Core.Models.Food", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("IdCategory")
                        .HasColumnType("int")
                        .HasColumnName("idCategory");

                    b.Property<string>("Name")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("name")
                        .HasDefaultValueSql("(N'Chưa đặt tên')");

                    b.Property<double>("Price")
                        .HasColumnType("float")
                        .HasColumnName("price");

                    b.HasKey("Id");

                    b.HasIndex("IdCategory");

                    b.ToTable("Food");
                });

            modelBuilder.Entity("Core.Models.FoodCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("name")
                        .HasDefaultValueSql("(N'Chưa đặt tên')");

                    b.HasKey("Id");

                    b.ToTable("FoodCategory");
                });

            modelBuilder.Entity("Core.Models.TableFood", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("name")
                        .HasDefaultValueSql("(N'Bàn chưa có tên')");

                    b.Property<string>("Status")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("status")
                        .HasDefaultValueSql("(N'Trống')");

                    b.HasKey("Id");

                    b.ToTable("TableFood");
                });

            modelBuilder.Entity("Core.Models.Bill", b =>
                {
                    b.HasOne("Core.Models.TableFood", "IdTableNavigation")
                        .WithMany("Bills")
                        .HasForeignKey("IdTable")
                        .HasConstraintName("FK__Bill__status__36B12243")
                        .IsRequired();

                    b.Navigation("IdTableNavigation");
                });

            modelBuilder.Entity("Core.Models.BillInfo", b =>
                {
                    b.HasOne("Core.Models.Bill", "IdBillNavigation")
                        .WithMany("BillInfos")
                        .HasForeignKey("IdBill")
                        .HasConstraintName("FK__BillInfo__count__3A81B327")
                        .IsRequired();

                    b.HasOne("Core.Models.Food", "IdFoodNavigation")
                        .WithMany("BillInfos")
                        .HasForeignKey("IdFood")
                        .HasConstraintName("FK__BillInfo__idFood__3B75D760")
                        .IsRequired();

                    b.Navigation("IdBillNavigation");

                    b.Navigation("IdFoodNavigation");
                });

            modelBuilder.Entity("Core.Models.Food", b =>
                {
                    b.HasOne("Core.Models.FoodCategory", "IdCategoryNavigation")
                        .WithMany("Foods")
                        .HasForeignKey("IdCategory")
                        .HasConstraintName("FK__Food__price__31EC6D26")
                        .IsRequired();

                    b.Navigation("IdCategoryNavigation");
                });

            modelBuilder.Entity("Core.Models.Bill", b =>
                {
                    b.Navigation("BillInfos");
                });

            modelBuilder.Entity("Core.Models.Food", b =>
                {
                    b.Navigation("BillInfos");
                });

            modelBuilder.Entity("Core.Models.FoodCategory", b =>
                {
                    b.Navigation("Foods");
                });

            modelBuilder.Entity("Core.Models.TableFood", b =>
                {
                    b.Navigation("Bills");
                });
#pragma warning restore 612, 618
        }
    }
}