using System.ComponentModel.DataAnnotations;
using CRUD.Data;
using CRUD.ServiceContracts.DTO.Enums;

namespace CRUD.ServiceContracts.DTO.PersonDTO;

public class PersonAddRequest
{
    [Required(ErrorMessage = "You must provide valid name")]
    public required string Name { get; set; }
    [EmailAddress(ErrorMessage = "Email should be valid ") , Required(ErrorMessage ="Email can't be empty")]
    public required string Email { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public GenderOptions? Gender { get; set; }
    public Guid? CountryId { get; set; }
    public string? Address { get; set; }
    public bool? ReceiveNewsLetters { get; set; }



    public Person ToPerson()
    {
        return new Person()
        {
            PersonId = Guid.NewGuid() ,
            Name = Name,
            Email = Email,
            DateOfBirth = DateOfBirth,
            Gender = Gender.ToString(),
            Address = Address,
            CountryId = CountryId,
            ReceiveNewsLetters = ReceiveNewsLetters

        };
    }
}