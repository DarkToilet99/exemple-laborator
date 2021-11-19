using System;
using System.Collections.Generic;
using Tema3.Models;
using static Tema3.CarutzItemsOperations;
using static Tema3.Models.PublishCarutzEvent;
using static Tema3.Models.CarutzStates;
using LanguageExt;
using System.Threading.Tasks;


namespace Tema3
{
    public class CarutzWorkflow
    {
        public async Task<ICarutzPublishedEvent> ExecuteAsync(PublishCarutzCommand command, Func<string, TryAsync<Item>> checkItemExists)
        {
            UnValidatedCarutzState unvalidatedItems = new UnValidatedCarutzState(command.InputItems);
            ICarutzStates items = await ValidateCarutzItems(checkItemExists, unvalidatedItems);
            items = CalculateSum(items);
            Console.Write("Address where to ship items:");
            var adr = Console.ReadLine();
            items = PublishCarutzItems(items, adr);

            return items.Match(
                    whenEmptyCarutzState: emptyCarutz => new CarutzPublishFaildEvent("Unexpected empty state") as ICarutzPublishedEvent,
                    whenUnValidatedCarutzState: unvalidcarutz => new CarutzPublishFaildEvent("Unexpected unvalidated state"),
                    whenInvalidatedCarutzState: invalidcarutzz=> new CarutzPublishFaildEvent(invalidcarutzz.Reason),
                    whenValidatedCarutzState: validCarutz => new CarutzPublishFaildEvent("Unexpected validated state"),
                    whenCalculatedTotalSum: calculatedCarutz => new CarutzPublishFaildEvent("Unexpected calculated state"),
                    whenPaidCarutzState: paidCarutz => new CarutzPublishScucceededEvent(paidCarutz.TotalPaid,paidCarutz.Adress, paidCarutz.PayDate,paidCarutz.Csv)
                );
        }
    }
}
