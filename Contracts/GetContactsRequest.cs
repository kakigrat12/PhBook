using PhoneBook.Contracts.Models;

namespace PhoneBook.Contracts;

public record GetContactsRequest()
{
    public Dictionary<string, object> Filters { get; set; } = new();
}

public record GetContactsResponse(IEnumerable<ContactItem> contacts) : Response
{
    public IEnumerable<ContactItem> Contacts = new List<ContactItem>();
}