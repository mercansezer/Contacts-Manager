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

        #region GetPersonByPersonID
        [Fact]
        public void GetPersonByPersonID_IDNull()
        {
            Guid? personID = null;
            Assert.Null(_personsService.GetPersonById(personID));
        }

        public void GetPersonByPersonID_Properly()
        {
            PersonAddRequest fake_request = new PersonAddRequest { PersonName = "Sezer Mercan", Email = "asdasda@hotmail.com", Address = "istanbul" };

            PersonRespone fake_response = _personsService.AddPerson(fake_request)!;


            PersonRespone expected_person = _personsService.GetPersonById(fake_response.PersonID);


            Assert.Equal(fake_response, expected_person);

        }

        [Fact]

        #endregion


        #region GetAllPerson
        public void GetAllPersons_Empty()
        {

            List<PersonAddRequest> fake_request = new List<PersonAddRequest>();

            Assert.Empty(fake_request);

        }

        public void GetAllPersons_Properly()
        {

            List<PersonAddRequest> fake_request = new List<PersonAddRequest>() {
                new PersonAddRequest() { PersonName="Sezer", Address="İstanbul",DateOfBirth=Convert.ToDateTime("23-09-1996"),Email="asdasd@gmail.com"
                },
                 new PersonAddRequest() { PersonName="Ahmet", Address="Ankara",DateOfBirth=Convert.ToDateTime("23-09-1990"),Email="asdaasdasdasdaasd@gmail.com"
                }
            };

            List<PersonRespone> fake_responses = new List<PersonRespone>();

            foreach (PersonAddRequest request in fake_request)
            {
                fake_responses.Add(_personsService.AddPerson(request)!);
            }

            List<PersonRespone> expected = _personsService.GetAllPersons();


            foreach (PersonRespone response in fake_responses)
            {
                Assert.Contains(response, expected);
            }


        }
        #endregion 

    }


}
