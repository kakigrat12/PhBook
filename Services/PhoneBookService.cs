using PhoneBook.Contracts;
using PhoneBook.Repository;

namespace PhoneBook.Services;

public class PhoneBookService(IPhoneBookRepository repository) : IPhoneBookService
{
    public async Task<Response> AddOrUpdateToTable(AddOrUpdateToTableRequest request)
    {
        if(!Constants.ChildTables.TableExists(request.TableName))
            return new ErrorResponse {Message = "Table not found"};
        try
        {
            if (request.Id.HasValue)
            {
                await repository.UpdateChildTable(request.TableName, request.Id.Value, request.Value);
                return new AddOrUpdateToTableResponse { Id = request.Id.Value };
            }
            else
            {
                return new AddOrUpdateToTableResponse
                {
                    Id = await repository.AddToChildTable(request.TableName, request.Value)
                };
            }
        }
        catch (Exception ex)
        {
            return new ErrorResponse {Message = ex.Message};
        }
    }

    public async Task<Response> DeleteFromTable(DeleteFromTableRequest request)
    {
        if(!Constants.ChildTables.TableExists(request.TableName))
            return new ErrorResponse {Message = "Table not found"};
        
        try
        {
            await repository.DeleteFromTable(request.TableName, request.Id);
            return new DeleteFromTableResponse();
        }
        catch (Exception ex)
        {
            return new ErrorResponse {Message = ex.Message};
        }
    }

    public async Task<Response> GetLikeFromTable(GetLikeFromTableRequest request)
    {
        if(!Constants.ChildTables.TableExists(request.TableName))
            return new ErrorResponse {Message = "Table not found"};

        return new GetLikeFromTableResponse
        {
            Items = await repository.GetLikeFromTable(request.TableName, request.Value)
        };
    }

    public async Task<Response> AddOrUpdateContact(AddOrUpdateContactRequest request)
    {
        return new AddOrUpdateContactResponse
        {
            Id = await repository.AddOrUpdateContact(request)
        };
    }

    public async Task<Response> DeleteContact(int id)
    {
        await repository.DeleteContact(id);
        return new DeleteContactResponse();
    }

    public async Task<GetContactsResponse> GetContacts(GetContactsRequest request)
    {
        var contacts = await repository.GetContacts(request);
        return new GetContactsResponse(contacts);
    }
}