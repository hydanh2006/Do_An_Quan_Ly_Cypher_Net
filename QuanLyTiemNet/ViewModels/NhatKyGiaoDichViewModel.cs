using QuanLyTiemNet.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace QuanLyTiemNet.ViewModels
{
    internal class NhatKyGiaoDichViewModel : BaseViewModel
    {
        private ObservableCollection<LichSuNapTien> _danhSachGiaoDich;
        public ObservableCollection<LichSuNapTien> DanhSachGiaoDich
        {
            get => _danhSachGiaoDich;
            set { _danhSachGiaoDich = value; OnPropertyChanged(nameof(DanhSachGiaoDich)); }
        }

        // Biến để chọn ngày lọc
        private DateTime? _ngayLoc;
        public DateTime? NgayLoc
        {
            get => _ngayLoc;
            set
            {
                _ngayLoc = value;
                OnPropertyChanged(nameof(NgayLoc));
                LoadNhatKy();
            }
        }

        public ICommand RefreshCommand { get; set; }
        public ICommand InHoaDonCommand { get; set; }

        public NhatKyGiaoDichViewModel()
        {
            LoadNhatKy();

            RefreshCommand = new RelayCommand((p) =>
            {
                NgayLoc = null;
                LoadNhatKy();
            });

            InHoaDonCommand = new RelayCommand((p) =>
            {
                if (p is LichSuNapTien giaoDichDaChon)
                {   
                    var dialog = new Views.DialogInHoaDon(giaoDichDaChon);
                    dialog.ShowDialog();
                }
            });
        }

        public void LoadNhatKy()
        {
            try
            {
                using (var db = new QuanLyTiemNetEntities1())
                {
                    // Dùng Include để kéo theo thông tin của bảng TaiKhoan (Khách) và TaiKhoan1 (Nhân viên)
                    var query = db.LichSuNapTiens.Include("TaiKhoan").Include("TaiKhoan1").AsQueryable();

                    // Lọc theo ngày (nếu người dùng có chọn ngày)
                    if (NgayLoc.HasValue)
                    {
                        var ngayBatDau = NgayLoc.Value.Date;
                        var ngayKetThuc = ngayBatDau.AddDays(1);
                        query = query.Where(x => x.ThoiGianNap >= ngayBatDau && x.ThoiGianNap < ngayKetThuc);
                    }

                    // Sắp xếp mới nhất lên đầu
                    var list = query.OrderByDescending(x => x.ThoiGianNap).ToList();

                    int stt = 1;
                    foreach (var item in list) item.STT = stt++;

                    DanhSachGiaoDich = new ObservableCollection<LichSuNapTien>(list);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải nhật ký: " + ex.Message);
            }
        }
    }
}