namespace QLCF.Model
{
    public class Ban
    {
        public int MaBan { get; set; }
        public string TenBan { get; set; } = string.Empty; 
        public int MaKhu { get; set; }
        public int? SucChua { get; set; }
        public string? TrangThai { get; set; }
        public string? TenKhu { get; set; } 
    }
}
