﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BackendAPI.Models
{
    public partial class BackendContext : DbContext
    {
        public BackendContext()
        {
        }

        public BackendContext(DbContextOptions<BackendContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Admin> Admins { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Student> Students { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=(localdb)\\Local;Database=Backend;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.HasIndex(e => e.Email, "UQ__Admins__AB6E6164090A2838")
                    .IsUnique();

                entity.Property(e => e.Email)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.Fname)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("fname");

                entity.Property(e => e.Lname)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("lname");

                entity.Property(e => e.UserId).HasColumnName("user_Id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Admins)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Admins__user_Id__30C33EC3");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.RoleName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("roleName");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasIndex(e => new { e.Email, e.StudentNumber }, "UQ__Students__B9DAE8005D7A20D2")
                    .IsUnique();

                entity.Property(e => e.Caddress)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("caddress");

                entity.Property(e => e.CaddressCity)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("caddress_city");

                entity.Property(e => e.CaddressNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("caddress_number");

                entity.Property(e => e.CaddressPost)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("caddress_post");

                entity.Property(e => e.Email)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.Fname)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("fname");

                entity.Property(e => e.Lname)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("lname");

                entity.Property(e => e.Phone)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("phone");

                entity.Property(e => e.Sex)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("sex")
                    .IsFixedLength();

                entity.Property(e => e.StudentNumber).HasColumnName("studentNumber");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Students__user_i__2CF2ADDF");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Username, "UQ__Users__F3DBC57203C2BF00")
                    .IsUnique();

                entity.Property(e => e.Password)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.PasswordSalt)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("password_salt");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.Username)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("username");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Users__role_id__29221CFB");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
