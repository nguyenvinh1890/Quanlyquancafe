using QLCF.DAL;
using QLCF.Model;
using System;
using System.Collections.Generic;
using System.Data;

namespace QLCF.BUS
{
    public class LuaChonToppingBUS
    {
        private readonly DatabaseHelper _db;

        public LuaChonToppingBUS(DatabaseHelper db)
        {
            _db = db;
        }

        public (bool, string, List<LuaChonTopping>) GetAll()
        {
            try
            {
                string sql = @"
                SELECT l.ma_lua_chon, l.ma_topping, t.ten_topping, l.ten_lua_chon, l.gia_them
                FROM lua_chon_topping l
                JOIN topping t ON l.ma_topping = t.ma_topping";

                DataTable dt = _db.ExecuteQuery(sql);
                var list = new List<LuaChonTopping>();

                foreach (DataRow r in dt.Rows)
                {
                    list.Add(new LuaChonTopping
                    {
                        MaLuaChon = (int)r["ma_lua_chon"],
                        MaTopping = (int)r["ma_topping"],
                        TenTopping = r["ten_topping"].ToString() ?? "",
                        TenLuaChon = r["ten_lua_chon"].ToString() ?? "",
                        GiaThem = Convert.ToDecimal(r["gia_them"])
                    });
                }

                return (true, $"Lấy {list.Count} lựa chọn topping thành công", list);
            }
            catch (Exception ex)
            {
                return (false, "Lỗi khi lấy lựa chọn topping: " + ex.Message, new List<LuaChonTopping>());
            }
        }

        public string Add(LuaChonTopping obj)
        {
            try
            {
                string sql = @"INSERT INTO lua_chon_topping(ma_topping, ten_lua_chon, gia_them) 
                               VALUES (@ma_topping, @ten_lua_chon, @gia_them)";
                var param = new Dictionary<string, object>
                {
                    {"@ma_topping", obj.MaTopping},
                    {"@ten_lua_chon", obj.TenLuaChon},
                    {"@gia_them", obj.GiaThem}
                };
                int rows = _db.ExecuteNonQuery(sql, param);
                return rows > 0 ? "Thêm lựa chọn topping thành công" : "Không thể thêm lựa chọn topping";
            }
            catch (Exception ex)
            {
                return "Lỗi khi thêm lựa chọn topping: " + ex.Message;
            }
        }

        public string Update(LuaChonTopping obj)
        {
            try
            {
                string sql = @"UPDATE lua_chon_topping 
                               SET ten_lua_chon=@ten_lua_chon, gia_them=@gia_them 
                               WHERE ma_lua_chon=@ma_lua_chon";
                var param = new Dictionary<string, object>
                {
                    {"@ma_lua_chon", obj.MaLuaChon},
                    {"@ten_lua_chon", obj.TenLuaChon},
                    {"@gia_them", obj.GiaThem}
                };
                int rows = _db.ExecuteNonQuery(sql, param);
                return rows > 0 ? "Cập nhật lựa chọn topping thành công" : "Không tìm thấy lựa chọn topping cần cập nhật";
            }
            catch (Exception ex)
            {
                return "Lỗi khi cập nhật lựa chọn topping: " + ex.Message;
            }
        }

        public string Delete(int id)
        {
            try
            {
                string sql = "DELETE FROM lua_chon_topping WHERE ma_lua_chon=@ma_lua_chon";
                var param = new Dictionary<string, object> { { "@ma_lua_chon", id } };
                int rows = _db.ExecuteNonQuery(sql, param);
                return rows > 0 ? "Xóa lựa chọn topping thành công" : "Không tìm thấy lựa chọn topping cần xóa";
            }
            catch (Exception ex)
            {
                return "Lỗi khi xóa lựa chọn topping: " + ex.Message;
            }
        }
    }
}
