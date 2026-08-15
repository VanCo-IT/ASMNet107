using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebCafePoly.Models;

public partial class PolyCafeContext : DbContext
{
    public PolyCafeContext()
    {
    }

    public PolyCafeContext(DbContextOptions<PolyCafeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ChiTietPhieu> ChiTietPhieus { get; set; }

    public virtual DbSet<KhachHang> KhachHangs { get; set; }

    public virtual DbSet<LoaiSanPham> LoaiSanPhams { get; set; }

    public virtual DbSet<NhanVien> NhanViens { get; set; }

    public virtual DbSet<PhieuBanHang> PhieuBanHangs { get; set; }

    public virtual DbSet<SanPham> SanPhams { get; set; }

    public virtual DbSet<TheLuuDong> TheLuuDongs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost\\SQL2022;Database=PolyCafe;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChiTietPhieu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ChiTietP__3214EC071063AA3E");

            entity.Property(e => e.MaPhieu).IsFixedLength();
            entity.Property(e => e.MaSanPham).IsFixedLength();

            entity.HasOne(d => d.MaPhieuNavigation).WithMany(p => p.ChiTietPhieus).HasConstraintName("FK_ChiTietPhieu_Phieu");

            entity.HasOne(d => d.MaSanPhamNavigation).WithMany(p => p.ChiTietPhieus).HasConstraintName("FK_ChiTietPhieu_SanPham");
        });

        modelBuilder.Entity<KhachHang>(entity =>
        {
            entity.HasKey(e => e.MaKhachHang).HasName("PK__KhachHan__88D2F0E5E384F439");
        });

        modelBuilder.Entity<LoaiSanPham>(entity =>
        {
            entity.HasKey(e => e.MaLoai).HasName("PK__LoaiSanP__730A5759CD145FB9");

            entity.Property(e => e.MaLoai).IsFixedLength();
        });

        modelBuilder.Entity<NhanVien>(entity =>
        {
            entity.HasKey(e => e.MaNhanVien).HasName("PK__NhanVien__77B2CA475676E33E");

            entity.Property(e => e.MaNhanVien).IsFixedLength();
            entity.Property(e => e.TrangThai).HasDefaultValue(true);
        });

        modelBuilder.Entity<PhieuBanHang>(entity =>
        {
            entity.HasKey(e => e.MaPhieu).HasName("PK__PhieuBan__2660BFE007B04E07");

            entity.Property(e => e.MaPhieu).IsFixedLength();
            entity.Property(e => e.MaNhanVien).IsFixedLength();
            entity.Property(e => e.MaThe).IsFixedLength();
            entity.Property(e => e.NgayTao).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.MaKhachHangNavigation).WithMany(p => p.PhieuBanHangs)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_PhieuBanHang_KhachHang");

            entity.HasOne(d => d.MaNhanVienNavigation).WithMany(p => p.PhieuBanHangs)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_PhieuBanHang_NhanVien");

            entity.HasOne(d => d.MaTheNavigation).WithMany(p => p.PhieuBanHangs)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_PhieuBanHang_The");
        });

        modelBuilder.Entity<SanPham>(entity =>
        {
            entity.HasKey(e => e.MaSanPham).HasName("PK__SanPham__FAC7442DF826DF08");

            entity.Property(e => e.MaSanPham).IsFixedLength();
            entity.Property(e => e.MaLoai).IsFixedLength();
            entity.Property(e => e.TrangThai).HasDefaultValue(true);

            entity.HasOne(d => d.MaLoaiNavigation).WithMany(p => p.SanPhams).HasConstraintName("FK_SanPham_Loai");
        });

        modelBuilder.Entity<TheLuuDong>(entity =>
        {
            entity.HasKey(e => e.MaThe).HasName("PK__TheLuuDo__314EEAAFDDFB78F1");

            entity.Property(e => e.MaThe).IsFixedLength();
            entity.Property(e => e.TrangThai).HasDefaultValue(true);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
