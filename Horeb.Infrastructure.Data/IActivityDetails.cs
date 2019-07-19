using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horeb.Infrastructure.Data
{
    public interface IActivityDetails
    {
        DateTime CreatedOn { get; set;}

        DateTime LastestUpdateOn { get; set; }

        string CreatedById { get; set; }

        string LastestUpdateById { get; set; }
    }
}
