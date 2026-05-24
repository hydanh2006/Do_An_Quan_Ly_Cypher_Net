using QuanLyTiemNet.Models;
using System;
using System.Linq;

namespace QuanLyTiemNet.ViewModels
{
    internal class LoginViewModel
    {
        public bool KiemTraDangNhap(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return false;

            using (var db = new QuanLyTiemNetEntities1())
            {
                var taiKhoanHopLe = db.TaiKhoans.FirstOrDefault(x => x.TenDangNhap == username
                                                                  && x.MatKhau == password
                                                                  && (x.VaiTro == 0 || x.VaiTro == 2));

                if (taiKhoanHopLe != null)
                {
                    PhienDangNhap.NhanVienHienTai = taiKhoanHopLe;

                    return true;
                }
            }
            return false;
        }
    }
}