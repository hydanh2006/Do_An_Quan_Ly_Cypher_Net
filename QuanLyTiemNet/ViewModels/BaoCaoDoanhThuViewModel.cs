using QuanLyTiemNet.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace QuanLyTiemNet.ViewModels
{
    public class CotBieuDo
    {
        public string Ngay { get; set; }
        public decimal DoanhThu { get; set; }
        public double ChieuCaoCot { get; set; } 
        public string DoanhThuString => string.Format("{0:N0} đ", DoanhThu);
    }

    internal class BaoCaoDoanhThuViewModel : BaseViewModel
    {
        public ICommand RefreshCommand { get; set; }

        // Các biến lưu doanh số
        private string _doanhThuHomNay;
        public string DoanhThuHomNay { get => _doanhThuHomNay; set { _doanhThuHomNay = value; OnPropertyChanged(nameof(DoanhThuHomNay)); } }

        private string _doanhThuThangNay;
        public string DoanhThuThangNay { get => _doanhThuThangNay; set { _doanhThuThangNay = value; OnPropertyChanged(nameof(DoanhThuThangNay)); } }

        private string _tongDoanhThu;
        public string TongDoanhThu { get => _tongDoanhThu; set { _tongDoanhThu = value; OnPropertyChanged(nameof(TongDoanhThu)); } }

        // Danh sách dữ liệu vẽ biểu đồ
        private ObservableCollection<CotBieuDo> _duLieuBieuDo;
        public ObservableCollection<CotBieuDo> DuLieuBieuDo { get => _duLieuBieuDo; set { _duLieuBieuDo = value; OnPropertyChanged(nameof(DuLieuBieuDo)); } }

        public BaoCaoDoanhThuViewModel()
        {
            LoadThongKe();

            RefreshCommand = new RelayCommand((p) =>
            {
                LoadThongKe();
            });
        }

        public void LoadThongKe()
        {
            try
            {
                using (var db = new QuanLyTiemNetEntities1())
                {
                    DateTime now = DateTime.Now;
                    DateTime startOfDay = now.Date;
                    DateTime startOfMonth = new DateTime(now.Year, now.Month, 1);

                    var tienHomNay = db.LichSuNapTiens.Where(x => x.ThoiGianNap >= startOfDay).Sum(x => (decimal?)x.SoTienNap) ?? 0m;
                    DoanhThuHomNay = string.Format("{0:N0} VNĐ", tienHomNay);

                    var tienThangNay = db.LichSuNapTiens.Where(x => x.ThoiGianNap >= startOfMonth).Sum(x => (decimal?)x.SoTienNap) ?? 0m;
                    DoanhThuThangNay = string.Format("{0:N0} VNĐ", tienThangNay);

                    var tongTien = db.LichSuNapTiens.Sum(x => (decimal?)x.SoTienNap) ?? 0m;
                    TongDoanhThu = string.Format("{0:N0} VNĐ", tongTien);

                    
                    var listBieuDo = new ObservableCollection<CotBieuDo>();
                    decimal maxDoanhThu = 0;
                    for (int i = 6; i >= 0; i--)
                    {
                        DateTime targetDate = now.AddDays(-i).Date;
                        DateTime nextDate = targetDate.AddDays(1);

       
                    var sumDoanhThu = db.LichSuNapTiens
                        .Where(x => x.ThoiGianNap >= targetDate && x.ThoiGianNap < nextDate)
                        .Sum(x => (decimal?)x.SoTienNap) ?? 0m;


                    if (sumDoanhThu > maxDoanhThu) maxDoanhThu = sumDoanhThu;

                        listBieuDo.Add(new CotBieuDo
                        {
                            Ngay = targetDate.ToString("dd/MM"),
                            DoanhThu = sumDoanhThu
                        });
                    }

                    // Tính tỷ lệ chiều cao
                    double maxPixelHeight = 200.0;
                    foreach (var item in listBieuDo)
                    {
                        if (maxDoanhThu == 0) item.ChieuCaoCot = 0;
                        else item.ChieuCaoCot = (double)(item.DoanhThu / maxDoanhThu) * maxPixelHeight;

                        // Đảm bảo cột có hiển thị tối thiểu 2px nếu có doanh thu nhưng quá nhỏ
                        if (item.DoanhThu > 0 && item.ChieuCaoCot < 2) item.ChieuCaoCot = 2;
                    }

                    DuLieuBieuDo = listBieuDo;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải báo cáo: " + ex.Message);
            }
        }
    }
}