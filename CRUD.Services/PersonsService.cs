using System.ComponentModel.DataAnnotations;
using System.Reflection;
using CRUD.Data;
using CRUD.ServiceContracts;
using CRUD.ServiceContracts.DTO.Enums;
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
        PersonResponse? personResponse = person.ToPersonResponse();
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
        Person? person = _persons.FirstOrDefault(p => p.PersonId == PersonId);
        if (person is null || PersonId is null)
        {
            return null;
        }

        return PersonToPersonResponse(person);
    }

    public List<PersonResponse>? GetFilteredPersons(string? searchBy, string searchString)
    {
        List<PersonResponse> allPersons = GetAllPersons();
        List<PersonResponse> matchingPerson = allPersons;

        if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString))
        {
            return matchingPerson;
        }


        switch (searchBy)
        {
            case nameof(Person.Name):
                matchingPerson = allPersons
                    .Where(p =>
                        p.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                break;

            case nameof(Person.Email):
                matchingPerson = allPersons
                    .Where(p => !string.IsNullOrEmpty(p.Email) &&
                                p.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                break;

            case nameof(Person.DateOfBirth):
                if (DateTime.TryParse(searchString, out DateTime parsedDate))
                {
                    matchingPerson = allPersons
                        .Where(p => p.DateOfBirth.HasValue &&
                                    p.DateOfBirth.Value.Date == parsedDate.Date)
                        .ToList();
                }

                break;

            case nameof(Person.Gender):
                matchingPerson = allPersons
                    .Where(p => !string.IsNullOrEmpty(p.Gender) &&
                                p.Gender.Equals(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                break;
            case nameof(Person.CountryId):
                if (Guid.TryParse(searchString, out Guid parsedGuid))
                {
                    matchingPerson = allPersons
                        .Where(p => p.CountryId.HasValue &&
                                    p.CountryId.Value == parsedGuid)
                        .ToList();
                }

                break;
            case nameof(Person.Address):
                matchingPerson = allPersons
                    .Where(p => !string.IsNullOrEmpty(p.Address) &&
                                p.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                break;

            case nameof(Person.ReceiveNewsLetters):
                if (bool.TryParse(searchString, out bool parsedBool))
                {
                    matchingPerson = allPersons
                        .Where(p => p.ReceiveNewsLetters.HasValue &&
                                    p.ReceiveNewsLetters.Value == parsedBool)
                        .ToList();
                }

                break;

            default:
                return allPersons;
        }

        return matchingPerson;
    }

    // public List<PersonResponse>? GetsortedredPerson(List<PersonResponse> allPersons, string sortBy, SortOrderOption sortOrder)
    // {
    //     if (string.IsNullOrEmpty(sortBy))
    //         return allPersons;
    //     
    //
    //     List<PersonResponse> sortedPersons = (sortBy, sortOrder)
    //         switch
    //         {
    //             //Name sort
    //             (nameof(PersonResponse.Name), SortOrderOption.ASC)
    //                 => allPersons.OrderBy(p => p.Name, StringComparer.OrdinalIgnoreCase).ToList(),
    //             (nameof(PersonResponse.Name), SortOrderOption.DESC)
    //                 => allPersons.OrderByDescending(p => p.Name, StringComparer.OrdinalIgnoreCase).ToList(),
    //
    //             //Email sort
    //             (nameof(PersonResponse.Email), SortOrderOption.ASC)
    //                 => allPersons.OrderBy(p => p.Email, StringComparer.OrdinalIgnoreCase).ToList(),
    //             (nameof(PersonResponse.Email), SortOrderOption.DESC)
    //                 => allPersons.OrderByDescending(p => p.Email, StringComparer.OrdinalIgnoreCase).ToList(),
    //
    //             //Date Of Birth sort
    //             (nameof(PersonResponse.DateOfBirth), SortOrderOption.ASC)
    //                 => allPersons.OrderBy(p => p.DateOfBirth).ToList(),
    //             (nameof(PersonResponse.DateOfBirth), SortOrderOption.DESC)
    //                 => allPersons.OrderByDescending(p => p.DateOfBirth).ToList(),
    //
    //             //Age sort
    //             (nameof(PersonResponse.Age), SortOrderOption.ASC)
    //                 => allPersons.OrderBy(p => p.Age).ToList(),
    //             (nameof(PersonResponse.Age), SortOrderOption.DESC)
    //                 => allPersons.OrderByDescending(p => p.Age).ToList(),
    //
    //
    //             //Gender sort
    //             (nameof(PersonResponse.Gender), SortOrderOption.ASC)
    //                 => allPersons.OrderBy(p => p.Gender, StringComparer.OrdinalIgnoreCase).ToList(),
    //             (nameof(PersonResponse.Gender), SortOrderOption.DESC)
    //                 => allPersons.OrderByDescending(p => p.Gender, StringComparer.OrdinalIgnoreCase).ToList(),
    //
    //
    //             //Country sort
    //             (nameof(PersonResponse.CountryName), SortOrderOption.ASC)
    //                 => allPersons.OrderBy(p => p.CountryName, StringComparer.OrdinalIgnoreCase).ToList(),
    //             (nameof(PersonResponse.CountryName), SortOrderOption.DESC)
    //                 => allPersons.OrderByDescending(p => p.CountryName, StringComparer.OrdinalIgnoreCase).ToList(),
    //
    //             //Address sort
    //             (nameof(PersonResponse.Address), SortOrderOption.ASC)
    //                 => allPersons.OrderBy(p => p.Address, StringComparer.OrdinalIgnoreCase).ToList(),
    //             (nameof(PersonResponse.Address), SortOrderOption.DESC)
    //                 => allPersons.OrderByDescending(p => p.Address, StringComparer.OrdinalIgnoreCase).ToList(),
    //
    //             //ReceiveNewsLetters sort
    //             (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOption.ASC)
    //                 => allPersons.OrderBy(p => p.ReceiveNewsLetters).ToList(),
    //             (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOption.DESC)
    //                 => allPersons.OrderByDescending(p => p.ReceiveNewsLetters).ToList(),
    //
    //             _ => allPersons
    //         };
    //     return sortedPersons;
    // }
    //
    
    //using reflection 
    public List<PersonResponse>? GetSortedPerson(List<PersonResponse> allPersons, string sortBy, SortOrderOption sortOrder)
    {
        if (allPersons == null || allPersons.Count == 0) 
            return allPersons; 

        if (string.IsNullOrEmpty(sortBy))
            return allPersons; 

        var propertyInfo = typeof(PersonResponse).GetProperty(sortBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        if (propertyInfo == null)
            throw new ArgumentException($"Invalid sort property: {sortBy}");

        IOrderedEnumerable<PersonResponse> sortedPersons = sortOrder switch
        {
            SortOrderOption.ASC => allPersons.OrderBy(p => propertyInfo.GetValue(p, null)),
            SortOrderOption.DESC => allPersons.OrderByDescending(p => propertyInfo.GetValue(p, null)),
            _ => throw new ArgumentOutOfRangeException(nameof(sortOrder), $"Invalid sort order: {sortOrder}")
        };

        return sortedPersons.ToList();
    }

    public PersonResponse? UpdatePerson(PersonUpdateRequest? personUpdateRequest)
    {
        if (personUpdateRequest is null || personUpdateRequest.PersonId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(personUpdateRequest));
        }

        ValidationHelper.ModelValidation(personUpdateRequest);

        Person person= _persons.FirstOrDefault(p => p.PersonId==personUpdateRequest.PersonId);
        if (person is null)
        {
         throw new ArgumentException("Person not found");   
        }
        person.Name = personUpdateRequest.Name;
        person.Email = personUpdateRequest.Email;
        person.DateOfBirth = personUpdateRequest.DateOfBirth;
        person.Gender = personUpdateRequest.Gender.ToString(); 
        person.Address = personUpdateRequest.Address;
        person.CountryId = personUpdateRequest.CountryId;
        person.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;

        return person.ToPersonResponse();
    }
}