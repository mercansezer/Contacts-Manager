using ServiceContracts;
using ServiceContracts.DTO;
using Services;

namespace CRUDTests
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;

        //Constructor
        public CountriesServiceTest()
        {
            _countriesService = new CountriesService();
        }



        #region AddCountry
        //Test when the CountryAddRequest is null
        [Fact]
        public void AddCountry_Null()
        {
            //Arrange
            CountryAddRequest? countryAddRequest = null;

            //Assert

            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                _countriesService.AddCountry(countryAddRequest!);
            });
        }

        //Test when the CountryAddRequest.CountryName is null
        [Fact]
        public void AddCountry_CountryNameNull()
        {
            //Arrange
            CountryAddRequest? countryAddRequest = new CountryAddRequest { CountryName = null };

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {

                //Act
                _countriesService.AddCountry(countryAddRequest);

            });
        }

        //Test when the CountryAddRequest.CountryName is duplicate

        [Fact]
        public void AddCountry_CountryNameDuplicate()
        {
            //Arrange

            CountryAddRequest? request1 = new CountryAddRequest() { CountryName = "USA" };
            CountryAddRequest? request2 = new CountryAddRequest() { CountryName = "USA" };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                _countriesService.AddCountry(request1);
                _countriesService.AddCountry(request2);
            });
        }

        //Test when CountryAddRequest is proper

        [Fact]
        public void AddCountry_Properly()
        {
            //Arrange
            CountryAddRequest? request = new CountryAddRequest() { CountryName = "Türkiye" };

            //Act
            CountryResponse? response = _countriesService.AddCountry(request);

            // Assert
            // 1. Dönen yanıt null olmamalı
            Assert.NotNull(response);

            // 2. ID gerçeğe uygun üretilmeli (Guid.Empty olmamalı)
            Assert.NotEqual(Guid.Empty, response?.CountryId);

            // 3. Gönderdiğimiz veriyle kaydedilen veri uyuşmalı!
            Assert.Equal(request.CountryName, response?.CountryName);

            List<CountryResponse> countryListFromService = _countriesService.GetAllCountries();

            Assert.Contains(response, countryListFromService);


        }
        #endregion


        #region GetCountries
        //Test when the countrylist is empty
        [Fact]
        public void GetAllCountries_EmptyList()
        {

            //Act
            List<CountryResponse> response = _countriesService.GetAllCountries();

            //Assert
            Assert.Empty(response);

        }


        //Test when everything is okay
        [Fact]
        public void GetAllCountries_AddFewCountries()
        {

            //Create some dummy ruquest
            List<CountryAddRequest> dummy_request = new List<CountryAddRequest>() {
                new CountryAddRequest(){CountryName="Türkiye"},
                new CountryAddRequest(){CountryName="Almanya"},
                new CountryAddRequest(){CountryName="Fransa"},
                new CountryAddRequest(){CountryName="Rusya"},

            };

            //Create one fake response
            List<CountryResponse> fake_response = new List<CountryResponse>();

            //Add all the dummy request to the response

            foreach (CountryAddRequest countryAddRequest in dummy_request)
            {
                fake_response.Add(_countriesService.AddCountry(countryAddRequest)!);
            }

            //Get response from service
            List<CountryResponse> response_from_service = _countriesService.GetAllCountries();


            //Check if both of them are equal or not.

            foreach (CountryResponse expectedResponse in fake_response)
            {
                Assert.Contains(expectedResponse, response_from_service);
            }

        }

        #endregion

        #region GetCountryById
        [Fact]
        public void GetCountryById_NullID()
        {
            //Arrange
            Guid? countryId = null;

            //Assert

            //Act
            Assert.Null(_countriesService.GetCountryById(countryId!));


        }

        [Fact]
        public void GetCountryById_NotFoundCountry()
        {
            List<CountryAddRequest> dummy_countries_list_request = new List<CountryAddRequest>() {
                new CountryAddRequest(){CountryName="Türkiye"},
                new CountryAddRequest(){CountryName="Fransa"},
                new CountryAddRequest(){CountryName="Amerika"}

            };

            foreach (CountryAddRequest countryAddRequest in dummy_countries_list_request)
            {
                _countriesService.AddCountry(countryAddRequest);
            }

            Assert.Null(_countriesService.GetCountryById(Guid.NewGuid()));

        }

        [Fact]
        public void GetCountryById_Properly()
        {
            List<CountryAddRequest> dummy_countries_list_request = new List<CountryAddRequest>() {
                new CountryAddRequest(){CountryName="Türkiye"},
                new CountryAddRequest(){CountryName="Fransa"},
                new CountryAddRequest(){CountryName="Amerika"}

            };


            List<CountryResponse> fake_country_response = new List<CountryResponse>();


            foreach (CountryAddRequest countryAddRequest in dummy_countries_list_request)
            {
                fake_country_response.Add(_countriesService.AddCountry(countryAddRequest)!);
            }



            CountryResponse expectedCountryResponse = _countriesService.GetCountryById(fake_country_response[0].CountryId)!;

            Assert.Contains(expectedCountryResponse, fake_country_response);

        }
        #endregion

    }
}
