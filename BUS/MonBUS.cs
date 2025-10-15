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
                SELECT m.ma_mon, m.ten_mon, MIN(s.gia) AS gia
                FROM mon m
                JOIN size_mon s ON m.ma_mon = s.ma_mon
                GROUP BY m.ma_mon, m.ten_mon";

                DataTable dt = _db.ExecuteQuery(sql);
                var result = new List<Mon>();

                foreach (DataRow r in dt.Rows)
                {
                    result.Add(new Mon
                    {
                        MaMon = (int)r["ma_mon"],
                        TenMon = r["ten_mon"].ToString(),
                        Gia = (decimal)r["gia"]
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
                string sql = "INSERT INTO mon(ten_mon) VALUES (@ten_mon)";
                var parameters = new Dictionary<string, object>
                {
                    {"@ten_mon", mon.TenMon}
                };

                int rows = _db.ExecuteNonQuery(sql, parameters);
                return rows > 0 ? "Thêm món thành công" : "Không thể thêm món";
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
                string sql = "UPDATE mon SET ten_mon = @ten_mon WHERE ma_mon = @ma_mon";
                var parameters = new Dictionary<string, object>
                {
                    {"@ten_mon", mon.TenMon},
                    {"@ma_mon", mon.MaMon}
                };

                int rows = _db.ExecuteNonQuery(sql, parameters);
                return rows > 0 ? "Cập nhật món thành công" : "Không tìm thấy món cần cập nhật";
            }
            catch (Exception ex)
            {
                return "Lỗi khi cập nhật món: " + ex.Message;
            }
        }

        public string Delete(int maMon)
        {
            try
            {
                string sql = "DELETE FROM mon WHERE ma_mon = @ma_mon";
                var parameters = new Dictionary<string, object>
                {
                    {"@ma_mon", maMon}
                };

                int rows = _db.ExecuteNonQuery(sql, parameters);
                return rows > 0 ? "Xóa món thành công" : "Không tìm thấy món cần xóa";
            }
            catch (Exception ex)
            {
                return "Lỗi khi xóa món: " + ex.Message;
            }
        }
    }
}
