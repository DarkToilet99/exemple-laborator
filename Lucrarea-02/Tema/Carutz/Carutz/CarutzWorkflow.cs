using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carutz.Domain;
using static Carutz.CarutzItemsOperations;
using static Carutz.Domain.PublishCarutzEvent;
using static Carutz.Domain.CarutzStates;


namespace Carutz
{
    public class CarutzWorkflow
    {
        public ICarutzPublishedEvent Execute(PublishCarutzCommand command, Func<Item, bool> checkItemExists)
        {
            UnValidatedCarutzState unvalidatedItems = new UnValidatedCarutzState(command.InputItems);
            ICarutzStates grades = ValidateCarutzItems(checkItemExists, unvalidatedItems);
            grades = CalculateSum(grades);
            Console.Write("Address where to ship items:");
            var adr = Console.ReadLine();
            grades = PublishCarutzItems(grades,adr);

            return grades.Match(
                    whenEmptyCarutzState: emptyCarutz => new CarutzPublishFaildEvent("Unexpected empty state") as ICarutzPublishedEvent,
                    whenUnValidatedCarutzState: unvalidcarutz => new CarutzPublishFaildEvent("Unexpected unvalidated state"),
                    whenInvalidatedCarutzState: invalidcarutzz=> new CarutzPublishFaildEvent(invalidcarutzz.Reason),
                    whenValidatedCarutzState: validCarutz => new CarutzPublishFaildEvent("Unexpected validated state"),
                    whenCalculatedTotalSum: calculatedCarutz => new CarutzPublishFaildEvent("Unexpected calculated state"),
                    whenPaidCarutzState: paidCarutz => new CarutzPublishScucceededEvent(paidCarutz.TotalPaid,paidCarutz.ItemList.ElementAt(0).adress, paidCarutz.PayDate)
                );
        }
    }
}
