using Horeb.Domain.UserModule;
using Horeb.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horeb.Domain.UserModule
{
    /// <summary>Exposes Horeb user information in the Horeb data store.</summary>
    public class HorebUser : BaseEntity, IContact
    {
        public HorebUser(string userName)
        {
            InitializeEntity();
            Id = userName;
        }

        /// <summary>Gets or sets the email address for the  user .</summary>
        /// <returns>The email address for the  user .</returns>
        public string Email { get; set; }


        /// <summary>Gets or sets the phone number for the user.</summary>
        /// <returns>The phone number for the user.</returns>
        public string PhoneNumber { get; set; }

        /// <summary>Gets or sets the sex type for the user .</summary>
        /// <returns>The sex type for the  user .</returns>
        public Gender Gender { get; set; }

        /// <summary>Gets or sets whether the  ser  is active.</summary>
        /// <returns>true if the user is active; otherwise, false.</returns>
        public bool IsActive { get; set; }

        /// <summary>Gets whether the user is currently online.</summary>
        /// <returns>true if the user is online; otherwise, false.</returns>
        /// <exception cref="T:System.PlatformNotSupportedException">This method is not available. This can occur if the application targets the .NET Framework 4 Client Profile. To prevent this exception, override the method, or change the application to target the full version of the .NET Framework.</exception>
        public virtual bool IsOnline
        {
            get
            {
                return this.LastActivityDate.ToUniversalTime() > DateTime.UtcNow.Subtract(new TimeSpan(0, 15, 0));
            }
        }

        /// <summary>Gets or sets the date and time when the  user  was last authenticated or accessed the application.</summary>
        /// <returns>The date and time when the  user  was last authenticated or accessed the application.</returns>
        public virtual DateTime LastActivityDate { get; set; }

        /// <summary>Gets the date and time when the  user 's password was last updated.</summary>
        /// <returns>The date and time when the  user 's password was last updated.</returns>
        public virtual DateTime LastPasswordChangedDate { get; set; }

        /// <summary>Represents an empty  user . This field is read-only.</summary>
        private static readonly HorebUser _nobody = new HorebUser("*");
        public static HorebUser Empty { get { return _nobody; } }
    }
}
