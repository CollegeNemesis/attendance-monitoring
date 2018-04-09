using SJBCS.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SJBCS.Services.Repository
{
    public class ContactsRepository : IContactsRepository
    {
        AmsModel _context;

        public Contact AddContact(Contact Contact)
        {
            using (_context = ConnectionHelper.CreateConnection())
            {
                _context.Contacts.Add(Contact);
                _context.SaveChanges();

                return Contact;
            }
        }

        public void DeleteContact(Guid id)
        {
            using (_context = ConnectionHelper.CreateConnection())
            {
                var Contact = _context.Contacts.FirstOrDefault(r => r.ContactID == id);
                _context.Entry(Contact).State = EntityState.Deleted;
                _context.SaveChanges();
            }
        }

        public Contact GetContact(Guid id)
        {
            using (_context = ConnectionHelper.CreateConnection())
            {
                var Contact = _context.Contacts
                    .Include(contact => contact.Student)
                    .FirstOrDefault(r => r.ContactID == id);

                return Contact;
            }
        }

        public List<Contact> GetContacts()
        {
            using (_context = ConnectionHelper.CreateConnection())
            {
                var Contacts = _context.Contacts
                    .Include(contact => contact.Student)
                    .ToList();

                return Contacts;
            }
        }

        public Contact UpdateContact(Contact Contact)
        {
            using (_context = ConnectionHelper.CreateConnection())
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
}
