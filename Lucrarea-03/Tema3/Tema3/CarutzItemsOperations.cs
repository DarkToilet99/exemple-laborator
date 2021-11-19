using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tema3.Models.CarutzStates;
using Tema3.Models;
using System.Threading.Tasks;
using System.Linq;
using LanguageExt;
using static LanguageExt.Prelude;

namespace Tema3
{
    public static class CarutzItemsOperations
    {
        private static readonly Random random = new Random();
        public static Task<ICarutzStates> ValidateCarutzItems(Func<string, TryAsync<Item>> checkItemExists, UnValidatedCarutzState carutzItems) =>
            carutzItems.ItemList
                      .Select(ValidateItem(checkItemExists))//returneaza practic o lista,select ii ca un for each care merge si aplica functia de validare pe fiecare element
                      .Aggregate(CreateEmptyValatedGradesList().ToAsync(), ReduceValidGrades)
                      .MatchAsync(
                            Right: validatedGrades => new ValidatedCarutzState(validatedGrades),
                            LeftAsync: errorMessage => Task.FromResult((ICarutzStates)new InvalidatedCarutzState(carutzItems.ItemList, errorMessage))
                      );

        private static Func<UnValidatedItem, EitherAsync<string, ValidatedItem>> ValidateItem(Func<string, TryAsync<Item>> checkItemExists) =>
           unvalidatedItem => ValidateItem(checkItemExists, unvalidatedItem);

        private static EitherAsync<string, ValidatedItem> ValidateItem(Func<string, TryAsync<Item>> checkItemExists, UnValidatedItem unvalidatedItem) =>
            from ItemId in Item.TryParseId(unvalidatedItem.id)
                                   .ToEitherAsync(() => $"Invalid id pattern ({unvalidatedItem.id}, {unvalidatedItem.quantity})")
            from ItemReturned in checkItemExists(ItemId)
                                   .ToEither(error => error.ToString())
            from ItemExists in Item.TryParseItem(ItemReturned)
                                    .ToEitherAsync(() => $"Item not available at this moment ({unvalidatedItem.id}, {unvalidatedItem.quantity})")
            from ItemQty in Item.TryParseQty(unvalidatedItem.quantity, ItemExists.Quantity)
                                   .ToEitherAsync(() => $"Invalid quantity ({unvalidatedItem.id}, {unvalidatedItem.quantity})")
                
            select new ValidatedItem(new Item(ItemId, ItemQty, ItemExists.Price));

        private static Either<string, List<ValidatedItem>> CreateEmptyValatedGradesList() =>
           Right(new List<ValidatedItem>());

        private static EitherAsync<string, List<ValidatedItem>> ReduceValidGrades(EitherAsync<string, List<ValidatedItem>> acc, EitherAsync<string, ValidatedItem> next) =>
            from list in acc
            from nextGrade in next
            select list.AppendValidItem(nextGrade);

        private static List<ValidatedItem> AppendValidItem(this List<ValidatedItem> list, ValidatedItem validItem)
        {
            list.Add(validItem);
            return list;
        }

        public static ICarutzStates CalculateSum(ICarutzStates itemSums) => itemSums.Match(
            whenEmptyCarutzState: emptyCarutz => emptyCarutz,
            whenUnValidatedCarutzState: unvalidatedCarutz => unvalidatedCarutz,
            whenInvalidatedCarutzState: invalidCarutz => invalidCarutz,
            whenCalculatedTotalSum: calculatedCarutz => calculatedCarutz,
            whenPaidCarutzState: paidCarutz => paidCarutz,
            whenValidatedCarutzState: validCarutzItems =>
            {
                var calculatedSum = validCarutzItems.ItemList.Select(validItem => new ItemSum(validItem.item, validItem.item.Quantity * validItem.item.Price));
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
                csv = calculatedCarutz.ItemList.Aggregate(new StringBuilder(), (export, item) => export.AppendLine($"{item.item.Item_code}, {item.item.Quantity}, {(item.calculatedPrice / item.item.Quantity)}, , {item.calculatedPrice}"));

                List<PaidItem> listy = new();
                var total = Decimal.Zero;
                foreach (var itemSum in calculatedCarutz.ItemList)
                {
                    listy.Add(new PaidItem(itemSum.item, itemSum.calculatedPrice));
                    total += itemSum.calculatedPrice;
                }

                PaidCarutzState publishedCarutzItems = new(listy, adress, DateTime.Now, total, csv.ToString());

                return publishedCarutzItems;
            });
    }
}
