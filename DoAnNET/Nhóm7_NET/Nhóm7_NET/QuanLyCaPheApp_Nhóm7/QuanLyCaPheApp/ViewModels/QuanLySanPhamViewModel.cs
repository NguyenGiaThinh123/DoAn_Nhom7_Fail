using QuanLyCaPheApp.Models;
using QuanLyCaPheApp.Repositories;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace QuanLyCaPheApp.ViewModels
{
    public class QuanLySanPhamViewModel : BaseViewModel
    {
        private readonly SanPhamRepository _repo = new();
        private ObservableCollection<SanPham> _danhSach = new();
        private ObservableCollection<LoaiSanPham> _danhSachLoai = new();
        private SanPham? _selected;
        private SanPham _editItem = new();
        private string _keyword = "";
        private int _filterLoai;
        private bool _isEditMode;
        private string _thongBao = "";

        public ObservableCollection<SanPham>    DanhSach     { get => _danhSach;     set => SetProperty(ref _danhSach,     value); }
        public ObservableCollection<LoaiSanPham> DanhSachLoai { get => _danhSachLoai; set => SetProperty(ref _danhSachLoai, value); }
        public SanPham? Selected  { get => _selected;  set { SetProperty(ref _selected, value); OnPropertyChanged(nameof(HasSelected)); } }
        public SanPham  EditItem  { get => _editItem;  set => SetProperty(ref _editItem, value); }
        public string Keyword     { get => _keyword;   set { SetProperty(ref _keyword, value); Search(); } }
        public int FilterLoai     { get => _filterLoai;set { SetProperty(ref _filterLoai, value); Search(); } }
        public bool IsEditMode    { get => _isEditMode; set => SetProperty(ref _isEditMode, value); }
        public string ThongBao    { get => _thongBao;  set => SetProperty(ref _thongBao, value); }
        public bool HasSelected   => Selected != null;

        public ICommand SearchCommand  { get; }
        public ICommand ThemCommand    { get; }
        public ICommand SuaCommand     { get; }
        public ICommand XoaCommand     { get; }
        public ICommand LuuCommand     { get; }
        public ICommand HuyCommand     { get; }
        public ICommand ThemLoaiCommand{ get; }

        public QuanLySanPhamViewModel()
        {
            SearchCommand   = new RelayCommand(_ => Search());
            ThemCommand     = new RelayCommand(_ => BatDauThem());
            SuaCommand      = new RelayCommand(_ => BatDauSua(), _ => HasSelected);
            XoaCommand      = new RelayCommand(_ => Xoa(),       _ => HasSelected);
            LuuCommand      = new RelayCommand(_ => Luu(),        _ => IsEditMode);
            HuyCommand      = new RelayCommand(_ => Huy(),        _ => IsEditMode);
            ThemLoaiCommand = new RelayCommand(_ => ThemLoai());
            Load();
        }

        public void Load()
        {
            DanhSachLoai = new ObservableCollection<LoaiSanPham>(
                new List<LoaiSanPham> { new() { MaLoai = 0, TenLoai = "-- Tất cả --" } }
                    .Concat(_repo.GetAllLoai(true)));
            Search();
        }

        private void Search()
            => DanhSach = new ObservableCollection<SanPham>(
                _repo.GetAll(Keyword, FilterLoai, true));

        private void BatDauThem()
        {
            EditItem   = new SanPham { TrangThai = true, DonVi = "Ly", TichDiemNhan = 1 };
            IsEditMode = true;
            ThongBao   = "";
        }

        private void BatDauSua()
        {
            if (Selected == null) return;
            EditItem = new SanPham
            {
                MaSanPham    = Selected.MaSanPham,
                TenSanPham   = Selected.TenSanPham,
                MaLoai       = Selected.MaLoai,
                GiaBan       = Selected.GiaBan,
                GiaGoc       = Selected.GiaGoc,
                PhanTramGiam = Selected.PhanTramGiam,
                MoTa         = Selected.MoTa,
                DonVi        = Selected.DonVi,
                TichDiemNhan = Selected.TichDiemNhan,
                TrangThai    = Selected.TrangThai,
            };
            IsEditMode = true;
            ThongBao   = "";
        }

        private void Luu()
        {
            if (string.IsNullOrWhiteSpace(EditItem.TenSanPham))  { ThongBao = "Vui long nhap ten san pham."; return; }
            if (EditItem.MaLoai == 0)                             { ThongBao = "Vui long chon loai san pham."; return; }
            if (EditItem.GiaBan <= 0)                             { ThongBao = "Gia ban phai lon hon 0."; return; }

            if (EditItem.MaSanPham == 0)
                _repo.Add(EditItem);
            else
                _repo.Update(EditItem);

            ThongBao   = EditItem.MaSanPham == 0 ? "Them san pham thanh cong!" : "Cap nhat thanh cong!";
            IsEditMode = false;
            Search();
        }

        private void Huy() { IsEditMode = false; ThongBao = ""; }

        private void Xoa()
        {
            if (Selected == null) return;
            _repo.Delete(Selected.MaSanPham);
            ThongBao = "Da an san pham.";
            Search();
        }

        private void ThemLoai()
        {
            // Simple quick-add via input
            var ten = Microsoft.VisualBasic.Interaction.InputBox("Nhap ten loai san pham moi:", "Them Loai", "");
            if (!string.IsNullOrWhiteSpace(ten))
            {
                _repo.AddLoai(new LoaiSanPham { TenLoai = ten, TrangThai = true });
                Load();
            }
        }
    }
}
