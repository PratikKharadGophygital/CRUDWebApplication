using OfficeOpenXml;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    /// <summary>
    /// S.O.L.I.D | Example of Open/Closed Princile whole file 
    /// New modification for praticular method like remove the sum validation and or any type dont change the  exisitiong functionality because this lead the new bugs create the functionality for those             praticular new method 
    /// </summary>
    public class PersonGetterServiceWithExcelFewField : IPersonGetterService
    {
        private readonly PersonGetterService _personGetterService;

        public PersonGetterServiceWithExcelFewField(PersonGetterService personGetterService)
        {
            _personGetterService = personGetterService;
        }
        public async Task<List<PersonResponse>> GetAllPersonList()
        {
            return await _personGetterService.GetAllPersonList();
        }

        public  async Task<List<PersonResponse>?> GetFilteredPerson(string searchBy, string? searchString)
        {
            return await _personGetterService.GetFilteredPerson(searchBy, searchString);
        }

        public async Task<PersonResponse?> GetPersonByPersonID(Guid? personID)
        {
            return await _personGetterService.GetPersonByPersonID(personID);
        }

        public async Task<MemoryStream> GetPersonCSV()
        {
            return await _personGetterService.GetPersonCSV();
        }

        public async Task<MemoryStream> GetPersonsExcel()
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
