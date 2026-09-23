using ServiceContracts.DTO;

namespace ServiceContracts
{
    public interface IPersonsService
    {

        PersonRespone? AddPerson(PersonAddRequest? personAddRequest);

        List<PersonRespone> GetAllPersons();

        PersonRespone? GetPersonById(Guid? personId);

        List<PersonRespone> GetFilteredPerson(string? searchBy, string? searchString);
    }
}
