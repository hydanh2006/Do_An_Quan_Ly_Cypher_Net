using QuanLyTiemNet.Models;
using QuanLyTiemNet.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;

namespace QuanLyTiemNet.ViewModels
{
    internal class MayTramViewModel : BaseViewModel
    {
        //-- Quy ước: 1 = Trống, 2 = Đang chơi, 3 = Bảo trì
        private QuanLyTiemNetEntities1 db = new QuanLyTiemNetEntities1();
        public ObservableCollection<MayTram> DS_May { get ; set; } = new ObservableCollection<MayTram>();

        public ICommand ThemMayCommand { get; set; }
        public ICommand MoMenuKhoiDongCommand { get; set; }
        public ICommand XoaMayCommand { get; set; }

        public int TongSoMay => DS_May.Count;

        public int MayDangDung => DS_May.Count(m => m.TrangThai == 2);

        public int MayBaoTri => DS_May.Count(m => m.TrangThai == 3);

        private string _loaiMayDuocChon = "Thường";
        public string LoaiMayDuocChon
        {
            get => _loaiMayDuocChon;
            set { _loaiMayDuocChon = value; OnPropertyChanged(LoaiMayDuocChon); }
        }


        
        public void CapNhatThongKe()
        {
            OnPropertyChanged(nameof(TongSoMay));
            OnPropertyChanged(nameof(MayDangDung));
            OnPropertyChanged(nameof(MayBaoTri));
        }

        public System.Windows.Visibility HienThiAdmin
        {
            get
            {
                if (Models.PhienDangNhap.NhanVienHienTai != null && Models.PhienDangNhap.NhanVienHienTai.VaiTro == 0)
                {
                    return System.Windows.Visibility.Visible;
                }
                return System.Windows.Visibility.Collapsed;
            }
        }

        public MayTramViewModel() 
        {
            loadData();
            ThemMayCommand = new RelayCommand(p =>
            {
                var maymoi = new MayTram
                {
                    TenMay = "Máy " + (DS_May.Count + 1) + " " + (LoaiMayDuocChon),
                    TrangThai = 1,
                    LoaiMay = this.LoaiMayDuocChon,
                    GiaMoiGio = this.LoaiMayDuocChon == "VIP" ? 12000 : 8000
                };

                db.MayTrams.Add(maymoi);
                db.SaveChanges();
                loadData();
                CapNhatThongKe();
            });

            XoaMayCommand = new RelayCommand(p =>
            {
                var may = p as MayTram;
                if (may == null) return;

                if (may.TrangThai == 2)
                {
                    System.Windows.MessageBox.Show("Không thể xóa máy đang có người chơi!", "Cảnh báo an toàn",
                        System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                    return;
                }

                var xacNhan = System.Windows.MessageBox.Show($"Bạn có chắc chắn muốn xóa {may.TenMay} vĩnh viễn không?",
                    "Xác nhận xóa", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question);

                if (xacNhan == System.Windows.MessageBoxResult.Yes)
                {
                    try
                    {
                        using (var db = new QuanLyTiemNetEntities1())
                        {
                            var mayInDb = db.MayTrams.Find(may.MaMay);
                            if (mayInDb != null)
                            {
                                var lichSuPhien = db.PhienChois.Where(pc => pc.MaMay == may.MaMay).ToList();
                                foreach (var phien in lichSuPhien)
                                {
                                    db.PhienChois.Remove(phien);
                                }

                                db.MayTrams.Remove(mayInDb);
                                db.SaveChanges();
                            }
                        }
                        loadData();
                        CapNhatThongKe();
                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show("Lỗi CSDL khi xóa: " + ex.Message, "Lỗi");
                    }
                }
            });

            MoMenuKhoiDongCommand = new RelayCommand(p => {
                var may = p as MayTram;
                if (may == null) return;

                DialogKhoiDong dialog = new DialogKhoiDong(may);
                if (dialog.ShowDialog() == true)
                {
                    loadData(); 
                    CapNhatThongKe();
                }
            });

            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (s, e) => {
                foreach (var m in DS_May)
                {
                    if (m.PhienHienTai != null)
                        m.OnPropertyChanged(nameof(m.ThoiGianHienThi));
                }
            };
            timer.Start();

            OnPropertyChanged(nameof(HienThiAdmin));
        }


        private void loadData()
        {
            using (var db = new QuanLyTiemNetEntities1())
            {
                var listmay = db.MayTrams
                        .Include("PhienChois")
                        .Include("PhienChois.TaiKhoan")
                        .ToList();
                DS_May.Clear();
                foreach (var item in listmay)
                {
                    DS_May.Add(item);
                }
            }

        }
    }
}
