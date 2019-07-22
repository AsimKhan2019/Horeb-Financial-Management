using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horeb.Service
{
    public enum CreateUserStatus
    {
        //
        // Summary:
        //     The user was successfully created.
        Success = 0,

        //
        // Summary:
        //     The user name was not found in the data source.
        InvalidUserName = 1,

        //
        // Summary:
        //     The password is not formatted correctly.
        InvalidPassword = 2,

        //
        // Summary:
        //     The e-mail address is not formatted correctly.
        InvalidEmail = 3,

        //
        // Summary:
        //     The e-mail address already exists in the database for the application.
        DuplicateEmail = 4,

        //
        // Summary:
        //     The user name already exists in the database for the application.
        DuplicateUserName = 5,
    }
}
