using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class PersonsService : IPersonsService
    {

        private List<Person> _persons;
        public PersonsService()
        {
            _persons = new List<Person>();
        }
        public PersonRespone? AddPerson(PersonAddRequest? personAddRequest)
        {

            if (personAddRequest == null)
            {
                throw new ArgumentNullException(nameof(personAddRequest));
            }

            if (personAddRequest.PersonName == null)
            {
                throw new ArgumentException(nameof(personAddRequest));
            }

            Person person = personAddRequest.ToPerson();

            person.PersonID = Guid.NewGuid();

            if (person == null)
            {

                return null;
            }

            _persons.Add(person);

            return person.ToPersonResponse();
        }

        public List<PersonRespone> GetAllPersons()
        {
            return _persons.Select(person => person.ToPersonResponse()).ToList();
        }
    }
}
