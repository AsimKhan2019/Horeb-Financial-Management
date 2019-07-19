using Horeb.Domain.Common;
using Horeb.Domain.UserModule;

namespace Horeb.Service
{
    public interface IUserService
    {
        HorebUser FindUser(string userName);
        HorebUser FindUserByEmail(string emailToMatch);
        HorebUser GetUser();
        HorebUserCollection GetAllUsers();
        HorebUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords);
        HorebUser CreateUser(string username, string password);
        int GetNumberOfUsersOnline();
        bool Login(string username, out LoginStatus logInStatus);
        bool Login(string username, string password, out LoginStatus logInStatus);
        void Logout();
        bool IsUserLoggedIn();
    }
}