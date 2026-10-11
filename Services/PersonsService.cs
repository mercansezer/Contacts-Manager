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
        public PersonsService(bool initialize = true)
        {
            _persons = new List<Person>();
            _countriesService = new CountriesService();

            if (initialize)
            {
                _persons.Add(new Person() { PersonID = Guid.Parse("8082ED0C-396D-4162-AD1D-29A13F929824"), PersonName = "Aguste", Email = "aleddy0@booking.com", DateOfBirth = DateTime.Parse("1993-01-02"), Gender = "Male", Address = "0858 Novick Terrace", ReceiveNewsLetters = false, CountryID = Guid.Parse("000C76EB-62E9-4465-96D1-2C41FDB64C3B") });

                _persons.Add(new Person() { PersonID = Guid.Parse("06D15BAD-52F4-498E-B478-ACAD847ABFAA"), PersonName = "Jasmina", Email = "jsyddie1@miibeian.gov.cn", DateOfBirth = DateTime.Parse("1991-06-24"), Gender = "Female", Address = "0742 Fieldstone Lane", ReceiveNewsLetters = true, CountryID = Guid.Parse("32DA506B-3EBA-48A4-BD86-5F93A2E19E3F") });

                _persons.Add(new Person() { PersonID = Guid.Parse("D3EA677A-0F5B-41EA-8FEF-EA2FC41900FD"), PersonName = "Kendall", Email = "khaquard2@arstechnica.com", DateOfBirth = DateTime.Parse("1993-08-13"), Gender = "Male", Address = "7050 Pawling Alley", ReceiveNewsLetters = false, CountryID = Guid.Parse("32DA506B-3EBA-48A4-BD86-5F93A2E19E3F") });

                _persons.Add(new Person() { PersonID = Guid.Parse("89452EDB-BF8C-4283-9BA4-8259FD4A7A76"), PersonName = "Kilian", Email = "kaizikowitz3@joomla.org", DateOfBirth = DateTime.Parse("1991-06-17"), Gender = "Male", Address = "233 Buhler Junction", ReceiveNewsLetters = true, CountryID = Guid.Parse("DF7C89CE-3341-4246-84AE-E01AB7BA476E") });

                _persons.Add(new Person() { PersonID = Guid.Parse("F5BD5979-1DC1-432C-B1F1-DB5BCCB0E56D"), PersonName = "Dulcinea", Email = "dbus4@pbs.org", DateOfBirth = DateTime.Parse("1996-09-02"), Gender = "Female", Address = "56 Sundown Point", ReceiveNewsLetters = false, CountryID = Guid.Parse("DF7C89CE-3341-4246-84AE-E01AB7BA476E") });

                _persons.Add(new Person() { PersonID = Guid.Parse("A795E22D-FAED-42F0-B134-F3B89B8683E5"), PersonName = "Corabelle", Email = "cadams5@t-online.de", DateOfBirth = DateTime.Parse("1993-10-23"), Gender = "Female", Address = "4489 Hazelcrest Place", ReceiveNewsLetters = false, CountryID = Guid.Parse("15889048-AF93-412C-B8F3-22103E943A6D") });

                _persons.Add(new Person() { PersonID = Guid.Parse("3C12D8E8-3C1C-4F57-B6A4-C8CAAC893D7A"), PersonName = "Faydra", Email = "fbischof6@boston.com", DateOfBirth = DateTime.Parse("1996-02-14"), Gender = "Female", Address = "2010 Farragut Pass", ReceiveNewsLetters = true, CountryID = Guid.Parse("80DF255C-EFE7-49E5-A7F9-C35D7C701CAB") });

                _persons.Add(new Person() { PersonID = Guid.Parse("7B75097B-BFF2-459F-8EA8-63742BBD7AFB"), PersonName = "Oby", Email = "oclutheram7@foxnews.com", DateOfBirth = DateTime.Parse("1992-05-31"), Gender = "Male", Address = "2 Fallview Plaza", ReceiveNewsLetters = false, CountryID = Guid.Parse("80DF255C-EFE7-49E5-A7F9-C35D7C701CAB") });

                _persons.Add(new Person() { PersonID = Guid.Parse("6717C42D-16EC-4F15-80D8-4C7413E250CB"), PersonName = "Seumas", Email = "ssimonitto8@biglobe.ne.jp", DateOfBirth = DateTime.Parse("1999-02-02"), Gender = "Male", Address = "76779 Norway Maple Crossing", ReceiveNewsLetters = false, CountryID = Guid.Parse("80DF255C-EFE7-49E5-A7F9-C35D7C701CAB") });

                _persons.Add(new Person() { PersonID = Guid.Parse("6E789C86-C8A6-4F18-821C-2ABDB2E95982"), PersonName = "Freemon", Email = "faugustin9@vimeo.com", DateOfBirth = DateTime.Parse("1996-04-27"), Gender = "Male", Address = "8754 Becker Street", ReceiveNewsLetters = false, CountryID = Guid.Parse("80DF255C-EFE7-49E5-A7F9-C35D7C701CAB") });

            }
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
                nameof(PersonRespone.PersonName) => _persons.Where(temp => temp.PersonName != null && temp.PersonName.Equals(searchString, StringComparison.OrdinalIgnoreCase)),

                nameof(PersonRespone.Email) => _persons.Where(temp => temp.Email != null && temp.Email.Equals(searchString, StringComparison.OrdinalIgnoreCase)),

                nameof(PersonRespone.Address) => _persons.Where(temp => temp.Address != null && temp.Address.Equals(searchString, StringComparison.OrdinalIgnoreCase)),

                nameof(PersonRespone.Gender) => _persons.Where(temp => temp.Gender != null && temp.Gender.Equals(searchString, StringComparison.OrdinalIgnoreCase)),

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
