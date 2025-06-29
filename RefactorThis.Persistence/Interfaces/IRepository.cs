using System.Collections.Generic;
using System.Threading.Tasks;

namespace RefactorThis.Persistence.Interfaces
{
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Get all Entities of Type T
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Get Entity of Type T by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> GetByIdAsync(int id);

        /// <summary>
        /// Add Entity of Type T
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task AddAsync(T entity);

        /// <summary>
        /// Update Entity of Type T
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task UpdateAsync(T entity);

        /// <summary>
        /// Delete Entity of Type T by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteByIdAsync(int id);
    }
}
