using RefactorThis.Persistence.Models;
using System.Threading.Tasks;

namespace RefactorThis.Persistence.Interfaces
{
    public interface IInvoiceRepository : IRepository<Invoice>
    {
        /// <summary>
        /// Get an Invoice by its reference.
        /// </summary>
        /// <param name="reference"></param>
        /// <returns></returns>
        Task<Invoice> GetInvoiceByReferenceAsync(string reference);
    }
}
