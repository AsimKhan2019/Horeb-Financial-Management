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
        private HorebUser _user;

        [TestInitialize]
        public void InitializeTestClass()
        {
            MockApplicationState applicationState = new MockApplicationState();
            _userService = new UserService(new MockUserDao(),
                applicationState,
                applicationState);
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
            _user = _userService.CreateUser("ArturoD93", "password123", out CreateUserStatus status);
            _user.CreatedById = "Administrator26";
            _user.LastestUpdateById = "AnotherAdmin78";
            _user.Email = "fakeEmail@fake.com";
            _user.PhoneNumber = "8587744185";
            _user.IsActive = true;
            _user.LastActivityDate = DateTime.Now;
            _userService.UpdateUser(_user);
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
            Assert.IsFalse(_userService.IsUserLoggedIn());
        }
    }
}
