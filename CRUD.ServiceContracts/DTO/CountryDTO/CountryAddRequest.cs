using CRUD.Data;

namespace CRUD.ServiceContracts.DTO.CountryDTO;

public class CountryAddRequest
{
    public string? CountryName { get; set; }

    public Country ToCountry()
    {
        return new Country() { CountryId = Guid.NewGuid(), CountryName = this.CountryName  };
    }
}