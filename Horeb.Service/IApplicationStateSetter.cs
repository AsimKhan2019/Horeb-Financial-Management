using System.Collections.Generic;
using Horeb.Domain.TransactionModule;
using Horeb.Domain.UserModule;

namespace Samba.Presentation.Services
{
    public interface IApplicationStateSetter
    {
        void SetCurrentLoggedInUser(string userName);        
        void SetCurrentTransaction(Transaction transaction);
        void SetApplicationLocked(bool isLocked);
    }
}