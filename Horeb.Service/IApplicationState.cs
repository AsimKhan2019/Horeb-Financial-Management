using Horeb.Domain.TransactionModule;
using Horeb.Domain.UserModule;
using System.Collections.Generic;

namespace Samba.Presentation.Services
{

    public interface IApplicationState
    {
        string CurrentLoggedInUser { get; }
        Transaction CurrentTransaction { get; }
        bool IsLocked { get; }
        void ResetState();
    }
}
