using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebTuiXachh_Gateway.Model
{
    public partial class WebTuiXachContext : DbContext
    {
        public WebTuiXachContext()
        {
        }

        public WebTuiXachContext(DbContextOptions<WebTuiXachContext> options) : base(options) { }

        // Định nghĩa DbSet cho từng bảng
        public DbSet<User> Users { get; set; }
        public DbSet<TuiXach> TuiXachs { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<TinTuc> TinTucs { get; set; }
        public DbSet<NhaCungCap> NhaCungCaps { get; set; }
        public DbSet<MauSac> MauSacs { get; set; }
        public DbSet<HoaDon> HoaDons { get; set; }
        public DbSet<DonHangNhap> DonHangNhaps { get; set; }
        public DbSet<DanhMucSanPham> DanhMucSanPhams { get; set; }
        public DbSet<ChiTietHoaDon> ChiTietHoaDons { get; set; }
        public DbSet<ChiTietDonHangNhap> ChiTietDonHangNhaps { get; set; }
        public DbSet<BinhLuan> BinhLuans { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");
                entity.HasKey(e => e.PerID);
                entity.Property(e => e.TaiKhoan).HasMaxLength(50).HasColumnName("tai_khoan");
                entity.Property(e => e.MatKhau).HasMaxLength(200).HasColumnName("mat_khau");
                entity.Property(e => e.HoTen).HasMaxLength(100).HasColumnName("ho_ten");
                entity.Property(e => e.NgaySinh).HasColumnType("datetime").HasColumnName("ngay_sinh");
                entity.Property(e => e.GioiTinh).HasMaxLength(10).HasColumnName("gioi_tinh");
                entity.Property(e => e.DiaChi).HasMaxLength(200).HasColumnName("dia_chi");
                entity.Property(e => e.Role).HasMaxLength(50).HasColumnName("role");

                entity.HasMany(e => e.BinhLuans)
                      .WithOne(e => e.User)
                      .HasForeignKey(e => e.PerID);

                entity.HasMany(e => e.HoaDons)
                      .WithOne(e => e.User)
                      .HasForeignKey(e => e.PerID);
            });

            modelBuilder.Entity<TuiXach>(entity =>
            {
                entity.ToTable("tui_xach");
                entity.HasKey(e => e.MaSp);
                entity.Property(e => e.MaDanhMuc).HasMaxLength(50).HasColumnName("ma_danh_muc");
                entity.Property(e => e.TenSp).HasMaxLength(100).HasColumnName("ten_sp");
                entity.Property(e => e.TenMau).HasMaxLength(50).HasColumnName("ten_mau");
                entity.Property(e => e.MaSize).HasMaxLength(20).HasColumnName("ma_size");
                entity.Property(e => e.GiaBan).HasColumnType("decimal(18,2)").HasColumnName("gia_ban");
                entity.Property(e => e.KhuyenMai).HasColumnType("decimal(18,2)").HasColumnName("khuyen_mai");
                entity.Property(e => e.TonKho).HasColumnName("ton_kho");
                entity.Property(e => e.MoTa).HasColumnType("text").HasColumnName("mo_ta");
                entity.Property(e => e.HinhAnh).HasMaxLength(255).HasColumnName("hinh_anh");
                entity.Property(e => e.SoLuotDanhGia).HasColumnName("so_luot_danh_gia");

                entity.HasOne(e => e.DanhMucSanPham)
                      .WithMany(e => e.TuiXachs)
                      .HasForeignKey(e => e.MaDanhMuc);

                entity.HasMany(e => e.ChiTietHoaDons)
                      .WithOne(e => e.TuiXach)
                      .HasForeignKey(e => e.MaSp);

                entity.HasMany(e => e.BinhLuans)
                      .WithOne(e => e.TuiXach)
                      .HasForeignKey(e => e.MaSp);
            });

            modelBuilder.Entity<Size>(entity =>
            {
                entity.ToTable("size");
                entity.HasKey(e => e.MaSize);
            });

            modelBuilder.Entity<TinTuc>(entity =>
            {
                entity.ToTable("tin_tuc");
                entity.HasKey(e => e.MaTinTuc);
                entity.Property(e => e.TieuDe).HasMaxLength(200).HasColumnName("tieu_de");
                entity.Property(e => e.NoiDung).HasColumnType("text").HasColumnName("noi_dung");
                entity.Property(e => e.HinhAnh).HasMaxLength(255).HasColumnName("hinh_anh");
                entity.Property(e => e.NgayDang).HasColumnType("datetime").HasColumnName("ngay_dang");
                entity.Property(e => e.NguoiDang).HasMaxLength(100).HasColumnName("nguoi_dang");
            });

            modelBuilder.Entity<NhaCungCap>(entity =>
            {
                entity.ToTable("nha_cung_cap");
                entity.HasKey(e => e.TenNCC);
                entity.Property(e => e.DiaChi).HasMaxLength(200).HasColumnName("dia_chi");
                entity.Property(e => e.LienHe).HasMaxLength(100).HasColumnName("lien_he");
            });

            modelBuilder.Entity<MauSac>(entity =>
            {
                entity.ToTable("mau_sac");
                entity.HasKey(e => e.TenMau);
            });

            modelBuilder.Entity<HoaDon>(entity =>
            {
                entity.ToTable("hoa_don");
                entity.HasKey(e => e.MaHD);
                entity.Property(e => e.PerID).HasColumnName("per_id");
                entity.Property(e => e.HoTen).HasMaxLength(100).HasColumnName("ho_ten");
                entity.Property(e => e.DiaChi).HasMaxLength(200).HasColumnName("dia_chi");
                entity.Property(e => e.SDT).HasMaxLength(15).HasColumnName("sdt");
                entity.Property(e => e.NgayDatHang).HasColumnType("datetime").HasColumnName("ngay_dat_hang");
                entity.Property(e => e.TrangThai).HasMaxLength(50).HasColumnName("trang_thai");
                entity.Property(e => e.NgayNhanHang).HasColumnType("datetime").HasColumnName("ngay_nhan_hang");

                entity.HasOne(e => e.User)
                      .WithMany(e => e.HoaDons)
                      .HasForeignKey(e => e.PerID);

                entity.HasMany(e => e.ChiTietHoaDons)
                      .WithOne(e => e.HoaDon)
                      .HasForeignKey(e => e.MaHD);
            });

            modelBuilder.Entity<DonHangNhap>(entity =>
            {
                entity.ToTable("don_hang_nhap");
                entity.HasKey(e => e.MaDHN);
                entity.Property(e => e.TenNCC).HasMaxLength(100).HasColumnName("ten_ncc");
                entity.Property(e => e.NgayNhap).HasColumnType("datetime").HasColumnName("ngay_nhap");

                entity.HasMany(e => e.ChiTietDonHangNhaps)
                      .WithOne(e => e.DonHangNhap)
                      .HasForeignKey(e => e.MaDHN);
            });

            modelBuilder.Entity<DanhMucSanPham>(entity =>
            {
                entity.ToTable("danh_muc_san_pham");
                entity.HasKey(e => e.MaDanhMuc);
                entity.Property(e => e.TenDanhMuc).HasMaxLength(100).HasColumnName("ten_danh_muc");

                entity.HasMany(e => e.TuiXachs)
                      .WithOne(e => e.DanhMucSanPham)
                      .HasForeignKey(e => e.MaDanhMuc);
            });

            modelBuilder.Entity<ChiTietHoaDon>(entity =>
            {
                entity.ToTable("chi_tiet_hoa_don");
                entity.HasKey(e => new { e.MaHD, e.MaSp });
                entity.Property(e => e.TenSp).HasMaxLength(100).HasColumnName("ten_sp");
                entity.Property(e => e.TenMau).HasMaxLength(50).HasColumnName("ten_mau");
                entity.Property(e => e.MaSize).HasMaxLength(20).HasColumnName("ma_size");
                entity.Property(e => e.SoLuong).HasColumnName("so_luong");
                entity.Property(e => e.GiaBan).HasColumnType("decimal(18,2)").HasColumnName("gia_ban");
                entity.Property(e => e.GhiChu).HasColumnType("text").HasColumnName("ghi_chu");
                entity.Property(e => e.KhuyenMai).HasColumnType("decimal(18,2)").HasColumnName("khuyen_mai");

                entity.HasOne(e => e.HoaDon)
                      .WithMany(e => e.ChiTietHoaDons)
                      .HasForeignKey(e => e.MaHD);

                entity.HasOne(e => e.TuiXach)
                      .WithMany(e => e.ChiTietHoaDons)
                      .HasForeignKey(e => e.MaSp);
            });

            modelBuilder.Entity<ChiTietDonHangNhap>(entity =>
            {
                entity.ToTable("chi_tiet_don_hang_nhap");
                entity.HasKey(e => new { e.MaDHN, e.MaSp });
                entity.Property(e => e.GiaNhap).HasColumnType("decimal(18,2)").HasColumnName("gia_nhap");
                entity.Property(e => e.SoLuong).HasColumnName("so_luong");

                entity.HasOne(e => e.DonHangNhap)
                      .WithMany(e => e.ChiTietDonHangNhaps)
                      .HasForeignKey(e => e.MaDHN);
            });

            modelBuilder.Entity<BinhLuan>(entity =>
            {
                entity.ToTable("binh_luan");
                entity.HasKey(e => e.MaBinhLuan);
                entity.Property(e => e.PerID).HasColumnName("per_id");
                entity.Property(e => e.MaSp).HasMaxLength(50).HasColumnName("ma_sp");
                entity.Property(e => e.NoiDung).HasColumnType("text").HasColumnName("noi_dung");
                entity.Property(e => e.NgayBinhLuan).HasColumnType("datetime").HasColumnName("ngay_binh_luan");

                entity.HasOne(e => e.User)
                      .WithMany(e => e.BinhLuans)
                      .HasForeignKey(e => e.PerID);

                entity.HasOne(e => e.TuiXach)
                      .WithMany(e => e.BinhLuans)
                      .HasForeignKey(e => e.MaSp);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
