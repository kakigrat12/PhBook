using PhoneBook.Contracts.Models;

namespace PhoneBook.Contracts;

public record GetLikeFromTableRequest
{
    public string TableName { get; set; }
    public string Value { get; set; }
}

public record GetLikeFromTableResponse : Response
{
    public IEnumerable<ChildTableItem> Items { get; set; }
}