using QLCF.DAL;
using QLCF.Model;
using System;
using System.Collections.Generic;
using System.Data;

namespace QLCF.BUS
{
    public class MonBUS
    {
        private readonly DatabaseHelper _db;

        public MonBUS(DatabaseHelper db)
        {
            _db = db;
        }

        public (bool IsSuccess, string Message, List<Mon> Data) GetAll()
        {
            try
            {
                string sql = @"
        SELECT m.ma_mon, m.ten_mon, ISNULL(MIN(s.gia), 0) AS gia
        FROM mon m
        LEFT JOIN size_mon s ON m.ma_mon = s.ma_mon
        GROUP BY m.ma_mon, m.ten_mon";

                DataTable dt = _db.ExecuteQuery(sql);
                var result = new List<Mon>();

                foreach (DataRow r in dt.Rows)
                {
                    result.Add(new Mon
                    {
                        MaMon = (int)r["ma_mon"],
                        TenMon = r["ten_mon"].ToString() ?? "",
                        Gia = Convert.ToDecimal(r["gia"])
                    });
                }

                return (true, $"Lấy {result.Count} món thành công", result);
            }
            catch (Exception ex)
            {
                return (false, "Lỗi khi lấy danh sách món: " + ex.Message, new List<Mon>());
            }
        }

        public string Add(Mon mon)
        {
            try
            {
                // Insert món mới và lấy ma_mon vừa tạo
                string sql = @"
                    INSERT INTO mon(ten_mon) 
                    VALUES (@ten_mon);
                    SELECT SCOPE_IDENTITY() AS ma_mon;";

                var parameters = new Dictionary<string, object>
                {
                    {"@ten_mon", mon.TenMon}
                };

                // Execute và lấy ma_mon vừa tạo
                DataTable dt = _db.ExecuteQuery(sql, parameters);
                if (dt.Rows.Count == 0)
                {
                    return "Không thể thêm món";
                }

                int maMon = Convert.ToInt32(dt.Rows[0]["ma_mon"]);

                // Nếu có giá, tự động tạo size mặc định "Vừa"
                if (mon.Gia > 0)
                {
                    string sqlSize = "INSERT INTO size_mon(ma_mon, ten_size, gia) VALUES (@ma_mon, @ten_size, @gia)";
                    var sizeParams = new Dictionary<string, object>
                    {
                        {"@ma_mon", maMon},
                        {"@ten_size", "Vừa"},
                        {"@gia", mon.Gia}
                    };

                    _db.ExecuteNonQuery(sqlSize, sizeParams);
                }

                return "Thêm món thành công";
            }
            catch (Exception ex)
            {
                return "Lỗi khi thêm món: " + ex.Message;
            }
        }

        public string Update(Mon mon)
        {
            try
            {
                // Kiểm tra xem món đã được bán ra chưa
                string sqlCheck = "SELECT COUNT(*) as count FROM chi_tiet_don WHERE ma_mon = @ma_mon";
                var checkParams = new Dictionary<string, object>
                {
                    {"@ma_mon", mon.MaMon}
                };

                DataTable dtCheck = _db.ExecuteQuery(sqlCheck, checkParams);
                bool hasSold = false;

                if (dtCheck.Rows.Count > 0)
                {
                    int count = Convert.ToInt32(dtCheck.Rows[0]["count"]);
                    hasSold = count > 0;
                }

                // Nếu món đã được bán ra, kiểm tra xem có sửa tên không
                if (hasSold)
                {
                    // Lấy tên món hiện tại trong database
                    string sqlGetCurrent = "SELECT ten_mon FROM mon WHERE ma_mon = @ma_mon";
                    DataTable dtCurrent = _db.ExecuteQuery(sqlGetCurrent, checkParams);

                    if (dtCurrent.Rows.Count > 0)
                    {
                        string currentTenMon = dtCurrent.Rows[0]["ten_mon"].ToString() ?? "";

                        // Nếu tên mới khác tên cũ, không cho phép sửa
                        if (currentTenMon != mon.TenMon)
                        {
                            return "Không thể sửa tên món này vì món đã được bán ra. Chỉ có thể sửa giá!";
                        }
                    }
                }

                // Cập nhật tên món trong bảng mon
                string sql = "UPDATE mon SET ten_mon = @ten_mon WHERE ma_mon = @ma_mon";
                var parameters = new Dictionary<string, object>
                {
                    {"@ten_mon", mon.TenMon},
                    {"@ma_mon", mon.MaMon}
                };

                int rows = _db.ExecuteNonQuery(sql, parameters);
                if (rows == 0)
                {
                    return "Không tìm thấy món cần cập nhật";
                }

                // Cập nhật giá trong bảng size_mon (cập nhật size "Vừa" hoặc tạo mới nếu chưa có)
                if (mon.Gia > 0)
                {
                    // Kiểm tra xem có size "Vừa" không
                    string sqlCheckSize = "SELECT COUNT(*) as count FROM size_mon WHERE ma_mon = @ma_mon AND ten_size = @ten_size";
                    var checkSizeParams = new Dictionary<string, object>
                    {
                        {"@ma_mon", mon.MaMon},
                        {"@ten_size", "Vừa"}
                    };

                    DataTable dtSizeCheck = _db.ExecuteQuery(sqlCheckSize, checkSizeParams);
                    bool hasSizeVua = false;

                    if (dtSizeCheck.Rows.Count > 0)
                    {
                        int sizeCount = Convert.ToInt32(dtSizeCheck.Rows[0]["count"]);
                        hasSizeVua = sizeCount > 0;
                    }

                    if (hasSizeVua)
                    {
                        // Cập nhật giá của size "Vừa"
                        string sqlUpdateSize = "UPDATE size_mon SET gia = @gia WHERE ma_mon = @ma_mon AND ten_size = @ten_size";
                        var updateSizeParams = new Dictionary<string, object>
                        {
                            {"@gia", mon.Gia},
                            {"@ma_mon", mon.MaMon},
                            {"@ten_size", "Vừa"}
                        };
                        _db.ExecuteNonQuery(sqlUpdateSize, updateSizeParams);
                    }
                    else
                    {
                        // Nếu không có size "Vừa", tạo size "Vừa" mới với giá mới
                        string sqlInsertSize = "INSERT INTO size_mon(ma_mon, ten_size, gia) VALUES (@ma_mon, @ten_size, @gia)";
                        var insertSizeParams = new Dictionary<string, object>
                        {
                            {"@ma_mon", mon.MaMon},
                            {"@ten_size", "Vừa"},
                            {"@gia", mon.Gia}
                        };
                        _db.ExecuteNonQuery(sqlInsertSize, insertSizeParams);
                    }
                }

                return "Cập nhật món thành công";
            }
            catch (Exception ex)
            {
                return "Lỗi khi cập nhật món: " + ex.Message;
            }
        }

