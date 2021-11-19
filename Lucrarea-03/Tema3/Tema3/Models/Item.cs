using LanguageExt;
using System;
using static LanguageExt.Prelude;
using System.Text.RegularExpressions;

namespace Tema3.Models
{
    public record Item
    {
        private static readonly Random random = new Random();
        private static readonly Regex ValidPattern = new("^i[0-9]{3}$");
        public string Item_code { get; }
        public decimal Quantity { get; set;}
        public decimal Price { get;}
        public Item(string item_code, decimal quantity, decimal price)
        {
            Quantity = quantity;
            Item_code = item_code;
            Price = price;
            //if(quantity >= 0)
            //    Quantity = quantity;
            //else
            //{
            //    throw new WrongCode("Invalid Quantity");
            //}

            //if (ValidPattern.IsMatch(item_code))
            //{
            //    Item_code = item_code;
            //}
            //else
            //{
            //    throw new WrongCode("Invalid Code");
            //}

            //if (price >= 0)
            //    Price = price;
            //else
            //{
            //    throw new WrongCode("Invalid price");
            //}
            //if(Item.checked_id_in_db(Item_code, out var price, out var qty))//ar validare de produs s-ar face numai din exterior cu un tryparse care returneaza un option
            //{
            //    Price = price;
            //    if(qty < Quantity)
            //    {
            //        Quantity = qty;
            //    }
            //}
            //else
            //{
            //    throw new WrongCode("Invalid Item");
            //}
        }

        public static decimal operator +(Item a, Item b) => (a.Price*a.Quantity)+(b.Price*b.Quantity);

        //public static bool checked_id_in_db(string id, out decimal price, out decimal max_qty)
        //{
        //    bool isValid = false;
        //    max_qty = 0;
        //    price = 0;
        //    if (random.Next(0 , 100)<90)// no db so if the item exists is totally random
        //    {
        //        isValid = true;
        //        price = random.Next(1, 99999);
        //        max_qty = random.Next(0, 40);
        //    }

        //    return isValid;
        //}

        public static Option<decimal> TryParseQty(string quantity, decimal max_qty)
        {
            if (decimal.TryParse(quantity, out decimal numericQty) && IsValid(numericQty))
            {
                if(numericQty > max_qty)
                    return Some<decimal>(max_qty);
                else
                    return Some<decimal>(numericQty);
            }
            else
            {
                return None;
            }
        }
        public static Option<Item> TryParseItem(Item item)
        {
            if (item.Quantity != 0 && item.Price > 0.00m)
            {
                return item;
            }
            else
            {
                return None;
            }
        }

        public static Option<string> TryParseId(string id)
        {
            if (ValidPattern.IsMatch(id))
            {
                return Some<string>(id);
            }
            else
            {
                return None;
            }
        }
        public override string ToString()
        {
            return "Code: " + Item_code + "\n" + "Quantity: " + Quantity + "\n";
        }

        private static bool IsValid(decimal value) => value > 0.0m;
    }
}
