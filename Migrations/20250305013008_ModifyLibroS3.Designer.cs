﻿// <auto-generated />
using System;
using FernandoLibrary.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FernandoLibrary.Migrations
{
    [DbContext(typeof(LibraryDbContext))]
    [Migration("20250305013008_ModifyLibroS3")]
    partial class ModifyLibroS3
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.2");

            modelBuilder.Entity("Autor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Autores");
                });

            modelBuilder.Entity("Categoria", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Categorias");
                });

            modelBuilder.Entity("Libro", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AutorId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Cantidad")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CategoriaId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AutorId");

                    b.HasIndex("CategoriaId");

                    b.ToTable("Libros");
                });

            modelBuilder.Entity("Prestamo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("FechaDevolucion")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("FechaPrestamo")
                        .HasColumnType("TEXT");

                    b.Property<int>("LibroId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("LibroId");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Prestamos");
                });

            modelBuilder.Entity("Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("Libro", b =>
                {
                    b.HasOne("Autor", "Autor")
                        .WithMany("Libros")
                        .HasForeignKey("AutorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Categoria", "Categoria")
                        .WithMany("Libros")
                        .HasForeignKey("CategoriaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Autor");

                    b.Navigation("Categoria");
                });

            modelBuilder.Entity("Prestamo", b =>
                {
                    b.HasOne("Libro", "Libro")
                        .WithMany("Prestamos")
                        .HasForeignKey("LibroId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Usuario", "Usuario")
                        .WithMany("Prestamos")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Libro");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("Autor", b =>
                {
                    b.Navigation("Libros");
                });

            modelBuilder.Entity("Categoria", b =>
                {
                    b.Navigation("Libros");
                });

            modelBuilder.Entity("Libro", b =>
                {
                    b.Navigation("Prestamos");
                });

            modelBuilder.Entity("Usuario", b =>
                {
                    b.Navigation("Prestamos");
                });
#pragma warning restore 612, 618
        }
    }
}
