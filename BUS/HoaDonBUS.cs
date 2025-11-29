using QLCF.DAL;
using QLCF.Model;
using System;
using System.Collections.Generic;
using System.Data;

namespace QLCF.BUS
{
    public class HoaDonBUS
    {
        private readonly DatabaseHelper _db;

        public HoaDonBUS(DatabaseHelper db)
        {
            _db = db;
        }

        public (bool, string, List<HoaDon>) GetAll()
        {
            try
            {
                string sql = @"
                SELECT ma_hd, ma_dh, trang_thai, tam_tinh, giam_gia, thue, tao_luc
                FROM hoa_don";

                DataTable dt = _db.ExecuteQuery(sql);
                var list = new List<HoaDon>();

                foreach (DataRow r in dt.Rows)
                {
                    list.Add(new HoaDon
                    {
                        MaHD = (int)r["ma_hd"],
                        MaDH = (int)r["ma_dh"],
                        TrangThai = r["trang_thai"].ToString() ?? "",
                        TamTinh = Convert.ToDecimal(r["tam_tinh"]),
                        GiamGia = Convert.ToDecimal(r["giam_gia"]),
                        Thue = Convert.ToDecimal(r["thue"]),
                        TaoLuc = Convert.ToDateTime(r["tao_luc"])
                    });
                }

                return (true, $"Lấy {list.Count} hóa đơn thành công", list);
            }
            catch (Exception ex)
            {
                return (false, "Lỗi khi lấy danh sách hóa đơn: " + ex.Message, new List<HoaDon>());
            }
        }

        public string Add(HoaDon hd)
        {
            try
            {
                // Kiểm tra xem đơn hàng đã có hóa đơn chưa
                string sqlCheck = "SELECT COUNT(*) as count FROM hoa_don WHERE ma_dh = @ma_dh";
                var checkParam = new Dictionary<string, object>
                {
                    {"@ma_dh", hd.MaDH}
                };

                DataTable dtCheck = _db.ExecuteQuery(sqlCheck, checkParam);
                if (dtCheck.Rows.Count > 0)
                {
                    int count = Convert.ToInt32(dtCheck.Rows[0]["count"]);
                    if (count > 0)
                    {
                        return "Đơn hàng này đã có hóa đơn. Mỗi đơn hàng chỉ được tạo 1 hóa đơn!";
                    }
                }

                // Nếu không có tao_luc hoặc là giá trị mặc định, tự động set là thời gian hiện tại
                if (hd.TaoLuc == default(DateTime) || hd.TaoLuc < new DateTime(1753, 1, 1))
                {
                    hd.TaoLuc = DateTime.Now;
                }

                string sql = @"INSERT INTO hoa_don (ma_dh, trang_thai, tam_tinh, giam_gia, thue, tao_luc)
                               VALUES (@ma_dh, @trang_thai, @tam_tinh, @giam_gia, @thue, @tao_luc)";
                var param = new Dictionary<string, object>
                {
                    {"@ma_dh", hd.MaDH},
                    {"@trang_thai", hd.TrangThai ?? "Chưa thanh toán"},
                    {"@tam_tinh", hd.TamTinh},
                    {"@giam_gia", hd.GiamGia},
                    {"@thue", hd.Thue},
                    {"@tao_luc", hd.TaoLuc}
                };

                int rows = _db.ExecuteNonQuery(sql, param);
                return rows > 0 ? "Thêm hóa đơn thành công" : "Không thể thêm hóa đơn";
            }
            catch (Exception ex)
            {
                return "Lỗi khi thêm hóa đơn: " + ex.Message;
            }
        }

