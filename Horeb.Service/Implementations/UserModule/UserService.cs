using System;
using Horeb.Pesistency;
using Horeb.Domain.UserModule;
using Samba.Presentation.Services;
using Horeb.Infrastructure.Data;
using Horeb.Infrastructure.Data.Security;
using Horeb.Domain.Common;

namespace Horeb.Service.UserModule
{
    /// <summary>Validates user credentials and manages user settings. This class cannot be inherited.</summary>
    public class UserService : IUserService
    {
        private IUserDao _userDao;
        private IApplicationState _applicationState;
        private IApplicationStateSetter _applicationStateSetter;

        public UserService(IUserDao userDao, IApplicationState applicationState, IApplicationStateSetter applicationStateSetter)
        {
            _userDao = userDao;
            _applicationState = applicationState;
            _applicationStateSetter = applicationStateSetter;
        }

        /// <summary>Gets the information from the data source and updates the last-activity date/time stamp for the current logged-on Horeb user.</summary>
        /// <returns>A <see cref="T:Horeb.Domain.UserModule.HorebUser" /> object representing the current logged-on user.</returns>
        /// <exception cref="T:System.ArgumentException">No Horeb user is currently logged in.</exception>    
        public HorebUser GetUser()
        {
            HorebUser user = HorebUser.Empty;
            if (IsUserLoggedIn())
            {
                user = _userDao.Find(_applicationState.CurrentLoggedInUser);
                _userDao.UpdateUserActivity(user.Id);   
            }                                
            else
                throw new ArgumentException("No Horeb user is currently logged in.");
            return user;
        }

        /// <summary>Gets a collection of all the users in the database.</summary>
        /// <returns>A <see cref="T:Horeb.Domain.UserModule.HorebUserCollection" /> 
        /// of <see cref="T:Horeb.Domain.UserModule.HorebUser" /> objects representing all of the users in the database.</returns>
        public HorebUserCollection GetAllUsers()
        {
            return _userDao.GetAll(0,int.MaxValue);
        }

        /// <summary>Gets a collection of all the users in the database in pages of data.</summary>
        /// <param name="pageIndex">The index of the page of results to return. Use 0 to indicate the first page.</param>
        /// <param name="pageSize">The size of the page of results to return. <paramref name="pageIndex" /> is zero-based.</param>
        /// <param name="totalRecords">The total number of users.</param>
        /// <returns>A <see cref="T:Horeb.Domain.UserModule.HorebUserCollection" /> of <see cref="T:Horeb.Domain.UserModule.HorebUser" /> objects representing all the users in the database for the configured <see langword="applicationName" />.</returns>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="pageIndex" /> is less than zero.-or-
        /// <paramref name="pageSize" /> is less than 1.</exception>
        public HorebUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            if (pageIndex < 0)
                throw new ArgumentException(SuperResource.GetString("PageIndex_bad"), nameof(pageIndex));
            if (pageSize < 1)
                throw new ArgumentException(SuperResource.GetString("PageSize_bad"), nameof(pageSize));
            totalRecords = _userDao.CountTotalUsers();
            return _userDao.GetAll(pageIndex * pageSize, pageSize);
        }

        /// <summary>Creates a user and returns an Horeb user from the Horeb data source.</summary>
        /// <param name="username">The user name (username) to create.</param>
        /// <param name="password">The password for the user</param>
        /// <returns> If successful return the newly created user. Returns an Emtpy user otherwise.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        public HorebUser CreateUser(string username, string password, out CreateUserStatus status)
        {
            if (!SecureUtility.ValidateParameter(ref username, true, true, true, 0))
            {
                status = CreateUserStatus.InvalidUserName;
                return HorebUser.Empty;
            }
            if (!SecureUtility.ValidatePasswordParameter(ref password, 0))
            {
                status = CreateUserStatus.InvalidPassword;
                return HorebUser.Empty;
            }
            if (_userDao.DoesUserExist(username))
            {
                status = CreateUserStatus.DuplicateUserName;
                return HorebUser.Empty;
            }
            status = CreateUserStatus.Success;
            return _userDao.Insert(username, password);
        }

        /// <summary>Updates a user to the Horeb data source.</summary>
        /// <param name="user">The user (HorebUser) to update.</param>
        /// <exception cref="T:System.ArgumentNullException">
        public void UpdateUser(HorebUser user)
        {
            if (user == null || user == HorebUser.Empty)
                throw new ArgumentNullException(nameof(user));
            string email = user.Email;
            SecureUtility.CheckParameter(ref email, true, true, true, 256, nameof(email));
            user.Email = email;
            _userDao.Update(user);
        }

