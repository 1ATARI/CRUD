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


    public PersonsService(bool Initialize = true)
    {
        _persons = new List<Person>();

        if (Initialize)
        {
            _persons.AddRange(new List<Person>
            {
                new Person
                {
                    PersonId = Guid.NewGuid(),
                    Name = "John Doe",
                    Email = "john.doe@example.com",
                    DateOfBirth = new DateTime(1990, 5, 14),
                    Gender = "Male",
                    CountryId = Guid.Parse("0836DDD2-E0D5-4F9D-ACDB-C9D066EC4DFD"),
                    Address = "123 Main St, Cairo",
                    ReceiveNewsLetters = true
                },
                new Person
                {
                    PersonId = Guid.NewGuid(),
                    Name = "Jane Smith",
                    Email = "jane.smith@example.com",
                    DateOfBirth = new DateTime(1985, 3, 22),
                    Gender = "Female",
                    CountryId = Guid.Parse("1AE34F09-ACFE-4A4A-862B-047A29FA5AA5"),
                    Address = "456 Oak St, New York",
                    ReceiveNewsLetters = false
                },
                new Person
                {
                    PersonId = Guid.NewGuid(),
                    Name = "Mike Johnson",
                    Email = "mike.johnson@example.com",
                    DateOfBirth = new DateTime(1992, 11, 30),
                    Gender = "Male",
                    CountryId = Guid.Parse("AC7E276F-6E30-4A91-B47F-0CE6BB1157A2"),
                    Address = "789 Pine St, Toronto",
                    ReceiveNewsLetters = true
                },
                new Person
                {
                    PersonId = Guid.NewGuid(),
                    Name = "Emily Davis",
                    Email = "emily.davis@example.com",
                    DateOfBirth = new DateTime(1988, 7, 18),
                    Gender = "Female",
                    CountryId = Guid.Parse("B69BFA71-B264-48B3-8511-A6C9C60C00AB"),
                    Address = "321 Cedar St, Paris",
                    ReceiveNewsLetters = false
                },
                new Person
                {
                    PersonId = Guid.NewGuid(),
                    Name = "Chris Lee",
                    Email = "chris.lee@example.com",
                    DateOfBirth = new DateTime(1995, 12, 2),
                    Gender = "Male",
                    CountryId = Guid.Parse("81BA47A0-54AC-441B-A863-D9444DFBD44A"),
                    Address = "654 Willow St, Berlin",
                    ReceiveNewsLetters = true
                }
            });
        }

        _countriesService = new CountriesService();
    }

    private PersonResponse? ConvertPersonToPersonResponse(Person person)
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
        return ConvertPersonToPersonResponse(person);
    }

    public List<PersonResponse> GetAllPersons()
    {
        return _persons.Select(p => ConvertPersonToPersonResponse(p)).ToList();
    }

    public PersonResponse? GetPersonById(Guid? PersonId)
    {
        Person? person = _persons.FirstOrDefault(p => p.PersonId == PersonId);
        if (person is null || PersonId is null)
        {
            return null;
        }

        return ConvertPersonToPersonResponse(person);
    }

    public List<PersonResponse>? GetFilteredPersons(string? searchBy, string? searchString)
    {
        searchBy = string.IsNullOrEmpty(searchBy) ? nameof(Person.Name) : searchBy;
        List<PersonResponse> allPersons = GetAllPersons();
        List<PersonResponse> matchingPerson = GetAllPersons();

        if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString))
        {
            return matchingPerson;
        }


        switch (searchBy)
        {
            case nameof(PersonResponse.Name):
                matchingPerson = allPersons
                    .Where(p =>
                        p.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                break;

            case nameof(PersonResponse.Email):
                matchingPerson = allPersons
                    .Where(p => !string.IsNullOrEmpty(p.Email) &&
                                p.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                break;

            case nameof(PersonResponse.DateOfBirth):
                if (DateTime.TryParse(searchString, out DateTime parsedDate))
                {
                    matchingPerson = allPersons
                        .Where(p => p.DateOfBirth.HasValue &&
                                    p.DateOfBirth.Value.Date == parsedDate.Date)
                        .ToList();
                }

                break;

            case nameof(PersonResponse.Gender):
                matchingPerson = allPersons
                    .Where(p => !string.IsNullOrEmpty(p.Gender) &&
                                p.Gender.Equals(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                break;
            case nameof(PersonResponse.CountryName):  
                matchingPerson = allPersons
                    .Where(p => !string.IsNullOrEmpty(p.CountryName) &&
                                p.CountryName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                break;
            case nameof(PersonResponse.Address):
                matchingPerson = allPersons
                    .Where(p => !string.IsNullOrEmpty(p.Address) &&
                                p.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                break;

            case nameof(PersonResponse.ReceiveNewsLetters):
                if (bool.TryParse(searchString, out bool parsedBool))
                {
                    matchingPerson = allPersons
                        .Where(p => p.ReceiveNewsLetters &&
                                    p.ReceiveNewsLetters == parsedBool)
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
    public List<PersonResponse>? GetSortedPerson(List<PersonResponse> allPersons, string sortBy,
        SortOrderOption sortOrder)
    {
        if (allPersons == null || allPersons.Count == 0)
            return allPersons;

        if (string.IsNullOrEmpty(sortBy))
            return allPersons;

        var propertyInfo = typeof(PersonResponse).GetProperty(sortBy,
            BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
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

        Person person = _persons.FirstOrDefault(p => p.PersonId == personUpdateRequest.PersonId);
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

        return ConvertPersonToPersonResponse(person);
    }

    public bool DeletePerson(Guid? personId)
    {
        if (personId is null)
        {
            throw new ArgumentNullException(nameof(personId));
        }

        Person? person = _persons.FirstOrDefault(p => p.PersonId == personId);
        if (person is null)
            return false;

        return _persons.Remove(person);
    }
}