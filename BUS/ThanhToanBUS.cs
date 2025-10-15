using QLCF.DAL;
using QLCF.Model;
using System;
using System.Collections.Generic;
using System.Data;

namespace QLCF.BUS
{
    public class ThanhToanBUS
    {
        private readonly DatabaseHelper _db;

        public ThanhToanBUS(DatabaseHelper db)
        {
            _db = db;
        }

        public (bool, string, List<ThanhToan>) GetAll()
        {
            try
            {
                string sql = @"
                SELECT t.ma_tt, t.ma_hd, t.phuong_thuc, t.so_tien, t.thu_boi, t.tra_luc, n.ho_ten AS thu_boi_ten
                FROM thanh_toan t
                LEFT JOIN nguoi_dung n ON t.thu_boi = n.ma_nd";

                DataTable dt = _db.ExecuteQuery(sql);
                var list = new List<ThanhToan>();

                foreach (DataRow r in dt.Rows)
                {
                    list.Add(new ThanhToan
                    {
                        MaTT = (int)r["ma_tt"],
                        MaHD = (int)r["ma_hd"],
                        PhuongThuc = r["phuong_thuc"].ToString() ?? "",
                        SoTien = Convert.ToDecimal(r["so_tien"]),
                        ThuBoi = r["thu_boi"] != DBNull.Value ? (int?)r["thu_boi"] : null,
                        ThuBoiTen = r["thu_boi_ten"].ToString() ?? "",
                        TraLuc = Convert.ToDateTime(r["tra_luc"])
                    });
                }

                return (true, $"Lấy {list.Count} thanh toán thành công", list);
            }
            catch (Exception ex)
            {
                return (false, "Lỗi khi lấy danh sách thanh toán: " + ex.Message, new List<ThanhToan>());
            }
        }

        public string Add(ThanhToan tt)
        {
            try
            {
                string sql = @"INSERT INTO thanh_toan (ma_hd, phuong_thuc, so_tien, thu_boi, tra_luc)
                               VALUES (@ma_hd, @phuong_thuc, @so_tien, @thu_boi, @tra_luc)";
                var param = new Dictionary<string, object>
                {
                    {"@ma_hd", tt.MaHD},
                    {"@phuong_thuc", tt.PhuongThuc},
                    {"@so_tien", tt.SoTien},
                    {"@thu_boi", tt.ThuBoi},
                    {"@tra_luc", tt.TraLuc}
                };

                int rows = _db.ExecuteNonQuery(sql, param);
                return rows > 0 ? "Thêm thanh toán thành công" : "Không thể thêm thanh toán";
            }
            catch (Exception ex)
            {
                return "Lỗi khi thêm thanh toán: " + ex.Message;
            }
        }

        public string Update(ThanhToan tt)
        {
            try
            {
                string sql = @"UPDATE thanh_toan 
                               SET phuong_thuc=@phuong_thuc, so_tien=@so_tien, thu_boi=@thu_boi 
                               WHERE ma_tt=@ma_tt";
                var param = new Dictionary<string, object>
                {
                    {"@ma_tt", tt.MaTT},
                    {"@phuong_thuc", tt.PhuongThuc},
                    {"@so_tien", tt.SoTien},
                    {"@thu_boi", tt.ThuBoi}
                };

                int rows = _db.ExecuteNonQuery(sql, param);
                return rows > 0 ? "Cập nhật thanh toán thành công" : "Không tìm thấy thanh toán cần cập nhật";
            }
            catch (Exception ex)
            {
                return "Lỗi khi cập nhật thanh toán: " + ex.Message;
            }
        }

        public string Delete(int id)
        {
            try
            {
                string sql = "DELETE FROM thanh_toan WHERE ma_tt=@ma_tt";
                var param = new Dictionary<string, object> { { "@ma_tt", id } };

                int rows = _db.ExecuteNonQuery(sql, param);
                return rows > 0 ? "Xóa thanh toán thành công" : "Không tìm thấy thanh toán cần xóa";
            }
            catch (Exception ex)
            {
                return "Lỗi khi xóa thanh toán: " + ex.Message;
            }
        }
    }
}
