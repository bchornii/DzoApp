using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Application.Web.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Application.Web.Custom
{
    public class SpreadSheetFacade
    {
        private readonly FileInfo _fi;

        public SpreadSheetFacade(string fileName)
        {
            _fi = new FileInfo(fileName);            
        }

        public void ExportData(List<TenderInformation> tenders)
        {
            using (var package = new ExcelPackage(_fi))
            {
                var worksheet = package.Workbook.Worksheets.Add("Tenders");

                // Add headers
                var colNames = new[] {"Номер", "Замовник", "Адреса",
                            "Телефон","e-mail","Контактна особа","Сума закупівлі/виграшу",
                            "Сума пропозиції","Переможець","К-сть камер"};

                for (var i = 1; i <= colNames.Length; i++)
                {
                    worksheet.Cells[1, i].Value = colNames[i - 1];
                }

                // format title
                using (var range = worksheet.Cells[1, 1, 1, 10])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                    range.Style.Font.Color.SetColor(Color.Black);
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                }

                // format values
                using (var range = worksheet.Cells[1, 1, tenders.Count + 1, 10])
                {
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                }

                // paste information into sheet
                for (var i = 1; i < tenders.Count + 1; i++)
                {
                    worksheet.Cells[i + 1, 1].Value = i;
                    worksheet.Cells[i + 1, 2].Value = tenders[i - 1].Owner;
                    worksheet.Cells[i + 1, 3].Value = tenders[i - 1].Address;
                    worksheet.Cells[i + 1, 4].Value = tenders[i - 1].Phone;
                    worksheet.Cells[i + 1, 5].Value = tenders[i - 1].Email;
                    worksheet.Cells[i + 1, 6].Value = tenders[i - 1].ContactPerson;
                    worksheet.Cells[i + 1, 7].Value = tenders[i - 1].ProposePrice;
                    worksheet.Cells[i + 1, 8].Value = tenders[i - 1].ProposePrice;
                    worksheet.Cells[i + 1, 9].Value = tenders[i - 1].Winner;
                    worksheet.Cells[i + 1, 10].Value = tenders[i - 1].Amount;
                }

                worksheet.Cells.AutoFitColumns(0);  //Autofit columns for all cells

                package.Save();
            }
        }
    }
}