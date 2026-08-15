namespace WebCafePoly.Models
{
    public class GioHang
    {
        public string MaSanPham { get; set; } = "";

        public string TenSanPham { get; set; } = "";

        public decimal DonGia { get; set; }

        public int SoLuong { get; set; }

        public string? HinhAnh { get; set; }

        public decimal ThanhTien => DonGia * SoLuong;
    }
}