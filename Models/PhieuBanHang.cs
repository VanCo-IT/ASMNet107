using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebCafePoly.Models;

[Table("PhieuBanHang")]
public partial class PhieuBanHang
{
    [Key]
    [StringLength(6)]
    [Unicode(false)]
    [Display(Name = "Mã phiếu")]
    [Required(ErrorMessage = "Không được để trống mã phiếu")]
    public string MaPhieu { get; set; } = null!;

    [StringLength(50)]
    [Display(Name = "Mã khách hàng")]
    public string? MaKhachHang { get; set; }

    [StringLength(6)]
    [Unicode(false)]
    [Display(Name = "Mã thẻ")]
    [Required(ErrorMessage = "Không được để trống mã thẻ")]
    public string? MaThe { get; set; }

    [StringLength(6)]
    [Unicode(false)]
    [Display(Name = "Mã nhân viên")]
    [Required(ErrorMessage = "Không được để trống mã nhân viên")]
    public string? MaNhanVien { get; set; }

    [Column(TypeName = "datetime")]
    [Display(Name = "Thời gian")]
    [Required(ErrorMessage = "Không được để trống thời gian")]
    public DateTime NgayTao { get; set; }

    public bool TrangThai { get; set; }

    [InverseProperty("MaPhieuNavigation")]
    public virtual ICollection<ChiTietPhieu> ChiTietPhieus { get; set; } = new List<ChiTietPhieu>();

    [ForeignKey("MaKhachHang")]
    [InverseProperty("PhieuBanHangs")]
    public virtual KhachHang? MaKhachHangNavigation { get; set; }

    [ForeignKey("MaNhanVien")]
    [InverseProperty("PhieuBanHangs")]
    public virtual NhanVien? MaNhanVienNavigation { get; set; }

    [ForeignKey("MaThe")]
    [InverseProperty("PhieuBanHangs")]
    public virtual TheLuuDong? MaTheNavigation { get; set; }
}
