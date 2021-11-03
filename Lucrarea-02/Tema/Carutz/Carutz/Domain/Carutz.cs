using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Carutz.Domain.CarutzStates;

namespace Carutz.Domain
{
    public class Carut
    {
        private static readonly Random random = new Random();
        private List<UnValidatedItem> List { get; }
        public ICarutzStates state;
        public Carut()
        {
           
            List = new() ;
            state = new CarutzStates.EmptyCarutzState();


        }

        public void pay(string adress)
        {
            //var paidlist = new List<PaidItem>();
            //foreach(UnPaidItem x in List)
            {
               // paidlist.Add(new PaidItem(x.item, adress));
            }
            //state = new CarutzStates.PaidCarutzState(paidlist, DateTime.Now);

            PublishCarutzCommand command = new(List);
            CarutzWorkflow workflow = new CarutzWorkflow();
            var result = workflow.Execute(command, (registrationNumber) => true);

            result.Match(
                    whenCarutzPublishFaildEvent: @event =>
                    {
                        Console.WriteLine($"Publish failed: {@event.Reason}");
                        return @event;
                    },
                    whenCarutzPublishScucceededEvent: @event =>
                    {
                        Console.WriteLine($"Publish succeeded.");
                        Console.WriteLine(String.Join(@event.Adress,@event.PublishedDate.ToString(),@event.Sum.ToString()));
                        return @event;
                    }
                );


        }
        public Carut(List<UnValidatedItem> list)
        {
            List = list;
            state = new CarutzStates.UnValidatedCarutzState(list);
        }

        public void addItem(UnValidatedItem x)
        {
            List.Add(x);
            state = new CarutzStates.UnValidatedCarutzState(List);
        }

        public void removeItem(UnValidatedItem x)
        {
            List.Remove(x);
            state = new CarutzStates.UnValidatedCarutzState(List);
        }
        public override string ToString()
        {
            var strlis = "";
            foreach (UnValidatedItem element in List)
            {
                strlis += element.item.ToString();
            }
            return strlis;
        }

        
    }
}
