using Horeb.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horeb.Domain.TransactionModule
{
    public class Transaction : BaseEntity
    {
        public Transaction()
        {
            InitializeEntity();
        }

        public Decimal Amount { get; set; }
        
        public string Description { get; set; }

        public string WalletId { get; set; }

        public string CategoryId { get; set; }

        public DateTime TransactionDate { get; set; }

        private static readonly Transaction _empty = new Transaction();
        public static Transaction Empty { get { return _empty; } }
    }
}
