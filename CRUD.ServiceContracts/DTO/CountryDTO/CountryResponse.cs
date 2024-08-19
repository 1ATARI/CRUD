using CRUD.Data;

namespace CRUD.ServiceContracts.DTO.CountryDTO;

public class CountryResponse
{
    public Guid CountryId { get; set; }
    public string? CountryName { get; set; }
    public override bool Equals(object? obj)
    {
        if (obj==null || obj.GetType() != typeof(CountryResponse))
        {
            return false;
        }
        CountryResponse objAsResponse = (CountryResponse)obj ;
        return CountryId ==  objAsResponse.CountryId && CountryName==objAsResponse.CountryName ;
    }
    
}
public static class CountryExtensions
{
    public static CountryResponse ToCountryResponse(this Country country)
         {
             return new CountryResponse
             {
                 CountryId = country.CountryId,
                 CountryName = country.CountryName
             };
         }
}