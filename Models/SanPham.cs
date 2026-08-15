using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace WebCafePoly.Models;

[Table("SanPham")]
public partial class SanPham
{
    [Key]
    [Required(ErrorMessage = "Mã sản phẩm không được để trống")]
    [StringLength(6)]
    [Unicode(false)]
    [Display(Name = "Mã sản phẩm")]
    public string MaSanPham { get; set; } = null!;

    [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
    [StringLength(100)]
    [Display(Name = "Tên sản phẩm")]
    public string TenSanPham { get; set; } = null!;

    [Required(ErrorMessage = "Đơn giá không được để trống")]
    [Range(1000, 10000000, ErrorMessage = "Đơn giá phải từ 1.000 trở lên")]
    [Column(TypeName = "decimal(10,0)")]
    [Display(Name = "Đơn giá")]
    public decimal DonGia { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn loại sản phẩm")]
    [StringLength(6)]
    [Unicode(false)]
    [Display(Name = "Loại sản phẩm")]
    public string MaLoai { get; set; } = null!;

    [Display(Name = "Hình ảnh")]
    public string? HinhAnh { get; set; }

    [Display(Name = "Trạng thái")]
    public bool TrangThai { get; set; }

    [InverseProperty("MaSanPhamNavigation")]
    public virtual ICollection<ChiTietPhieu> ChiTietPhieus { get; set; } = new List<ChiTietPhieu>();

    [ForeignKey("MaLoai")]
    [InverseProperty("SanPhams")]
    [ValidateNever]
    public virtual LoaiSanPham MaLoaiNavigation { get; set; } = null!;
}
