using BasicService.Data;
using BasicService.Models;
using BasicService.Validation;
using Microsoft.AspNetCore.Mvc;

namespace BasicService.Controllers
{
    [ApiController] // Telling its API Controller not MVC Controller
    [Route("api/[controller]")] // It will take name from the controller name
    public class ContatcsController : Controller
    {
        private readonly ContactsAPIDbContext dbContxt;

        public ContatcsController(ContactsAPIDbContext dbContext) // Inject DbContext
        {
            this.dbContxt = dbContext;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<DBContact>))]
        public IActionResult GetContacts()
        {
            return  Ok(dbContxt.Contacts.ToList());
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DBContact))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetContact([FromRoute] Guid id)
        {
            DBContact dBcontact = await dbContxt.Contacts.FindAsync(id);
            if(dBcontact != null)
            {
                return Ok(dBcontact);
            }
            return NoContent();

        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Contact))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(FluentValidation.Results.ValidationResult))]
        //Return type of async method is either void or , Task<t>
        public async Task<IActionResult> AddContact([FromBody] Contact data) {
            var contactValidator = new ContactValidator();

            // Call Validate or ValidateAsync and pass the object which needs to be validated
            var result = contactValidator.Validate(data);

            if (result.IsValid)
            {
                DBContact contact = new DBContact()
                {
                    Id = Guid.NewGuid(),
                    Name = data.Name,
                    Email = data.Email,
                    Phone = data.Phone,
                    Address = data.Address
                };

                await dbContxt.Contacts.AddAsync(contact);
                await dbContxt.SaveChangesAsync();
                return Ok(contact);
            }

            var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
            return BadRequest(errorMessages);
  
        }
       
        
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Contact))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(FluentValidation.Results.ValidationResult))]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateContact([FromRoute] Guid id, Contact data)
        {
            var dBcontact = await dbContxt.Contacts.FindAsync(id);

            if(dBcontact != null)
            {
                dBcontact.Name = data.Name;
                dBcontact.Email = data.Email;
                dBcontact.Phone = data.Phone;
                dBcontact.Address = data.Address;

                await dbContxt.SaveChangesAsync();
                return Ok(dBcontact);
            }

            return NotFound();
        }
      
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Contact))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteContact([FromRoute] Guid id)
        {
            var dBcontact = await dbContxt.Contacts.FindAsync(id);
            if (dBcontact != null)
            {
                dbContxt.Remove(dBcontact);
                await dbContxt.SaveChangesAsync();
                return Ok(dBcontact);
            }
            return NotFound();
            

        }
        /*
        [
              {
                "id": "74901648-8341-4784-ad5b-ad649ffc7ff4",
                "name": "TestModied",
                "email": "sasas@email.com",
                "phone": 6666666,
                "address": "sttest1ring"
              },
              {
                "id": "0ef01d7e-42ba-4f23-8a1f-1dff394440be",
                "name": "Test2",
                "email": "sasas2222@email.com",
                "phone": 66666662,
                "address": "sttest1ring222"
              }
        ]
        */

    }
}
