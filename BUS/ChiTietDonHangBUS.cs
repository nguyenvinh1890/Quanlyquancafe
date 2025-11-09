using QLCF.DAL;
using QLCF.Model;
using System;
using System.Collections.Generic;
using System.Data;

namespace QLCF.BUS
{
    public class ChiTietDonHangBUS
    {
        private readonly DatabaseHelper _db;

        public ChiTietDonHangBUS(DatabaseHelper db)
        {
            _db = db;
        }

        public (bool, string, List<ChiTietDonHang>) GetAll()
        {
            try
            {
                string sql = @"
                    SELECT c.ma_dh, c.ma_mon, c.ma_size, c.so_luong, c.don_gia, 
                           (c.so_luong * c.don_gia) AS thanh_tien,
                           m.ten_mon, s.ten_size
                    FROM chi_tiet_don c
                    JOIN mon m ON c.ma_mon = m.ma_mon
                    LEFT JOIN size_mon s ON c.ma_size = s.ma_size";

                DataTable dt = _db.ExecuteQuery(sql);
                var list = new List<ChiTietDonHang>();

                foreach (DataRow r in dt.Rows)
                {
                    list.Add(new ChiTietDonHang
                    {
                        MaDH = (int)r["ma_dh"],
                        MaMon = (int)r["ma_mon"],
                        MaSize = r["ma_size"] == DBNull.Value ? null : (int?)r["ma_size"],
                        SoLuong = (int)r["so_luong"],
                        DonGia = Convert.ToDecimal(r["don_gia"]),
                        ThanhTien = Convert.ToDecimal(r["thanh_tien"]),
                        TenMon = r["ten_mon"].ToString() ?? "",
                        TenSize = r["ten_size"]?.ToString() ?? ""
                    });
                }

                return (true, $"Lấy {list.Count} chi tiết đơn hàng thành công", list);
            }
            catch (Exception ex)
            {
                return (false, "Lỗi khi lấy chi tiết đơn hàng: " + ex.Message, new List<ChiTietDonHang>());
            }
        }

        public string Add(ChiTietDonHang ctdh)
        {
            try
            {
                string sql = @"
            INSERT INTO chi_tiet_don(ma_dh, ma_mon, ma_size, so_luong, don_gia)
            VALUES (@ma_dh, @ma_mon, @ma_size, @so_luong, @don_gia)";
                var param = new Dictionary<string, object>
        {
            {"@ma_dh", ctdh.MaDH},
            {"@ma_mon", ctdh.MaMon},
            {"@ma_size", ctdh.MaSize ?? (object)DBNull.Value},
            {"@so_luong", ctdh.SoLuong},
            {"@don_gia", ctdh.DonGia}
        };
                int rows = _db.ExecuteNonQuery(sql, param);
                return rows > 0 ? "Thêm chi tiết đơn hàng thành công" : "Không thể thêm chi tiết đơn hàng";
            }
            catch (Exception ex)
            {
                return "Lỗi khi thêm chi tiết đơn hàng: " + ex.Message;
            }
        }


        public string Update(ChiTietDonHang ctdh)
        {
            try
            {
                string sql = @"
            UPDATE chi_tiet_don
            SET so_luong=@so_luong, don_gia=@don_gia
            WHERE ma_dh=@ma_dh AND ma_mon=@ma_mon";
                var param = new Dictionary<string, object>
        {
            {"@ma_dh", ctdh.MaDH},
            {"@ma_mon", ctdh.MaMon},
            {"@so_luong", ctdh.SoLuong},
            {"@don_gia", ctdh.DonGia}
        };
                int rows = _db.ExecuteNonQuery(sql, param);
                return rows > 0 ? "Cập nhật chi tiết đơn hàng thành công" : "Không tìm thấy chi tiết cần cập nhật";
            }
            catch (Exception ex)
            {
                return "Lỗi khi cập nhật chi tiết đơn hàng: " + ex.Message;
            }
        }


        public string Delete(int maDH, int maMon)
        {
            try
            {
                string sql = "DELETE FROM chi_tiet_don WHERE ma_dh=@ma_dh AND ma_mon=@ma_mon";
                var param = new Dictionary<string, object>
                {
                    {"@ma_dh", maDH},
                    {"@ma_mon", maMon}
                };
                int rows = _db.ExecuteNonQuery(sql, param);
                return rows > 0 ? "Xóa chi tiết đơn hàng thành công" : "Không tìm thấy chi tiết cần xóa";
            }
            catch (Exception ex)
            {
                return "Lỗi khi xóa chi tiết đơn hàng: " + ex.Message;
            }
        }

        public (bool, string, List<ChiTietDonHang>) GetByDonHang(int maDH)
        {
            try
            {
                string sql = @"
                    SELECT c.ma_dh, c.ma_mon, c.ma_size, c.so_luong, c.don_gia, 
                           (c.so_luong * c.don_gia) AS thanh_tien,
                           m.ten_mon, s.ten_size
                    FROM chi_tiet_don c
                    JOIN mon m ON c.ma_mon = m.ma_mon
                    LEFT JOIN size_mon s ON c.ma_size = s.ma_size
                    WHERE c.ma_dh = @ma_dh";

                var param = new Dictionary<string, object>
                {
                    {"@ma_dh", maDH}
                };

                DataTable dt = _db.ExecuteQuery(sql, param);
                var list = new List<ChiTietDonHang>();

                foreach (DataRow r in dt.Rows)
                {
                    list.Add(new ChiTietDonHang
                    {
                        MaDH = (int)r["ma_dh"],
                        MaMon = (int)r["ma_mon"],
                        MaSize = r["ma_size"] == DBNull.Value ? null : (int?)r["ma_size"],
                        SoLuong = (int)r["so_luong"],
                        DonGia = Convert.ToDecimal(r["don_gia"]),
                        ThanhTien = Convert.ToDecimal(r["thanh_tien"]),
                        TenMon = r["ten_mon"].ToString() ?? "",
                        TenSize = r["ten_size"]?.ToString() ?? ""
                    });
                }

                return (true, $"Lấy {list.Count} chi tiết đơn hàng thành công", list);
            }
            catch (Exception ex)
            {
                return (false, "Lỗi khi lấy chi tiết đơn hàng: " + ex.Message, new List<ChiTietDonHang>());
            }
        }
    }
}
