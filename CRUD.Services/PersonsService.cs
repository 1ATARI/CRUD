using System.ComponentModel.DataAnnotations;
using CRUD.Data;
using CRUD.ServiceContracts;
using CRUD.ServiceContracts.DTO.PersonDTO;
using CRUD.Services.Helpers;

namespace CRUD.Services;

public class PersonsService : IPersonsService
{
    private readonly List<Person> _persons;
    private readonly ICountriesService _countriesService;
    

    public PersonsService()
    {
        _persons = new List<Person>();
        _countriesService = new CountriesService();
    }

    private PersonResponse? PersonToPersonResponse(Person person)
    {
        PersonResponse? personResponse =  person.ToPersonResponse();
        personResponse.CountryName = _countriesService.GetCountryById(person.CountryId)?.CountryName;
        return personResponse;
    }
    public PersonResponse? AddPerson(PersonAddRequest? personAddRequest)
    {
        if (personAddRequest is null)
        {
            throw new ArgumentNullException(nameof(personAddRequest));
        }
 
        ValidationHelper.ModelValidation(personAddRequest);
   
        Person person = personAddRequest.ToPerson();
        _persons.Add(person);
        return PersonToPersonResponse(person);
    }

    public List<PersonResponse> GetAllPersons()
    {
        return _persons.Select(p => p.ToPersonResponse()).ToList();
    }

    public PersonResponse? GetPersonById(Guid? PersonId)
    {
        Person? person= _persons.FirstOrDefault(p=>p.PersonId ==PersonId);
        if (person is null || PersonId is null)
        {
            return null;
        }

        return PersonToPersonResponse(person);
    }
    
}