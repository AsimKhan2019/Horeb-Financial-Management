using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Horeb.Domain.Common
{
    public enum LoginStatus
    {
        LoggedIn,
        InvalidPassword,
        UserDoesNotExist,
    }
}