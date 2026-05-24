using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyTiemNet.Models
{
        public partial class MayTram : INotifyPropertyChanged
        {
            public PhienChoi PhienHienTai => PhienChois?.FirstOrDefault(p => p.GioKetThuc == null);


            public string TenKhachHang => PhienHienTai == null ? "Sẵn sàng" :
                (PhienHienTai.TaiKhoan != null ? PhienHienTai.TaiKhoan.HoTen : "Khách vãng lai");


            public string ThoiGianHienThi
            {
                get
                {
                    if (PhienHienTai == null || PhienHienTai.GioBatDau == null) return "";

                    if (GiaMoiGio <= 0) return "Lỗi Giá Máy";

                    if (PhienHienTai.MaTaiKhoan == null && PhienHienTai.TienGio.GetValueOrDefault() > 0)
                    {
                        double soGioMua = (double)(PhienHienTai.TienGio.Value / GiaMoiGio);
                        DateTime gioHetHan = PhienHienTai.GioBatDau.Value.AddHours(soGioMua);
                        TimeSpan thoiGianConLai = gioHetHan - DateTime.Now;

                        if (thoiGianConLai.TotalSeconds <= 0) return "00:00:00";
                        return thoiGianConLai.ToString(@"hh\:mm\:ss");
                    }

                    if (PhienHienTai.MaTaiKhoan != null && PhienHienTai.TaiKhoan != null)
                    {
                        double soGioMua = (double)(PhienHienTai.TaiKhoan.SoDu / GiaMoiGio);
                        DateTime gioHetHan = PhienHienTai.GioBatDau.Value.AddHours(soGioMua);
                        TimeSpan thoiGianConLai = gioHetHan - DateTime.Now;

                        if (thoiGianConLai.TotalSeconds <= 0) return "00:00:00";
                        return thoiGianConLai.ToString(@"hh\:mm\:ss");
                    }

                    TimeSpan daChoi = DateTime.Now - PhienHienTai.GioBatDau.Value;
                    return daChoi.ToString(@"hh\:mm\:ss");
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            public void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
}

