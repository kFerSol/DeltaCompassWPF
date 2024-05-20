using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace DeltaCompassWPF.Repositories.Authentication
{
    public class CustomPrincipal : IPrincipal
    {
        public CustomPrincipal(CustomIdentity identity)
        {
            Identity = identity;
        }

        public IIdentity Identity { get; }
        public bool IsInRole(string roleName) => false;

        public CustomIdentity CustomIdentity => (CustomIdentity)Identity;
    }
}
