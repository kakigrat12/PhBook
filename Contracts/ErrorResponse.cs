namespace PhoneBook.Contracts;

public record ErrorResponse : Response
{
    public string Message { get; set; }
}