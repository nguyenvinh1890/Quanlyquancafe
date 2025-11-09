namespace QLCF.Model
{
    public class SizeMon
    {
        public int MaSize { get; set; }
        public int MaMon { get; set; }
        public string TenSize { get; set; } = string.Empty;
        public decimal Gia { get; set; }
        public string? TenMon { get; set; } // Tên món (để hiển thị)
    }
}
