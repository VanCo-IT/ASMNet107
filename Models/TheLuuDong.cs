using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebCafePoly.Models;

[Table("TheLuuDong")]
public partial class TheLuuDong
{
    [Key]
    [StringLength(6)]
    [Unicode(false)]
    [Display(Name = "Mã thẻ")]
    [Required(ErrorMessage = "Không được để trống mã thẻ")]
    public string MaThe { get; set; } = null!;

    [StringLength(100)]
    [Display(Name = "Chủ sỡ hữu")]
    [Required(ErrorMessage = "Không được để trống chủ sỡ hữu")]
    public string ChuSoHuu { get; set; } = null!;

    public bool TrangThai { get; set; }

    [InverseProperty("MaTheNavigation")]
    public virtual ICollection<PhieuBanHang> PhieuBanHangs { get; set; } = new List<PhieuBanHang>();
}
