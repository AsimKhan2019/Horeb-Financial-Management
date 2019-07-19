using Horeb.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horeb.Domain.WalletModule
{
    public class Wallet : BaseEntity
    {
        public Wallet(string name)
        {
            InitializeEntity();
            Name = name;
        }

        public Decimal Amount { get; set; }

        public bool IsActive { get; set; }

        public string Description { get; set; }

        private static readonly Wallet _empty = new Wallet("Default Wallet");
        public static Wallet Empty { get { return _empty; } }
    }
}
