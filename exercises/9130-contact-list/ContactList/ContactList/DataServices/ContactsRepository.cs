using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactList.DataServices
{
    public record Contact(int Id, string FirstName, String LastName, string Email);
    public class ContactsRepository : IContactsRepository
    {
        private List<Contact> Contacts { get; } = new();

        public List<Contact> GetAllContacts() => Contacts;

        public List<Contact> FindContactByName(string filter)
        {
            if (string.IsNullOrEmpty(filter) && (filter.Any(f => char.IsDigit(f)) == true)) throw new ArgumentException();

            return Contacts.Where(c => c.FirstName.Contains(filter) || c.LastName.Contains(filter)).ToList();
        }

        public Contact AddContact(Contact contact)
        {
            if ( string.IsNullOrEmpty(contact.FirstName) || string.IsNullOrEmpty(contact.LastName) || string.IsNullOrEmpty(contact.Email)) throw new ArgumentException();
            Contacts.Add(contact);
            return contact;
        }

        public bool DeleteContact(int id)
        {
            if (id < 1) throw new ArgumentException("Id is invalid.");

            // false if not found
            return Contacts.Remove(Contacts.FirstOrDefault(c => c.Id == id));
        }

        public Contact GetContactById(int id)
        {
            return Contacts.FirstOrDefault(c => c.Id == id);
        }
    }
}
