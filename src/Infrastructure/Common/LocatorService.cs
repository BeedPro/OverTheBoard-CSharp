using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverTheBoard.Infrastructure.Common
{
    public class LocatorService<TInterface, TObject> : ILocatorService<TInterface, TObject> where TInterface : ILocatorInterface<TObject>
    {
        private readonly IEnumerable<TInterface> _enumerable;
        public LocatorService(IEnumerable<TInterface> enumerable)
        {
            _enumerable = enumerable;
        }

        public TInterface Get(TObject parameter)
        {
            var item = _enumerable.FirstOrDefault(e => e.CanSelect(parameter, false)); 

            if (item == null)
            {
                item = _enumerable.FirstOrDefault(e => e.CanSelect(parameter, true));
            }

            return item;
        }


    }

}
