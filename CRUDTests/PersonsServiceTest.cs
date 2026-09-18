using ServiceContracts;
using ServiceContracts.DTO;
using Services;

namespace CRUDTests
{
    public class PersonsServiceTest
    {

        private readonly IPersonsService _personsService;

        public PersonsServiceTest()
        {
            _personsService = new PersonsService();
        }


        #region AddPerson
        [Fact]
        public void AddPerson_Null()
        {
            PersonAddRequest? request = null;

            Assert.Throws<ArgumentNullException>(() =>
            {
                _personsService.AddPerson(request!);
            });

        }

        [Fact]
        public void AddPerson_PersonNameEmpty()
        {
            PersonAddRequest? request = new PersonAddRequest { PersonName = null, Address = "Ankara", Email = "asda@gmail.com" };

            Assert.Throws<ArgumentException>(() =>
            {
                _personsService.AddPerson(request!);
            });

        }


        [Fact]
        public void AddPerson_Properly()
        {
            PersonAddRequest? request = new PersonAddRequest { PersonName = "Sezer", Address = "Ankara", Email = "asda@gmail.com" };

            PersonRespone? personResponse = _personsService.AddPerson(request!);


            Assert.NotNull(personResponse);

            Assert.NotEqual(Guid.Empty, personResponse?.PersonID);

            Assert.Equal(request.PersonName, personResponse?.PersonName);

            List<PersonRespone> responseList = _personsService.GetAllPersons();


            Assert.Contains(personResponse, responseList);


        }

        #endregion

    }
}
