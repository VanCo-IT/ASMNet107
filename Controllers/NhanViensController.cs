
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebCafePoly.Models;

public class NhanViensController : Controller
{
    private readonly PolyCafeContext _context;

    public NhanViensController(PolyCafeContext context)
    {
        _context = context;
    }
    //hàm ổ khóa chỉ đăng nhập mới vào trang được
    public override void OnActionExecuting(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext context)
    {
        if (HttpContext.Session.GetString("MaNhanVien") == null)
        {
            context.Result = RedirectToAction("Login", "DangNhap");
            return;
        }
        //hàm phân quyền truy cập
        if (HttpContext.Session.GetString("VaiTro") != "True")
        {
            context.Result = Content("Bạn không có quyền truy cập.");
            return;
        }

        base.OnActionExecuting(context);
    }

    // GET: NHANVIENS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.NhanViens.ToListAsync());
    }

    // GET: NHANVIENS/Details/5
    public async Task<IActionResult> Details(string? manhanvien)
    {
        if (manhanvien == null)
        {
            return NotFound();
        }

        var nhanvien = await _context.NhanViens
            .FirstOrDefaultAsync(m => m.MaNhanVien == manhanvien);
        if (nhanvien == null)
        {
            return NotFound();
        }

        return View(nhanvien);
    }

    // GET: NHANVIENS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: NHANVIENS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("MaNhanVien,HoTen,Email,MatKhau,VaiTro,TrangThai")] NhanVien nhanvien)
    {
        var nhanVienCuoi = await _context.NhanViens
            .OrderByDescending(x => x.MaNhanVien)
            .FirstOrDefaultAsync();

        string maMoi = "NV001";

        if (nhanVienCuoi != null)
        {
            int so = 0;
            int.TryParse(nhanVienCuoi.MaNhanVien.Substring(2), out so);
            maMoi = "NV" + (so + 1).ToString("D3");
        }

        nhanvien.MaNhanVien = maMoi;

        ModelState.Remove("MaNhanVien");

        if (ModelState.IsValid)
        {
            _context.Add(nhanvien);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(nhanvien);
    }
    // GET: NHANVIENS/Edit/5
    public async Task<IActionResult> Edit(string? manhanvien)
    {
        if (manhanvien == null)
        {
            return NotFound();
        }

        var nhanvien = await _context.NhanViens.FindAsync(manhanvien);
        if (nhanvien == null)
        {
            return NotFound();
        }
        return View(nhanvien);
    }

    // POST: NHANVIENS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string? manhanvien, [Bind("MaNhanVien,HoTen,Email,MatKhau,VaiTro,TrangThai")] NhanVien nhanvien)
    {
        if (manhanvien != nhanvien.MaNhanVien)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(nhanvien);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NhanVienExists(nhanvien.MaNhanVien))
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
        return View(nhanvien);
    }

    // GET: NHANVIENS/Delete/5
    public async Task<IActionResult> Delete(string? manhanvien)
    {
        if (manhanvien == null)
        {
            return NotFound();
        }

        var nhanvien = await _context.NhanViens
            .FirstOrDefaultAsync(m => m.MaNhanVien == manhanvien);
        if (nhanvien == null)
        {
            return NotFound();
        }

        return View(nhanvien);
    }

    // POST: NHANVIENS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string? manhanvien)
    {
        var nhanvien = await _context.NhanViens.FindAsync(manhanvien);
        if (nhanvien != null)
        {
            _context.NhanViens.Remove(nhanvien);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool NhanVienExists(string? manhanvien)
    {
        return _context.NhanViens.Any(e => e.MaNhanVien == manhanvien);
    }
}
