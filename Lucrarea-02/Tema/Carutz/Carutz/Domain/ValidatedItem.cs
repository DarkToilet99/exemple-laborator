using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carutz.Domain
{
    public record ValidatedItem(Item item, decimal price)
    {

    };
}
