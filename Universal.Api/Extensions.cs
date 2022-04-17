using System;
using System.IO;
using System.Security.Claims;
using System.Security.Principal;
using Universal.Api.Data;

namespace Universal.Api
{
    public static class Extensions
    {

        public static byte[] ReadFully(this Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
