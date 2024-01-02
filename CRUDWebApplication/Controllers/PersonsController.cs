using CRUDWebApplication.Filters;
using CRUDWebApplication.Filters.ActionFilters;
using CRUDWebApplication.Filters.AuthorizationFilter;
using CRUDWebApplication.Filters.ExceptionFilters;
using CRUDWebApplication.Filters.ResourceFilters;
using CRUDWebApplication.Filters.ResultFilters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace CRUDWebApplication.Controllers
{
    // overrie th conventional rounting 
    [Route("persons")]
    // Handle exception for controller level for filters only model binding both action method and filter 
    // [TypeFilter(typeof(HandleExceptionFilter))]
    // This filter applied for the all action method for this controller.
    //[TypeFilter(typeof(ResponseHeaderActionFilter), Arguments = new object[] { "My-Key-From-Controller", "My-Value-From-Controller", 3 }, Order = 3)]
    // East way to access the ResponseHeaderActionFilter class using IFilterFactory 
    [ResponseHeaderFilterFactoryAttribute("My-Key-From-Controller", "My-Value-From-Controller", 3)]
    [TypeFilter(typeof(PersonAlwaysRunResultFilter))]
    public class PersonsController : Controller
    {
        // private field 
        //private readonly IPersonService _personService;

        //S.O.L.I.D | I > Interface Segregation Principle according create the specific interface for the purposeful operation like only personadder interface only add the person not update or deleted functionality added to this interface 
        private readonly IPersonAdderService _personAdderService;
        private readonly IPersonUpdaterService _personUpdaterService;
        private readonly IPersonDeleterService _personDeleterService;
        private readonly IPersonGetterService _personGetterService;
        private readonly IPersonSorterService _personSorterService;
        private readonly ICountriesService _countriesService;
        private readonly ILogger<PersonsController> _logger;

        // constructor
        public PersonsController(IPersonAdderService personAdderService, IPersonUpdaterService personUpdaterService, IPersonDeleterService personDeleterService, IPersonGetterService personGetterService, IPersonSorterService personSorterService, ICountriesService countriesService, ILogger<PersonsController> logger)
        {
            _personAdderService = personAdderService;
            _personUpdaterService = personUpdaterService;
            _personDeleterService = personDeleterService;
            _personGetterService = personGetterService;
            _personSorterService = personSorterService;
            _countriesService = countriesService;
            _logger = logger;

        }


        [Route("index")]
        [Route("/")]
        [ServiceFilter(typeof(PersonListActionFilters), Order = 4)]
        //[TypeFilter(typeof(ResponseHeaderActionFilter), Arguments = new object[] { "My-Key-From-Action", "My-Value-From-Action", 1 }, Order = 1)]
        [ResponseHeaderFilterFactoryAttribute("My-Key-From-Action", "My-Value-From-Action", 1)]
        [TypeFilter(typeof(PersonListResultFilter))]
        [SkipFilter]
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

            List<PersonResponse>? persons = await _personGetterService.GetFilteredPerson(serachBy, searchString);

            // when use ViewBag : when transfer data controller to view 
            // return the given search field value controller to view           
            //ViewBag.CurrentSearchBy = serachBy;
            //ViewBag.CurrentSearchString = searchString;

            // sorting 
            List<PersonResponse>? sortedPersons = await _personSorterService.GetSortedPersons(persons, sortBy, sortOrder);
            //ViewBag.CurrentSortBy = sortBy.ToString();
            //ViewBag.CurrentSearchString = searchString;

            return View(sortedPersons);
        }

        #region create
        [Route("create")]
        [HttpGet]
        //[TypeFilter(typeof(ResponseHeaderActionFilter), Arguments = new object[] { "My-Key-From-Action", "My-Value-From-Action", 4 })]
        public async Task<IActionResult> Create()
        {
            List<CountryResponse> countries = await _countriesService.GetAllCountryList();
            // using SelectListItem > create the drop down list for view 
            ViewBag.Countries = countries.Select(temp => new SelectListItem() { Text = temp.CountryName, Value = temp.CountryID.ToString() });

            return View();
        }

        [HttpPost]
        [Route("Create")]
        [TypeFilter(typeof(PersonCreateAndEditPostActionFilter))]
        [TypeFilter(typeof(FeatureDisabledResourceFilter), Arguments = new object[] { false })]
        public async Task<IActionResult> Create(PersonAddRequest personRequest)
        {

            PersonResponse personResponse = await _personAdderService.AddPerson(personRequest);

            // navigate to index() action method it (it makes another get request to "persons/index")
            return RedirectToAction("Index", "Persons");
        }
        #endregion

        #region Edit
        [HttpGet]
        [Route("[action]/{personID}")]
        [TypeFilter(typeof(TokenResultFilter))]
        public async Task<IActionResult> Edit(Guid personID)
        {
            PersonResponse? personResponse = await _personGetterService.GetPersonByPersonID(personID);

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
        [TypeFilter(typeof(PersonCreateAndEditPostActionFilter))]
        [TypeFilter(typeof(TokenAuthorizationFilter))]
        public async Task<IActionResult> Edit(PersonUpdateRequest personRequest)
        {
            PersonResponse? personResponse = await _personGetterService.GetPersonByPersonID(personRequest.PersonID);

            if (personResponse == null)
            {
                return RedirectToAction("Index");
            }
            personRequest.PersonID = new Guid();
            PersonResponse updatePerson = await _personUpdaterService.UpdatePerson(personRequest);
            return RedirectToAction("Index");



        }
        #endregion

        #region Delete

        [HttpGet]
        [Route("[action]/{personID}")]
        public async Task<IActionResult> Delete(Guid? personID)
        {
            PersonResponse? personResponse = await _personGetterService.GetPersonByPersonID(personID);

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
            PersonResponse? personResponse = await _personGetterService.GetPersonByPersonID(personUpdateRequest.PersonID);

            if (personResponse == null)
            {
                return RedirectToAction("Index");
            }

            await _personDeleterService.DeletePerson(personUpdateRequest.PersonID);
            return RedirectToAction("Index");
        }

        [Route("PersonPDF")]
        public async Task<IActionResult> PersonPDF()
        {
            List<PersonResponse> persons = await _personGetterService.GetAllPersonList();

            //return view as pdf file using rotativa package inbuild method
            return new ViewAsPdf("PersonPDF", persons, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins() { Top = 20, Right = 20, Bottom = 20, Left = 20 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape

            };
        }

        [Route("PersonCSV")]
        public async Task<IActionResult> PersonCSV()
        {
            MemoryStream memoryStream = await _personGetterService.GetPersonCSV();
            return File(memoryStream, "application/octet-stream", "persons.csv");
        }

        [Route("PersonsExcel")]
        public async Task<IActionResult> PersonsExcel()
        {
            MemoryStream memoryStream = await _personGetterService.GetPersonsExcel();
            return File(memoryStream, "application/vnd.openxmformats-officedocument.spreadsheetml.sheet", "persons.xlsx");
        }
        #endregion
    }
}