        public string Update(HoaDon hd)
        {
            try
            {
                // Lấy thông tin hóa đơn hiện tại
                string sqlGetCurrent = "SELECT ma_dh FROM hoa_don WHERE ma_hd = @ma_hd";
                var paramGetCurrent = new Dictionary<string, object> { { "@ma_hd", hd.MaHD } };
                DataTable dtCurrent = _db.ExecuteQuery(sqlGetCurrent, paramGetCurrent);

                if (dtCurrent.Rows.Count == 0)
                {
                    return "Không tìm thấy hóa đơn cần cập nhật";
                }

                int currentMaDH = Convert.ToInt32(dtCurrent.Rows[0]["ma_dh"]);

                // Nếu thay đổi đơn hàng (ma_dh), kiểm tra đơn hàng mới đã có hóa đơn chưa
                if (hd.MaDH != currentMaDH)
                {
                    string sqlCheck = "SELECT COUNT(*) as count FROM hoa_don WHERE ma_dh = @ma_dh AND ma_hd != @ma_hd";
                    var checkParam = new Dictionary<string, object>
                    {
                        {"@ma_dh", hd.MaDH},
                        {"@ma_hd", hd.MaHD}
                    };

                    DataTable dtCheck = _db.ExecuteQuery(sqlCheck, checkParam);
                    if (dtCheck.Rows.Count > 0)
                    {
                        int count = Convert.ToInt32(dtCheck.Rows[0]["count"]);
                        if (count > 0)
                        {
                            return "Đơn hàng này đã có hóa đơn khác. Mỗi đơn hàng chỉ được có 1 hóa đơn!";
                        }
                    }
                }

                string sql = @"UPDATE hoa_don 
                               SET ma_dh=@ma_dh, trang_thai=@trang_thai, tam_tinh=@tam_tinh, giam_gia=@giam_gia, thue=@thue 
                               WHERE ma_hd=@ma_hd";
                var param = new Dictionary<string, object>
                {
                    {"@ma_hd", hd.MaHD},
                    {"@ma_dh", hd.MaDH},
                    {"@trang_thai", hd.TrangThai},
                    {"@tam_tinh", hd.TamTinh},
                    {"@giam_gia", hd.GiamGia},
                    {"@thue", hd.Thue}
                };

                int rows = _db.ExecuteNonQuery(sql, param);
                return rows > 0 ? "Cập nhật hóa đơn thành công" : "Không tìm thấy hóa đơn cần cập nhật";
            }
            catch (Exception ex)
            {
                return "Lỗi khi cập nhật hóa đơn: " + ex.Message;
            }
        }

        public string Delete(int id)
        {
            try
            {
                // Kiểm tra xem hóa đơn có tồn tại không và lấy trạng thái
                string sqlCheckExist = @"
                    SELECT ma_hd, trang_thai 
                    FROM hoa_don 
                    WHERE ma_hd = @ma_hd";
                var checkExistParam = new Dictionary<string, object> { { "@ma_hd", id } };
                DataTable dtExist = _db.ExecuteQuery(sqlCheckExist, checkExistParam);
                if (dtExist.Rows.Count == 0)
                {
                    return "Không tìm thấy hóa đơn cần xóa";
                }

                // Lấy trạng thái của hóa đơn
                string trangThai = dtExist.Rows[0]["trang_thai"]?.ToString() ?? "";

                // Chỉ cho phép xóa khi trạng thái là "Đã thanh toán" hoặc "Hủy"
                if (trangThai != "Đã thanh toán" && trangThai != "Hủy")
                {
                    return $"Không thể xóa hóa đơn này! Chỉ có thể xóa hóa đơn khi trạng thái là 'Đã thanh toán' hoặc 'Hủy'. Trạng thái hiện tại: '{trangThai}'";
                }

                // Xóa các record trong thanh_toan trước (có foreign key đến hoa_don)
                // Cần xóa trước vì thanh_toan tham chiếu đến hoa_don
                try
                {
                    string sqlDeleteThanhToan = "DELETE FROM thanh_toan WHERE ma_hd = @ma_hd";
                    var paramThanhToan = new Dictionary<string, object> { { "@ma_hd", id } };
                    _db.ExecuteNonQuery(sqlDeleteThanhToan, paramThanhToan);
                }
                catch (Exception exThanhToan)
                {
                    // Nếu có lỗi khi xóa thanh_toan, vẫn tiếp tục (có thể không có record nào)
                    System.Diagnostics.Debug.WriteLine($"Warning: Could not delete thanh_toan: {exThanhToan.Message}");
                }

                // Xóa các record trong chi_tiet_hoa_don trước (có foreign key đến hoa_don)
            
                try
                {
                    string sqlDeleteChiTiet = "DELETE FROM chi_tiet_hoa_don WHERE ma_hd = @ma_hd";
                    var paramChiTiet = new Dictionary<string, object> { { "@ma_hd", id } };
                    _db.ExecuteNonQuery(sqlDeleteChiTiet, paramChiTiet);
                }
                catch (Exception exChiTiet)
                {
                    // Nếu có lỗi khi xóa chi_tiet_hoa_don, vẫn tiếp tục thử xóa hóa đơn
                    // (có thể không có record nào trong chi_tiet_hoa_don)
                    System.Diagnostics.Debug.WriteLine($"Warning: Could not delete chi_tiet_hoa_don: {exChiTiet.Message}");
                }

                // Sau đó mới xóa hóa đơn
                string sql = "DELETE FROM hoa_don WHERE ma_hd=@ma_hd";
                var param = new Dictionary<string, object> { { "@ma_hd", id } };

                int rows = _db.ExecuteNonQuery(sql, param);
                return rows > 0 ? "Xóa hóa đơn thành công" : "Không tìm thấy hóa đơn cần xóa";
            }
            catch (Exception ex)
            {
                return "Lỗi khi xóa hóa đơn: " + ex.Message;
            }
        }
    }
}
