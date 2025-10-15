using QLCF.DAL;
using QLCF.Model;
using System;
using System.Collections.Generic;
using System.Data;

namespace QLCF.BUS
{
    public class KhuVucBUS
    {
        private readonly DatabaseHelper _db;

        public KhuVucBUS(DatabaseHelper db)
        {
            _db = db;
        }

        public (bool, string, List<KhuVuc>) GetAll()
        {
            try
            {
                string sql = "SELECT ma_khu, ten_khu, thu_tu FROM khu_vuc";
                DataTable dt = _db.ExecuteQuery(sql);

                var list = new List<KhuVuc>();
                foreach (DataRow r in dt.Rows)
                {
                    list.Add(new KhuVuc
                    {
                        MaKV = (int)r["ma_khu"],
                        TenKV = r["ten_khu"].ToString() ?? "",
                        MoTa = r["thu_tu"]?.ToString()
                    });
                }

                return (true, $"Lấy {list.Count} khu vực thành công", list);
            }
            catch (Exception ex)
            {
                return (false, "Lỗi khi lấy danh sách khu vực: " + ex.Message, new List<KhuVuc>());
            }
        }

        public string Add(KhuVuc kv)
        {
            try
            {
                string sql = "INSERT INTO khu_vuc(ten_khu, thu_tu) VALUES (@ten_khu, @thu_tu)";
                var param = new Dictionary<string, object>
                {
                    {"@ten_khu", kv.TenKV},
                    {"@thu_tu", kv.MoTa ?? "0"}
                };
                int rows = _db.ExecuteNonQuery(sql, param);
                return rows > 0 ? "Thêm khu vực thành công" : "Không thể thêm khu vực";
            }
            catch (Exception ex)
            {
                return "Lỗi khi thêm khu vực: " + ex.Message;
            }
        }

        public string Update(KhuVuc kv)
        {
            try
            {
                string sql = "UPDATE khu_vuc SET ten_khu=@ten_khu, thu_tu=@thu_tu WHERE ma_khu=@ma_khu";
                var param = new Dictionary<string, object>
                {
                    {"@ma_khu", kv.MaKV},
                    {"@ten_khu", kv.TenKV},
                    {"@thu_tu", kv.MoTa ?? "0"}
                };
                int rows = _db.ExecuteNonQuery(sql, param);
                return rows > 0 ? "Cập nhật khu vực thành công" : "Không tìm thấy khu vực cần cập nhật";
            }
            catch (Exception ex)
            {
                return "Lỗi khi cập nhật khu vực: " + ex.Message;
            }
        }

        public string Delete(int id)
        {
            try
            {
                string sql = "DELETE FROM khu_vuc WHERE ma_khu=@ma_khu";
                var param = new Dictionary<string, object> { { "@ma_khu", id } };
                int rows = _db.ExecuteNonQuery(sql, param);
                return rows > 0 ? "Xóa khu vực thành công" : "Không tìm thấy khu vực cần xóa";
            }
            catch (Exception ex)
            {
                return "Lỗi khi xóa khu vực: " + ex.Message;
            }
        }
    }
}
