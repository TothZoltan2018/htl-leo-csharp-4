using ContactList.DataServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactList.Controllers
{
    //[Route("api/[controller]")]
    [Route("/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IContactsRepository contactsRepository;

        public ContactsController(IContactsRepository contactsRepository)
        {
            this.contactsRepository = contactsRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Contact>))] // IEnumerable<> ?
        public IActionResult getAllPeople() => Ok(contactsRepository.GetAllContacts());

        [HttpGet]
        [Route("findByName/{filter}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Contact>))] // IEnumerable<> ?
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult findPersonByName(string filter)
        {
            List<Contact> output = new();
            try
            {
                output = contactsRepository.FindContactByName(filter);
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
            return Ok(output);
        }

        // Not in the SwaggerSpec, just to us in the AddContact method
        [HttpGet("{id}", Name = nameof(GetPersonById))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Contact))]
        public IActionResult GetPersonById(int id)
        {
            return Ok(contactsRepository.GetContactById(id));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Contact))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult addPerson(Contact contact)
        {
            try
            {
                contactsRepository.AddContact(contact);
            }
            catch (ArgumentException)
            {
                return BadRequest("At least one filed is missing of the contact");
            }
            
            return CreatedAtAction(nameof(GetPersonById), new { id = contact.Id}, contact);
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult deletePerson(int id)
        {
            bool result;
            try
            {
                result = contactsRepository.DeleteContact(id);
            }
            catch (Exception)
            { // id < 1
                return BadRequest();
            }

            if (result) return NoContent();
            return NotFound();            
        }
    }
}
