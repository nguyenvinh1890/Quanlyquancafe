using QLCF.DAL;
using QLCF.Model;
using System;
using System.Collections.Generic;
using System.Data;

namespace QLCF.BUS
{
    public class HoaDonBUS
    {
        private readonly DatabaseHelper _db;

        public HoaDonBUS(DatabaseHelper db)
        {
            _db = db;
        }

        public (bool, string, List<HoaDon>) GetAll()
        {
            try
            {
                string sql = @"
                SELECT ma_hd, ma_dh, trang_thai, tam_tinh, giam_gia, thue, tao_luc
                FROM hoa_don";

                DataTable dt = _db.ExecuteQuery(sql);
                var list = new List<HoaDon>();

                foreach (DataRow r in dt.Rows)
                {
                    list.Add(new HoaDon
                    {
                        MaHD = (int)r["ma_hd"],
                        MaDH = (int)r["ma_dh"],
                        TrangThai = r["trang_thai"].ToString() ?? "",
                        TamTinh = Convert.ToDecimal(r["tam_tinh"]),
                        GiamGia = Convert.ToDecimal(r["giam_gia"]),
                        Thue = Convert.ToDecimal(r["thue"]),
                        TaoLuc = Convert.ToDateTime(r["tao_luc"])
                    });
                }

                return (true, $"Lấy {list.Count} hóa đơn thành công", list);
            }
            catch (Exception ex)
            {
                return (false, "Lỗi khi lấy danh sách hóa đơn: " + ex.Message, new List<HoaDon>());
            }
        }

        public string Add(HoaDon hd)
        {
            try
            {
                string sql = @"INSERT INTO hoa_don (ma_dh, trang_thai, tam_tinh, giam_gia, thue, tao_luc)
                               VALUES (@ma_dh, @trang_thai, @tam_tinh, @giam_gia, @thue, @tao_luc)";
                var param = new Dictionary<string, object>
                {
                    {"@ma_dh", hd.MaDH},
                    {"@trang_thai", hd.TrangThai},
                    {"@tam_tinh", hd.TamTinh},
                    {"@giam_gia", hd.GiamGia},
                    {"@thue", hd.Thue},
                    {"@tao_luc", hd.TaoLuc}
                };

                int rows = _db.ExecuteNonQuery(sql, param);
                return rows > 0 ? "Thêm hóa đơn thành công" : "Không thể thêm hóa đơn";
            }
            catch (Exception ex)
            {
                return "Lỗi khi thêm hóa đơn: " + ex.Message;
            }
        }

        public string Update(HoaDon hd)
        {
            try
            {
                string sql = @"UPDATE hoa_don 
                               SET trang_thai=@trang_thai, tam_tinh=@tam_tinh, giam_gia=@giam_gia, thue=@thue 
                               WHERE ma_hd=@ma_hd";
                var param = new Dictionary<string, object>
                {
                    {"@ma_hd", hd.MaHD},
                    {"@trang_thai", hd.TrangThai},
                    {"@tam_tinh", hd.TamTinh},
                    {"@giam_gia", hd.GiamGia},
                    {"@thue", hd.Thue}
                };

                int rows = _db.ExecuteNonQuery(sql, param);
                return rows > 0 ? "Cập nhật hóa đơn thành công" : "Không tìm thấy hóa đơn cần cập nhật";
            }
            catch (Exception ex)
            {
                return "Lỗi khi cập nhật hóa đơn: " + ex.Message;
            }
        }

        public string Delete(int id)
        {
            try
            {
                string sql = "DELETE FROM hoa_don WHERE ma_hd=@ma_hd";
                var param = new Dictionary<string, object> { { "@ma_hd", id } };

                int rows = _db.ExecuteNonQuery(sql, param);
                return rows > 0 ? "Xóa hóa đơn thành công" : "Không tìm thấy hóa đơn cần xóa";
            }
            catch (Exception ex)
            {
                return "Lỗi khi xóa hóa đơn: " + ex.Message;
            }
        }
    }
}
