using QLCF.DAL;
using QLCF.Model;
using System;
using System.Collections.Generic;
using System.Data;

namespace QLCF.BUS
{
    public class BanBUS
    {
        private readonly DatabaseHelper _db;

        public BanBUS(DatabaseHelper db)
        {
            _db = db;
        }

        public (bool, string, List<Ban>) GetAll()
        {
            try
            {
                string sql = @"
                    SELECT b.ma_ban, b.ky_hieu, b.ma_khu, b.suc_chua, b.trang_thai, k.ten_khu
                    FROM ban b
                    JOIN khu_vuc k ON b.ma_khu = k.ma_khu";

                DataTable dt = _db.ExecuteQuery(sql);
                var list = new List<Ban>();

                foreach (DataRow r in dt.Rows)
                {
                    list.Add(new Ban
                    {
                        MaBan = (int)r["ma_ban"],
                        TenBan = r["ky_hieu"].ToString() ?? "",
                        MaKhu = (int)r["ma_khu"],
                        SucChua = r["suc_chua"] != DBNull.Value ? Convert.ToInt32(r["suc_chua"]) : 0,
                        TrangThai = r["trang_thai"]?.ToString(),
                        TenKhu = r["ten_khu"]?.ToString()
                    });
                }

                return (true, $"Lấy {list.Count} bàn thành công", list);
            }
            catch (Exception ex)
            {
                return (false, "Lỗi khi lấy danh sách bàn: " + ex.Message, new List<Ban>());
            }
        }

        public string Add(Ban b)
        {
            try
            {
               
                string sql = @"
                    DECLARE @nextId INT = ISNULL((SELECT MAX(ma_ban) + 1 FROM ban), 1);
                    INSERT INTO ban(ma_ban, ky_hieu, ma_khu, suc_chua, trang_thai)
                    VALUES (@nextId, @ky_hieu, @ma_khu, @suc_chua, @trang_thai);";

                var param = new Dictionary<string, object>
                {
                    {"@ky_hieu", b.TenBan},
                    {"@ma_khu", b.MaKhu},
                    {"@suc_chua", b.SucChua},
                    {"@trang_thai", b.TrangThai ?? ""}
                };

                int rows = _db.ExecuteNonQuery(sql, param);
                return rows > 0 ? "Thêm bàn thành công" : "Không thể thêm bàn";
            }
            catch (Exception ex)
            {
                return "Lỗi khi thêm bàn: " + ex.Message;
            }
        }

        public string Update(Ban b)
        {
            try
            {
                string sql = @"
                    UPDATE ban 
                    SET ky_hieu=@ky_hieu, ma_khu=@ma_khu, suc_chua=@suc_chua, trang_thai=@trang_thai 
                    WHERE ma_ban=@ma_ban";

                var param = new Dictionary<string, object>
                {
                    {"@ma_ban", b.MaBan},
                    {"@ky_hieu", b.TenBan},
                    {"@ma_khu", b.MaKhu},
                    {"@suc_chua", b.SucChua},
                    {"@trang_thai", b.TrangThai ?? ""}
                };

                int rows = _db.ExecuteNonQuery(sql, param);
                return rows > 0 ? "Cập nhật bàn thành công" : "Không tìm thấy bàn cần cập nhật";
            }
            catch (Exception ex)
            {
                return "Lỗi khi cập nhật bàn: " + ex.Message;
            }
        }

        public string Delete(int id)
        {
            try
            {
                string sql = "DELETE FROM ban WHERE ma_ban=@ma_ban";
                var param = new Dictionary<string, object> { { "@ma_ban", id } };
                int rows = _db.ExecuteNonQuery(sql, param);
                return rows > 0 ? "Xóa bàn thành công" : "Không tìm thấy bàn cần xóa";
            }
            catch (Exception ex)
            {
                return "Lỗi khi xóa bàn: " + ex.Message;
            }
        }
    }
}
