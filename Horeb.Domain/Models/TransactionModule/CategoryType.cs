using Horeb.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horeb.Domain.TransactionModule
{
    public class CategoryType : BaseEntity
    {
        public CategoryType(string name)
        {
            InitializeEntity();
            Name = name;
        }

        public bool IsActive { get; set; }

        private static readonly CategoryType _empty = new CategoryType("Default CategoryType");
        public static CategoryType Empty { get { return _empty; } }
    }
}
