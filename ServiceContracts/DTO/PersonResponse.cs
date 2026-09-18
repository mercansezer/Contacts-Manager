using Entities;

public class PersonRespone
{
    public Guid PersonID { get; set; }
    public string? PersonName { get; set; }
    public string? Email { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public Guid? CountryID { get; set; }
    public string? Country { get; set; }
    public string? Address { get; set; }
    public bool ReceiveNewsLetters { get; set; }
    public double? Age { get; set; }


    public override bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;

        }

        if (obj.GetType() != typeof(PersonRespone))
        {
            return false;
        }


        var personResponse = (PersonRespone)obj;

        return PersonID == personResponse.PersonID && PersonName == personResponse.PersonName && Email == personResponse.Email &&
            DateOfBirth == personResponse.DateOfBirth && Gender == personResponse.Gender && CountryID == personResponse.CountryID && Address == personResponse.Address && ReceiveNewsLetters == personResponse.ReceiveNewsLetters;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }


}
public static class PersonExtensions
{
    public static PersonRespone ToPersonResponse(this Person person)
    {

        return new PersonRespone
        {
            PersonID = person.PersonID,
            PersonName = person.PersonName,
            Email = person.Email,
            DateOfBirth = person.DateOfBirth,
            ReceiveNewsLetters = person.ReceiveNewsLetters,
            Address = person.Address,
            CountryID = person.CountryID,
            Gender = person.Gender,
            Age = (person.DateOfBirth != null) ? Math.Round((DateTime.Now - person.DateOfBirth.Value).TotalDays / 365.25) : null
        };

    }
}