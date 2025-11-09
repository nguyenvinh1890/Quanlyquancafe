using QLCF.DAL;
using QLCF.Model;
using System;
using System.Collections.Generic;
using System.Data;

namespace QLCF.BUS
{
    public class SizeMonBUS
    {
        private readonly DatabaseHelper _db;

        public SizeMonBUS(DatabaseHelper db)
        {
            _db = db;
        }

        public (bool, string, List<SizeMon>) GetAll()
        {
            try
            {
                string sql = @"
                SELECT s.ma_size, s.ma_mon, s.ten_size, s.gia, m.ten_mon
                FROM size_mon s
                JOIN mon m ON s.ma_mon = m.ma_mon";

                DataTable dt = _db.ExecuteQuery(sql);
                var list = new List<SizeMon>();
                foreach (DataRow r in dt.Rows)
                {
                    list.Add(new SizeMon
                    {
                        MaSize = (int)r["ma_size"],
                        MaMon = (int)r["ma_mon"],
                        TenSize = r["ten_size"].ToString() ?? "",
                        Gia = Convert.ToDecimal(r["gia"]),
                        TenMon = r["ten_mon"]?.ToString() ?? "" // Map tên món từ database
                    });
                }

                return (true, $"Lấy {list.Count} size món thành công", list);
            }
            catch (Exception ex)
            {
                return (false, "Lỗi khi lấy danh sách size món: " + ex.Message, new List<SizeMon>());
            }
        }

        public string Add(SizeMon size)
        {
            try
            {
                string sql = "INSERT INTO size_mon(ma_mon, ten_size, gia) VALUES (@ma_mon, @ten_size, @gia)";
                var param = new Dictionary<string, object>
                {
                    {"@ma_mon", size.MaMon},
                    {"@ten_size", size.TenSize},
                    {"@gia", size.Gia}
                };

                int rows = _db.ExecuteNonQuery(sql, param);
                return rows > 0 ? "Thêm size món thành công" : "Không thể thêm size món";
            }
            catch (Exception ex)
            {
                return "Lỗi khi thêm size món: " + ex.Message;
            }
        }

        public string Update(SizeMon size)
        {
            try
            {
                string sql = "UPDATE size_mon SET ten_size=@ten_size, gia=@gia WHERE ma_size=@ma_size";
                var param = new Dictionary<string, object>
                {
                    {"@ma_size", size.MaSize},
                    {"@ten_size", size.TenSize},
                    {"@gia", size.Gia}
                };

                int rows = _db.ExecuteNonQuery(sql, param);
                return rows > 0 ? "Cập nhật size món thành công" : "Không tìm thấy size món cần cập nhật";
            }
            catch (Exception ex)
            {
                return "Lỗi khi cập nhật size món: " + ex.Message;
            }
        }

        public string Delete(int maSize)
        {
            try
            {
                string sql = "DELETE FROM size_mon WHERE ma_size = @ma_size";
                var param = new Dictionary<string, object> { { "@ma_size", maSize } };
                int rows = _db.ExecuteNonQuery(sql, param);
                return rows > 0 ? "Xóa size món thành công" : "Không tìm thấy size món cần xóa";
            }
            catch (Exception ex)
            {
                return "Lỗi khi xóa size món: " + ex.Message;
            }
        }
    }
}
