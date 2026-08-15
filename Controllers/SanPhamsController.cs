
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebCafePoly.Models;



public class SanPhamsController : Controller
{
    private readonly PolyCafeContext _context;

    public SanPhamsController(PolyCafeContext context)
    {
        _context = context;
    }
    public override void OnActionExecuting(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext context)
    {
        if (HttpContext.Session.GetString("MaNhanVien") == null)
        {
            context.Result = RedirectToAction("Login", "DangNhap");
        }

        base.OnActionExecuting(context);
    }

    // GET: SANPHAMS

    //Thêm chức năng timnf kkieems sản phẩm ở sản phẩm
    public async Task<IActionResult> Index(string? timkiem)    
    {
        var ds =  _context.SanPhams
        .Include(x => x.MaLoaiNavigation)
        .AsQueryable();
        //.ToListAsync();
        if (!string.IsNullOrEmpty(timkiem))
        {
            ds = ds.Where(x => x.TenSanPham.Contains(timkiem));
        }

        return View(await ds.ToListAsync());
    }
    public async Task<IActionResult> TimKiem(string? timkiem)
    {
        var ds = _context.SanPhams
            .Include(x => x.MaLoaiNavigation)
            .AsQueryable();

        if (!string.IsNullOrEmpty(timkiem))
        {
            ds = ds.Where(x => x.TenSanPham.Contains(timkiem));
        }

        return PartialView("_PhanBangSanPham", await ds.ToListAsync());
    }

    // GET: SANPHAMS/Details/5
    public async Task<IActionResult> Details(string? masanpham)
    {
        if (masanpham == null)
        {
            return NotFound();
        }

        var sanpham = await _context.SanPhams
            .Include(x => x.MaLoaiNavigation)
            .FirstOrDefaultAsync(m => m.MaSanPham == masanpham);
        if (sanpham == null)
        {
            return NotFound();
        }

        return View(sanpham);
    }

    // GET: SANPHAMS/Create
    public IActionResult Create()
    {
        ViewBag.MaLoai = new SelectList(_context.LoaiSanPhams, "MaLoai", "TenLoai");
        return View();
    }

    // POST: SANPHAMS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("MaSanPham,TenSanPham,DonGia,MaLoai,HinhAnh,TrangThai")] SanPham sanpham)
    {
        if (ModelState.IsValid)
        {
            _context.Add(sanpham);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.MaLoai = new SelectList(
            _context.LoaiSanPhams,
            "MaLoai",
            "TenLoai",
            sanpham.MaLoai);
        return View(sanpham);
    }

    // GET: SANPHAMS/Edit/5
    public async Task<IActionResult> Edit(string? masanpham)
    {
        if (masanpham == null)
        {
            return NotFound();
        }

        var sanpham = await _context.SanPhams.FindAsync(masanpham);
        if (sanpham == null)
        {
            return NotFound();
        }
        ViewBag.MaLoai = new SelectList(
            _context.LoaiSanPhams,
            "MaLoai",
            "TenLoai",
            sanpham.MaLoai);

        return View(sanpham);
    }

    // POST: SANPHAMS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string? masanpham, [Bind("MaSanPham,TenSanPham,DonGia,MaLoai,HinhAnh,TrangThai")] SanPham sanpham)
    {
        if (masanpham != sanpham.MaSanPham)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(sanpham);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SanPhamExists(sanpham.MaSanPham))
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
        ViewBag.MaLoai = new SelectList(_context.LoaiSanPhams,
                                "MaLoai",
                                "TenLoai",
                                sanpham.MaLoai);
        return View(sanpham);
    }

    // GET: SANPHAMS/Delete/5
    public async Task<IActionResult> Delete(string? masanpham)
    {
        if (masanpham == null)
        {
            return NotFound();
        }

        var sanpham = await _context.SanPhams
            .FirstOrDefaultAsync(m => m.MaSanPham == masanpham);
        if (sanpham == null)
        {
            return NotFound();
        }

        return View(sanpham);
    }

    // POST: SANPHAMS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string? masanpham)
    {
        var sanpham = await _context.SanPhams.FindAsync(masanpham);
        if (sanpham != null)
        {
            _context.SanPhams.Remove(sanpham);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool SanPhamExists(string? masanpham)
    {
        return _context.SanPhams.Any(e => e.MaSanPham == masanpham);
    }
}
