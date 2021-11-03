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
        private List<UnPaidItem> List { get; }
        public ICarutzStates state;
        public Carut()
        {
           
            List = new() ;
            state = new CarutzStates.EmptyCarutzState();


        }

        public void pay(string adress)
        {
            var paidlist = new List<PaidItem>();
            foreach(UnPaidItem x in List)
            {
                paidlist.Add(new PaidItem(x.item, adress));
            }    
            state = new CarutzStates.PaidCarutzState(paidlist, DateTime.Now);
        }
        public Carut(List<UnPaidItem> list)
        {
            List = list;
            state = new CarutzStates.ValidatedCarutzState(list);
        }

        public void addItem(UnPaidItem x)
        {
            List.Add(x);
            state = new CarutzStates.ValidatedCarutzState(List);
        }

        public void removeItem(UnPaidItem x)
        {
            List.Remove(x);
            state = new CarutzStates.ValidatedCarutzState(List);
        }
        public override string ToString()
        {
            var strlis = "";
            foreach (UnPaidItem element in List)
            {
                strlis += element.item.ToString();
            }
            return strlis;
        }

        
    }
}
