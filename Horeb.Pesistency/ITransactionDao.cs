using Horeb.Domain.TransactionModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horeb.Pesistency
{
    public interface ITransactionDao
    {
        Transaction Insert(string transaction);
        void Update(Transaction transaction);
        bool Delete(string transactionId);
        Transaction FindById(string Id);
        IEnumerable<Transaction> FindByDateRange(string s_startDate, string s_endDate);
        IEnumerable<Transaction> FindByWalletId(string walletid);
        IEnumerable<Transaction> FindByAmount(Decimal amount);
        IEnumerable<Transaction> FindByCategoryId(string categoryId);
        IEnumerable<Transaction> FindByDescription(string description);
        IEnumerable<Transaction> GetAll();
        IEnumerable<Transaction> GetAll(int startAt, int range);
    }
}
