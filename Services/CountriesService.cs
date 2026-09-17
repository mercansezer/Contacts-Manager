using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private List<Country> _countries;

        public CountriesService()
        {
            _countries = new List<Country>();
        }

        public CountryResponse? AddCountry(CountryAddRequest countryAddRequest)
        {
            //Throw error if the countryAddRequest is null
            if (countryAddRequest == null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest));
            }

            //Throw error if the countryAddRequest.CountryName is null
            if (countryAddRequest.CountryName == null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest));
            }

            //Throw ArgumentException if the CountryName is duplicate
            if (_countries.Where(country => country.CountryName == countryAddRequest.CountryName).Any())
            {
                throw new ArgumentException(nameof(countryAddRequest));
            }

            //If everything is okay then insert the country to our list.
            Country country = countryAddRequest.ToCountry();

            country.CountryId = Guid.NewGuid();

            _countries.Add(country);

            return country.ToCountryResponse();

        }

        public List<CountryResponse> GetAllCountries()
        {

            return _countries.Select(country => country.ToCountryResponse()).ToList();

        }



        public CountryResponse? GetCountryById(Guid? countryId)
        {
            if (countryId == null)
            {
                return null;
            }

            Country? country = _countries.FirstOrDefault(country => country.CountryId == countryId);

            if (country == null)
            {
                return null;
            }

            CountryResponse countryResponse = country.ToCountryResponse();

            return countryResponse;
        }



    }
}
