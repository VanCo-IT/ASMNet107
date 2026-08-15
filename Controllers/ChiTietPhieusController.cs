
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebCafePoly.Models;

public class ChiTietPhieusController : Controller
{
    private readonly PolyCafeContext _context;

    public ChiTietPhieusController(PolyCafeContext context)
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

    // GET: CHITIETPHIEUS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.ChiTietPhieus.ToListAsync());
    }

    // GET: CHITIETPHIEUS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var chitietphieu = await _context.ChiTietPhieus
            .FirstOrDefaultAsync(m => m.Id == id);
        if (chitietphieu == null)
        {
            return NotFound();
        }

        return View(chitietphieu);
    }

    // GET: CHITIETPHIEUS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: CHITIETPHIEUS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,MaPhieu,MaSanPham,SoLuong,DonGia,MaPhieuNavigation,MaSanPhamNavigation")] ChiTietPhieu chitietphieu)
    {
        if (ModelState.IsValid)
        {
            _context.Add(chitietphieu);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(chitietphieu);
    }

    // GET: CHITIETPHIEUS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var chitietphieu = await _context.ChiTietPhieus.FindAsync(id);
        if (chitietphieu == null)
        {
            return NotFound();
        }
        return View(chitietphieu);
    }

    // POST: CHITIETPHIEUS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,MaPhieu,MaSanPham,SoLuong,DonGia,MaPhieuNavigation,MaSanPhamNavigation")] ChiTietPhieu chitietphieu)
    {
        if (id != chitietphieu.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(chitietphieu);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChiTietPhieuExists(chitietphieu.Id))
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
        return View(chitietphieu);
    }

    // GET: CHITIETPHIEUS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var chitietphieu = await _context.ChiTietPhieus
            .FirstOrDefaultAsync(m => m.Id == id);
        if (chitietphieu == null)
        {
            return NotFound();
        }

        return View(chitietphieu);
    }

    // POST: CHITIETPHIEUS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var chitietphieu = await _context.ChiTietPhieus.FindAsync(id);
        if (chitietphieu != null)
        {
            _context.ChiTietPhieus.Remove(chitietphieu);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ChiTietPhieuExists(int? id)
    {
        return _context.ChiTietPhieus.Any(e => e.Id == id);
    }
}
