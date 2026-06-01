using QuanLyCaPheApp.Models;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Controls;

namespace QuanLyCaPheApp.Services
{
    public static class PrintService
    {
        public static void InHoaDon(HoaDon hd)
        {
            var doc = new FlowDocument { PageWidth = 400, PagePadding = new Thickness(20) };

            // Header
            var title = new Paragraph(new Run("QUẢN LÝ QUÁN CÀ PHÊ"))
            { TextAlignment = TextAlignment.Center, FontSize = 16, FontWeight = FontWeights.Bold };
            doc.Blocks.Add(title);

            doc.Blocks.Add(new Paragraph(new Run($"Hóa đơn #: {hd.MaHoaDon}")) { TextAlignment = TextAlignment.Center });
            doc.Blocks.Add(new Paragraph(new Run($"Bán: {hd.TenBan}  |  {DateTime.Now:dd/MM/yyyy HH:mm}")) { TextAlignment = TextAlignment.Center });
            doc.Blocks.Add(new Paragraph(new Run("----------------------------------------------")) { TextAlignment = TextAlignment.Center });

            // Chi tiet mon
            foreach (var ct in hd.ChiTiet)
            {
                var line = new Paragraph(new Run($"{ct.TenSanPham}  x{ct.SoLuong}   {ct.ThanhTien:N0}d"));
                doc.Blocks.Add(line);
            }

            doc.Blocks.Add(new Paragraph(new Run("----------------------------------------------")) { TextAlignment = TextAlignment.Center });
            doc.Blocks.Add(new Paragraph(new Run($"Tạm tính:     {hd.TongTamTinh:N0}d")));
            if (hd.SoTienGiam > 0)
                doc.Blocks.Add(new Paragraph(new Run($"Giảm giá :    -{hd.SoTienGiam:N0}d")));
            doc.Blocks.Add(new Paragraph(new Run($"Thanh toán:   {hd.TongThanhToan:N0}d"))
                { FontWeight = FontWeights.Bold, FontSize = 14 });
            if (hd.TienKhachDua.HasValue)
                doc.Blocks.Add(new Paragraph(new Run($"Khach đưa:    {hd.TienKhachDua:N0}d")));
            if (hd.TienThua.HasValue)
                doc.Blocks.Add(new Paragraph(new Run($"Tien thua:    {hd.TienThua:N0}d")));
            doc.Blocks.Add(new Paragraph(new Run("Cam on quy khach. Hen gap lai!")) { TextAlignment = TextAlignment.Center });

            // Print dialog
            var pd = new PrintDialog();
            if (pd.ShowDialog() == true)
            {
                doc.PageWidth  = pd.PrintableAreaWidth;
                var docPaginator = ((IDocumentPaginatorSource)doc).DocumentPaginator;
                pd.PrintDocument(docPaginator, $"HoaDon_{hd.MaHoaDon}");
            }
        }
    }
}
