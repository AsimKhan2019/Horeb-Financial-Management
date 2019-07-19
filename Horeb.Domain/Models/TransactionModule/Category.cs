using Horeb.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horeb.Domain.TransactionModule
{
    public class Category : BaseEntity
    {
        public Category(string name)
        {
            InitializeEntity();
            Name = name;
        }

        public CategoryType CategoryType { get; set; }

        public string WalletId { get; set; }

        private static readonly Category _empty = new Category("Default Category");
        public static Category Empty { get { return _empty; } }
    }
}
