using QLCF.DAL;
using QLCF.Model;
using QLCF.Model.DTOs;
using System;
using System.Collections.Generic;
using System.Data;

namespace QLCF.BUS
{
    public class NguoiDungBUS
    {
        private readonly DatabaseHelper _db;

        public NguoiDungBUS(DatabaseHelper db)
        {
            _db = db;
        }

        public (bool, string, List<NguoiDung>) GetAll()
        {
            try
            {
                string sql = @"
                SELECT n.ma_nd, n.ho_ten, n.tai_khoan, n.mat_khau, n.ma_vt, 
                       n.hoat_dong, n.tao_luc
                FROM nguoi_dung n";

                DataTable dt = _db.ExecuteQuery(sql);
                var list = new List<NguoiDung>();

                foreach (DataRow r in dt.Rows)
                {
                    list.Add(new NguoiDung
                    {
                        MaND = (int)r["ma_nd"],
                        HoTen = r["ho_ten"].ToString() ?? "",
                        TaiKhoan = r["tai_khoan"].ToString() ?? "",
                        MatKhauStr = r["mat_khau"].ToString() ?? "",
                        MaVT = Convert.ToByte(r["ma_vt"]),
                        HoatDong = Convert.ToBoolean(r["hoat_dong"]),
                        TaoLuc = Convert.ToDateTime(r["tao_luc"])
                    });
                }

                return (true, $"Lấy {list.Count} người dùng thành công", list);
            }
            catch (Exception ex)
            {
                return (false, "Lỗi khi lấy danh sách người dùng: " + ex.Message, new List<NguoiDung>());
            }
        }

        public string Add(NguoiDung nd)
        {
            try
            {
                // Kiểm tra tài khoản đã tồn tại chưa
                string sqlCheck = "SELECT COUNT(*) as count FROM nguoi_dung WHERE tai_khoan = @tai_khoan";
                var checkParam = new Dictionary<string, object>
                {
                    {"@tai_khoan", nd.TaiKhoan}
                };
                DataTable dtCheck = _db.ExecuteQuery(sqlCheck, checkParam);
                if (dtCheck.Rows.Count > 0)
                {
                    int count = Convert.ToInt32(dtCheck.Rows[0]["count"]);
                    if (count > 0)
                    {
                        return "Tài khoản đã tồn tại. Vui lòng chọn tài khoản khác!";
                    }
                }

                string sql = @"
                INSERT INTO nguoi_dung (ho_ten, tai_khoan, mat_khau, ma_vt, hoat_dong)
                VALUES (@ho_ten, @tai_khoan, @mat_khau, @ma_vt, @hoat_dong)";
                var param = new Dictionary<string, object>
                {
                    {"@ho_ten", nd.HoTen},
                    {"@tai_khoan", nd.TaiKhoan},
                    {"@mat_khau", nd.MatKhauStr ?? ""},
                    {"@ma_vt", nd.MaVT},
                    {"@hoat_dong", nd.HoatDong}
                };
                int rows = _db.ExecuteNonQuery(sql, param);
                return rows > 0 ? "Thêm người dùng thành công" : "Không thể thêm người dùng";
            }
            catch (Exception ex)
            {
                // Kiểm tra nếu lỗi do duplicate key
                if (ex.Message.Contains("UNIQUE KEY") || ex.Message.Contains("duplicate key"))
                {
                    return "Tài khoản đã tồn tại. Vui lòng chọn tài khoản khác!";
                }
                return "Lỗi khi thêm người dùng: " + ex.Message;
            }
        }

        public string Update(NguoiDung nd)
        {
            try
            {
                string sql = @"
                UPDATE nguoi_dung 
                SET ho_ten=@ho_ten, ma_vt=@ma_vt, hoat_dong=@hoat_dong
                WHERE ma_nd=@ma_nd";
                var param = new Dictionary<string, object>
                {
                    {"@ma_nd", nd.MaND},
                    {"@ho_ten", nd.HoTen},
                    {"@ma_vt", nd.MaVT},
                    {"@hoat_dong", nd.HoatDong}
                };
                int rows = _db.ExecuteNonQuery(sql, param);
                return rows > 0 ? "Cập nhật người dùng thành công" : "Không tìm thấy người dùng cần cập nhật";
            }
            catch (Exception ex)
            {
                return "Lỗi khi cập nhật người dùng: " + ex.Message;
            }
        }

        public string Delete(int id)
        {
            try
            {
                string sql = "DELETE FROM nguoi_dung WHERE ma_nd=@ma_nd";
                var param = new Dictionary<string, object> { { "@ma_nd", id } };
                int rows = _db.ExecuteNonQuery(sql, param);
                return rows > 0 ? "Xóa người dùng thành công" : "Không tìm thấy người dùng cần xóa";
            }
            catch (Exception ex)
            {
                return "Lỗi khi xóa người dùng: " + ex.Message;
            }
        }

        public LoginResponse Login(LoginRequest request)
        {
            try
            {
                // Kiểm tra dữ liệu đầu vào
                if (string.IsNullOrWhiteSpace(request.TaiKhoan) || string.IsNullOrWhiteSpace(request.MatKhau))
                {
                    return new LoginResponse
                    {
                        Success = false,
                        Message = "Tài khoản và mật khẩu không được để trống",
                        Data = null
                    };
                }

                // Truy vấn thông tin người dùng kèm tên vai trò
                string sql = @"
                SELECT n.ma_nd, n.ho_ten, n.tai_khoan, n.mat_khau, n.ma_vt, n.hoat_dong,
                       v.ten_vt
                FROM nguoi_dung n
                INNER JOIN vai_tro v ON n.ma_vt = v.ma_vt
                WHERE n.tai_khoan = @tai_khoan";

                var param = new Dictionary<string, object>
                {
                    { "@tai_khoan", request.TaiKhoan }
                };

                DataTable dt = _db.ExecuteQuery(sql, param);

                // Kiểm tra người dùng có tồn tại không
                if (dt.Rows.Count == 0)
                {
                    return new LoginResponse
                    {
                        Success = false,
                        Message = "Tài khoản không tồn tại",
                        Data = null
                    };
                }

                DataRow row = dt.Rows[0];

                // Kiểm tra tài khoản có đang hoạt động không
                bool hoatDong = Convert.ToBoolean(row["hoat_dong"]);
                if (!hoatDong)
                {
                    return new LoginResponse
                    {
                        Success = false,
                        Message = "Tài khoản đã bị khóa",
                        Data = null
                    };
                }

                // ✅ So sánh mật khẩu dạng chuỗi
                string storedPassword = row["mat_khau"].ToString() ?? "";
                if (storedPassword != request.MatKhau)
                {
                    return new LoginResponse
                    {
                        Success = false,
                        Message = "Mật khẩu không đúng",
                        Data = null
                    };
                }

                // Đăng nhập thành công - trả về thông tin người dùng
                return new LoginResponse
                {
                    Success = true,
                    Message = "Đăng nhập thành công",
                    Data = new UserInfo
                    {
                        MaND = (int)row["ma_nd"],
                        HoTen = row["ho_ten"].ToString() ?? "",
                        TaiKhoan = row["tai_khoan"].ToString() ?? "",
                        MaVT = Convert.ToByte(row["ma_vt"]),
                        TenVaiTro = row["ten_vt"].ToString() ?? "",
                        HoatDong = hoatDong
                    }
                };
            }
            catch (Exception ex)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "Lỗi khi đăng nhập: " + ex.Message,
                    Data = null
                };
            }
        }
    }
}
