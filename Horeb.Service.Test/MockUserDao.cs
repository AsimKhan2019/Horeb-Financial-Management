using Horeb.Domain.UserModule;
using Horeb.Pesistency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horeb.Service.Test
{
    public class MockUserDao : IUserDao
    {
        private HorebUserCollection _userCollection;

        public MockUserDao ()
        {
            _userCollection = new HorebUserCollection();
            HorebUser userOne = Insert("ricardoDuran1", "easyPass123");
            userOne.CreatedById = "rootUser";
            userOne.Email = "easyEmail@test.com";
            userOne.PhoneNumber = "6197201905";
            Update(userOne);

            HorebUser userTwo = Insert("mockUser9", "anotherEasyPass321");
            userOne.CreatedById = "rootUser";
            userOne.Email = "testEmail@easy.com";
            userOne.PhoneNumber = "8585617899";
            Update(userOne);
        }

        public int CountTotalUsers()
        {
            return _userCollection.Count;
        }

        public bool Delete(string userId)
        {
            _userCollection.Remove(userId);
            return true;
        }

        public bool DoesUserExist(string userName)
        {
            bool doesUserExist = false;
            foreach (HorebUser currentUser in _userCollection)
            {
                if (currentUser.Id.Equals(userName.Trim(), StringComparison.CurrentCultureIgnoreCase))
                    doesUserExist = true;
            }
            return doesUserExist;
        }

        public HorebUser Find(string userName)
        {
            HorebUser user = HorebUser.Empty;
            foreach (HorebUser currentUser in _userCollection)
            {
                if (currentUser.Id.Equals(userName.Trim(), StringComparison.CurrentCultureIgnoreCase))
                    user = currentUser;
            }
            return user;
        }

        public HorebUser FindByEmail(string email)
        {
            HorebUser user = HorebUser.Empty;
            foreach (HorebUser currentUser in _userCollection)
            {
                if (currentUser.Email.Equals(email.Trim(),StringComparison.CurrentCultureIgnoreCase))
                    user = currentUser;
            }
            return user;
        }

        public HorebUserCollection FindByName(string name)
        {
            HorebUserCollection collection = new HorebUserCollection();
            foreach (HorebUser currentUser in _userCollection)
            {
                if (currentUser.Name.Contains(name))
                    collection.Add(currentUser);
            }
            return collection;            
        }

        public string FindUserPassword(string userId)
        {
            return string.Empty;
        }

        public HorebUserCollection GetAll()
        {
            return _userCollection;
        }

        public HorebUserCollection GetAll(int startAt, int range)
        {
            HorebUserCollection collection = new HorebUserCollection();
            int loopCounter = 0;
            foreach (HorebUser currentUser in _userCollection)
            {
                if (loopCounter >= startAt && loopCounter < startAt + range)
                    collection.Add(currentUser);
            }
            return collection;
        }

        public int GetNumberOfUsersOnline()
        {
            int numberOfOnlineUsers = 0;
            foreach (HorebUser currentUser in _userCollection)
            {
                if (currentUser.IsOnline)
                    numberOfOnlineUsers++;   
            }
            return numberOfOnlineUsers;
        }

        public HorebUser Insert(string username, string password)
        {
            HorebUser user = new HorebUser(username);
            _userCollection.Add(user);
            return user;
        }

        public void Update(HorebUser user)
        {
            _userCollection.Remove(user.Id);
            _userCollection.Add(user);
        }

        public void UpdateUserActivity(string userId)
        {
            HorebUser user = HorebUser.Empty;
            bool wasUserFound = false;
            foreach (HorebUser currentUser in _userCollection)
            {
                if (currentUser.Id.Equals(userId))
                {
                    wasUserFound = true;
                    currentUser.LastActivityDate = DateTime.Now;
                    user = currentUser;
                }
            }
            if(wasUserFound)
                Update(user);
        }
    }
}
