using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OverTheBoard.Data;

namespace OverTheBoard.UnitTests
{
    internal class ContextMock
    {
        public static ApplicationDbContext GetApplicationDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
