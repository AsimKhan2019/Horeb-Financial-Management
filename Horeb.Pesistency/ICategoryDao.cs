using Horeb.Domain.TransactionModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horeb.Pesistency
{
    public interface ICategoryDao
    {
        Category Insert(string category);
        void Update(Category category);
        bool Delete(string categoryId);
        Category Find(string categoryId);
        Category FindByName(string name);
        IEnumerable<Category> FindByType(string CategoryTypeId);
    }
}
