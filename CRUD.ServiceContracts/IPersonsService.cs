using CRUD.ServiceContracts.DTO.PersonDTO;

namespace CRUD.ServiceContracts;

public interface IPersonsService
{
    PersonResponse? AddPerson(PersonAddRequest? personAddRequest);

    List<PersonResponse> GetAllPersons();
    PersonResponse? GetPersonById(Guid? PersonId);

}