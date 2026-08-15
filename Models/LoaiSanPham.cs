using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCafePoly.Models;

[Table("LoaiSanPham")]
[Index("TenLoai", Name = "UQ__LoaiSanP__E29B104251013E38", IsUnique = true)]
public partial class LoaiSanPham
{
    [Key]
    [Required(ErrorMessage = "Mã loại không được để trống")]
    [StringLength(6, MinimumLength = 6,
    ErrorMessage = "Mã loại phải đúng 6 ký tự")]
    [RegularExpression(@"^LSP(00[1-9]|0[1-9]\d|[1-9]\d{2})$",
    ErrorMessage = "Mã loại phải từ LSP001 trở lên")]
    [Unicode(false)]
    [Display(Name = "Mã loại")]
    public string MaLoai { get; set; } = null!;

    //code kiểm tra ràng buộc dữ liệu
    [Required(ErrorMessage = "Tên loại không được để trống")]
    //
    [StringLength(100)]
    //code hiển thị tên trường trong form
    [Display(Name = "Tên loại")]
    //
    public string TenLoai { get; set; } = null!;

    [Display(Name = "Ghi chú")]
    public string? GhiChu { get; set; }

    //kiểm tra ràng buộc dữ liệu
    [ValidateNever]
    [InverseProperty("MaLoaiNavigation")]
    public virtual ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
}
