namespace QLCF.Model
{
    public class NguoiDung
    {
        public int MaND { get; set; }
        public string HoTen { get; set; } = string.Empty;
        public string TaiKhoan { get; set; } = string.Empty;
        public byte[]? MatKhau { get; set; } 
        public byte MaVT { get; set; }
        public bool HoatDong { get; set; }
        public DateTime TaoLuc { get; set; }
    }
}
