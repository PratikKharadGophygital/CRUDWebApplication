using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using RepositoryContracts;
using Serilog;
using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{



    /// <summary>
    /// Open/Close Principle Using inheritance. 
    /// Real world project we are not use the Inheritance because the violating lisco principle. 
    ///  This example for just we are use the inheritances. 
    /// This is the example of Open/Close Principle with inheritances  how can we implement the method          without modify the exsiting method create the new method using inheritances overrride the               base class. 
    /// </summary>
    public class PersonsGetterServiceChild : PersonGetterService
    {
        // We are supply the same base class constructore 
        public PersonsGetterServiceChild(IPersonsRepository personsRepository, ILogger<PersonGetterService> logger, IDiagnosticContext diagnosticContext) : base (personsRepository, logger, diagnosticContext)
        {

        }

        public override async Task<MemoryStream> GetPersonsExcel()
        {
            MemoryStream memoryStream = new MemoryStream();
            using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("PersonsSheet");
                worksheet.Cells["A1"].Value = "Person Name";
                worksheet.Cells["B1"].Value = "Email";
                worksheet.Cells["C1"].Value = "Date of birth";


                using (ExcelRange headerCells = worksheet.Cells["A1:H1"])
                {
                    headerCells.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    headerCells.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    headerCells.Style.Font.Bold = true;
                }

                int row = 2;
                List<PersonResponse> persons = await GetAllPersonList();

                foreach (PersonResponse person in persons)
                {
                    worksheet.Cells[row, 1].Value = person.PersonName;
                    worksheet.Cells[row, 2].Value = person.Email;
                    if (person.DateOfBirth.HasValue)
                        worksheet.Cells[row, 3].Value = person.DateOfBirth.Value.ToString("yyyy-MM-dd");
                    row++;
                }

                worksheet.Cells[$"A1:H{row}"].AutoFitColumns();

                await excelPackage.SaveAsync();
            }
            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}
