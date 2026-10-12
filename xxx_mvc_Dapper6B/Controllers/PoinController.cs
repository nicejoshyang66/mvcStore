using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using mvc_Dapper.Models.ViewModel;
using mvc_Dapper.Repositories;

namespace mvc_Dapper.Controllers
{
    public class PoinController : Controller
    {
        private readonly IPoinRepository _poinRepository;

        public PoinController(IPoinRepository poinRepository)
        {
            _poinRepository = poinRepository;
        }

        public async Task<IActionResult> List(DateTime? datefm, DateTime? dateto, string? factno)
        {
            var today = DateTime.Today;

            var defaultDateFm = new DateTime(today.Year, today.Month, 1);
            var defaultDateTo = defaultDateFm.AddMonths(1).AddDays(-1);

            var vm = new PoinListVM
            {
                datefm = datefm ?? defaultDateFm,
                dateto = dateto ?? defaultDateTo,
                factno = factno
            };

            vm.Items = await _poinRepository.ListAsync(vm.datefm, vm.dateto, vm.factno);

            vm.TotalCount = vm.Items.Count;
            vm.TotalAmount = vm.Items.Sum(x => x.tran_amt);

            return View(vm);
        }

        public async Task<IActionResult> ExportExcel(DateTime? datefm, DateTime? dateto, string? factno)
        {
            var today = DateTime.Today;

            var defaultDateFm = new DateTime(today.Year, today.Month, 1);
            var defaultDateTo = defaultDateFm.AddMonths(1).AddDays(-1);

            var realDateFm = datefm ?? defaultDateFm;
            var realDateTo = dateto ?? defaultDateTo;

            var items = await _poinRepository.ListAsync(realDateFm, realDateTo, factno);

            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("進貨資料查詢");

            int row = 1;

            // 表頭
            ws.Cell(row, 1).Value = "流水編號";
            ws.Cell(row, 2).Value = "日期";
            ws.Cell(row, 3).Value = "廠商名稱";
            ws.Cell(row, 4).Value = "金額";
            ws.Cell(row, 5).Value = "備註";

            // 表頭樣式：只簡單加粗，不加篩選倒三角
            var headerRange = ws.Range(row, 1, row, 5);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            row++;

            // 明細資料：單純拋資料，不做隔行底色
            foreach (var item in items)
            {
                ws.Cell(row, 1).Value = item.tran_no;
                ws.Cell(row, 2).Value = item.po_date;
                ws.Cell(row, 2).Style.DateFormat.Format = "yyyy-mm-dd";
                ws.Cell(row, 3).Value = item.brief_name;
                ws.Cell(row, 4).Value = item.tran_amt;
                ws.Cell(row, 4).Style.NumberFormat.Format = "#,##0";
                ws.Cell(row, 5).Value = item.fact_memo;

                row++;
            }

            // 欄寬
            ws.Column(1).Width = 16;
            ws.Column(2).Width = 14;
            ws.Column(3).Width = 30;
            ws.Column(4).Width = 14;
            ws.Column(5).Width = 40;

            // 對齊
            ws.Column(1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Column(2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Column(4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            var fileName = $"進貨資料查詢_{DateTime.Now:yyyyMMddHHmmss}.xlsx";

            return File(
                stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName
            );
        }


    }
}