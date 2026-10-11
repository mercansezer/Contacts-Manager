using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

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
        public IActionResult Index(string searchBy, string? searchString)
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

            return View(persons);
        }
    }
}
