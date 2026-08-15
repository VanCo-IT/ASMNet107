
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebCafePoly.Models;

public class PhieuBanHangsController : Controller
{
    private readonly PolyCafeContext _context;

    public PhieuBanHangsController(PolyCafeContext context)
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
    // GET: PHIEUBANHANGS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.PhieuBanHangs.ToListAsync());
    }

    // GET: PHIEUBANHANGS/Details/5
    public async Task<IActionResult> Details(string? maphieu)
    {
        if (maphieu == null)
        {
            return NotFound();
        }

        var phieubanhang = await _context.PhieuBanHangs
            .FirstOrDefaultAsync(m => m.MaPhieu == maphieu);
        if (phieubanhang == null)
        {
            return NotFound();
        }

        return View(phieubanhang);
    }

    // GET: PHIEUBANHANGS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: PHIEUBANHANGS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("MaPhieu,MaKhachHang,MaThe,MaNhanVien,NgayTao,TrangThai")] PhieuBanHang phieubanhang)
    {
        if (await _context.PhieuBanHangs.AnyAsync(x => x.MaPhieu == phieubanhang.MaPhieu))
        {
            ModelState.AddModelError("MaPhieu", "Mã phiếu này đã tồn tại trên hệ thống.");
        }
        if (ModelState.IsValid)
        {
            _context.Add(phieubanhang);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(phieubanhang);
    }

    // GET: PHIEUBANHANGS/Edit/5
    public async Task<IActionResult> Edit(string? maphieu)
    {
        if (maphieu == null)
        {
            return NotFound();
        }

        var phieubanhang = await _context.PhieuBanHangs.FindAsync(maphieu);
        if (phieubanhang == null)
        {
            return NotFound();
        }
        return View(phieubanhang);
    }

    // POST: PHIEUBANHANGS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string? maphieu, [Bind("MaPhieu,MaKhachHang,MaThe,MaNhanVien,NgayTao,TrangThai")] PhieuBanHang phieubanhang)
    {
        if (maphieu != phieubanhang.MaPhieu)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(phieubanhang);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhieuBanHangExists(phieubanhang.MaPhieu))
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
        return View(phieubanhang);
    }

    // GET: PHIEUBANHANGS/Delete/5
    public async Task<IActionResult> Delete(string? maphieu)
    {
        if (maphieu == null)
        {
            return NotFound();
        }

        var phieubanhang = await _context.PhieuBanHangs
            .FirstOrDefaultAsync(m => m.MaPhieu == maphieu);
        if (phieubanhang == null)
        {
            return NotFound();
        }

        return View(phieubanhang);
    }

    // POST: PHIEUBANHANGS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string? maphieu)
    {
        var phieubanhang = await _context.PhieuBanHangs.FindAsync(maphieu);
        if (phieubanhang != null)
        {
            _context.PhieuBanHangs.Remove(phieubanhang);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool PhieuBanHangExists(string? maphieu)
    {
        return _context.PhieuBanHangs.Any(e => e.MaPhieu == maphieu);
    }
}
