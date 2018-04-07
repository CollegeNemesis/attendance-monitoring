using SJBCS.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SJBCS.Services.Repository
{
    public class ContactsRepository : IContactsRepository
    {
        AmsModel _context = ConnectionHelper.CreateConnection();

        public Contact AddContact(Contact Contact)
        {
            _context.Contacts.Add(Contact);
            _context.SaveChanges();
            return Contact;
        }

        public void DeleteContact(Guid id)
        {
            var Contact = _context.Contacts.FirstOrDefault(r => r.ContactID == id);
            if (Contact != null)
            {
                _context.Contacts.Remove(Contact);
            }
            _context.SaveChanges();
        }

        public Contact GetContact(Guid id)
        {
            return _context.Contacts.FirstOrDefault(r => r.ContactID == id);
        }

        public List<Contact> GetContacts()
        {
            return _context.Contacts.ToList();
        }

        public Contact UpdateContact(Contact Contact)
        {
            if (!_context.Contacts.Local.Any(r => r.ContactID == Contact.ContactID))
            {
                _context.Contacts.Attach(Contact);
            }
            _context.Entry(Contact).State = EntityState.Modified;
            _context.SaveChanges();
            return Contact;
        }
    }
}
