using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyTiemNet.Models
{
    public static class PhienDangNhap
    {
        public static TaiKhoan NhanVienHienTai { get; set; }

        public static bool LaAdmin => NhanVienHienTai != null && NhanVienHienTai.VaiTro == 0;
    }
}
