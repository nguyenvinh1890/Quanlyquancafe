namespace QLCF.Model
{
    public class DonHang
    {
        public int MaDH { get; set; }
        public int? MaBan { get; set; }
        public string LoaiDH { get; set; } = string.Empty;
        public string TrangThai { get; set; } = string.Empty;
        public int? MoBoi { get; set; }
        public string MoBoiTen { get; set; } = string.Empty;
        public DateTime MoLuc { get; set; }
    }
}
