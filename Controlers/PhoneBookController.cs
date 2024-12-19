using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PhoneBook.Contracts;
using PhoneBook.Services;

namespace PhoneBook.Controlers;

[ApiController]
[Route("api/[controller]")]
[EnableCors("AllowAllOrigins")]
public class PhoneBookController(IPhoneBookService phoneBookService) : ControllerBase
{
    [HttpPost("AddOrUpdateToTable")]
    [ProducesResponseType(typeof(AddOrUpdateToTableResponse), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    public async Task<IActionResult> AddOrUpdateToTable([FromBody] AddOrUpdateToTableRequest request)
    {
        var response = await phoneBookService.AddOrUpdateToTable(request);
        return response is AddOrUpdateToTableResponse 
            ? Ok(response) 
            : BadRequest(response);
    }
    
    [HttpPost("DeleteFromTable")]
    [ProducesResponseType(typeof(DeleteFromTableResponse), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    public async Task<IActionResult> AddOrUpdateToTable([FromBody] DeleteFromTableRequest request)
    {
        var response = await phoneBookService.DeleteFromTable(request);
        return response is DeleteFromTableResponse 
            ? Ok(response) 
            : BadRequest(response);
    }
    
    [HttpPost("GetLikeFromTable")]
    [ProducesResponseType(typeof(GetLikeFromTableResponse), 200)]
    public async Task<IActionResult> GetLikeFromTable([FromBody] GetLikeFromTableRequest request)
    {
        return Ok(await phoneBookService.GetLikeFromTable(request));
    }
    
    
        
    [HttpPost("AddOrUpdateContact")]
    [ProducesResponseType(typeof(AddOrUpdateContactResponse), 200)]
    public async Task<IActionResult> AddOrUpdateContact(AddOrUpdateContactRequest request)
    {
        return Ok(await phoneBookService.AddOrUpdateContact(request));
    }
    
    [HttpGet("DeleteContact")]
    [ProducesResponseType(typeof(DeleteContactResponse), 200)]
    public async Task<IActionResult> DeleteContact(int id)
    {
        return Ok(await phoneBookService.DeleteContact(id));
    }
    
    [HttpPost("GetContacts")]
    [ProducesResponseType(typeof(GetContactsResponse), 200)]
    public async Task<IActionResult> GetContacts([FromBody] GetContactsRequest request)
    {
        return Ok(await phoneBookService.GetContacts(request));
    }
}