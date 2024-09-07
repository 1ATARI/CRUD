using CRUD.ServiceContracts.DTO.Enums;
using CRUD.ServiceContracts.DTO.PersonDTO;

namespace CRUD.ServiceContracts;

public interface IPersonsService
{
    PersonResponse? AddPerson(PersonAddRequest? personAddRequest);

    List<PersonResponse> GetAllPersons();
    PersonResponse? GetPersonById(Guid? PersonId);
    List<PersonResponse>? GetFilteredPersons(string? searchBy , string searchString);
    
    List<PersonResponse>? GetSortedPerson(List<PersonResponse> allPersons,string sortBy , SortOrderOption sortOrderOption);
    
    PersonResponse? UpdatePerson(PersonUpdateRequest? personUpdateRequest);
    
    bool DeletePerson(Guid? personId);
}