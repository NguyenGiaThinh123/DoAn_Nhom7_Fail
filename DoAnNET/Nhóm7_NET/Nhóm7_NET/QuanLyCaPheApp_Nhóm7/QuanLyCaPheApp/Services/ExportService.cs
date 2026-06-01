using ClosedXML.Excel;
using QuanLyCaPheApp.Repositories;
using System;
using System.Collections.Generic;
using System.IO;

namespace QuanLyCaPheApp.Services
{
    public static class ExportService
    {
        public static string XuatThongKeExcel(List<ThongKeNgay> data, DateTime tuNgay, DateTime denNgay)
        {
            var fileName = $"BaoCao_DoanhThu_{tuNgay:yyyyMMdd}_{denNgay:yyyyMMdd}.xlsx";
            var folder   = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "CafeReports");
            Directory.CreateDirectory(folder);
            var filePath = Path.Combine(folder, fileName);

            using var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Doanh Thu");

            // Title
            ws.Cell(1, 1).Value = "BAO CAO DOANH THU QUAN CA PHE";
            ws.Range(1, 1, 1, 7).Merge();
            ws.Cell(1, 1).Style.Font.Bold = true;
            ws.Cell(1, 1).Style.Font.FontSize = 16;
            ws.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell(2, 1).Value = $"Tu ngay: {tuNgay:dd/MM/yyyy}  Den ngay: {denNgay:dd/MM/yyyy}";
            ws.Range(2, 1, 2, 7).Merge();
            ws.Cell(2, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            // Header row
            int row = 4;
            string[] headers = { "STT", "Ngay", "So Hoa Don", "Doanh Thu Goc", "Tong Giam Gia", "Thuc Thu", "So Khach" };
            for (int c = 0; c < headers.Length; c++)
            {
                ws.Cell(row, c + 1).Value = headers[c];
                ws.Cell(row, c + 1).Style.Font.Bold = true;
                ws.Cell(row, c + 1).Style.Fill.BackgroundColor = XLColor.FromHtml("#5C3D2E");
                ws.Cell(row, c + 1).Style.Font.FontColor = XLColor.White;
                ws.Cell(row, c + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            }

            // Data rows
            decimal tongDT = 0;
            int stt = 1;
            foreach (var d in data)
            {
                row++;
                ws.Cell(row, 1).Value = stt++;
                ws.Cell(row, 2).Value = d.Ngay.ToString("dd/MM/yyyy");
                ws.Cell(row, 3).Value = d.SoHoaDon;
                ws.Cell(row, 4).Value = (double)d.DoanhThuGoc;
                ws.Cell(row, 5).Value = (double)d.TongGiamGia;
                ws.Cell(row, 6).Value = (double)d.ThucThu;
                ws.Cell(row, 7).Value = d.SoKhachHang;

                ws.Cell(row, 4).Style.NumberFormat.Format = "#,##0";
                ws.Cell(row, 5).Style.NumberFormat.Format = "#,##0";
                ws.Cell(row, 6).Style.NumberFormat.Format = "#,##0";

                if (stt % 2 == 0)
                    ws.Row(row).Style.Fill.BackgroundColor = XLColor.FromHtml("#FBF7F4");
                tongDT += d.ThucThu;
            }

            // Total row
            row++;
            ws.Cell(row, 1).Value = "Tong cong";
            ws.Range(row, 1, row, 3).Merge();
            ws.Cell(row, 6).Value = (double)tongDT;
            ws.Cell(row, 6).Style.NumberFormat.Format = "#,##0";
            ws.Row(row).Style.Font.Bold = true;
            ws.Row(row).Style.Fill.BackgroundColor = XLColor.FromHtml("#D4A76A");

            ws.Columns().AdjustToContents();
            wb.SaveAs(filePath);
            return filePath;
        }

        public static string XuatDanhSachSanPham(List<Models.SanPham> data)
        {
            var fileName = $"SanPham_{DateTime.Now:yyyyMMdd_HHmm}.xlsx";
            var folder   = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "CafeReports");
            Directory.CreateDirectory(folder);
            var filePath = Path.Combine(folder, fileName);

            using var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("San Pham");

            ws.Cell(1, 1).Value = "DANH SACH SAN PHAM";
            ws.Range(1, 1, 1, 6).Merge();
            ws.Cell(1, 1).Style.Font.Bold = true;
            ws.Cell(1, 1).Style.Font.FontSize = 14;
            ws.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            string[] headers = { "STT", "Ten San Pham", "Loai", "Gia Ban", "Giam Gia", "Don Vi" };
            for (int c = 0; c < headers.Length; c++)
            {
                ws.Cell(2, c + 1).Value = headers[c];
                ws.Cell(2, c + 1).Style.Font.Bold = true;
                ws.Cell(2, c + 1).Style.Fill.BackgroundColor = XLColor.FromHtml("#5C3D2E");
                ws.Cell(2, c + 1).Style.Font.FontColor = XLColor.White;
            }

            int row = 2;
            int stt = 1;
            foreach (var sp in data)
            {
                row++;
                ws.Cell(row, 1).Value = stt++;
                ws.Cell(row, 2).Value = sp.TenSanPham;
                ws.Cell(row, 3).Value = sp.TenLoai;
                ws.Cell(row, 4).Value = (double)sp.GiaBan;
                ws.Cell(row, 4).Style.NumberFormat.Format = "#,##0";
                ws.Cell(row, 5).Value = sp.PhanTramGiam > 0 ? $"{sp.PhanTramGiam}%" : "-";
                ws.Cell(row, 6).Value = sp.DonVi;
            }

            ws.Columns().AdjustToContents();
            wb.SaveAs(filePath);
            return filePath;
        }
    }
}
