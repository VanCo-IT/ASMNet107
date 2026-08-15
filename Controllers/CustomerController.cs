using Microsoft.AspNetCore.Mvc;
using WebCafePoly.Models;
using System.Text.Json;
using WebCafePoly.Extensions;
using Microsoft.EntityFrameworkCore;

namespace WebCafePoly.Controllers
{
    public class CustomerController : Controller
    {
        // Kiểm tra đăng nhập trước khi vào các trang Customer
        public override void OnActionExecuting(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext context)
        {
            if (HttpContext.Session.GetString("MaKhachHang") == null)
            {
                context.Result = RedirectToAction("DangNhap", "GuestAccount");
            }

            base.OnActionExecuting(context);
        }

        // Trang chủ Customer
        public IActionResult Index()
        {
            ViewBag.HoTen = HttpContext.Session.GetString("HoTenKhachHang");
            return View();
        }
        private readonly PolyCafeContext _context;

        public CustomerController(PolyCafeContext context)
        {
            _context = context;
        }
        public IActionResult HoSoKhachHang()
        {
            string maKH = HttpContext.Session.GetString("MaKhachHang");

            var kh = _context.KhachHangs.FirstOrDefault(x => x.MaKhachHang == maKH);

            if (kh == null)
            {
                return RedirectToAction("DangNhap", "GuestAccount");
            }

            return View(kh);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Profile(KhachHang khachHang)
        {
            if (!ModelState.IsValid)
            {
                return View(khachHang);
            }

            var kh = _context.KhachHangs
                .FirstOrDefault(x => x.MaKhachHang == khachHang.MaKhachHang);

            if (kh == null)
            {
                return NotFound();
            }

            kh.HoTen = khachHang.HoTen;
            kh.Email = khachHang.Email;
            kh.SoDienThoai = khachHang.SoDienThoai;
            kh.DiaChi = khachHang.DiaChi;
            kh.MatKhau = khachHang.MatKhau;

            _context.SaveChanges();

            ViewBag.ThongBao = "Cập nhật thành công";

            return View(kh);
        }
        public IActionResult ThemVaoGio(string id)
        {
            var sanPham = _context.SanPhams.FirstOrDefault(x => x.MaSanPham == id);

            if (sanPham == null)
            {
                return NotFound();
            }

            var gioHang = HttpContext.Session.GetObject<List<GioHang>>("GioHang");

            if (gioHang == null)
            {
                gioHang = new List<GioHang>();
            }

            var item = gioHang.FirstOrDefault(x => x.MaSanPham == id);

            if (item == null)
            {
                gioHang.Add(new GioHang
                {
                    MaSanPham = sanPham.MaSanPham,
                    TenSanPham = sanPham.TenSanPham,
                    DonGia = sanPham.DonGia,
                    SoLuong = 1,
                    HinhAnh = sanPham.HinhAnh
                });
            }
            else
            {
                item.SoLuong++;
            }

            HttpContext.Session.SetObject("GioHang", gioHang);

            return RedirectToAction("GioHang");
        }
        public IActionResult GioHang()
        {
            var gioHang = HttpContext.Session.GetObject<List<GioHang>>("GioHang");

            if (gioHang == null)
            {
                gioHang = new List<GioHang>();
            }

            return View(gioHang);
        }
        public IActionResult TangSoLuong(string id)
        {
            var gioHang = HttpContext.Session.GetObject<List<GioHang>>("GioHang");

            if (gioHang != null)
            {
                var item = gioHang.FirstOrDefault(x => x.MaSanPham == id);

                if (item != null)
                {
                    item.SoLuong++;
                }

                HttpContext.Session.SetObject("GioHang", gioHang);
            }

            return RedirectToAction("GioHang");
        }
        public IActionResult GiamSoLuong(string id)
        {
            var gioHang = HttpContext.Session.GetObject<List<GioHang>>("GioHang");

            if (gioHang != null)
            {
                var item = gioHang.FirstOrDefault(x => x.MaSanPham == id);

                if (item != null)
                {
                    item.SoLuong--;

                    if (item.SoLuong <= 0)
                    {
                        gioHang.Remove(item);
                    }
                }

                HttpContext.Session.SetObject("GioHang", gioHang);
            }

            return RedirectToAction("GioHang");
        }
        private string TaoMaPhieu()
        {
            var maCuoi = _context.PhieuBanHangs
                .OrderByDescending(x => x.MaPhieu)
                .Select(x => x.MaPhieu)
                .FirstOrDefault();

            if (string.IsNullOrEmpty(maCuoi))
                return "PBH001";

            int so = int.Parse(maCuoi.Substring(3));

            return $"PBH{(so + 1):D3}";
        }
        public IActionResult ThanhToan()
        {
            var gioHang = HttpContext.Session.GetObject<List<GioHang>>("GioHang");

            if (gioHang == null || !gioHang.Any())
            {
                return RedirectToAction("GioHang");
            }

            string maPhieu = TaoMaPhieu();

            var phieu = new PhieuBanHang
            {
                MaPhieu = maPhieu,
                MaKhachHang = HttpContext.Session.GetString("MaKhachHang"),
                MaNhanVien = "NV001",
                MaThe = "THE001",
                NgayTao = DateTime.Now,
                TrangThai = true
            };

            _context.PhieuBanHangs.Add(phieu);
            foreach (var item in gioHang)
            {
                ChiTietPhieu ct = new ChiTietPhieu
                {
                    MaPhieu = maPhieu,
                    MaSanPham = item.MaSanPham,
                    SoLuong = item.SoLuong,
                    DonGia = item.DonGia
                };

                _context.ChiTietPhieus.Add(ct);
            }

            _context.SaveChanges();

            HttpContext.Session.Remove("GioHang");

            return RedirectToAction("GioHang");
        }
        public IActionResult LichSuDonHang()
        {
            string maKH = HttpContext.Session.GetString("MaKhachHang");

            var ds = _context.PhieuBanHangs
                .Where(x => x.MaKhachHang == maKH)
                .OrderByDescending(x => x.NgayTao)
                .ToList();

            return View(ds);
        }
        public IActionResult ChiTietHoaDon(string id)
        {
            var chiTiet = _context.ChiTietPhieus
                .Include(x => x.MaSanPhamNavigation)
                .Include(x => x.MaPhieuNavigation)
                .Where(x => x.MaPhieu == id)
                .ToList();

            if (!chiTiet.Any())
            {
                return NotFound();
            }

            return View(chiTiet);
        }
    }
}
