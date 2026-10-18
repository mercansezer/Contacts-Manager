using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.Enums;

namespace ContactsManager.Controllers
{
    public class PersonsController : Controller

    {
        private readonly IPersonsService _personsService;
        public PersonsController(IPersonsService personsService)
        {

            _personsService = personsService;
        }

        [Route("persons/index")]
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
    }
}
