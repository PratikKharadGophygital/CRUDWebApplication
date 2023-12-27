using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

namespace CRUDWebApplication.Controllers
{
    [Route("Countries")]
    public class CountriesController : Controller
    {

        private readonly IPersonService_ _personService;
        public CountriesController(IPersonService_ personService)
        {
            _personService = personService;
        }

        [Route("UploadFromExcel")]
        [HttpGet]
        public IActionResult UploadFromExcel()
        {
            return View();
        }


        [Route("UploadFromExcel")]
        [HttpPost]
        public async Task<IActionResult> UploadFromExcel(IFormFile excelFile)
        {
            try
            {
                if (excelFile == null || excelFile.Length == 0)
                {
                    ViewBag.ErrorMessage = "Please select an xlsx file";
                    return View();
                }

                if (Path.GetExtension(excelFile.FileName).Equals("xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    ViewBag.ErrorMessage = "Unsupported  file. 'xlsx' file is expected";
                    return View();
                }
                int countriesCountInserted = await _personService.UploadCountriesFromExcelFile(excelFile);

                ViewBag.Messages = $"{countriesCountInserted} Contries Uploaded";
                return View();
            }
            catch (Exception)
            {

                throw;
            }



        }

    }
}
