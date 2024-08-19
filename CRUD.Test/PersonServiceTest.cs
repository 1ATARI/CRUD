using CRUD.Data;
using CRUD.ServiceContracts;
using CRUD.ServiceContracts.DTO.CountryDTO;
using CRUD.ServiceContracts.DTO.Enums;
using CRUD.ServiceContracts.DTO.PersonDTO;
using CRUD.Services;
using Xunit.Abstractions;

namespace CRUD.Test;

public class PersonServiceTest
{
    private readonly IPersonsService _personsService;
    private readonly ICountriesService _countriesService;
    private readonly ITestOutputHelper _testOutputHelper;

    public PersonServiceTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _personsService = new PersonsService();
        _countriesService = new CountriesService();
    }

    #region AddPerson

    [Fact]
    public void AddPerson_NullPerson()
    {
        PersonAddRequest? request = null;

        Assert.Throws<ArgumentNullException>(() =>
            _personsService.AddPerson(request)
        );
    }

    [Fact]
    public void AddPerson_NullPersonName()
    {
        PersonAddRequest? personAddRequest = new PersonAddRequest
        {
            Name = null,
            Email = null
        };

        Assert.Throws<ArgumentException>(() =>
            _personsService.AddPerson(personAddRequest)
        );
    }

    [Fact]
    public void AddPerson_ProperPerson()
    {
        CountryAddRequest countryAddRequest = new CountryAddRequest()
        {
            CountryName = "Egypt"
        };
        CountryResponse country = _countriesService.AddCountry(countryAddRequest);
        PersonAddRequest? personAddRequest = new PersonAddRequest
        {
            Name = "youssef",
            Email = "mohamed@gmail.com",
            Address = "<SCRIPT>alert(\"aaaA\")</SCRIPT>",
            CountryId = country.CountryId,
            Gender = GenderOptions.Male,
            DateOfBirth = DateTime.Parse("2000-01-01"),
            ReceiveNewsLetters = true,
        };

        PersonResponse? addPerson = _personsService.AddPerson(personAddRequest);

        List<PersonResponse> personsList = _personsService.GetAllPersons();

        Assert.True(addPerson != null && addPerson.PersonId != Guid.Empty);
        Assert.Contains(addPerson, personsList);
    }

    #endregion

    #region GetPersonById

    [Fact]
    public void GetPersonById_NullPersonById()
    {
        Guid guid = Guid.Empty;
        PersonResponse? personResponse = _personsService.GetPersonById(guid);
        Assert.Null(personResponse);
    }

    [Fact]
    public void GetPersonById_ProperPerson()
    {
        CountryAddRequest countryAddRequest = new CountryAddRequest()
        {
            CountryName = "Egypt"
        };
        CountryResponse country = _countriesService.AddCountry(countryAddRequest);
        PersonAddRequest? personAddRequest = new PersonAddRequest
        {
            Name = "youssef",
            Email = "mohamed@gmail.com",
            Address = "12st alexandria ....",
            CountryId = country.CountryId,
            Gender = GenderOptions.Male,
            DateOfBirth = DateTime.Parse("2000-01-01"),
            ReceiveNewsLetters = true,
        };

        PersonResponse? addPerson = _personsService.AddPerson(personAddRequest);

        PersonResponse? getPerson = _personsService.GetPersonById(addPerson?.PersonId);

        Assert.Equal(addPerson, getPerson);
    }

    #endregion

    #region GetAllPersons

    [Fact]
    public void GetAllPersons__EmptyList()
    {
        Assert.Empty(_personsService.GetAllPersons());
    }

    [Fact]
    public void GetAllPersons_AddPersons()
    {
        CountryAddRequest countryAddRequest = new CountryAddRequest()
        {
            CountryName = "Egypt"
        };
        CountryResponse country = _countriesService.AddCountry(countryAddRequest);
        PersonAddRequest? person1 = new PersonAddRequest
        {
            Name = "youssef",
            Email = "mohamed@gmail.com",
            Address = "12st alexandria ....",
            CountryId = country.CountryId,
            Gender = GenderOptions.Male,
            DateOfBirth = DateTime.Parse("2000-01-01"),
            ReceiveNewsLetters = true,
        };
        PersonAddRequest? person2 = new PersonAddRequest
        {
            Name = "ahmed",
            Email = "a@gmail.com",
            Address = "a12st alexandria ....",
            CountryId = country.CountryId,
            Gender = GenderOptions.Male,
            DateOfBirth = DateTime.Parse("2005-01-01"),
            ReceiveNewsLetters = false,
        };
        List<PersonResponse> personList = new List<PersonResponse>();
        List<PersonAddRequest> personsRequest = new List<PersonAddRequest>()
        {
            person1, person2
        };
        foreach (var person in personsRequest)
        {
            PersonResponse? response = _personsService.AddPerson(person);
            personList.Add(response);
        }

        _testOutputHelper.WriteLine("Expected");

        foreach (var person in personList)
        {
            _testOutputHelper.WriteLine(person.ToString());
        }

        List<PersonResponse> results = _personsService.GetAllPersons();

        _testOutputHelper.WriteLine("Actual");

        foreach (var result in results)
        {
            _testOutputHelper.WriteLine(results.ToString());
        }

        foreach (var person in personList)
        {
            Assert.Contains(person, results);
        }
    }

    #endregion

    #region GetFilteredPersons

    [Fact]
    public void GetFilteredPersons_EmptySearch()
    {
        CountryAddRequest countryAddRequest = new CountryAddRequest()
        {
            CountryName = "Egypt"
        };
        CountryResponse country = _countriesService.AddCountry(countryAddRequest);
        PersonAddRequest? person1 = new PersonAddRequest
        {
            Name = "youssef",
            Email = "mohamed@gmail.com",
            Address = "12st alexandria ....",
            CountryId = country.CountryId,
            Gender = GenderOptions.Male,
            DateOfBirth = DateTime.Parse("2000-01-01"),
            ReceiveNewsLetters = true,
        };
        PersonAddRequest? person2 = new PersonAddRequest
        {
            Name = "ahmed",
            Email = "a@gmail.com",
            Address = "a12st alexandria ....",
            CountryId = country.CountryId,
            Gender = GenderOptions.Male,
            DateOfBirth = DateTime.Parse("2005-01-01"),
            ReceiveNewsLetters = false,
        };
        List<PersonResponse> personList = new List<PersonResponse>();
        List<PersonAddRequest> personsRequest = new List<PersonAddRequest>()
        {
            person1, person2
        };
        foreach (var person in personsRequest)
        {
            PersonResponse? response = _personsService.AddPerson(person);
            personList.Add(response);
        }

        _testOutputHelper.WriteLine("Expected");

        foreach (var person in personList)
        {
            _testOutputHelper.WriteLine(person.ToString());
        }

        List<PersonResponse>? resultsFromSearch = _personsService.GetFilteredPersons(nameof(Person.Name), "");

        _testOutputHelper.WriteLine("Actual");

        foreach (var result in resultsFromSearch)
        {
            _testOutputHelper.WriteLine(resultsFromSearch.ToString());
        }

        foreach (var person in personList)
        {
            Assert.Contains(person, resultsFromSearch);
        }
    }

    [Fact]
    public void GetFilteredPersons_SearchByName()
    {
        CountryAddRequest countryAddRequest = new CountryAddRequest()
        {
            CountryName = "Egypt"
        };
        CountryResponse country = _countriesService.AddCountry(countryAddRequest);
        PersonAddRequest? person1 = new PersonAddRequest
        {
            Name = "youssef",
            Email = "mohamed@gmail.com",
            Address = "12st alexandria ....",
            CountryId = country.CountryId,
            Gender = GenderOptions.Male,
            DateOfBirth = DateTime.Parse("2000-01-01"),
            ReceiveNewsLetters = true,
        };
        PersonAddRequest? person2 = new PersonAddRequest
        {
            Name = "sssssyoune",
            Email = "a@gmail.com",
            Address = "a12st alexandria ....",
            CountryId = country.CountryId,
            Gender = GenderOptions.Male,
            DateOfBirth = DateTime.Parse("2005-01-01"),
            ReceiveNewsLetters = false,
        };
        PersonAddRequest? person3 = new PersonAddRequest
        {
            Name = "ahmed",
            Email = "a@aaaagmail.com",
            Address = "a12st alexandria ....",
            CountryId = country.CountryId,
            Gender = GenderOptions.Male,
            DateOfBirth = DateTime.Parse("2005-01-01"),
            ReceiveNewsLetters = false,
        };
        List<PersonResponse> personList = new List<PersonResponse>();
        List<PersonAddRequest> personsRequest = new List<PersonAddRequest>()
        {
            person1, person2, person3
        };
        foreach (var person in personsRequest)
        {
            PersonResponse? response = _personsService.AddPerson(person);
            personList.Add(response);
        }


        List<PersonResponse>? resultsFromSearch = _personsService.GetFilteredPersons(nameof(Person.Name), "yo");

        _testOutputHelper.WriteLine("Actual");

        foreach (var result in resultsFromSearch)
        {
            _testOutputHelper.WriteLine(result.ToString());
        }

        foreach (PersonResponse person in personList)
        {
            if (person.Name != null)
            {
                if (person.Name.Contains("yu", StringComparison.OrdinalIgnoreCase))
                {
                    Assert.Contains(person, resultsFromSearch);
                }
            }
        }
    }

    #endregion

    #region GetStoredPerson

    [Fact]
    public void GetStoredPerson()
    {
        CountryAddRequest countryAddRequest = new CountryAddRequest()
        {
            CountryName = "Egypt"
        };
        CountryResponse country = _countriesService.AddCountry(countryAddRequest);
        PersonAddRequest? person1 = new PersonAddRequest
        {
            Name = "youssef",
            Email = "mohamed@gmail.com",
            Address = "12st alexandria ....",
            CountryId = country.CountryId,
            Gender = GenderOptions.Male,
            DateOfBirth = DateTime.Parse("2000-01-01"),
            ReceiveNewsLetters = true,
        };
        PersonAddRequest? person2 = new PersonAddRequest
        {
            Name = "sssssyoune",
            Email = "a@gmail.com",
            Address = "a12st alexandria ....",
            CountryId = country.CountryId,
            Gender = GenderOptions.Male,
            DateOfBirth = DateTime.Parse("2005-01-01"),
            ReceiveNewsLetters = false,
        };
        PersonAddRequest? person3 = new PersonAddRequest
        {
            Name = "ahmed",
            Email = "a@aaaagmail.com",
            Address = "a12st alexandria ....",
            CountryId = country.CountryId,
            Gender = GenderOptions.Male,
            DateOfBirth = DateTime.Parse("2005-01-01"),
            ReceiveNewsLetters = false,
        };
        List<PersonResponse> personList = new List<PersonResponse>();
        List<PersonAddRequest> personsRequest = [person1, person2, person3];
        foreach (var person in personsRequest)
        {
            PersonResponse? response = _personsService.AddPerson(person);
            personList.Add(response);
        }

        List<PersonResponse> allPersons = _personsService.GetAllPersons();

        List<PersonResponse>? resultsFromSort =
            _personsService.GetSortedPerson(allPersons, nameof(Person.Name), SortOrderOption.DESC);

        _testOutputHelper.WriteLine("Actual");

        foreach (var result in resultsFromSort)
        {
            _testOutputHelper.WriteLine(result.ToString());
        }
        personList = personList.OrderByDescending(p => p.Name).ToList();

        for (int i = 0; i < personList.Count; i++)
        {
            Assert.Equal(personList[i].Name, resultsFromSort[i].Name);
        }
    }

    #endregion
}