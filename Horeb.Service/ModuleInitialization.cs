using Horeb.Service.Implementations.Application;
using Samba.Presentation.Services;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horeb.Service
{
    public class ModuleInitialization : Registry
    {
        public ModuleInitialization()
        {
            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();

            });

            For<IApplicationState>().Use<ApplicationState>();
            For<IApplicationStateSetter>().Use<ApplicationState>();
        }
        
    }
}
