using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharp.Choices;


namespace Tema3.Models
{
    [AsChoice]

    public static partial class CarutzStates
    {
        public interface ICarutzStates { }

        public record EmptyCarutzState() : ICarutzStates
        {
            
        }

        public record UnValidatedCarutzState : ICarutzStates
        {
            public UnValidatedCarutzState(IReadOnlyCollection<UnValidatedItem> itemList)
            {
                ItemList = itemList;
            }

            public IReadOnlyCollection<UnValidatedItem> ItemList { get; }
        }

        public record InvalidatedCarutzState: ICarutzStates
        {
            internal InvalidatedCarutzState(IReadOnlyCollection<UnValidatedItem> itemList, string reason)
            {
                ItemList = itemList;
                Reason = reason;
            }

            public IReadOnlyCollection<UnValidatedItem> ItemList { get; }
            public string Reason { get; }
        }

        public record ValidatedCarutzState: ICarutzStates
        {
            internal ValidatedCarutzState(IReadOnlyCollection<ValidatedItem> itemList)
            {
                ItemList = itemList;
            }
            public IReadOnlyCollection<ValidatedItem> ItemList { get; }
        }

        public record CalculatedTotalSum : ICarutzStates
        {
            internal CalculatedTotalSum(IReadOnlyCollection<ItemSum> itemList)
            {
                ItemList = itemList;
            }

            public IReadOnlyCollection<ItemSum> ItemList { get; }
        }
        public record PaidCarutzState : ICarutzStates
        {
            internal PaidCarutzState(IReadOnlyCollection<PaidItem> itemList, string adress, DateTime TimeStamp, decimal TOTAL, string csv)
            {
                ItemList = itemList;
                PayDate = TimeStamp;
                TotalPaid = TOTAL;
                Adress = adress;
                Csv = csv;
            }
            public IReadOnlyCollection<PaidItem> ItemList { get; }
            public DateTime PayDate { get; }
            public decimal TotalPaid { get; }
            public string Adress { get; }
            public string Csv { get; }
        }
    }
}

