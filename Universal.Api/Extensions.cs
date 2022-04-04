using System;
using System.Security.Principal;

namespace Universal.Api
{
    public static class Extensions
    {
        public static bool IsCustomer(this IPrincipal principal)
        {
            return principal.IsInRole("Customer");
        }

        public static bool IsAgent(this IPrincipal principal)
        {
            return principal.IsInRole("Agent");
        }

    }
}
