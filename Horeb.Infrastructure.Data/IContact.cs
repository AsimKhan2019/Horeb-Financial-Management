using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horeb.Infrastructure.Data
{
    public interface IContact
    {
        string Email { get; set; }
        string PhoneNumber { get; set; }
    }
}
