using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SJBCS.Data;

namespace SJBCS.Services
{
    public class ContactsRepository : IContactsRepository
    {
        AmsDbContext _context = new AmsDbContext();

        public async Task<Contact> AddContactAsync(Contact contact)
        {
            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();
            return contact;
        }

        public async Task DeleteContactAsync(Guid contactId)
        {
            var contact = _context.Contacts.FirstOrDefault(c => c.ContactID == contactId);
            if (contact != null)
            {
                _context.Contacts.Remove(contact);
            }
            await _context.SaveChangesAsync();
        }

        public Task<Contact> GetContactAsync(Guid id)
        {
            return _context.Contacts.FirstOrDefaultAsync(c => c.ContactID == id);
        }

        public Task<List<Contact>> GetContactsAsync()
        {
            return _context.Contacts.ToListAsync();
        }

        public async Task<Contact> UpdateContactAsync(Contact contact)
        {
            if (!_context.Contacts.Local.Any(c => c.ContactID == contact.ContactID))
            {
                _context.Contacts.Attach(contact);
            }
            _context.Entry(contact).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return contact;
        }
    }
}
