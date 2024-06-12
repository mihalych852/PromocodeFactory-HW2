using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain;
namespace PromoCodeFactory.DataAccess.Repositories
{
    public class InMemoryRepository<T>: IRepository<T> where T: BaseEntity
    {
        protected IEnumerable<T> Data { get; set; }

        public InMemoryRepository(IEnumerable<T> data)
        {
            Data = data;
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            return Task.FromResult(Data);
        }

        public Task<T> GetByIdAsync(Guid id)
        {
            return Task.FromResult(Data.FirstOrDefault(x => x.Id == id));
        }

        public Task CreateAsync(T objectData)
        {
            if (objectData == null) 
            {
                throw new ArgumentNullException();
            }

            if (!Data.Any(item => item.Id == objectData.Id))
            {
                Data = Data.Concat<T>(new[] { objectData });
                //Data.Append(objectData);
            }

            return Task.CompletedTask;
        }

        public Task UpdateAsync(T updatedObject, T newData)
        {
            if (updatedObject == null || newData == null)
            {
                throw new ArgumentNullException();
            }

            if (updatedObject is IMutableEntity<T> updatedData)
            {
                updatedData.Update(newData);
            }

            return Task.CompletedTask;
        }

        public Task DeleteAsync(Guid id)
        {
            Data = Data.Where(x => x.Id != id);

            return Task.CompletedTask;
        }
    }
}