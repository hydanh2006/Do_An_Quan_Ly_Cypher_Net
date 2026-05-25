using System;

namespace QuanLyTiemNet.Models
{
    public partial class DichVu
    {
        public int STT { get; set; }
        public string DonGiaString
        {
            get
            {
                return string.Format("{0:N0} VNĐ", DonGia);
            }
        }

        public string PhanLoaiBg
        {
            get
            {
                if (PhanLoai == "Đồ ăn") return "#78350F";
                if (PhanLoai == "Nước uống") return "#1E3A8A";
                return "#064E3B";
            }
        }

        public string PhanLoaiFg
        {
            get
            {
                if (PhanLoai == "Đồ ăn") return "#FBBF24";
                if (PhanLoai == "Nước uống") return "#60A5FA";
                return "#34D399";
            }
        }
    }
}