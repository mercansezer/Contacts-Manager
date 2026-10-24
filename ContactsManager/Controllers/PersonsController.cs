using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ContactsManager.Controllers
{

    //[Route("persons")]
    [Route("[controller]")]
    public class PersonsController : Controller

    {
        private readonly IPersonsService _personsService;

        private readonly ICountriesService _countriesService;
        public PersonsController(IPersonsService personsService, ICountriesService countriesService)
        {

            _personsService = personsService;
            _countriesService = countriesService;

        }

        //[Route("index")]
        [Route("[action]")]
        [Route("/")]
        public IActionResult Index(string searchBy, string? searchString, string sortBy = nameof(PersonRespone.PersonName), SortOrderOptions sortOption = SortOrderOptions.ASC)
        {

            ViewBag.SearchFields = new Dictionary<string, string>()
            {
                 { nameof(PersonRespone.PersonName), "Person Name" },
                 { nameof(PersonRespone.Email), "Email" },
                 { nameof(PersonRespone.DateOfBirth), "Date of Birth" },
                 { nameof(PersonRespone.Gender), "Gender" },
                 { nameof(PersonRespone.CountryID), "Country" },
                 { nameof(PersonRespone.Address), "Address" }
            };

            List<PersonRespone> persons = _personsService.GetFilteredPerson(searchBy, searchString);

            ViewBag.CurrentSearchBy = searchBy;
            ViewBag.CurrentSearchString = searchString;


            List<PersonRespone> sortedPersons = _personsService.GetSortedPersons(persons, sortBy, sortOption);

            ViewBag.CurrentSortBy = sortBy;

            ViewBag.CurrentSortOrder = sortOption;


            return View(sortedPersons);
        }



        [Route("[action]")]
        [HttpGet]
        public IActionResult Create()
        {

            List<CountryResponse> countries = _countriesService.GetAllCountries();
            ViewBag.Countries = countries;

            return View();
        }


        [Route("[action]")]
        [HttpPost]
        public IActionResult Create(PersonAddRequest personAddRequest)
        {

            if (!ModelState.IsValid)
            {
                List<CountryResponse> countries = _countriesService.GetAllCountries();
                ViewBag.Countries = countries;

                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                return View();
            }

            _personsService.AddPerson(personAddRequest);

            return RedirectToAction("Index", "Persons");
        }
    }
}
