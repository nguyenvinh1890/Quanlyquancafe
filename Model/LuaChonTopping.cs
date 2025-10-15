namespace QLCF.Model
{
    public class LuaChonTopping
    {
        public int MaLuaChon { get; set; }
        public int MaTopping { get; set; }
        public string TenTopping { get; set; } = string.Empty;
        public string TenLuaChon { get; set; } = string.Empty;
        public decimal GiaThem { get; set; }
    }
}
