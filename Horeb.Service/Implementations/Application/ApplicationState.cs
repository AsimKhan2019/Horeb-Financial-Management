using Horeb.Domain.TransactionModule;
using Samba.Presentation.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horeb.Service.Implementations.Application
{
    public class ApplicationState : IApplicationState, IApplicationStateSetter
    {
        private string _currentLoggedInUser;
        private Transaction _currentTransaction;
        private bool _isLocked;

        public ApplicationState()
        {
            ResetState();
        }

        public string CurrentLoggedInUser
        {
            get { return _currentLoggedInUser; }
        }

        public Transaction CurrentTransaction
        {
            get { return _currentTransaction; }
        }

        public bool IsLocked
        {
            get { return _isLocked; }
        }

        public void ResetState()
        {
            _currentLoggedInUser = string.Empty;
            _currentTransaction = Transaction.Empty;
            _isLocked = false;
        }

        public void SetApplicationLocked(bool isLocked)
        {
            _isLocked = isLocked;
        }

        public void SetCurrentLoggedInUser(string userName)
        {
            _currentLoggedInUser = userName;
        }

        public void SetCurrentTransaction(Transaction transaction)
        {
            _currentTransaction = transaction;
        }
    }
}
