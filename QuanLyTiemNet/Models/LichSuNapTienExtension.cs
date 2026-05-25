using System;

namespace QuanLyTiemNet.Models
{
    public partial class LichSuNapTien
    {
        public int STT { get; set; }
        public string SoTienNapString
        {
            get
            {
                if (SoTienNap == 20000 || SoTienNap == 50000 || SoTienNap == 100000 || SoTienNap == 200000 || SoTienNap == 80000)
                {
                    return string.Format("💰 +{0:N0} đ (Nạp TK)", SoTienNap);
                }
                return string.Format("🍔 +{0:N0} đ (Căng tin)", SoTienNap);
            }
        }
        public string ThoiGianNapString
        {
            get
            {
                if (ThoiGianNap.HasValue)
                    return ThoiGianNap.Value.ToString("dd/MM/yyyy HH:mm:ss");
                return "";
            }
        }
    }
}