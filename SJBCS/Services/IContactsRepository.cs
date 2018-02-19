using SJBCS.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SJBCS.Services
{
    interface IContactsRepository
    {
        Task<List<Contact>> GetContactsAsync();
        Task<Contact> GetContactAsync(Guid id);
        Task<Contact> AddContactAsync(Contact contact);
        Task<Contact> UpdateContactAsync(Contact contact);
        Task DeleteContactAsync(Guid contactId);
    }
}
