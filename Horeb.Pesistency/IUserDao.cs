using Horeb.Domain.UserModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horeb.Pesistency
{
    public interface IUserDao
    {
        HorebUser Insert(string username, string password);
        void Update(HorebUser user);
        bool Delete(string userId);    
        HorebUser Find(string userName);
        HorebUser FindByEmail(string email);
        HorebUserCollection FindByName(string name);
        HorebUserCollection GetAll();
        HorebUserCollection GetAll(int startAt, int range);
        int CountTotalUsers();
        bool DoesUserExist(string userName);
        string FindUserPassword(string userId);
        void UpdateUserActivity(string userId);
        int GetNumberOfUsersOnline();
    }
}
