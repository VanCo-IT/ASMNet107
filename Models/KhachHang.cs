using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebCafePoly.Models;

[Table("KhachHang")]
public partial class KhachHang
{
    [Key]
    [StringLength(50)]
    [Display(Name = "Mã khách hàng")]
    [Required(ErrorMessage = "Không được để trống mã khách hàng")]
    public string MaKhachHang { get; set; } = null!;

    [StringLength(50)]
    //ham code kiểm tra dữ liệu nhập vào
    [Display(Name = "Họ tên")]
    [Required(ErrorMessage = "Không được để trống họ tên")]
    public string? HoTen { get; set; }

    [StringLength(50)]
    [Display(Name = "Email")]
    [Required(ErrorMessage = "Không được để trống email")]
    [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
    public string? Email { get; set; }

    [StringLength(50)]
    [Display(Name = "Số điện thoại")]
    public string? SoDienThoai { get; set; }

    [StringLength(50)]
    [Display(Name = "Mật khẩu")]
    [Required(ErrorMessage = "Không được để trống mật khẩu")]
    public string? MatKhau { get; set; }

    [StringLength(50)]
    [Display(Name = "Địa chỉ")]
    public string? DiaChi { get; set; }

    [Display(Name = "Trạng thái")]
    public bool? TrangThai { get; set; }

    [InverseProperty("MaKhachHangNavigation")]
    public virtual ICollection<PhieuBanHang> PhieuBanHangs { get; set; } = new List<PhieuBanHang>();
}
