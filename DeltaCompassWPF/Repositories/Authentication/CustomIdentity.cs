using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace DeltaCompassWPF.Repositories.Authentication
{
    public class CustomIdentity : IIdentity
    {
        public CustomIdentity(string name, int id) 
        {
            Name = name;
            Id = id;
        }

        public string AuthenticationType => "Custom";
        public bool IsAuthenticated => true;
        public string Name { get; }
        public int Id { get; }
    }
}
