using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharp.Choices;

namespace Carutz.Domain
{
    [AsChoice]
    public static partial class PublishCarutzEvent
    {
        public interface ICarutzPublishedEvent { }

        public record CarutzPublishScucceededEvent : ICarutzPublishedEvent
        {
            public string Adress { get; }
            public decimal Sum { get; }
            public DateTime PublishedDate { get; }

            internal CarutzPublishScucceededEvent(decimal sum,string adress, DateTime publishedDate)
            {
                Adress = adress;
                PublishedDate = publishedDate;
                Sum = sum;
            }
        }

        public record CarutzPublishFaildEvent : ICarutzPublishedEvent
        {
            public string Reason { get; }

            internal CarutzPublishFaildEvent(string reason)
            {
                Reason = reason;
            }
        }
    }
   
}
