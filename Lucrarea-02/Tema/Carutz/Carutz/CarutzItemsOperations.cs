using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Carutz.Domain.CarutzStates;
using Carutz.Domain;

namespace Carutz
{
    public static class CarutzItemsOperations
    {
        private static readonly Random random = new Random();
        public static ICarutzStates ValidateCarutzItems(Func<Item, bool> checkStudentExists, UnValidatedCarutzState carutzItems)
        {
            List<ValidatedItem> validatedItems = new();
            bool isValidList = true;
            string invalidReson = string.Empty;
            foreach (var item in carutzItems.ItemList)
            {
                if (Item.checked_id_in_db(item.item.Item_code, out decimal price, out decimal max_qty))
                {
                    invalidReson = $"Invalid item number ({item.item.Item_code})";
                    isValidList = false;
                    break;
                }
                if (max_qty == 0)
                {
                    invalidReson = $"Item not in stock";
                    isValidList = false;
                    break;
                }
                if (item.item.Quantity> max_qty)
                {
                    item.item.Quantity = max_qty;
                }

                ValidatedItem validItem = new(item.item, price);
                validatedItems.Add(validItem);
            }

            if (isValidList)
            {
                return new ValidatedCarutzState(validatedItems);
            }
            else
            {
                return new InvalidatedCarutzState(carutzItems.ItemList, invalidReson);
            }

        }

        public static ICarutzStates CalculateSum(ICarutzStates itemSums) => itemSums.Match(
            whenEmptyCarutzState: emptyCarutz => emptyCarutz,
            whenUnValidatedCarutzState: unvalidatedCarutz => unvalidatedCarutz,
            whenInvalidatedCarutzState: invalidCarutz => invalidCarutz,
            whenCalculatedTotalSum: calculatedCarutz => calculatedCarutz,
            whenPaidCarutzState: paidCarutz => paidCarutz,
            whenValidatedCarutzState: validCarutzItems =>
            {
                var calculatedSum = validCarutzItems.ItemList.Select(validItem =>new TotalSum(validItem.item,validItem.item.Quantity + validItem.price));
                return new CalculatedTotalSum(calculatedSum.ToList().AsReadOnly());
            }
        );

        public static ICarutzStates PublishCarutzItems(ICarutzStates carutzItems, string adress) => carutzItems.Match(
            whenEmptyCarutzState: emptyCarutz => emptyCarutz,
            whenUnValidatedCarutzState: unvalidatedCarutz => unvalidatedCarutz,
            whenInvalidatedCarutzState: invalidCarutz => invalidCarutz,
            whenValidatedCarutzState: validCarutzItems => validCarutzItems,
            whenPaidCarutzState: paidCarutz => paidCarutz,
            whenCalculatedTotalSum: calculatedCarutz =>
            {
                StringBuilder csv = new();
                calculatedCarutz.ItemList.Aggregate(csv, (export, item) => export.AppendLine($"{item.item.Item_code}, {item.item.Quantity}, {(item.calculatedPrice/item.item.Quantity)}, , {item.calculatedPrice}"));

                List<PaidItem> listy = new();
                var total = 0;
                foreach( var item in calculatedCarutz.ItemList)
                {
                    listy.Add(new PaidItem(item, adress));
                    total += ((int)item.calculatedPrice);
                }

                PaidCarutzState publishedCarutzItems = new(listy, DateTime.Now, total);

                return publishedCarutzItems;
            });
    }
}
