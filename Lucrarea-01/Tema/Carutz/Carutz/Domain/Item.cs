using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Carutz.Domain
{
    public record Item
    {
        private static readonly Random random = new Random();
        private static readonly Regex ValidPattern = new("^i[0-9]{3}$");
        public string Item_code { get; }
        public decimal Quantity { get; }

        public Item(string item_code, decimal quantity)
        {
            var stoc = random.Next(50);
             
            if (quantity > 0 && quantity <= (stoc > 10 ? stoc : 10))
            {
                Quantity = quantity;
            }
            else
            {
                Quantity = stoc;
            }

            if (ValidPattern.IsMatch(item_code))
            {
                Item_code = item_code;
            }
            else
            {
                throw new WrongCode("Wrong Code! Not according to the pattern");
            }

        }
        public override string ToString()
        {
            return "Code: " + Item_code + "\n" + "Quantity: " + Quantity + "\n";
        }

       
    }
}
