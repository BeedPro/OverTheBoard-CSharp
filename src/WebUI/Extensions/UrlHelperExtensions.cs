using System;
using Microsoft.AspNetCore.Mvc;

namespace OverTheBoard.WebUI.Extensions
{
    public static class UrlHelperExtensions
    {
        public static string GetQuery(this IUrlHelper helper, string key)
        {
            if (helper == null)
            {
                throw new ArgumentNullException(nameof(helper));
            }

            return helper.ActionContext?.HttpContext?.Request?.Query[key] ?? string.Empty;
        }
        
        public static string GetRoute(this IUrlHelper helper, string key)
        {
            if (helper == null)
            {
                throw new ArgumentNullException(nameof(helper));
            }

            object value = string.Empty;
            if ((helper.ActionContext?.HttpContext?.Request.RouteValues.TryGetValue(key, out value) ?? false) && value != null)
            {
                return value.ToString();
            }

            return string.Empty;
        }
    }
}
