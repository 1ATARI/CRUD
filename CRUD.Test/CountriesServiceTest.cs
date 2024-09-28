using CRUD.ServiceContracts;
using CRUD.ServiceContracts.DTO;
using CRUD.ServiceContracts.DTO.CountryDTO;
using CRUD.Services;

namespace CRUD.Test;

public class CountriesServiceTest
{
    private readonly ICountriesService _countriesService;

    public CountriesServiceTest()
    {
        _countriesService = new CountriesService(false);
    }

    #region AddCountry

    [Fact]
    public void AddCountry_NullCountry()
    {
        CountryAddRequest? request = null;

        Assert.Throws<ArgumentNullException>(() =>
            _countriesService.AddCountry(request)
        );
    }

    [Fact]
    public void AddCountry_CountryNameIsNull()
    {
        CountryAddRequest? request = new CountryAddRequest() { CountryName = null };

        Assert.Throws<ArgumentException>(() =>
            _countriesService.AddCountry(request)
        );
    }

    [Fact]
    public void AddCountry_DuplicateCountryName()
    {
        CountryAddRequest? request1 = new CountryAddRequest() { CountryName = "USA" };
        CountryAddRequest? request2 = new CountryAddRequest() { CountryName = "USA" };

        Assert.Throws<ArgumentException>(() =>
            {
                _countriesService.AddCountry(request1);
                _countriesService.AddCountry(request2);
            }
        );
    }

    [Fact]
    public void AddCountry_ProperCountryDetails()
    {
        CountryAddRequest? request = new CountryAddRequest() { CountryName = "USA" };


        CountryResponse response = _countriesService.AddCountry(request);
        List<CountryResponse> allCountries = _countriesService.GetAllCountries();
        Assert.True(response.CountryId != Guid.Empty);
        Assert.Contains(response, allCountries);
    }

    #endregion

    #region GetAllCountries

    [Fact]
    public void GetAllCountries_EmptyList()
    {
        List<CountryResponse> actualCountryResponses  =_countriesService.GetAllCountries();
        
        Assert.Empty(actualCountryResponses);
    }
    [Fact]
    public void GetAllCountries_AddFewCountries()
    {
        List<CountryAddRequest> countryRequestList  = new List<CountryAddRequest>()
        {
            new CountryAddRequest()
            {
                CountryName = "Egypt" ,
            },
            new CountryAddRequest()
            {
                CountryName = "USA" ,
            }
        };
        List<CountryResponse> countriesListAdded = new List<CountryResponse>();
        foreach (var country in countryRequestList)
        {
            countriesListAdded.Add(_countriesService.AddCountry(country));
        }

        List<CountryResponse> actualCountryResponses =_countriesService.GetAllCountries();
        foreach (var expected in countriesListAdded)
        {
            Assert.Contains(expected, actualCountryResponses);
        }
        
    }
    #endregion

    #region GetCountryById

    [Fact]
    public void GetCountryById_NullCountryId()
    {
        Guid? countryId = null;
        CountryResponse? response = _countriesService.GetCountryById(countryId);
        
        Assert.Null(response);
    }
    [Fact]
    public void GetCountryById_ValidCountryId()
    {
        CountryAddRequest? addRequest = new CountryAddRequest()
        {
            CountryName = "Egypt"
        };

        CountryResponse? addResponse   =_countriesService.AddCountry(addRequest);
        CountryResponse? getResponse =  _countriesService.GetCountryById(addResponse.CountryId);
        
        Assert.Equal(addResponse , getResponse);

    }
    

    #endregion
}