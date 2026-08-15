using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebCafePoly.Models;

namespace WebCafePoly.Controllers
{
    public class GuestController : Controller
    {
        private readonly PolyCafeContext _context;

        public GuestController(PolyCafeContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string? keyword, string? maLoai, decimal? giaTu, decimal? giaDen)
        {
            var sanPhams = _context.SanPhams
                .Include(sp => sp.MaLoaiNavigation)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                sanPhams = sanPhams.Where(sp =>
                    sp.TenSanPham.Contains(keyword));
            }

            if (!string.IsNullOrWhiteSpace(maLoai))
            {
                sanPhams = sanPhams.Where(sp =>
                    sp.MaLoai == maLoai);
            }

            if (giaTu.HasValue)
            {
                sanPhams = sanPhams.Where(sp =>
                    sp.DonGia >= giaTu.Value);
            }

            if (giaDen.HasValue)
            {
                sanPhams = sanPhams.Where(sp =>
                    sp.DonGia <= giaDen.Value);
            }

            ViewBag.Keyword = keyword;
            ViewBag.MaLoai = maLoai;
            ViewBag.GiaTu = giaTu;
            ViewBag.GiaDen = giaDen;

            ViewBag.LoaiSanPham = await _context.LoaiSanPhams.ToListAsync();

            return View(await sanPhams.ToListAsync());
        }
        public async Task<IActionResult> ChiTiet(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var sanPham = await _context.SanPhams
                .Include(sp => sp.MaLoaiNavigation)
                .FirstOrDefaultAsync(sp => sp.MaSanPham == id);

            if (sanPham == null)
            {
                return NotFound();
            }

            return View(sanPham);
        }
    }
}
