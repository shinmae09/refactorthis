using Microsoft.EntityFrameworkCore;
using RefactorThis.Persistence.Constants;
using RefactorThis.Persistence.Extensions;
using RefactorThis.Persistence.Interfaces;
using RefactorThis.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RefactorThis.Persistence
{
    public class InvoiceRepository : IInvoiceRepository
    {
        protected readonly DbContext _context;
        protected readonly DbSet<Invoice> _invoices;

        public InvoiceRepository(DbContext context)
        {
            _context = context.ThrowIfNull(nameof(context));
            _invoices = _context.Set<Invoice>();
        }

        public async Task<Invoice> GetInvoiceByReferenceAsync(string reference)
        {
            return await _invoices.FirstOrDefaultAsync(i => i.Reference.Equals(reference));
        }

        public async Task<IEnumerable<Invoice>> GetAllAsync()
        {
            return await _invoices.ToListAsync();
        }

        public async Task<Invoice> GetByIdAsync(int id)
        {
            return await _invoices.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task AddAsync(Invoice entity)
        {
            entity.ThrowIfNull(nameof(entity));

            await _invoices.AddAsync(entity);
        }

        public async Task UpdateAsync(Invoice entity)
        {
            entity.ThrowIfNull(nameof(entity));

            _invoices.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            var invoice = await GetByIdAsync(id);
            if (invoice == null)
            {
                throw new InvalidOperationException(string.Format(ValidationMessage.INVOICE_ID_NOT_FOUND, id));
            }

            _invoices.Remove(invoice);
            await _context.SaveChangesAsync();
        }
    }
}