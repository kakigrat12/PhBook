namespace PhoneBook.Contracts;

public record AddOrUpdateContactRequest
{
    public int? Id { get; set; }
    public string Surname { get; set; }
    public string Name { get; set; }
    public string Otch { get; set; }
    
    public string Street { get; set; }
    public string House { get; set; }
    public string Corp { get; set; }
    public int Apart { get; set; }
    public string Tel { get; set; }
}

public record AddOrUpdateContactResponse : Response
{
    public int Id { get; set; }
}