
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebCafePoly.Models;


public class LoaiSanPhamsController : Controller
{
    private readonly PolyCafeContext _context;

    public LoaiSanPhamsController(PolyCafeContext context)
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

    // GET: LOAISANPHAMS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.LoaiSanPhams.ToListAsync());
    }

    // GET: LOAISANPHAMS/Details/5
    public async Task<IActionResult> Details(string? maloai)
    {
        if (maloai == null)
        {
            return NotFound();
        }

        var loaisanpham = await _context.LoaiSanPhams
            .FirstOrDefaultAsync(m => m.MaLoai == maloai);
        if (loaisanpham == null)
        {
            return NotFound();
        }

        return View(loaisanpham);
    }

    // GET: LOAISANPHAMS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: LOAISANPHAMS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("MaLoai,TenLoai,GhiChu")] LoaiSanPham loaisanpham)
    {
        if (_context.LoaiSanPhams.Any(x => x.MaLoai == loaisanpham.MaLoai))
        {
            ModelState.AddModelError("MaLoai",
                "Mã loại sản phẩm bạn vừa nhập đã có trên hệ thống.");
        }

        if (ModelState.IsValid)
        {
            _context.Add(loaisanpham);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(loaisanpham);
    }

    // GET: LOAISANPHAMS/Edit/5
    public async Task<IActionResult> Edit(string? maloai)
    {
        if (maloai == null)
        {
            return NotFound();
        }

        var loaisanpham = await _context.LoaiSanPhams.FindAsync(maloai);
        if (loaisanpham == null)
        {
            return NotFound();
        }
        return View(loaisanpham);
    }

    // POST: LOAISANPHAMS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string? maloai, [Bind("MaLoai,TenLoai,GhiChu")] LoaiSanPham loaisanpham)
    {
        if (maloai != loaisanpham.MaLoai)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(loaisanpham);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoaiSanPhamExists(loaisanpham.MaLoai))
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
        return View(loaisanpham);
    }

    // GET: LOAISANPHAMS/Delete/5
    public async Task<IActionResult> Delete(string? maloai)
    {
        if (maloai == null)
        {
            return NotFound();
        }

        var loaisanpham = await _context.LoaiSanPhams
            .FirstOrDefaultAsync(m => m.MaLoai == maloai);
        if (loaisanpham == null)
        {
            return NotFound();
        }

        return View(loaisanpham);
    }

    // POST: LOAISANPHAMS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string? maloai)
    {
        var loaisanpham = await _context.LoaiSanPhams.FindAsync(maloai);
        if (loaisanpham != null)
        {
            _context.LoaiSanPhams.Remove(loaisanpham);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool LoaiSanPhamExists(string? maloai)
    {
        return _context.LoaiSanPhams.Any(e => e.MaLoai == maloai);
    }
}
