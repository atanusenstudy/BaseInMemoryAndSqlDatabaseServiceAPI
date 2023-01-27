using BasicService.Data;
using BasicService.Models;
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
        public IActionResult GetContacts()
        {
            return  Ok(dbContxt.Contacts.ToList());
        }

        [HttpGet]
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
        //Return type of async method is either void or , Task<t>
        public async Task<IActionResult> AddContact(Contact data) { 
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
       
        
        [HttpPut]
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