        public bool CheckHasSold(int maMon)
        {
            try
            {
                string sql = "SELECT COUNT(*) as count FROM chi_tiet_don WHERE ma_mon = @ma_mon";
                var parameters = new Dictionary<string, object>
                {
                    {"@ma_mon", maMon}
                };

                DataTable dt = _db.ExecuteQuery(sql, parameters);

                if (dt.Rows.Count > 0)
                {
                    int count = Convert.ToInt32(dt.Rows[0]["count"]);
                    return count > 0;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string Delete(int maMon)
        {
            try
            {
                // Bước 1: Kiểm tra xem món đã được bán ra chưa (có trong chi_tiet_don)
                string sqlCheck = "SELECT COUNT(*) as count FROM chi_tiet_don WHERE ma_mon = @ma_mon";
                var checkParams = new Dictionary<string, object>
                {
                    {"@ma_mon", maMon}
                };

                DataTable dtCheck = _db.ExecuteQuery(sqlCheck, checkParams);

                // COUNT(*) luôn trả về ít nhất 1 row với giá trị >= 0
                if (dtCheck.Rows.Count > 0)
                {
                    int count = Convert.ToInt32(dtCheck.Rows[0]["count"]);
                    if (count > 0)
                    {
                        return "Không thể xóa món này vì món đã được bán ra. Vui lòng kiểm tra lại!";
                    }
                }

                // Bước 2: Xóa các bản ghi trong mon_topping nếu có
                string sqlDeleteMonTopping = "DELETE FROM mon_topping WHERE ma_mon = @ma_mon";
                var deleteParams = new Dictionary<string, object>
                {
                    {"@ma_mon", maMon}
                };
                _db.ExecuteNonQuery(sqlDeleteMonTopping, deleteParams);

                // Bước 3: Xóa tất cả size_mon liên quan
                string sqlDeleteSize = "DELETE FROM size_mon WHERE ma_mon = @ma_mon";
                _db.ExecuteNonQuery(sqlDeleteSize, deleteParams);

                // Bước 4: Xóa các bản ghi trong cong_thuc nếu có
                string sqlDeleteCongThuc = "DELETE FROM cong_thuc WHERE ma_mon = @ma_mon";
                _db.ExecuteNonQuery(sqlDeleteCongThuc, deleteParams);

                // Bước 5: Xóa món
                string sql = "DELETE FROM mon WHERE ma_mon = @ma_mon";
                int rows = _db.ExecuteNonQuery(sql, deleteParams);

                return rows > 0 ? "Xóa món thành công" : "Không tìm thấy món cần xóa";
            }
            catch (Exception ex)
            {
                return "Lỗi khi xóa món: " + ex.Message;
            }
        }
    }
}
