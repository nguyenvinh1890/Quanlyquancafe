using QLCF.DAL;
using QLCF.Model;
using System;
using System.Collections.Generic;
using System.Data;

namespace QLCF.BUS
{
    public class CaLamBUS
    {
        private readonly DatabaseHelper _db;

        public CaLamBUS(DatabaseHelper db)
        {
            _db = db;
        }

        public (bool, string, List<CaLam>) GetAll()
        {
            try
            {
                string sql = "SELECT ma_ca, ten_ca, thoi_gian_bat_dau, thoi_gian_ket_thuc FROM vw_CaLam_GetAll";
                DataTable dt = _db.ExecuteQuery(sql);
                var list = new List<CaLam>();

                foreach (DataRow r in dt.Rows)
                {
                    list.Add(new CaLam
                    {
                        MaCa = (int)r["ma_ca"],
                        TenCa = r["ten_ca"].ToString() ?? "",
                        GioBatDau = r["thoi_gian_bat_dau"]?.ToString() ?? "",
                        GioKetThuc = r["thoi_gian_ket_thuc"]?.ToString() ?? ""
                    });
                }

                return (true, $"Lấy {list.Count} ca làm thành công", list);
            }
            catch (Exception ex)
            {
                return (false, "Lỗi khi lấy danh sách ca làm: " + ex.Message, new List<CaLam>());
            }
        }


        public string Add(CaLam c)
        {
            try
            {
                string sql = "INSERT INTO ca_lam(ten_ca, thoi_gian_bat_dau, thoi_gian_ket_thuc) VALUES (@ten_ca, @thoi_gian_bat_dau, @thoi_gian_ket_thuc)";
                var param = new Dictionary<string, object>
                {
                    {"@ten_ca", c.TenCa},
                    {"@thoi_gian_bat_dau", c.GioBatDau},
                    {"@thoi_gian_ket_thuc", c.GioKetThuc}
                };
                int rows = _db.ExecuteNonQuery(sql, param);
                return rows > 0 ? "Thêm ca làm thành công" : "Không thể thêm ca làm";
            }
            catch (Exception ex)
            {
                return "Lỗi khi thêm ca làm: " + ex.Message;
            }
        }

        public string Update(CaLam c)
        {
            try
            {
                string sql = "UPDATE ca_lam SET ten_ca=@ten_ca, thoi_gian_bat_dau=@thoi_gian_bat_dau, thoi_gian_ket_thuc=@thoi_gian_ket_thuc WHERE ma_ca=@ma_ca";
                var param = new Dictionary<string, object>
                {
                    {"@ma_ca", c.MaCa},
                    {"@ten_ca", c.TenCa},
                    {"@thoi_gian_bat_dau", c.GioBatDau},
                    {"@thoi_gian_ket_thuc", c.GioKetThuc}
                };
                int rows = _db.ExecuteNonQuery(sql, param);
                return rows > 0 ? "Cập nhật ca làm thành công" : "Không tìm thấy ca làm cần cập nhật";
            }
            catch (Exception ex)
            {
                return "Lỗi khi cập nhật ca làm: " + ex.Message;
            }
        }

        public string Delete(int id)
        {
            try
            {
                string sql = "DELETE FROM ca_lam WHERE ma_ca=@ma_ca";
                var param = new Dictionary<string, object> { { "@ma_ca", id } };
                int rows = _db.ExecuteNonQuery(sql, param);
                return rows > 0 ? "Xóa ca làm thành công" : "Không tìm thấy ca làm cần xóa";
            }
            catch (Exception ex)
            {
                return "Lỗi khi xóa ca làm: " + ex.Message;
            }
        }
    }
}
