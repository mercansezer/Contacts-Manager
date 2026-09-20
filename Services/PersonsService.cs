using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helper;

namespace Services
{
    public class PersonsService : IPersonsService
    {

        private List<Person> _persons;

        private readonly ICountriesService _countriesService;
        public PersonsService()
        {
            _persons = new List<Person>();
            _countriesService = new CountriesService();
        }

        private PersonRespone ToPersonResponse(Person person)
        {


            PersonRespone personRespone = person.ToPersonResponse();
            personRespone.Country = _countriesService.GetCountryById(person.CountryID)?.CountryName;
            return personRespone;
        }
        public PersonRespone? AddPerson(PersonAddRequest? personAddRequest)
        {

            if (personAddRequest == null)
            {
                throw new ArgumentNullException(nameof(personAddRequest));
            }


            ValidationHelpers.ModelValidation(personAddRequest);

            Person person = personAddRequest.ToPerson();

            person.PersonID = Guid.NewGuid();

            if (person == null)
            {

                return null;
            }

            _persons.Add(person);



            return ToPersonResponse(person);
        }

        public List<PersonRespone> GetAllPersons()
        {
            return _persons.Select(person => person.ToPersonResponse()).ToList();
        }
    }
}
