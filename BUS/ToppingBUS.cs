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
                // Lấy cả giá từ topping, nếu không có cột gia thì dùng 0
                string sql = @"
                    SELECT ma_topping, ten_topping, 
                           ISNULL(gia, 0) AS gia
                    FROM topping";
                DataTable dt = _db.ExecuteQuery(sql);

                var list = new List<Topping>();
                foreach (DataRow r in dt.Rows)
                {
                    list.Add(new Topping
                    {
                        MaTopping = (int)r["ma_topping"],
                        TenTopping = r["ten_topping"].ToString() ?? "",
                        Gia = r["gia"] != DBNull.Value ? Convert.ToDecimal(r["gia"]) : 0
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
                // Thêm cả giá nếu cột gia tồn tại
                string sql = "INSERT INTO topping(ten_topping, gia) VALUES (@ten_topping, @gia)";
                var p = new Dictionary<string, object>
                {
                    { "@ten_topping", topping.TenTopping },
                    { "@gia", topping.Gia }
                };
                int rows = _db.ExecuteNonQuery(sql, p);
                return rows > 0 ? "Thêm topping thành công" : "Không thể thêm topping";
            }
            catch (Exception ex)
            {
                // Nếu lỗi do không có cột gia, thử insert không có gia
                try
                {
                    string sql = "INSERT INTO topping(ten_topping) VALUES (@ten_topping)";
                    var p = new Dictionary<string, object> { { "@ten_topping", topping.TenTopping } };
                    int rows = _db.ExecuteNonQuery(sql, p);
                    return rows > 0 ? "Thêm topping thành công" : "Không thể thêm topping";
                }
                catch
                {
                    return "Lỗi khi thêm topping: " + ex.Message;
                }
            }
        }

        public string Update(Topping topping)
        {
            try
            {
                // Kiểm tra xem cột gia có tồn tại không
                string checkColumnSql = @"
                    SELECT COUNT(*) as count 
                    FROM INFORMATION_SCHEMA.COLUMNS 
                    WHERE TABLE_NAME = 'topping' AND COLUMN_NAME = 'gia'";
                DataTable dtCheck = _db.ExecuteQuery(checkColumnSql);
                bool hasGiaColumn = false;
                if (dtCheck.Rows.Count > 0)
                {
                    int count = Convert.ToInt32(dtCheck.Rows[0]["count"]);
                    hasGiaColumn = count > 0;
                }

                if (hasGiaColumn)
                {
                    // Cập nhật cả tên và giá
                    string sql = "UPDATE topping SET ten_topping=@ten_topping, gia=@gia WHERE ma_topping=@ma_topping";
                    var p = new Dictionary<string, object>
                    {
                        {"@ma_topping", topping.MaTopping},
                        {"@ten_topping", topping.TenTopping},
                        {"@gia", topping.Gia}
                    };
                    int rows = _db.ExecuteNonQuery(sql, p);
                    return rows > 0 ? "Cập nhật topping thành công" : "Không tìm thấy topping cần cập nhật";
                }
                else
                {
                    // Nếu chưa có cột gia, chỉ cập nhật tên và thông báo
                    string sql = "UPDATE topping SET ten_topping=@ten_topping WHERE ma_topping=@ma_topping";
                    var p = new Dictionary<string, object>
                    {
                        {"@ma_topping", topping.MaTopping},
                        {"@ten_topping", topping.TenTopping}
                    };
                    int rows = _db.ExecuteNonQuery(sql, p);
                    return rows > 0 ? "Cập nhật tên topping thành công. Vui lòng thêm cột 'gia' vào bảng topping trong database để có thể cập nhật giá." : "Không tìm thấy topping cần cập nhật";
                }
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
