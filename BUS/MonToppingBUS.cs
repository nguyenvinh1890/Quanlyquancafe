using QLCF.DAL;
using QLCF.Model;
using System;
using System.Collections.Generic;
using System.Data;

namespace QLCF.BUS
{
    public class MonToppingBUS
    {
        private readonly DatabaseHelper _db;

        public MonToppingBUS(DatabaseHelper db)
        {
            _db = db;
        }

        public (bool, string, List<MonTopping>) GetAll()
        {
            try
            {
                // Join với lua_chon_topping để lấy tên lựa chọn topping
                string sql = @"
                    SELECT mt.ma_mon, mt.ma_topping, m.ten_mon, 
                           ISNULL(lct.ten_lua_chon, t.ten_topping) AS ten_topping
                    FROM mon_topping mt
                    JOIN mon m ON mt.ma_mon = m.ma_mon
                    JOIN topping t ON mt.ma_topping = t.ma_topping
                    LEFT JOIN lua_chon_topping lct ON mt.ma_topping = lct.ma_topping";

                DataTable dt = _db.ExecuteQuery(sql);
                var list = new List<MonTopping>();

                foreach (DataRow r in dt.Rows)
                {
                    list.Add(new MonTopping
                    {
                        MaMon = (int)r["ma_mon"],
                        MaTopping = (int)r["ma_topping"],
                        TenMon = r["ten_mon"].ToString() ?? "",
                        TenTopping = r["ten_topping"].ToString() ?? ""
                    });
                }

                return (true, $"Lấy {list.Count} dòng mon_topping thành công", list);
            }
            catch (Exception ex)
            {
                return (false, "Lỗi khi lấy danh sách mon_topping: " + ex.Message, new List<MonTopping>());
            }
        }

        public string Add(MonTopping mt)
        {
            try
            {
                string sql = "INSERT INTO mon_topping(ma_mon, ma_topping) VALUES (@ma_mon, @ma_topping)";
                var param = new Dictionary<string, object>
                {
                    {"@ma_mon", mt.MaMon},
                    {"@ma_topping", mt.MaTopping}
                };
                int rows = _db.ExecuteNonQuery(sql, param);
                return rows > 0 ? "Thêm mon_topping thành công" : "Không thể thêm mon_topping";
            }
            catch (Exception ex)
            {
                return "Lỗi khi thêm mon_topping: " + ex.Message;
            }
        }

        public string Delete(int maMon, int maTopping)
        {
            try
            {
                string sql = "DELETE FROM mon_topping WHERE ma_mon=@ma_mon AND ma_topping=@ma_topping";
                var param = new Dictionary<string, object>
                {
                    {"@ma_mon", maMon},
                    {"@ma_topping", maTopping}
                };
                int rows = _db.ExecuteNonQuery(sql, param);
                return rows > 0 ? "Xóa mon_topping thành công" : "Không tìm thấy dữ liệu cần xóa";
            }
            catch (Exception ex)
            {
                return "Lỗi khi xóa mon_topping: " + ex.Message;
            }
        }
    }
}
