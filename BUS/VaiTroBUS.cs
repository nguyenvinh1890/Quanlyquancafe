using QLCF.DAL;
using QLCF.Model;
using System;
using System.Collections.Generic;
using System.Data;

namespace QLCF.BUS
{
    public class VaiTroBUS
    {
        private readonly DatabaseHelper _db;

        public VaiTroBUS(DatabaseHelper db)
        {
            _db = db;
        }

        public (bool, string, List<VaiTro>) GetAll()
        {
            try
            {
                string sql = "SELECT ma_vt, ten_vt FROM vai_tro";
                DataTable dt = _db.ExecuteQuery(sql);

                var list = new List<VaiTro>();
                foreach (DataRow r in dt.Rows)
                {
                    list.Add(new VaiTro
                    {
                        MaVT = Convert.ToByte(r["ma_vt"]),
                        TenVT = r["ten_vt"].ToString() ?? ""
                    });
                }

                return (true, $"Lấy {list.Count} vai trò thành công", list);
            }
            catch (Exception ex)
            {
                return (false, "Lỗi khi lấy danh sách vai trò: " + ex.Message, new List<VaiTro>());
            }
        }

        public string Add(VaiTro vt)
        {
            try
            {
                string sql = "INSERT INTO vai_tro(ten_vt) VALUES (@ten_vt)";
                var param = new Dictionary<string, object> { { "@ten_vt", vt.TenVT } };
                int rows = _db.ExecuteNonQuery(sql, param);
                return rows > 0 ? "Thêm vai trò thành công" : "Không thể thêm vai trò";
            }
            catch (Exception ex)
            {
                return "Lỗi khi thêm vai trò: " + ex.Message;
            }
        }

        public string Update(VaiTro vt)
        {
            try
            {
                string sql = "UPDATE vai_tro SET ten_vt=@ten_vt WHERE ma_vt=@ma_vt";
                var param = new Dictionary<string, object>
                {
                    {"@ma_vt", vt.MaVT},
                    {"@ten_vt", vt.TenVT}
                };
                int rows = _db.ExecuteNonQuery(sql, param);
                return rows > 0 ? "Cập nhật vai trò thành công" : "Không tìm thấy vai trò cần cập nhật";
            }
            catch (Exception ex)
            {
                return "Lỗi khi cập nhật vai trò: " + ex.Message;
            }
        }

        public string Delete(int id)
        {
            try
            {
                string sql = "DELETE FROM vai_tro WHERE ma_vt=@ma_vt";
                var param = new Dictionary<string, object> { { "@ma_vt", id } };
                int rows = _db.ExecuteNonQuery(sql, param);
                return rows > 0 ? "Xóa vai trò thành công" : "Không tìm thấy vai trò cần xóa";
            }
            catch (Exception ex)
            {
                return "Lỗi khi xóa vai trò: " + ex.Message;
            }
        }
    }
}
