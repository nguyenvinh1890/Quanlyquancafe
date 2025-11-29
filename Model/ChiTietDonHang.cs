namespace QLCF.Model
{
    public class ChiTietDonHang
    {
        public int MaCT { get; set; }
        public int MaDH { get; set; }
        public int MaMon { get; set; }
        public int? MaSize { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien { get; set; }

        public string? TenMon { get; set; }
        public string? TenSize { get; set; }

        // Danh sách topping đã chọn (từ frontend)
        public List<ToppingSelected>? Toppings { get; set; }
    }

    public class ToppingSelected
    {
        public int MaLuaChon { get; set; }
        public decimal GiaThem { get; set; }
    }
}
