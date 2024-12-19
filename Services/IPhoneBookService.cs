using PhoneBook.Contracts;
using PhoneBook.Contracts.Models;

namespace PhoneBook.Services;

public interface IPhoneBookService
{
    /// <summary>
    /// Метод добавления/измения записи в дочерних таблицах
    /// </summary>
    public Task<Response> AddOrUpdateToTable(AddOrUpdateToTableRequest request);
    
    /// <summary>
    /// Метод удаления из дочерней таблицы
    /// </summary>
    public Task<Response> DeleteFromTable(DeleteFromTableRequest request);
    
    /// <summary>
    /// Метод получения похожих записей из дочерних таблиц
    /// </summary>
    public Task<Response> GetLikeFromTable(GetLikeFromTableRequest request);
        
    
    /// <summary>
    /// Метод добавления/изменения контакта
    /// </summary>
    public Task<Response> AddOrUpdateContact(AddOrUpdateContactRequest request);
    
    /// <summary>
    /// Метод удаления контакта
    /// </summary>
    public Task<Response> DeleteContact(int id);
    
    /// <summary>
    /// Метод получения контактов по фильтрам
    /// </summary>
    public Task<GetContactsResponse> GetContacts(GetContactsRequest request);
}