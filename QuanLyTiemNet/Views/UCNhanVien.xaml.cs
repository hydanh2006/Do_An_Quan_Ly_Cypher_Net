using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using QuanLyTiemNet.ViewModels;
namespace QuanLyTiemNet.Views
{
    /// <summary>
    /// Interaction logic for UCNhanVien.xaml
    /// </summary>
    public partial class UCNhanVien : UserControl
    {
        public UCNhanVien()
        {
            InitializeComponent();
            this.DataContext = new NhanVienViewModel();
        }
    }
}
