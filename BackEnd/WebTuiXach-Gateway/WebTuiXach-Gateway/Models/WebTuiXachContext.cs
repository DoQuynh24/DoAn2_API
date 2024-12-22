using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebTuiXach_Gateway.Models
{
    public partial class WebTuiXachContext : DbContext
    {
        public WebTuiXachContext()
        {
        }

        public WebTuiXachContext(DbContextOptions<WebTuiXachContext> options)
            : base(options)
        {
        }

        // Định nghĩa DbSet cho từng bảng
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<TuiXach> TuiXachs { get; set; } = null!;
        public virtual DbSet<TinTuc> TinTucs { get; set; } = null!;
        public virtual DbSet<Size> Sizes { get; set; } = null!;
        public virtual DbSet<HoaDon> HoaDons { get; set; } = null!;
        public virtual DbSet<DanhMucSanPham> DanhMucSanPhams { get; set; } = null!;
        public virtual DbSet<ChiTietHoaDon> ChiTietHoaDons { get; set; } = null!;
        public virtual DbSet<BinhLuan> BinhLuans { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=Quynh;Initial Catalog=WebTuiXach;Integrated Security=True;TrustServerCertificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");
                entity.HasKey(e => e.PerID);
                entity.Property(e => e.TaiKhoan).HasMaxLength(30).IsUnicode(false).HasColumnName("taikhoan");
                entity.Property(e => e.MatKhau).HasMaxLength(60).IsUnicode(false).HasColumnName("matkhau");
                entity.Property(e => e.HoTen).HasMaxLength(150).HasColumnName("hoten");
                entity.Property(e => e.Email).HasMaxLength(150).IsUnicode(false).HasColumnName("email");
                entity.Property(e => e.SDT).HasMaxLength(15).HasColumnName("sdt");
                entity.Property(e => e.DiaChi).HasMaxLength(250).HasColumnName("diachi");
                entity.Property(e => e.Role).HasMaxLength(30).IsUnicode(false).HasColumnName("role");
                entity.Property(e => e.NgaySinh).HasColumnType("datetime").HasColumnName("ngay_sinh");
                entity.Property(e => e.GioiTinh).HasMaxLength(10).HasColumnName("gioi_tinh");
                entity.Property(e => e.Avatar).HasColumnName("avatar");
            });

            modelBuilder.Entity<TuiXach>(entity =>
            {
                entity.ToTable("tui_xach");
                entity.HasKey(e => e.MaSp);
                entity.Property(e => e.TenSp).HasMaxLength(150).HasColumnName("ten_sp");
                entity.Property(e => e.GiaSp).HasColumnName("gia_sp");
                entity.Property(e => e.KhuyenMai).HasColumnName("khuyen_mai");
                entity.Property(e => e.TonKho).HasColumnName("ton_kho");
                entity.Property(e => e.MauSac).HasMaxLength(50).HasColumnName("mau_sac");
                entity.Property(e => e.MaSize).HasMaxLength(10).HasColumnName("ma_size");
                entity.Property(e => e.MoTa).HasColumnName("mo_ta");
                entity.Property(e => e.SoLuotDanhGia).HasColumnName("so_luot_danh_gia");
                entity.Property(e => e.MaDanhMuc).HasMaxLength(50).HasColumnName("ma_danh_muc");
            });

            modelBuilder.Entity<TinTuc>(entity =>
            {
                entity.ToTable("tin_tuc");
                entity.HasKey(e => e.MaTinTuc);
                entity.Property(e => e.TieuDe).HasMaxLength(150).HasColumnName("tieu_de");
                entity.Property(e => e.NoiDung).HasColumnName("noi_dung");
                entity.Property(e => e.NgayDang).HasColumnType("datetime").HasColumnName("ngay_dang");
                entity.Property(e => e.NguoiDang).HasMaxLength(100).HasColumnName("nguoi_dang");
            });

            modelBuilder.Entity<Size>(entity =>
            {
                entity.ToTable("size");
                entity.HasKey(e => e.MaSize);
                entity.Property(e => e.MaSize).HasMaxLength(10).HasColumnName("ma_size");
            });

            modelBuilder.Entity<HoaDon>(entity =>
            {
                entity.HasKey(e => e.MaHD);
                entity.ToTable("hoa_don");
                entity.Property(e => e.MaHD).HasColumnName("ma_hd");
                entity.Property(e => e.PerID).HasColumnName("per_id");
                entity.Property(e => e.NgayBan).HasColumnType("datetime").HasColumnName("ngay_ban");
                entity.Property(e => e.TrangThai).HasMaxLength(50).HasColumnName("trang_thai");

                entity.HasOne(d => d.Users)
                    .WithMany(p => p.HoaDons)
                    .HasForeignKey(d => d.PerID)
                    .HasConstraintName("FK_hoa_don_users");
            });

            modelBuilder.Entity<DanhMucSanPham>(entity =>
            {
                entity.HasKey(e => e.MaDanhMuc);
                entity.ToTable("danh_muc_san_pham");
                entity.Property(e => e.MaDanhMuc).HasMaxLength(50).IsUnicode(false).HasColumnName("ma_danh_muc");
                entity.Property(e => e.TenDanhMuc).HasMaxLength(150).HasColumnName("ten_danh_muc");
            });

            modelBuilder.Entity<ChiTietHoaDon>(entity =>
            {
                entity.HasKey(e => e.MaChiTietHD);
                entity.ToTable("chi_tiet_hoa_don");
                entity.Property(e => e.MaChiTietHD).HasColumnName("ma_chi_tiet_hd");
                entity.Property(e => e.MaHD).HasColumnName("ma_hd");
                entity.Property(e => e.MaSp).HasMaxLength(50).IsUnicode(false).HasColumnName("ma_sp");
                entity.Property(e => e.SoLuong).HasColumnName("so_luong");
                entity.Property(e => e.GiaBan).HasColumnType("decimal(18, 2)").HasColumnName("gia_ban");
                entity.Property(e => e.KhuyenMai).HasColumnType("decimal(18, 2)").HasColumnName("khuyen_mai");

                entity.HasOne(d => d.HoaDon)
                    .WithMany(p => p.ChiTietHoaDons)
                    .HasForeignKey(d => d.MaHD)
                    .HasConstraintName("FK_chi_tiet_hoa_don_hoa_don");

                entity.HasOne(d => d.SanPham)
                    .WithMany(p => p.ChiTietHoaDons)
                    .HasForeignKey(d => d.MaSp)
                    .HasConstraintName("FK_chi_tiet_hoa_don_san_pham");
            });

            modelBuilder.Entity<BinhLuan>(entity =>
            {
                entity.HasKey(e => e.MaBinhLuan);
                entity.ToTable("binh_luan");
                entity.Property(e => e.MaBinhLuan).HasColumnName("ma_binh_luan");
                entity.Property(e => e.PerID).HasColumnName("per_id");
                entity.Property(e => e.MaSp).HasMaxLength(50).IsUnicode(false).HasColumnName("ma_sp");
                entity.Property(e => e.NoiDung).HasColumnType("nvarchar(max)").HasColumnName("noi_dung");
                entity.Property(e => e.NgayBinhLuan).HasColumnType("datetime").HasColumnName("ngay_binh_luan");

                entity.HasOne(d => d.Users)
                    .WithMany(p => p.BinhLuans)
                    .HasForeignKey(d => d.PerID)
                    .HasConstraintName("FK_binh_luan_users");

                entity.HasOne(d => d.SanPham)
                    .WithMany(p => p.BinhLuans)
                    .HasForeignKey(d => d.MaSp)
                    .HasConstraintName("FK_binh_luan_san_pham");
            });
        }
    }
}
