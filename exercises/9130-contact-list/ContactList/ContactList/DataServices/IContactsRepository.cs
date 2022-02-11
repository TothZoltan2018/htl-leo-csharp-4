using System.Collections.Generic;

namespace ContactList.DataServices
{
    public interface IContactsRepository
    {
        Contact AddContact(Contact contact);
        bool DeleteContact(int id);
        List<Contact> FindContactByName(string filter);
        List<Contact> GetAllContacts();
        Contact GetContactById(int id);
    }
}