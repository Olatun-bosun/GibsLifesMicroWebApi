using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Universal.Api.Data
{
    public class AuthContext
    {
        private readonly IHttpContextAccessor _context;

        public AuthUser User
        {
            get
            {
                var user = _context.HttpContext.User;

                if (user.IsInRole("CUST"))
                    return new CustomerUser(user);

                else if (user.IsInRole("AGENT"))
                    return new AgentUser(user);

                else if (user.IsInRole("APP"))
                    return new AppUser(user);

                else
                    return null; // Anonymous User
            }
        }

        public AuthContext(IHttpContextAccessor context)
        {
            _context = context;
        }
    }

    public abstract class AuthUser
    {
        public string AppId { get; }

        public AuthUser(ClaimsPrincipal user)
        {
            AppId = user.FindFirst(c => c.Type == "AppID").Value;
        }
    }

    public class AppUser: AuthUser
    {
        public AppUser(ClaimsPrincipal user) : base(user)
        {
        }
    }
    public class AgentUser : AuthUser
    {
        public string AgentId { get; }
        public string PartyId { get; }
        public AgentUser(ClaimsPrincipal user) : base(user)
        {
            AgentId = user.FindFirst(c => c.Type == "UserID").Value;
            PartyId = user.FindFirst(c => c.Type == "TableID").Value;
        }
    }
    public class CustomerUser : AuthUser
    {
        public string CustomerId { get; }
        public string InsuredId { get; }
        public CustomerUser(ClaimsPrincipal user) : base(user)
        {
            CustomerId = user.FindFirst(c => c.Type == "UserID").Value;
            InsuredId = user.FindFirst(c => c.Type == "TableID").Value;
        }
    }
}