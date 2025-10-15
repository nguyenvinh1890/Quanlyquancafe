namespace QLCF.Model
{
    public class ThanhToan
    {
        public int MaTT { get; set; }
        public int MaHD { get; set; }
        public string PhuongThuc { get; set; } = string.Empty;
        public decimal SoTien { get; set; }
        public int? ThuBoi { get; set; }
        public string ThuBoiTen { get; set; } = string.Empty;
        public DateTime TraLuc { get; set; }
    }
}
