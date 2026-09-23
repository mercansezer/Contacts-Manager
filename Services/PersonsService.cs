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

        public PersonRespone? GetPersonById(Guid? personId)
        {

            if (personId == null) return null;


            PersonRespone? response = _persons.FirstOrDefault(p => p.PersonID == personId)?.ToPersonResponse();

            if (response == null) return null;

            return response;
        }

        public List<PersonRespone> GetFilteredPerson(string? searchBy, string? searchString)
        {

            if (searchBy == null || searchString == null) return _persons.Select(p => p.ToPersonResponse()).ToList();

            var filteredQuery = searchBy switch
            {
                nameof(PersonRespone.PersonName) => _persons.Where(temp => temp.PersonName != null && temp.PersonName.Contains(searchString)),

                nameof(PersonRespone.Email) => _persons.Where(temp => temp.Email != null && temp.Email.Contains(searchString)),

                nameof(PersonRespone.Address) => _persons.Where(temp => temp.Address != null && temp.Address.Contains(searchString)),

                _ => _persons
            };

            return filteredQuery.Select(temp => temp.ToPersonResponse()).ToList();
        }
    }
}
