using CRUD.Data;
using CRUD.ServiceContracts;
using CRUD.ServiceContracts.DTO;
using CRUD.ServiceContracts.DTO.CountryDTO;

namespace CRUD.Services;

public class CountriesService : ICountriesService
{
    private readonly List<Country> _countries;

    public CountriesService()
    {
        _countries = new List<Country>();
    }
    public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
    {
        // if (countryAddRequest == null)
        // {
        //     throw new ArgumentNullException(nameof(countryAddRequest));
        // }
        //
        // return new CountryResponse { CountryName = countryAddRequest.CountryName };

        if (countryAddRequest == null)
        {
            throw new ArgumentNullException(nameof(countryAddRequest));
        }

        if (countryAddRequest.CountryName == null)
        {
            throw new ArgumentException(nameof(countryAddRequest.CountryName));
        }

        if (_countries.Where(country => country.CountryName == countryAddRequest.CountryName).Any())
        {
            throw new ArgumentException("Country Name Already Exist");
        }
        Country country = countryAddRequest.ToCountry();
        _countries.Add(country);
        return country.ToCountryResponse();
    }

    public List<CountryResponse> GetAllCountries()
    {
        return _countries.Select(country => country.ToCountryResponse()).ToList();
    }

    public CountryResponse? GetCountryById(Guid? countryId)
    {
        Country  response= _countries.FirstOrDefault(c=>c.CountryId ==countryId);
        if (countryId ==null || response==null)
        {
            return null;
        }

        return response.ToCountryResponse();
    }
}