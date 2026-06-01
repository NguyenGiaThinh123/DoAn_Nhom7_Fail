using QuanLyCaPheApp.Repositories;
using QuanLyCaPheApp.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace QuanLyCaPheApp.ViewModels
{
    public class XuatBaoCaoViewModel : BaseViewModel
    {
        private readonly ThongKeRepository  _tkRepo = new();
        private readonly HoaDonRepository   _hdRepo = new();
        private ObservableCollection<ThongKeNgay>  _danhSach = new();
        private DateTime _tuNgay  = DateTime.Today.AddDays(-30);
        private DateTime _denNgay = DateTime.Today;
        private string   _thongBao = "";
        private bool     _daDangTai;

        public ObservableCollection<ThongKeNgay> DanhSach { get => _danhSach; set => SetProperty(ref _danhSach, value); }
        public DateTime TuNgay  { get => _tuNgay;    set => SetProperty(ref _tuNgay,  value); }
        public DateTime DenNgay { get => _denNgay;   set => SetProperty(ref _denNgay, value); }
        public string ThongBao  { get => _thongBao;  set => SetProperty(ref _thongBao, value); }
        public bool DaDangTai   { get => _daDangTai; set => SetProperty(ref _daDangTai, value); }

        public ICommand XemBaoCaoCommand   { get; }
        public ICommand XuatExcelCommand   { get; }
        public ICommand InHoaDonCommand    { get; }

        public XuatBaoCaoViewModel()
        {
            XemBaoCaoCommand = new RelayCommand(_ => XemBaoCao());
            XuatExcelCommand = new RelayCommand(_ => XuatExcel(), _ => DanhSach.Count > 0);
            InHoaDonCommand  = new RelayCommand(p => { /* In hoa don cu the */ });
            XemBaoCao();
        }

        private void XemBaoCao()
        {
            var ds = _tkRepo.GetThongKeTheoNgay(TuNgay, DenNgay);
            DanhSach = new ObservableCollection<ThongKeNgay>(ds);
            ThongBao = $"Tim thay {ds.Count} ngay co doanh thu.";
        }

        private async void XuatExcel()
        {
            DaDangTai = true;
            ThongBao  = "Dang xuat...";
            try
            {
                var filePath = await Task.Run(() =>
                    ExportService.XuatThongKeExcel(DanhSach.ToList(), TuNgay, DenNgay));
                ThongBao = $"Xuat thanh cong: {filePath}";
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(filePath) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                ThongBao = "Loi: " + ex.Message;
            }
            finally { DaDangTai = false; }
        }
    }
}
