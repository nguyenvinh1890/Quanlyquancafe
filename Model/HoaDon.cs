namespace QLCF.Model
{
    public class HoaDon
    {
        public int MaHD { get; set; }
        public int MaDH { get; set; }
        public string TrangThai { get; set; } = string.Empty;
        public decimal TamTinh { get; set; }
        public decimal GiamGia { get; set; }
        public decimal Thue { get; set; }
        public DateTime TaoLuc { get; set; }
    }
}
