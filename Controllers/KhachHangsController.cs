
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebCafePoly.Models;


public class KhachHangsController : Controller
{
    private readonly PolyCafeContext _context;

    public KhachHangsController(PolyCafeContext context)
    {
        _context = context;
    }
    //hàm ổ khóa chỉ đăng nhập mới vào trang được
    public override void OnActionExecuting(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext context)
    {
        if (HttpContext.Session.GetString("MaNhanVien") == null)
        {
            context.Result = RedirectToAction("Login", "DangNhap");
        }

        base.OnActionExecuting(context);
    }

    // GET: KHACHHANGS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.KhachHangs.ToListAsync());
    }

    // GET: KHACHHANGS/Details/5
    public async Task<IActionResult> Details(string? makhachhang)
    {
        if (makhachhang == null)
        {
            return NotFound();
        }

        var khachhang = await _context.KhachHangs
            .FirstOrDefaultAsync(m => m.MaKhachHang == makhachhang);
        if (khachhang == null)
        {
            return NotFound();
        }

        return View(khachhang);
    }

    // GET: KHACHHANGS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: KHACHHANGS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("HoTen,Email,SoDienThoai,MatKhau,DiaChi,TrangThai")] KhachHang khachhang)
    {
        // Lấy khách hàng có mã lớn nhất
        var khCuoi = await _context.KhachHangs
            .OrderByDescending(x => x.MaKhachHang)
            .FirstOrDefaultAsync();

        string maMoi = "KH001";

        if (khCuoi != null)
        {
            int so = 0;
            int.TryParse(khCuoi.MaKhachHang.Substring(2), out so);
            maMoi = "KH" + (so + 1).ToString("D3");
        }

        khachhang.MaKhachHang = maMoi;

        // Xóa lỗi validation của MaKhachHang vì hệ thống tự sinh
        ModelState.Remove("MaKhachHang");
        if (ModelState.IsValid)
        {
            _context.Add(khachhang);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(khachhang);
    }

    // GET: KHACHHANGS/Edit/5
    public async Task<IActionResult> Edit(string? makhachhang)
    {
        if (makhachhang == null)
        {
            return NotFound();
        }

        var khachhang = await _context.KhachHangs.FindAsync(makhachhang);
        if (khachhang == null)
        {
            return NotFound();
        }
        return View(khachhang);
    }

    // POST: KHACHHANGS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string? makhachhang, [Bind("MaKhachHang,HoTen,Email,SoDienThoai,MatKhau,DiaChi,TrangThai")] KhachHang khachhang)
    {
        if (makhachhang != khachhang.MaKhachHang)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(khachhang);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KhachHangExists(khachhang.MaKhachHang))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(khachhang);
    }

    // GET: KHACHHANGS/Delete/5
    public async Task<IActionResult> Delete(string? makhachhang)
    {
        if (makhachhang == null)
        {
            return NotFound();
        }

        var khachhang = await _context.KhachHangs
            .FirstOrDefaultAsync(m => m.MaKhachHang == makhachhang);
        if (khachhang == null)
        {
            return NotFound();
        }

        return View(khachhang);
    }

    // POST: KHACHHANGS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string? makhachhang)
    {
        var khachhang = await _context.KhachHangs.FindAsync(makhachhang);
        if (khachhang != null)
        {
            _context.KhachHangs.Remove(khachhang);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool KhachHangExists(string? makhachhang)
    {
        return _context.KhachHangs.Any(e => e.MaKhachHang == makhachhang);
    }
}
