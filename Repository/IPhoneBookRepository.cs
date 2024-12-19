using PhoneBook.Contracts;
using PhoneBook.Contracts.Models;

namespace PhoneBook.Repository;

public interface IPhoneBookRepository
{
    public Task<int> AddToChildTable(string tableName, string value);
    public Task UpdateChildTable(string tableName, int id, string value);
    public Task DeleteFromTable(string tableName, int id);
    public Task<IEnumerable<ChildTableItem>> GetLikeFromTable(string tableName, string value);
    
    public Task<int> AddContact(AddOrUpdateContactRequest request);
    public Task<int> AddOrUpdateContact(AddOrUpdateContactRequest request);
    public Task DeleteContact(int id);
    public Task<IEnumerable<ContactItem>> GetContacts(GetContactsRequest request);
}