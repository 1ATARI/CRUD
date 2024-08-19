using CRUD.Data;
using CRUD.ServiceContracts.DTO.Enums;

namespace CRUD.ServiceContracts.DTO.PersonDTO;

public class PersonResponse
{
    public Guid PersonId { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public Guid? CountryId { get; set; }
    public string? CountryName { get; set; }
    public string? Address { get; set; }
    public bool? ReceiveNewsLetters { get; set; }
    public double? Age { get; set; }


    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != typeof(PersonResponse))
        {
            return false;
        }

        PersonResponse person = (PersonResponse)obj;
        return this.PersonId == person.PersonId && Name == person.Name && Email == person.Email &&
               DateOfBirth == person.DateOfBirth && Gender == person.Gender && CountryId == person.CountryId &&
               Address == person.Address && ReceiveNewsLetters == person.ReceiveNewsLetters && Age == person.Age;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return
            $"Person ID :{PersonId} Name {Name} Email: {Email} DateOfBirth: {DateOfBirth} Age {Age}" +
            $" Gender: {Gender} Country ID: {CountryId} Country Name: {CountryName} Address: {Address} Receive News Letters: {ReceiveNewsLetters}";
    }
}

public static class PersonExtensions
{
    public static PersonResponse? ToPersonResponse(this Person person)
    {
        return new PersonResponse
        {
            PersonId = person.PersonId, Name = person.Name, Email = person.Email,
            DateOfBirth = person.DateOfBirth, Gender = person.Gender, CountryId = person.CountryId,
            Address = person.Address, ReceiveNewsLetters = person.ReceiveNewsLetters,
            Age = (person.DateOfBirth != null)
                ? Math.Round((DateTime.Now - person.DateOfBirth.Value).TotalDays / 365.25)
                : null
        };
    }
}