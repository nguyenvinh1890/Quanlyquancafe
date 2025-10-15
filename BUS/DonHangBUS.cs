using QLCF.DAL;
using QLCF.Model;
using System;
using System.Collections.Generic;
using System.Data;

namespace QLCF.BUS
{
    public class DonHangBUS
    {
        private readonly DatabaseHelper _db;

        public DonHangBUS(DatabaseHelper db)
        {
            _db = db;
        }

        public (bool, string, List<DonHang>) GetAll()
        {
            try
            {
                string sql = @"
                SELECT d.ma_dh, d.ma_ban, d.loai_dh, d.trang_thai, d.mo_luc, n.ho_ten AS mo_boi_ten
                FROM don_hang d
                LEFT JOIN nguoi_dung n ON d.mo_boi = n.ma_nd";

                DataTable dt = _db.ExecuteQuery(sql);
                var list = new List<DonHang>();

                foreach (DataRow r in dt.Rows)
                {
                    list.Add(new DonHang
                    {
                        MaDH = (int)r["ma_dh"],
                        MaBan = r["ma_ban"] != DBNull.Value ? (int)r["ma_ban"] : 0,
                        LoaiDH = r["loai_dh"].ToString() ?? "",
                        TrangThai = r["trang_thai"].ToString() ?? "",
                        MoLuc = Convert.ToDateTime(r["mo_luc"]),
                        MoBoiTen = r["mo_boi_ten"].ToString() ?? ""
                    });
                }

                return (true, $"Lấy {list.Count} đơn hàng thành công", list);
            }
            catch (Exception ex)
            {
                return (false, "Lỗi khi lấy danh sách đơn hàng: " + ex.Message, new List<DonHang>());
            }
        }

        public string Add(DonHang dh)
        {
            try
            {
                string sql = @"INSERT INTO don_hang (ma_ban, loai_dh, trang_thai, mo_boi, mo_luc)
                               VALUES (@ma_ban, @loai_dh, @trang_thai, @mo_boi, @mo_luc)";
                var param = new Dictionary<string, object>
                {
                    {"@ma_ban", dh.MaBan},
                    {"@loai_dh", dh.LoaiDH},
                    {"@trang_thai", dh.TrangThai},
                    {"@mo_boi", dh.MoBoi},
                    {"@mo_luc", dh.MoLuc}
                };
                int rows = _db.ExecuteNonQuery(sql, param);
                return rows > 0 ? "Thêm đơn hàng thành công" : "Không thể thêm đơn hàng";
            }
            catch (Exception ex)
            {
                return "Lỗi khi thêm đơn hàng: " + ex.Message;
            }
        }

        public string Update(DonHang dh)
        {
            try
            {
                string sql = @"UPDATE don_hang 
                               SET ma_ban=@ma_ban, loai_dh=@loai_dh, trang_thai=@trang_thai 
                               WHERE ma_dh=@ma_dh";
                var param = new Dictionary<string, object>
                {
                    {"@ma_dh", dh.MaDH},
                    {"@ma_ban", dh.MaBan},
                    {"@loai_dh", dh.LoaiDH},
                    {"@trang_thai", dh.TrangThai}
                };
                int rows = _db.ExecuteNonQuery(sql, param);
                return rows > 0 ? "Cập nhật đơn hàng thành công" : "Không tìm thấy đơn hàng cần cập nhật";
            }
            catch (Exception ex)
            {
                return "Lỗi khi cập nhật đơn hàng: " + ex.Message;
            }
        }

        public string Delete(int id)
        {
            try
            {
                string sql = "DELETE FROM don_hang WHERE ma_dh=@ma_dh";
                var param = new Dictionary<string, object> { { "@ma_dh", id } };
                int rows = _db.ExecuteNonQuery(sql, param);
                return rows > 0 ? "Xóa đơn hàng thành công" : "Không tìm thấy đơn hàng cần xóa";
            }
            catch (Exception ex)
            {
                return "Lỗi khi xóa đơn hàng: " + ex.Message;
            }
        }
    }
}
