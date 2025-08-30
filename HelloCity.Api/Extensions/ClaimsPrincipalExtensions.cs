using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace HelloCity.Api.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// get subid from JWT claims
        /// </summary>
        public static string? GetSub(this ClaimsPrincipal user)
        {
            if (user == null) return null;

            return user.FindFirst("sub")?.Value
                ?? user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}