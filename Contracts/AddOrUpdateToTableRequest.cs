namespace PhoneBook.Contracts;

public record AddOrUpdateToTableRequest
{
    public int? Id { get; set; }
    public string TableName { get; set; } 
    public string Value { get; set; }
}

public record AddOrUpdateToTableResponse : Response
{
    public int Id { get; set; }
}