using QuanLyCaPheApp.Models;
using QuanLyCaPheApp.Repositories;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace QuanLyCaPheApp.ViewModels
{
    public class QuanLyKhachHangViewModel : BaseViewModel
    {
        private readonly KhachHangRepository _repo = new();
        private readonly TichDiemRepository _tichDiemRepo = new();
        private ObservableCollection<KhachHang> _danhSach = new();
        private ObservableCollection<TichDiem>  _lichSuDiem = new();
        private KhachHang? _selected;
        private KhachHang  _editItem = new();
        private string _keyword = "";
        private bool _isEditMode;
        private string _thongBao = "";

        public ObservableCollection<KhachHang> DanhSach    { get => _danhSach;    set => SetProperty(ref _danhSach, value); }
        public ObservableCollection<TichDiem>  LichSuDiem  { get => _lichSuDiem;  set => SetProperty(ref _lichSuDiem, value); }
        public KhachHang? Selected  { get => _selected;  set { SetProperty(ref _selected, value); OnSelected(); } }
        public KhachHang  EditItem  { get => _editItem;  set => SetProperty(ref _editItem, value); }
        public string Keyword       { get => _keyword;   set { SetProperty(ref _keyword, value); Search(); } }
        public bool IsEditMode      { get => _isEditMode; set => SetProperty(ref _isEditMode, value); }
        public string ThongBao      { get => _thongBao;  set => SetProperty(ref _thongBao, value); }
        public bool HasSelected     => Selected != null;

        public ICommand SearchCommand { get; }
        public ICommand ThemCommand   { get; }
        public ICommand SuaCommand    { get; }
        public ICommand XoaCommand    { get; }
        public ICommand LuuCommand    { get; }
        public ICommand HuyCommand    { get; }

        public QuanLyKhachHangViewModel()
        {
            SearchCommand = new RelayCommand(_ => Search());
            ThemCommand   = new RelayCommand(_ => BatDauThem());
            SuaCommand    = new RelayCommand(_ => BatDauSua(),  _ => HasSelected);
            XoaCommand    = new RelayCommand(_ => Xoa(),        _ => HasSelected);
            LuuCommand    = new RelayCommand(_ => Luu(),         _ => IsEditMode);
            HuyCommand    = new RelayCommand(_ => Huy(),         _ => IsEditMode);
            Search();
        }

        private void Search()
            => DanhSach = new ObservableCollection<KhachHang>(_repo.GetAll(Keyword));

        private void OnSelected()
        {
            OnPropertyChanged(nameof(HasSelected));
            if (Selected != null)
                LichSuDiem = new ObservableCollection<TichDiem>(
                    _tichDiemRepo.GetByKhachHang(Selected.MaKhachHang));
            else
                LichSuDiem.Clear();
        }

        private void BatDauThem()
        {
            EditItem   = new KhachHang();
            IsEditMode = true;
            ThongBao   = "";
        }

        private void BatDauSua()
        {
            if (Selected == null) return;
            EditItem = new KhachHang
            {
                MaKhachHang = Selected.MaKhachHang,
                HoTen       = Selected.HoTen,
                SoDienThoai = Selected.SoDienThoai,
                Email       = Selected.Email,
                DiaChi      = Selected.DiaChi,
                NgaySinh    = Selected.NgaySinh,
                GhiChu      = Selected.GhiChu,
            };
            IsEditMode = true;
            ThongBao   = "";
        }

        private void Luu()
        {
            if (string.IsNullOrWhiteSpace(EditItem.HoTen))        { ThongBao = "Vui long nhap ho ten."; return; }
            if (string.IsNullOrWhiteSpace(EditItem.SoDienThoai))  { ThongBao = "Vui long nhap so dien thoai."; return; }
            if (EditItem.MaKhachHang == 0)
                _repo.Add(EditItem);
            else
                _repo.Update(EditItem);
            ThongBao   = "Luu thanh cong!";
            IsEditMode = false;
            Search();
        }

        private void Huy() { IsEditMode = false; ThongBao = ""; }

        private void Xoa()
        {
            if (Selected == null) return;
            try { _repo.Delete(Selected.MaKhachHang); ThongBao = "Da xoa khach hang."; Search(); }
            catch { ThongBao = "Khong the xoa. Khach hang co du lieu lien quan."; }
        }
    }
}
