namespace QLCF.Model
{
    public class ChiTietDonHang
    {
        public int MaDH { get; set; }
        public int MaMon { get; set; }
        public int? MaSize { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien { get; set; }

        public string? TenMon { get; set; } 
        public string? TenSize { get; set; }
    }
}
