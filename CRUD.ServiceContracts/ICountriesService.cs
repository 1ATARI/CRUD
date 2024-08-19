using CRUD.ServiceContracts.DTO;
using CRUD.ServiceContracts.DTO.CountryDTO;

namespace CRUD.ServiceContracts;

public interface ICountriesService
{
    
    CountryResponse AddCountry(CountryAddRequest? countryAddRequest);
    List<CountryResponse> GetAllCountries();

    CountryResponse? GetCountryById(Guid? countryId);

}