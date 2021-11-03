using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharp.Choices;


namespace Carutz.Domain
{
    [AsChoice]

    public static partial class CarutzStates
    {
        public interface ICarutzStates { }

        public record EmptyCarutzState() : ICarutzStates;

        public record InvalidatedCarutzState(IReadOnlyCollection<UnPaidItem> ItemList, string reason) : ICarutzStates;

        public record ValidatedCarutzState(IReadOnlyCollection<UnPaidItem> ItemList) : ICarutzStates;

        public record PaidCarutzState(IReadOnlyCollection<PaidItem> ItemList, DateTime PayDate) : ICarutzStates;
    }
}

