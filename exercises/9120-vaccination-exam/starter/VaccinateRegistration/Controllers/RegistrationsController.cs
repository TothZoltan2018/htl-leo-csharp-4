using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VaccinateRegistration.Data;

namespace VaccinateRegistration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationsController : ControllerBase
    {
        private readonly VaccinateDbContext context;

        public RegistrationsController(VaccinateDbContext context) 
        {
            this.context = context; 
        }

        // This class is NOT COMPLETE.
        // Todo: Complete the class according to the requirements

        [HttpGet] // eg.: https://localhost:44378/api/Registrations?ssn=3594171252&pin=802585
        public async Task<GetRegistrationResult?> GetRegistration([FromQuery] long ssn, [FromQuery] int pin)
        {
            var reg = await context.GetRegistration(ssn, pin);
            return reg;
        }
        

        [HttpGet]
        [Route("timeSlots")]
        public async Task<IEnumerable<DateTime>> GetTimeslots([FromQuery] DateTime date)
            => await context.GetTimeslots(date);       
    }
}
