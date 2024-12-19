namespace PhoneBook.Contracts;

public record DeleteFromTableRequest
{
    public string TableName { get; set; }
    public int Id { get; set; }
}

public record DeleteFromTableResponse : Response
{
}