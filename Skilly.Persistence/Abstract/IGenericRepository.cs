﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skilly.Persistence.Abstract
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync (string id);
        Task AddAsync (T entity);
        Task UpdateAsync (T entity);
        Task DeleteAsync (string id);


    }
}
