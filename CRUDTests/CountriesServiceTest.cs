using ServiceContracts;
using ServiceContracts.DTO;
using Services;

namespace CRUDTests
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;
        public CountriesServiceTest()
        {
            _countriesService = new CountriesService();
        }

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
                _countriesService.AddCountry(countryAddRequest);
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
            _countriesService.AddCountry(request1);
            //Assert
            Assert.Throws<ArgumentException>(() =>
            {

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


        }

    }
}
