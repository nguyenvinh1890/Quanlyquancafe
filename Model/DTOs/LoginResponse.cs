namespace QLCF.Model.DTOs
{
    // K?t qu? tr? v? c?a API ��ng nh?p
    public class LoginResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public UserInfo? Data { get; set; }
    }

    // Th�ng tin ng�?i d�ng sau khi ��ng nh?p th�nh c�ng
    public class UserInfo
    {
        public int MaND { get; set; }
        public string HoTen { get; set; } = string.Empty;
        public string TaiKhoan { get; set; } = string.Empty;
        public byte MaVT { get; set; }
        public string TenVaiTro { get; set; } = string.Empty;
        public bool HoatDong { get; set; }

      
        public string? VaiTroMoTa { get; set; }
    }
}
