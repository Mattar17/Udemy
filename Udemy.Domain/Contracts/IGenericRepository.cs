using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Udemy.Domain.Contracts
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(ISpecificationBase<T> specification);
        Task<T> GetByIdAsync (ISpecificationBase<T> specification);
        Task<T> GetById(string id);
        Task<T> GetByIntId(int id);
        Task Add(T item);
        void Update(T item);
        void Delete(T item);
    }
}
