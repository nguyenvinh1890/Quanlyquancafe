using QLCF.DAL;
using QLCF.Model;
using System;
using System.Collections.Generic;
using System.Data;

namespace QLCF.BUS
{
    public class ToppingBUS
    {
        private readonly DatabaseHelper _db;

        public ToppingBUS(DatabaseHelper db)
        {
            _db = db;
        }

        public (bool, string, List<Topping>) GetAll()
        {
            try
            {
                string sql = "SELECT ma_topping, ten_topping FROM topping";
                DataTable dt = _db.ExecuteQuery(sql);

                var list = new List<Topping>();
                foreach (DataRow r in dt.Rows)
                {
                    list.Add(new Topping
                    {
                        MaTopping = (int)r["ma_topping"],
                        TenTopping = r["ten_topping"].ToString() ?? ""
                    });
                }

                return (true, $"Lấy {list.Count} topping thành công", list);
            }
            catch (Exception ex)
            {
                return (false, "Lỗi khi lấy topping: " + ex.Message, new List<Topping>());
            }
        }

        public string Add(Topping topping)
        {
            try
            {
                string sql = "INSERT INTO topping(ten_topping) VALUES (@ten_topping)";
                var p = new Dictionary<string, object> { { "@ten_topping", topping.TenTopping } };
                int rows = _db.ExecuteNonQuery(sql, p);
                return rows > 0 ? "Thêm topping thành công" : "Không thể thêm topping";
            }
            catch (Exception ex)
            {
                return "Lỗi khi thêm topping: " + ex.Message;
            }
        }

        public string Update(Topping topping)
        {
            try
            {
                string sql = "UPDATE topping SET ten_topping=@ten_topping WHERE ma_topping=@ma_topping";
                var p = new Dictionary<string, object>
                {
                    {"@ma_topping", topping.MaTopping},
                    {"@ten_topping", topping.TenTopping}
                };
                int rows = _db.ExecuteNonQuery(sql, p);
                return rows > 0 ? "Cập nhật topping thành công" : "Không tìm thấy topping cần cập nhật";
            }
            catch (Exception ex)
            {
                return "Lỗi khi cập nhật topping: " + ex.Message;
            }
        }

        public string Delete(int maTopping)
        {
            try
            {
                string sql = "DELETE FROM topping WHERE ma_topping=@ma_topping";
                var p = new Dictionary<string, object> { { "@ma_topping", maTopping } };
                int rows = _db.ExecuteNonQuery(sql, p);
                return rows > 0 ? "Xóa topping thành công" : "Không tìm thấy topping cần xóa";
            }
            catch (Exception ex)
            {
                return "Lỗi khi xóa topping: " + ex.Message;
            }
        }
    }
}
