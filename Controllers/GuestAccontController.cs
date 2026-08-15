using Microsoft.AspNetCore.Mvc;
using WebCafePoly.Models;
using Microsoft.EntityFrameworkCore;

namespace WebCafePoly.Controllers
{
    public class GuestAccountController : Controller
    {
        private readonly PolyCafeContext _context;

        public GuestAccountController(PolyCafeContext context)
        {
            _context = context;
        }

        public IActionResult DangKy()
        {
            return View();
        }
        // GET
        public IActionResult DangNhap()
        {
            if (HttpContext.Session.GetString("MaKhachHang") != null)
            {
                return RedirectToAction("HoSoKhachHang", "Customer");
            }

            ViewBag.Email = Request.Cookies["Email"];
            return View();
        }
        private string TaoMaKhachHang()
        {
            var maCuoi = _context.KhachHangs
                .OrderByDescending(k => k.MaKhachHang)
                .Select(k => k.MaKhachHang)
                .FirstOrDefault();

            if (string.IsNullOrEmpty(maCuoi))
                return "KH001";

            int so = int.Parse(maCuoi.Substring(2));

            return $"KH{(so + 1):D3}";
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DangKy(KhachHang khachHang)
        {
            khachHang.MaKhachHang = TaoMaKhachHang();
            khachHang.TrangThai = true;

            // Xóa lỗi validation của mã khách hàng
            ModelState.Remove(nameof(KhachHang.MaKhachHang));

            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => $"{x.Key}: {string.Join(", ", x.Value.Errors.Select(e => e.ErrorMessage))}");

                return Content(string.Join("<br>", errors));
            }

            // Kiểm tra Email đã tồn tại chưa
            bool daTonTai = await _context.KhachHangs
                .AnyAsync(k => k.Email == khachHang.Email);

            if (daTonTai)
            {
                ModelState.AddModelError("Email", "Email này đã được sử dụng.");
                return View(khachHang);
            }

            // Thêm khách hàng
            _context.KhachHangs.Add(khachHang);

            // Lưu vào SQL Server
            await _context.SaveChangesAsync();

            // Thông báo thành công
            return Content("Đăng ký thành công");

        }

        // POST
        [HttpPost]
        public IActionResult DangNhap(string email, string matKhau, bool remember)
        {
            var kh = _context.KhachHangs.FirstOrDefault(x =>
                x.Email == email &&
                x.MatKhau == matKhau &&
                x.TrangThai == true);

            if (kh == null)
            {
                ViewBag.Loi = "Sai email hoặc mật khẩu";
                return View();
            }

            HttpContext.Session.SetString("MaKhachHang", kh.MaKhachHang);
            HttpContext.Session.SetString("HoTenKhachHang", kh.HoTen);

            if (remember)
            {
                CookieOptions options = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(30)
                };

                Response.Cookies.Append("Email", email, options);
            }
            else
            {
                Response.Cookies.Delete("Email");
            }

            return RedirectToAction("Index", "Guest");
        }
        public IActionResult DangXuat()
        {
            HttpContext.Session.Remove("MaKhachHang");
            HttpContext.Session.Remove("HoTenKhachHang");

            return RedirectToAction("DangNhap");
        }
    }
}