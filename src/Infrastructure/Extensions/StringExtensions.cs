using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverTheBoard.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string target)
        {
            return string.IsNullOrEmpty(target);
        }

        public static Guid ToGuid(this string target)
        {
            return Guid.Parse(target);
        }
    }
}
