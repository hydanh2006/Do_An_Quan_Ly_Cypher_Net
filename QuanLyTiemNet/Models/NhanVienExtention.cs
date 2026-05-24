using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyTiemNet.Models
{
    public partial class NhanVien
    {
        public int STT { get; set; }

        public string LuongCoBanString
        {
            get
            {
                return string.Format("{0:N0} VNĐ", MucLuong);
            }
        }
        public string TrangThaiText
        {
            get
            {
                if (TrangThai == "Đang làm việc") return "Đang làm việc";
                if (TrangThai == "Tạm nghỉ") return "Tạm nghỉ";
                return "Đã nghỉ việc";
            }
        }
        public string StatusBg
        {
            get
            {
                if (TrangThai == "Đang làm việc") return "#064E3B";
                if (TrangThai == "Tạm nghỉ") return "#78350F";
                return "#7F1D1D";
            }
        }

        public string StatusFg
        {
            get
            {
                if (TrangThai == "Đang làm việc") return "#34D399";
                if (TrangThai == "Tạm nghỉ") return "#FBBF24";
                return "#F87171";
            }
        }
    }
}
