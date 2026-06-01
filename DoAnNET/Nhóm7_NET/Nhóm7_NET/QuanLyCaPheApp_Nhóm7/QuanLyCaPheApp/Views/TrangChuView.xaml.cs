using QuanLyCaPheApp.Models;
using QuanLyCaPheApp.ViewModels;
using System.Windows.Controls;

namespace QuanLyCaPheApp.Views
{
    public partial class TrangChuView : Page
    {
        private TrangChuViewModel _vm;

        public TrangChuView()
        {
            InitializeComponent();
            _vm = new TrangChuViewModel();
            DataContext = _vm;
            _vm.BanDuocChon = OnBanChon;
            // WPF Page dùng Loaded event thay vì OnNavigatedTo
            this.Loaded += (s, e) => _vm.Load();
        }

        private void OnBanChon(Ban ban)
        {
            switch (ban.TrangThai)
            {
                case "Trống":
                    NavigationService?.Navigate(new DatBanView());
                    break;
                case "Đang dùng":
                    var goi = new GoiMonView();
                    goi.SetBan(ban);
                    NavigationService?.Navigate(goi);
                    break;
                case "Đã đặt":
                    NavigationService?.Navigate(new DatBanView());
                    break;
            }
        }
    }
}
