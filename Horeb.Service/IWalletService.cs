using Horeb.Domain.WalletModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horeb.Service
{
    public interface IWalletService
    {
        Wallet FindWallet(string id);
        List<Wallet> FindWalletByName(string nameToMatch);
        List<Wallet> GetAllWallets();
        Wallet CreateWallet(string walletName);
        bool DeleteWallet(string id);
        bool SaveWallet(Wallet wallet);
    }
}
