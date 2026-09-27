using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
using Xunit.Abstractions;

namespace CRUDTests
{
    public class PersonsServiceTest
    {

        private readonly IPersonsService _personsService;
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly ICountriesService _countriesService;

        public PersonsServiceTest(ITestOutputHelper testOutputHelper)
        {
            _personsService = new PersonsService();
            _testOutputHelper = testOutputHelper;
            _countriesService = new CountriesService();
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

            _testOutputHelper.WriteLine(personResponse?.ToString());


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


        #endregion


        #region GetAllPerson

        [Fact]
        public void GetAllPersons_Empty()
        {

            List<PersonAddRequest> fake_request = new List<PersonAddRequest>();

            Assert.Empty(fake_request);

        }



        [Fact]
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


        #region GetFilteredPersons
        [Fact]
        public void GetFilteredPerson_SearchStringNull()
        {
            string? searchBy = null;
            string? searchString = null;

            List<PersonAddRequest> fake_person_requests = new List<PersonAddRequest>() {
                new PersonAddRequest() { Address = "İstanbul", DateOfBirth = Convert.ToDateTime("23.09.1996"), Email = "sezermercan12@hotmail.com", Gender = GenderOptions.Male, PersonName = "Sezer" },
                  new PersonAddRequest() { Address = "Ankara", DateOfBirth = Convert.ToDateTime("03.04.1993"), Email = "alimercan12@hotmail.com", Gender = GenderOptions.Male, PersonName = "Ali" },
                    new PersonAddRequest() { Address = "Bingöl", DateOfBirth = Convert.ToDateTime("12.03.1999"), Email = "mehmetmercan12@hotmail.com", Gender = GenderOptions.Male, PersonName = "Mehmet" }
            };

            List<PersonRespone> fake_person_responses = new List<PersonRespone>();

            foreach (PersonAddRequest fake_request in fake_person_requests)
            {
                fake_person_responses.Add(_personsService.AddPerson(fake_request)!);
            }


            List<PersonRespone> actual_responses = _personsService.GetFilteredPerson(searchBy, searchString);

            Assert.Equal(fake_person_responses, actual_responses);
        }

        [Fact]
        public void GetFilteredPerson_Properly()
        {
            string? searchBy = "PersonName";
            string? searchString = "Sezer";

            List<PersonAddRequest> fake_person_requests = new List<PersonAddRequest>() {
                new PersonAddRequest() { Address = "İstanbul", DateOfBirth = Convert.ToDateTime("23.09.1996"), Email = "sezermercan12@hotmail.com", Gender = GenderOptions.Male, PersonName = "Sezer" },
                  new PersonAddRequest() { Address = "Ankara", DateOfBirth = Convert.ToDateTime("03.04.1993"), Email = "alimercan12@hotmail.com", Gender = GenderOptions.Male, PersonName = "Ali" },
                    new PersonAddRequest() { Address = "Bingöl", DateOfBirth = Convert.ToDateTime("12.03.1999"), Email = "mehmetmercan12@hotmail.com", Gender = GenderOptions.Male, PersonName = "Mehmet" }
            };

            List<PersonRespone> fake_person_responses = new List<PersonRespone>();

            foreach (PersonAddRequest fake_request in fake_person_requests)
            {
                fake_person_responses.Add(_personsService.AddPerson(fake_request)!);
            }

            List<PersonRespone> expected_values = fake_person_responses.Where(p => p.PersonName == searchString).ToList();

            List<PersonRespone> actualResponses = _personsService.GetFilteredPerson(searchBy, searchString);

            // Önce listelerin eleman sayısı aynı mı diye bakabilirsin
            Assert.Equal(expected_values.Count, actualResponses.Count);


            foreach (var expectedPerson in expected_values)
            {
                Assert.Contains(actualResponses, actual =>
                    actual.PersonID == expectedPerson.PersonID &&
                    actual.PersonName == expectedPerson.PersonName &&
                    actual.Email == expectedPerson.Email);
            }
        }

        #endregion

        #region GetSortedPersons
        [Fact]
        public void GetSortedPersons()
        {

            //Arrange
            PersonAddRequest person1 = new PersonAddRequest() { Address = "İstanbul", Email = "sezermercan12@hotmail.com", PersonName = "Sezer", Gender = GenderOptions.Male, DateOfBirth = DateTime.Parse("23.09.1996") };

            PersonAddRequest person2 = new PersonAddRequest() { Address = "Ankara", Email = "ahmetmercan12@hotmail.com", PersonName = "Ahmet", Gender = GenderOptions.Male, DateOfBirth = DateTime.Parse("23.09.1990") };

            PersonAddRequest person3 = new PersonAddRequest() { Address = "Bingöl", Email = "yusufmercan12@hotmail.com", PersonName = "Yusug", Gender = GenderOptions.Male, DateOfBirth = DateTime.Parse("23.09.1992") };

            PersonAddRequest person4 = new PersonAddRequest() { Address = "Muş", Email = "kerim12@hotmail.com", PersonName = "Kerim", Gender = GenderOptions.Male, DateOfBirth = DateTime.Parse("23.09.2000") };


            List<PersonAddRequest> fake_requests = new List<PersonAddRequest> { person1, person2, person3, person4 };

            List<PersonRespone> fake_response = new List<PersonRespone>();

            foreach (PersonAddRequest fake_request in fake_requests)
            {
                fake_response.Add(_personsService.AddPerson(fake_request)!);
            }


            string sortBy = "PersonName";

            SortOrderOptions sortOption = SortOrderOptions.ASC;


            List<PersonRespone> expected_values = fake_response.OrderBy(x => x.PersonName).ToList();


            List<PersonRespone> actual_values = _personsService.GetSortedPersons(fake_response, sortBy, sortOption);

            Assert.Equal(expected_values, actual_values);


        }


        #endregion

        #region GetUpdatedPerson

        [Fact]
        public void GetUpdatedPerson_Null()
        {

            PersonUpdateRequest? personUpdateRequest = null;

            Assert.Throws<ArgumentNullException>(() =>
            {
                _personsService.UpdatePerson(personUpdateRequest);
            });

        }

        [Fact]
        public void GetUpdatedPerson_PersonNull()
        {

            PersonUpdateRequest fake_request_update = new PersonUpdateRequest() { PersonID = Guid.NewGuid(), Address = "Berlin", DateOfBirth = DateTime.Parse("23.09.1996"), PersonName = "Sezer", Email = "sezermercan12@hotmail.com", Gender = GenderOptions.Male };


            Assert.Throws<ArgumentException>(() =>
            {
                _personsService.UpdatePerson(fake_request_update);
            });

        }

        [Fact]
        public void GetUpdatedPerson_Properly()
        {
            PersonAddRequest person1 = new PersonAddRequest() { Address = "İstanbul", Email = "sezermercan12@hotmail.com", PersonName = "Sezer", Gender = GenderOptions.Male, DateOfBirth = DateTime.Parse("23.09.1996") };

            PersonAddRequest person2 = new PersonAddRequest() { Address = "Ankara", Email = "ahmetmercan12@hotmail.com", PersonName = "Ahmet", Gender = GenderOptions.Male, DateOfBirth = DateTime.Parse("23.09.1990") };

            PersonAddRequest person3 = new PersonAddRequest() { Address = "Bingöl", Email = "yusufmercan12@hotmail.com", PersonName = "Yusug", Gender = GenderOptions.Male, DateOfBirth = DateTime.Parse("23.09.1992") };

            PersonAddRequest person4 = new PersonAddRequest() { Address = "Muş", Email = "kerim12@hotmail.com", PersonName = "Kerim", Gender = GenderOptions.Male, DateOfBirth = DateTime.Parse("23.09.2000") };


            List<PersonAddRequest> fake_requests = new List<PersonAddRequest> { person1, person2, person3, person4 };

            List<PersonRespone> fake_responses = new List<PersonRespone>();

            foreach (PersonAddRequest fake_request in fake_requests)
            {
                fake_responses.Add(_personsService.AddPerson(fake_request)!);
            }


            PersonUpdateRequest fake_request_update = new PersonUpdateRequest() { PersonID = fake_responses[0].PersonID, Address = "Berlin", DateOfBirth = DateTime.Parse("23.09.1996"), PersonName = "Sezer", Email = "sezermercan12@hotmail.com", Gender = GenderOptions.Male };


            PersonRespone expected_value = fake_request_update.ToPerson().ToPersonResponse();

            PersonRespone? actual_value = _personsService.UpdatePerson(fake_request_update);

            Assert.Equal(expected_value, actual_value);



        }
        #endregion
    }


}
