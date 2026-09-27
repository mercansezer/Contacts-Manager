using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts
{
    public interface IPersonsService
    {

        PersonRespone? AddPerson(PersonAddRequest? personAddRequest);

        List<PersonRespone> GetAllPersons();

        PersonRespone? GetPersonById(Guid? personId);

        List<PersonRespone> GetFilteredPerson(string? searchBy, string? searchString);


        List<PersonRespone> GetSortedPersons(List<PersonRespone> allPersons, string sortBy, SortOrderOptions sortOption);


    }
}
