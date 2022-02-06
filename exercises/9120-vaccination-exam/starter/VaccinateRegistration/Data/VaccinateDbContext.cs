using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor.

namespace VaccinateRegistration.Data
{
    public record GetRegistrationResult(int Id, long Ssn, string FirstName, string LastName);

    public record StoreVaccination(int RegistrationId, DateTime Datetime);
    
    
    public class VaccinateDbContext : DbContext
    {
        public VaccinateDbContext(DbContextOptions<VaccinateDbContext> options) : base(options) { }

        public DbSet<Vaccination> Vaccinations { get; set; }

        public DbSet<Registration> Registrations { get; set; }

        // Not needed.
//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            modelBuilder.Entity<Vaccination>()
//                .HasOne(v => v.Registration)
//#pragma warning disable CS8603 // Possible null reference return.
//                .WithOne(r => r.Vaccination)
//#pragma warning restore CS8603 // Possible null reference return.
//                .OnDelete(DeleteBehavior.NoAction);

//            modelBuilder.Entity<Registration>()
//                .HasOne(r => r.Vaccination)
//#pragma warning disable CS8602 // Dereference of a possibly null reference.
//                .WithOne(v => v.Registration)
//#pragma warning restore CS8602 // Dereference of a possibly null reference.
//                .OnDelete(DeleteBehavior.NoAction);
//        }

        /// <summary>
        /// Import registrations from JSON file
        /// </summary>
        /// <param name="registrationsFileName">Name of the file to import</param>
        /// <returns>
        /// Collection of all imported registrations
        /// </returns>
        public async Task<IEnumerable<Registration>> ImportRegistrations(string registrationsFileName)
        {
            string registrationsJson = await File.ReadAllTextAsync(registrationsFileName);
            var registrations = JsonSerializer.Deserialize<IEnumerable<Registration>>(registrationsJson);
                        
            using var transaction = await Database.BeginTransactionAsync();            
            await Registrations.AddRangeAsync(registrations);
            await SaveChangesAsync();
            await transaction.CommitAsync();            

#pragma warning disable CS8603 // Possible null reference return.
            return registrations;
#pragma warning restore CS8603 // Possible null reference return.
        }

        /// <summary>
        /// Delete everything (registrations, vaccinations)
        /// </summary>
        public async Task DeleteEverything()
        {
            using var transaction = await Database.BeginTransactionAsync();
            await Database.ExecuteSqlRawAsync("DELETE FROM Registrations");
            await Database.ExecuteSqlRawAsync("DELETE FROM Vaccinations");
            await transaction.CommitAsync();
        }

        /// <summary>
        /// Get registration by social security number (SSN) and PIN
        /// </summary>
        /// <param name="ssn">Social Security Number</param>
        /// <param name="pin">PIN code</param>
        /// <returns>
        /// Registration result or null if no registration with given SSN and PIN was found.
        /// </returns>
        public async Task<GetRegistrationResult?> GetRegistration(long ssn, int pin)
        {
            var reg = await Registrations//.Include(r => r.Vaccination)
                .Where(r => r.SocialSecurityNumber == ssn && r.PinCode == pin)
                .SingleOrDefaultAsync();
            if (reg.FirstName != "")
            {// If valid value was found
                return new(reg.Id, ssn, reg.FirstName, reg.LastName);                
            }
            return null;            
        }

        /// <summary>
        /// Get available time slots on the given date
        /// </summary>
        /// <param name="date">Date (without time, i.e. time is 00:00:00)</param>
        /// <returns>
        /// Collection of all available time slots
        /// </returns>
        public async Task<IEnumerable<DateTime>> GetTimeslots(DateTime date)
        {
            var freeSLots = GenerateTimeslotsOnADay(date);
            // substract the slots already are in the DB.
            await Vaccinations.ForEachAsync(v => freeSLots.Remove(v.VaccinationDate));
 
            return freeSLots;
        }

        private List<DateTime> GenerateTimeslotsOnADay(DateTime date)
        {
            List<DateTime> timeSlots = new();

            var startTime = date.AddHours(8);// 8 AM
            
            timeSlots.Add(startTime);
            var endTime = startTime.AddMinutes(165);// 10:45 AM

            while (timeSlots.Last()< endTime)
            {
                var newSlot = timeSlots.Last().Add(TimeSpan.FromMinutes(15));
                timeSlots.Add(newSlot);
            }

            return timeSlots;
        }

        /// <summary>
        /// Store a vaccination
        /// </summary>
        /// <param name="vaccination">Vaccination to store</param>
        /// <returns>
        /// Stored vaccination after it has been written to the database.
        /// </returns>
        /// <remarks>
        /// If a vaccination with the given vaccination.RegistrationID already exists,
        /// overwrite it. Otherwise, insert a new vaccination.
        /// </remarks>
        public async Task<Vaccination> StoreVaccination(StoreVaccination vaccination)
        {
            var vaccinationEntry = await Vaccinations.Where(v => v.RegistrationId == vaccination.RegistrationId).SingleOrDefaultAsync();
            
            if (vaccinationEntry == null) // Not yet in DB            
                vaccinationEntry = new Vaccination { RegistrationId = vaccination.RegistrationId, VaccinationDate = vaccination.Datetime };            
            else
                vaccinationEntry.VaccinationDate = vaccination.Datetime;

            Vaccinations.Update(vaccinationEntry);
            await SaveChangesAsync();

            return vaccinationEntry;
        }
    }
}
