using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace CRUDWebApplication.Controllers
{
    [Route("persons")]
    public class PersonsController : Controller
    {
        // private field 
        private readonly IPersonService _personService;
        private readonly ICountriesService _countriesService;
        private readonly ILogger<PersonsController> _logger;

        // constructor
        public PersonsController(IPersonService personService, ICountriesService countriesService, ILogger<PersonsController> logger)
        {
            _personService = personService;
            _countriesService = countriesService;
            _logger = logger;

        }


        [Route("index")]
        [Route("/")]
        public async Task<IActionResult> Index(string serachBy, string? searchString, string sortBy = nameof(PersonResponse.PersonName), SortOrderOptions sortOrder = SortOrderOptions.ASC)
        {
            //if user not select any value then arugment set the default value.
            // if the user is select the any value then send the request argument are ignore the default value this is concept of the deafult argument
            // example : if user send the sortBy = Email then argument is ignore the PersonResponse.PersonName if argument is null the send the PersonResponse.PersonName value.

            // when use Dictionary : when show the value of key and value paire
            // using dictionary show the drop down value

            _logger.LogInformation("Index action method personcontroller");
            _logger.LogDebug($"serachBy: {serachBy},searchString: {searchString}, sortBy:{nameof(PersonResponse.PersonName)},sortOrder:{SortOrderOptions.ASC} ");

            // searching
            ViewBag.SerachFields = new Dictionary<string, string>()
            {
                {nameof(PersonResponse.PersonName), "Person Name" },
                {nameof(PersonResponse.Email), "Email" },
                {nameof(PersonResponse.DateOfBirth), "Date Of Birth" },
                {nameof(PersonResponse.Geneder), "Geneder" },
                {nameof(PersonResponse.CountryID), "CountryID" },
                {nameof(PersonResponse.Address), "Address" },
            };

            List<PersonResponse>? persons = await _personService.GetFilteredPerson(serachBy, searchString);

            // when use ViewBag : when transfer data controller to view 
            // return the given search field value controller to view           
            ViewBag.CurrentSearchBy = serachBy;
            ViewBag.CurrentSearchString = searchString;

            // sorting 
            List<PersonResponse>? sortedPersons = await _personService.GetSortedPersons(persons, sortBy, sortOrder);
            ViewBag.CurrentSortBy = sortBy.ToString();
            ViewBag.CurrentSearchString = searchString;

            return View(sortedPersons);
        }

        #region create
        [Route("create")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            List<CountryResponse> countries = await _countriesService.GetAllCountryList();
            // using SelectListItem > create the drop down list for view 
            ViewBag.Countries = countries.Select(temp => new SelectListItem() { Text = temp.CountryName, Value = temp.CountryID.ToString() });

            return View();
        }

        [Route("Create")]
        [HttpPost]
        public async Task<IActionResult> Create(PersonAddRequest personAddRequest)
        {
            if (!ModelState.IsValid)
            {
                List<CountryResponse> countries = await _countriesService.GetAllCountryList();
                ViewBag.Countries = countries;
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).SelectMany(e => e.ErrorMessage).ToList();
                return View();
            }

            PersonResponse personResponse = await _personService.AddPerson(personAddRequest);

            // navigate to index() action method it (it makes another get request to "persons/index")
            return RedirectToAction("Index", "Persons");
        }
        #endregion

        #region Edit
        [Route("[action]/{personID}")]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid personID)
        {
            PersonResponse? personResponse = await _personService.GetPersonByPersonID(personID);

            if (personResponse == null)
            {
                return RedirectToAction("Index");
            }

            PersonUpdateRequest? personUpdateRequest = personResponse.ToPersonUpdateRequest();

            List<CountryResponse> countries = await _countriesService.GetAllCountryList();


            // using SelectListItem > create the drop down list for view 
            ViewBag.Countries = countries.Select(temp => new SelectListItem() { Text = temp.CountryName, Value = temp.CountryID.ToString() });

            return View(personUpdateRequest);
        }


        [HttpPost]
        [Route("[action]/{personID}")]
        public async Task<IActionResult> Edit(PersonUpdateRequest personUpdateRequest)
        {
            PersonResponse? personResponse = await _personService.GetPersonByPersonID(personUpdateRequest.PersonID);

            if (personResponse == null)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {

                PersonResponse updatePerson = await _personService.UpdatePerson(personUpdateRequest);
                return RedirectToAction("Index");
            }
            else
            {
                List<CountryResponse> countries = await _countriesService.GetAllCountryList();
                ViewBag.Countries = countries;
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).SelectMany(e => e.ErrorMessage).ToList();
                return View(personResponse.ToPersonUpdateRequest());
            }


        }
        #endregion

        #region Delete

        [HttpGet]
        [Route("[action]/{personID}")]
        public async Task<IActionResult> Delete(Guid? personID)
        {
            PersonResponse personResponse = await _personService.GetPersonByPersonID(personID);

            if (personResponse == null)
            {
                return RedirectToAction("Index");
            }

            return View(personResponse);
        }

        [HttpPost]
        [Route("[action]/{personID}")]
        public async Task<IActionResult> Delete(PersonUpdateRequest personUpdateRequest)
        {
            PersonResponse? personResponse = await _personService.GetPersonByPersonID(personUpdateRequest.PersonID);

            if (personResponse == null)
            {
                return RedirectToAction("Index");
            }

           await _personService.DeletePerson(personUpdateRequest.PersonID);
            return RedirectToAction("Index");
        }

        [Route("PersonPDF")]
        public async Task<IActionResult> PersonPDF()
        {
            List<PersonResponse> persons = await _personService.GetAllPersonList();

            //return view as pdf file using rotativa package inbuild method
            return new ViewAsPdf("PersonPDF", persons, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins() { Top =20,Right=20,Bottom=20,Left=20},PageOrientation= Rotativa.AspNetCore.Options.Orientation.Landscape

            };
        }

        [Route("PersonCSV")]
        public async Task<IActionResult> PersonCSV()
        {
            MemoryStream memoryStream = await _personService.GetPersonCSV();
            return File(memoryStream, "application/octet-stream", "persons.csv");
        }

        [Route("PersonsExcel")]
        public async Task<IActionResult> PersonsExcel()
        {
            MemoryStream memoryStream = await _personService.GetPersonsExcel();
            return File(memoryStream, "application/vnd.openxmformats-officedocument.spreadsheetml.sheet", "persons.xlsx");
        }
        #endregion
    }
}
