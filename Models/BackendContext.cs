using System;
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
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.Property(e => e.AdminId)
                    .ValueGeneratedNever()
                    .HasColumnName("adminId");

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

                entity.Property(e => e.RoleId).HasColumnName("roleId");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Admins)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__Admins__roleId__2D27B809");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.RoleName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("roleName");

                entity.Property(e => e.User_Id).HasColumnName("userId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Roles)
                    .HasForeignKey(d => d.User_Id)
                    .HasConstraintName("FK__Roles__userId__276EDEB3");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.Property(e => e.StudentId)
                    .ValueGeneratedNever()
                    .HasColumnName("studentId");

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

                entity.Property(e => e.StudentNumber)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("studentNumber");

                entity.Property(e => e.RoleId).HasColumnName("roleId");

                entity.Property(e => e.Sex)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("sex")
                    .IsFixedLength();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__Students__roleId__2A4B4B5E");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Username, "UQ__Users__F3DBC5724CF1B5F0")
                    .IsUnique();

                entity.Property(e => e.UserId)
                    .ValueGeneratedNever()
                    .HasColumnName("userId");

                entity.Property(e => e.Password)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.Username)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("username");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
