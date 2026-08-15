using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebCafePoly.Models;
using Microsoft.EntityFrameworkCore;

namespace WebCafePoly.Controllers
{
    public class HomeController : Controller
    {
        private readonly PolyCafeContext _context;

        public HomeController(PolyCafeContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            ViewBag.SoSanPham = _context.SanPhams.Count();

            ViewBag.SoKhachHang = _context.KhachHangs.Count();

            ViewBag.SoNhanVien = _context.NhanViens.Count();

            ViewBag.SoPhieuBan = _context.PhieuBanHangs.Count();

            ViewBag.HoTen = HttpContext.Session.GetString("HoTen");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
