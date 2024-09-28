using CRUD.Data;
using CRUD.ServiceContracts;
using CRUD.ServiceContracts.DTO;
using CRUD.ServiceContracts.DTO.CountryDTO;

namespace CRUD.Services;

public class CountriesService : ICountriesService
{
    private readonly List<Country> _countries;

    public CountriesService(bool Initialize = true)
    {
        _countries = new List<Country>();
        if (Initialize )
        {
            _countries.AddRange(new List<Country>
            {
                new Country { CountryId = Guid.Parse("0836DDD2-E0D5-4F9D-ACDB-C9D066EC4DFD"), CountryName = "Egypt" },
                new Country { CountryId = Guid.Parse("1AE34F09-ACFE-4A4A-862B-047A29FA5AA5"), CountryName = "USA" },
                new Country { CountryId = Guid.Parse("AC7E276F-6E30-4A91-B47F-0CE6BB1157A2"), CountryName = "UAE" },
                new Country { CountryId = Guid.Parse("B69BFA71-B264-48B3-8511-A6C9C60C00AB"), CountryName = "KSA" },
                new Country { CountryId = Guid.Parse("81BA47A0-54AC-441B-A863-D9444DFBD44A"), CountryName = "Germany" },
            });
        }
    }

    public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
    {
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
        Country response = _countries.FirstOrDefault(c => c.CountryId == countryId);
        if (countryId == null || response == null)
        {
            return null;
        }

        return response.ToCountryResponse();
    }
}