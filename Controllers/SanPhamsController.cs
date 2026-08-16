using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebCafePoly.Models;



public class SanPhamsController : AdminController
{
    private readonly PolyCafeContext _context;

    public SanPhamsController(PolyCafeContext context)
    {
        _context = context;
    }
    // GET: SANPHAMS

    
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
    public async Task<IActionResult> Create([Bind("MaSanPham,TenSanPham,DonGia,MaLoai,TrangThai")] SanPham sanpham,IFormFile? imageFile)
    {
        // Kiểm tra mã sản phẩm đã tồn tại
        if (_context.SanPhams.Any(x => x.MaSanPham == sanpham.MaSanPham))
        {
            ModelState.AddModelError(
                "MaSanPham",
                "Mã sản phẩm này đã tồn tại. Vui lòng nhập mã khác."
            );
        }
        //thêm anh vào thư mục wwwroot/images/products
        if (ModelState.IsValid)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);

                var folderPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images",
                    "products");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                sanpham.HinhAnh = fileName;
            }
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
    public async Task<IActionResult> Edit(
        string? masanpham,
        [Bind("MaSanPham,TenSanPham,DonGia,MaLoai,TrangThai")] SanPham sanpham,
        IFormFile? imageFile)
    {
        if (masanpham != sanpham.MaSanPham)
        {
            return NotFound();
        }

        // Lấy sản phẩm cũ
        var sanPhamCu = await _context.SanPhams
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.MaSanPham == masanpham);

        if (sanPhamCu == null)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            // Giữ ảnh cũ
            sanpham.HinhAnh = sanPhamCu.HinhAnh;

            // Nếu chọn ảnh mới
            if (imageFile != null && imageFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString()
                            + Path.GetExtension(imageFile.FileName);

                var folderPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images",
                    "products");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                // Xóa ảnh cũ
                if (!string.IsNullOrEmpty(sanPhamCu.HinhAnh))
                {
                    var oldFilePath = Path.Combine(
                        folderPath,
                        sanPhamCu.HinhAnh);

                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                sanpham.HinhAnh = fileName;
            }

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

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        ViewBag.MaLoai = new SelectList(
            _context.LoaiSanPhams,
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
