using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tema3.Models
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
