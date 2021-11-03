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
        public decimal Quantity { get; set;}

        public Item(string item_code, decimal quantity)
        {
            if(quantity > 0)
                Quantity = quantity;
            else
            {
                throw new WrongCode("We don't sell air~");
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

        public static bool checked_id_in_db(string id, out decimal price, out decimal max_qty)
        {
            bool isValid = false;
            price = 0;
            max_qty = 0;
            if (random.Next(0 , 100)<70)// no db so if the item exists is totally random
            {
                isValid = true;
                price = random.Next(1, 99999);
                max_qty = random.Next(0, 40);
            }

            return isValid;
        }
        public override string ToString()
        {
            return "Code: " + Item_code + "\n" + "Quantity: " + Quantity + "\n";
        }

       
    }
}
