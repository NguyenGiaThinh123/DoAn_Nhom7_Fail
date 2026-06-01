using QuanLyCaPheApp.Repositories;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace QuanLyCaPheApp.ViewModels
{
    public class ThongKeDoanhThuViewModel : BaseViewModel
    {
        private readonly ThongKeRepository _repo = new();
        private ObservableCollection<ThongKeNgay> _danhSach = new();
        private ObservableCollection<TopSanPham>  _topSP    = new();
        private DateTime _tuNgay  = DateTime.Today.AddDays(-30);
        private DateTime _denNgay = DateTime.Today;
        private decimal  _tongDT;
        private int      _tongHD;
        private decimal  _tongGiam;

        public ObservableCollection<ThongKeNgay> DanhSach   { get => _danhSach; set => SetProperty(ref _danhSach, value); }
        public ObservableCollection<TopSanPham>  TopSanPham { get => _topSP;    set => SetProperty(ref _topSP,    value); }
        public DateTime TuNgay  { get => _tuNgay;  set => SetProperty(ref _tuNgay,  value); }
        public DateTime DenNgay { get => _denNgay; set => SetProperty(ref _denNgay, value); }
        public decimal  TongDoanhThu { get => _tongDT;   set { SetProperty(ref _tongDT,   value); OnPropertyChanged(nameof(TongDTText)); } }
        public int      TongHoaDon  { get => _tongHD;    set => SetProperty(ref _tongHD,   value); }
        public decimal  TongGiamGia { get => _tongGiam;  set => SetProperty(ref _tongGiam, value); }
        public string   TongDTText  => $"{TongDoanhThu:N0} d";

        public ICommand TraCuuCommand   { get; }
        public ICommand HomNayCommand   { get; }
        public ICommand ThangNayCommand { get; }

        public ThongKeDoanhThuViewModel()
        {
            TraCuuCommand   = new RelayCommand(_ => Load());
            HomNayCommand   = new RelayCommand(_ => { TuNgay = DenNgay = DateTime.Today; Load(); });
            ThangNayCommand = new RelayCommand(_ => {
                TuNgay = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                DenNgay = DateTime.Today; Load(); });
            Load();
        }

        public void Load()
        {
            var ds = _repo.GetThongKeTheoNgay(TuNgay, DenNgay);
            DanhSach    = new ObservableCollection<ThongKeNgay>(ds);
            TopSanPham  = new ObservableCollection<TopSanPham>(_repo.GetTopSanPham(TuNgay, DenNgay, 10));
            TongDoanhThu = ds.Sum(d => d.ThucThu);
            TongHoaDon   = ds.Sum(d => d.SoHoaDon);
            TongGiamGia  = ds.Sum(d => d.TongGiamGia);
        }
    }
}
