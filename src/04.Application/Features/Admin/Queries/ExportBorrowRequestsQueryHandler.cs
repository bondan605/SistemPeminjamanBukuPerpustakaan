using ClosedXML.Excel;
using LibraryManagementSystem.Application.Interfaces;
using MediatR;

namespace LibraryManagementSystem.Application.Features.Admin.Queries;

public class ExportBorrowRequestsQueryHandler : IRequestHandler<ExportBorrowRequestsQuery, byte[]>
{
    private readonly IUnitOfWork _unitOfWork;

    public ExportBorrowRequestsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<byte[]> Handle(ExportBorrowRequestsQuery request, CancellationToken cancellationToken)
    {
        var records = await _unitOfWork.BorrowRequests.GetAllWithDetailsAsync();

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Laporan Peminjaman");

        worksheet.Cell(1, 1).Value = "ID";
        worksheet.Cell(1, 2).Value = "Peminjam";
        worksheet.Cell(1, 3).Value = "Judul Buku";
        worksheet.Cell(1, 4).Value = "Tanggal Pinjam";
        worksheet.Cell(1, 5).Value = "Tanggal Kembali";
        worksheet.Cell(1, 6).Value = "Status";
        worksheet.Cell(1, 7).Value = "Disetujui Oleh";
        worksheet.Cell(1, 8).Value = "Catatan";

        var headerRow = worksheet.Range("A1:H1");
        headerRow.Style.Font.Bold = true;
        headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;

        int currentRow = 2;
        foreach (var item in records)
        {
            worksheet.Cell(currentRow, 1).Value = item.Id;
            worksheet.Cell(currentRow, 2).Value = item.User?.Name ?? "-";
            worksheet.Cell(currentRow, 3).Value = item.Book?.Title ?? "-";
            worksheet.Cell(currentRow, 4).Value = item.BorrowDate.ToString("yyyy-MM-dd");
            worksheet.Cell(currentRow, 5).Value = item.ReturnDate.ToString("yyyy-MM-dd");
            worksheet.Cell(currentRow, 6).Value = item.Status.ToString();
            worksheet.Cell(currentRow, 7).Value = item.ApprovedBy?.Name ?? "-";
            worksheet.Cell(currentRow, 8).Value = item.Notes ?? "-";

            currentRow++;
        }

        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}