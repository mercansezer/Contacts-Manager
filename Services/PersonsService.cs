using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
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

        public List<PersonRespone> GetSortedPersons(List<PersonRespone> allPersons, string sortBy, SortOrderOptions sortOption)
        {
            if (string.IsNullOrEmpty(sortBy))
                return allPersons;

            List<PersonRespone> sortedPersons = (sortBy, sortOption) switch
            {
                (nameof(PersonRespone.PersonName), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonRespone.PersonName), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonRespone.Email), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonRespone.Email), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonRespone.DateOfBirth), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.DateOfBirth).ToList(),

                (nameof(PersonRespone.DateOfBirth), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.DateOfBirth).ToList(),

                (nameof(PersonRespone.Age), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Age).ToList(),

                (nameof(PersonRespone.Age), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Age).ToList(),

                (nameof(PersonRespone.Gender), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonRespone.Gender), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonRespone.Country), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonRespone.Country), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonRespone.Address), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonRespone.Address), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonRespone.ReceiveNewsLetters), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.ReceiveNewsLetters).ToList(),

                (nameof(PersonRespone.ReceiveNewsLetters), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.ReceiveNewsLetters).ToList(),

                _ => allPersons
            };

            return sortedPersons;
        }

        public PersonRespone? UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {

            if (personUpdateRequest is null) throw new ArgumentNullException(nameof(personUpdateRequest));


            ValidationHelpers.ModelValidation(personUpdateRequest);

            Person? person = _persons.FirstOrDefault(person => person.PersonID == personUpdateRequest.PersonID);

            if (person == null) throw new ArgumentException(nameof(personUpdateRequest));

            person.PersonName = personUpdateRequest.PersonName;
            person.Email = personUpdateRequest.Email;
            person.DateOfBirth = personUpdateRequest.DateOfBirth;
            person.Gender = personUpdateRequest.Gender?.ToString();
            person.Address = personUpdateRequest.Address;
            person.CountryID = personUpdateRequest.CountryID;
            person.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;



            return person.ToPersonResponse();
        }

        public bool DeletePerson(Guid? personId)
        {
            if (personId is null) throw new ArgumentNullException(nameof(personId));

            Person? findedPerson = _persons.FirstOrDefault(person => person.PersonID == personId);

            if (findedPerson == null) return false;

            bool result = _persons.Remove(findedPerson);

            return result;
        }
    }
}