        /// <summary>Gets an Horeb users where the user name contains the specified username to match.</summary>
        /// <param name="userName">The user name to search for.</param>
        /// <returns>A <see cref="T:Horeb.Domain.UserModule.HorebUser" /> that contains all users that match the <paramref name="userName" /> parameter.</returns>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="userName" /> is an empty string.</exception>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="userName" /> is <see langword="null" />.</exception>
        public HorebUser FindUser(string userName)
        {
            SecureUtility.CheckParameter(ref userName, true, true, true, 256, nameof(userName));
            return _userDao.Find(userName);
        }

        /// <summary>Gets an Horeb users where the e-mail address contains the specified e-mail address to match.</summary>
        /// <param name="emailToMatch">The e-mail address to search for.</param>
        /// <returns>A <see cref="T:Horeb.Domain.UserModule.HorebUser" /> that contains all users that match the <paramref name="emailToMatch" />
        /// parameter.Leading and trailing spaces are trimmed from the <paramref name="emailToMatch" /> parameter value.</returns>
        public HorebUser FindUserByEmail(string emailToMatch)
        {
            SecureUtility.CheckParameter(ref emailToMatch, true, false, false, 256, nameof(emailToMatch));
            return _userDao.FindByEmail(emailToMatch);
        }

        /// <summary>Logs in a the given Horeb user and updates the last-activity date/time stamp if sucessful.</summary>
        /// <param name="username">The user name to log-in.</param>
        /// <param name="logInStatus">The status of the log-in action.</param>
        /// <returns> True if the user was logged on successfully. False otherwise.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        public bool Login(string username, out LoginStatus logInStatus)
        {
            SecureUtility.CheckParameter(ref username, true, false, true, 256, nameof(username));
            if (!_userDao.DoesUserExist(username))
            {
                logInStatus = LoginStatus.UserDoesNotExist;
                return false;
            }
            HorebUser user = _userDao.Find(username);
            _userDao.UpdateUserActivity(user.Id);
            _applicationStateSetter.SetCurrentLoggedInUser(username);
            logInStatus = LoginStatus.LoggedIn;
            return true;
        }

        /// <summary>Logs in a the given Horeb user and updates the last-activity date/time stamp if sucessful.</summary>
        /// <param name="username">The user name (username) to log-in.</param>
        /// <param name="password">The password to match to the user to log-in</param>
        /// <param name="logInStatus">The status of the log-in action.</param>
        /// <returns> True if the user was logged on successfully. False otherwise.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        public bool Login(string username, string password, out LoginStatus logInStatus)
        {
            SecureUtility.CheckParameter(ref username, true, false, true, 256, nameof(username));
            SecureUtility.CheckPasswordParameter(ref password, 15, nameof(username));
            if (_userDao.DoesUserExist(username))
            {
                logInStatus = LoginStatus.UserDoesNotExist;
                return false;
            }
            HorebUser user = _userDao.Find(username);
            if (!_userDao.FindUserPassword(user.Id).Equals(password))
            {
                logInStatus = LoginStatus.InvalidPassword;
                return false;
            }
            return  Login(username, out logInStatus);            
        }

        /// <summary>Logs out the current logged in user if exist one.</summary>
        public void Logout()
        {
            _applicationStateSetter.SetCurrentLoggedInUser(string.Empty);
        }

        /// <summary> Tells if a user was logged on or not.</summary>
        /// <returns> True if the there is a Horeb user was logged-in . False otherwise.</returns>
        public bool IsUserLoggedIn()
        {
            string userName = _applicationState.CurrentLoggedInUser;
            return !string.IsNullOrWhiteSpace(userName)
                && _userDao.DoesUserExist(userName);
        }

        /// <summary>Gets the number of users currently accessing an application.</summary>
        /// <returns>The number of users currently accessing an application.</returns>
        public int GetNumberOfUsersOnline()
        {
            return _userDao.GetNumberOfUsersOnline();
        }

        /// <summary> Tells if a user exist on the data source.</summary>
        /// <returns> True if the there is a Horeb user on the Horeb data source. False otherwise.</returns>
        public bool DoesUserExist(string username)
        {
            return _userDao.DoesUserExist(username);
        }
    }
}
