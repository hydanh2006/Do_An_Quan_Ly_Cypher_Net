using QuanLyTiemNet.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using QuanLyTiemNet.Views;
namespace QuanLyTiemNet.ViewModels
{
    internal class CangTinViewModel : BaseViewModel
    {
        private ObservableCollection<DichVu> _danhSachDichVu;
        public ObservableCollection<DichVu> DanhSachDichVu
        {
            get => _danhSachDichVu;
            set { _danhSachDichVu = value; OnPropertyChanged(nameof(DanhSachDichVu)); }
        }

        private string _boLocPhanLoai = "Tất cả";
        public string BoLocPhanLoai
        {
            get => _boLocPhanLoai;
            set
            {
                _boLocPhanLoai = value;
                OnPropertyChanged(nameof(BoLocPhanLoai));
                LoadDanhSachDichVu();
            }
        }

        public Visibility HienThiAdmin
        {
            get => (PhienDangNhap.NhanVienHienTai != null && PhienDangNhap.NhanVienHienTai.VaiTro == 0)
                ? Visibility.Visible : Visibility.Collapsed;
        }

        public ICommand AddDichVuCommand { get; set; }
        public ICommand SuaDichVuCommand { get; set; }
        public ICommand XoaDichVuCommand { get; set; }
        public ICommand MoPhongGoiMonCommand { get; set; }
        public ICommand MoPhongGoiMonNgauNhienCommand { get; set; }

        public CangTinViewModel()
        {
            LoadDanhSachDichVu();

            // Lệnh Thêm
            AddDichVuCommand = new RelayCommand((p) =>
            {
                var dialog = new Views.DialogAddDichVu();
                if (dialog.ShowDialog() == true) LoadDanhSachDichVu();
            });

            // Lệnh Sửa
            SuaDichVuCommand = new RelayCommand((p) =>
            {
                if (p is DichVu dv)
                {
                    var dialog = new Views.DialogEditDichVu(dv);
                    if (dialog.ShowDialog() == true) LoadDanhSachDichVu();
                }
            });

            // Lệnh Xóa
            XoaDichVuCommand = new RelayCommand((p) =>
            {
                if (p is DichVu dv)
                {
                    var result = MessageBox.Show($"Bạn có chắc muốn xóa món [{dv.TenDichVu}] khỏi Menu?", "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        try
                        {
                            using (var db = new QuanLyTiemNetEntities1())
                            {
                                var item = db.DichVus.FirstOrDefault(d => d.MaDichVu == dv.MaDichVu);
                                if (item != null)
                                {
                                    db.DichVus.Remove(item);
                                    db.SaveChanges();
                                    LoadDanhSachDichVu();
                                }
                            }
                        }
                        catch
                        {
                            MessageBox.Show("Không thể xóa! Món này đã từng được khách gọi (Dính dữ liệu lịch sử). Hãy chọn Sửa và đổi Tồn kho về 0.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            });

            // Lệnh Mô phỏng khách gọi món (Dùng async để xài tính năng hẹn giờ ẩn thông báo)
            MoPhongGoiMonCommand = new RelayCommand(async (p) =>
            {
                if (p is DichVu dv)
                {
                    if (dv.TonKho > 0)
                    {
                        // 1. Trừ tồn kho ảo
                        dv.TonKho -= 1;
                        System.Windows.Data.CollectionViewSource.GetDefaultView(DanhSachDichVu).Refresh();

                        // 2. Random một số máy tính gọi món
                        Random rnd = new Random();
                        int maySo = rnd.Next(1, 30); 

                        ThongBaoText = $"🔔 Máy {maySo:D2} vừa order 1 {dv.TenDichVu}!";
                        HienThiThongBao = Visibility.Visible;

                        // 4. Hẹn giờ tự động tắt thông báo sau 3 giây
                        await System.Threading.Tasks.Task.Delay(3000);
                        HienThiThongBao = Visibility.Collapsed;
                    }
                    else
                    {
                        MessageBox.Show("Món này đã hết hàng (Tồn kho = 0)!", "Hết hàng", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            });

            MoPhongGoiMonNgauNhienCommand = new RelayCommand(async (p) =>
            {
                var cacMonConHang = DanhSachDichVu.Where(x => x.TonKho > 0).ToList();

                if (cacMonConHang.Count == 0)
                {
                    MessageBox.Show("Tất cả các món trong kho đều đã hết sạch!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                Random rnd = new Random();

                int monIndex = rnd.Next(cacMonConHang.Count);
                var monDuocChon = cacMonConHang[monIndex];

                int maySo = rnd.Next(1, 31);

                monDuocChon.TonKho -= 1;
                System.Windows.Data.CollectionViewSource.GetDefaultView(DanhSachDichVu).Refresh();
              
                try
                {
                    using (var db = new QuanLyTiemNetEntities1())
                    {
                        var khachAo = db.TaiKhoans.FirstOrDefault(t => t.VaiTro == 1);
                        int idKhach = khachAo != null ? khachAo.MaTaiKhoan : 1;

                        int idThuNgan = PhienDangNhap.NhanVienHienTai != null ? PhienDangNhap.NhanVienHienTai.MaTaiKhoan : 1;

                        var doanhThuMoi = new LichSuNapTien
                        {
                            MaTaiKhoanKhach = idKhach,
                            MaTaiKhoanNhanVien = idThuNgan,
                            SoTienNap = monDuocChon.DonGia,
                            ThoiGianNap = DateTime.Now
                        };

                        db.LichSuNapTiens.Add(doanhThuMoi);
                        db.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                   
                }

                string tienThuDuoc = string.Format("{0:N0}đ", monDuocChon.DonGia);
                ThongBaoText = $"🎲 Máy {maySo:D2} gọi {monDuocChon.TenDichVu} (+{tienThuDuoc})";
                HienThiThongBao = Visibility.Visible;

                await System.Threading.Tasks.Task.Delay(3000);
                HienThiThongBao = Visibility.Collapsed;
            });


        }

        public void LoadDanhSachDichVu()
        {
            try
            {
                using (var db = new QuanLyTiemNetEntities1())
                {
                    var query = db.DichVus.AsQueryable();
                    if (BoLocPhanLoai != "Tất cả")
                    {
                        query = query.Where(x => x.PhanLoai == BoLocPhanLoai);
                    }

                    var list = query.ToList();
                    int stt = 1;
                    foreach (var item in list) item.STT = stt++;
                    DanhSachDichVu = new ObservableCollection<DichVu>(list);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải thực đơn: " + ex.Message);
            }
        }

        private string _thongBaoText;
        public string ThongBaoText
        {
            get => _thongBaoText;
            set { _thongBaoText = value; OnPropertyChanged(nameof(ThongBaoText)); }
        }

        private Visibility _hienThiThongBao = Visibility.Collapsed;
        public Visibility HienThiThongBao
        {
            get => _hienThiThongBao;
            set { _hienThiThongBao = value; OnPropertyChanged(nameof(HienThiThongBao)); }
        }

    }
}