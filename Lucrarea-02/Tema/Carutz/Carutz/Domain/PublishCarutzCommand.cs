using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carutz.Domain
{
    public record PublishCarutzCommand
    {
        public PublishCarutzCommand(IReadOnlyCollection<UnValidatedItem> inputItems)
        {
            InputItems = inputItems;
        }

        public IReadOnlyCollection<UnValidatedItem> InputItems { get; }
    }
}
