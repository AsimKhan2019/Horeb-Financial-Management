using System;
using System.Collections.Generic;
using Horeb.Domain.Common;
using Horeb.Domain.UserModule;
using Horeb.Service.UserModule;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Horeb.Service.Test
{
    [TestClass]
    public class UserServiceTest
    {
        private IUserService _userService;
        private HorebUser[] _userArray;            
        private HorebUser _user;

        [TestInitialize]
        public void InitializeTestClass()
        {
            _userService = new UserService(new MockUserDao(),
                new MockApplicationState(),
                new MockApplicationState());
        }

        [TestMethod]
        public void ShouldLogInUser()
        {
            GivenAUser();
            WhenAUserLogsIn();
            ThenUserShouldBeLoggedIn();
        }

        [TestMethod]
        public void ShouldLogOutUser()
        {
            GivenAUser();
            WhenAUserLogsOut();
            ThenUserShouldBeLoggedOut();
        }



        private void GivenAUser()
        {
            _user = new HorebUser("ArturoD93")
            {
                CreatedById = "Administrator26",
                LastestUpdateById = "AnotherAdmin78",
                Email = "fakeEmail@fake.com",
                PhoneNumber = "8587744185",
                IsActive = true,
                LastActivityDate = DateTime.Now
            };
        }

        private void WhenAUserLogsIn()
        {
            _userService.Login(_user.Id, out LoginStatus loginStatus);
        }

        private void WhenAUserLogsOut()
        {
            WhenAUserLogsIn();
            _userService.Logout();
        }

        private void ThenUserShouldBeLoggedIn()
        {
            Assert.IsTrue(_userService.IsUserLoggedIn());
            Assert.AreEqual(_userService.GetUser().Id, _user.Id);
        }

        private void ThenUserShouldBeLoggedOut()
        {

        }
    }
}
