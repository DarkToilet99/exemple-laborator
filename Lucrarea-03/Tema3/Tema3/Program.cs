using System;
using Tema3.Models;
using System.Threading.Tasks;
using LanguageExt;
using static LanguageExt.Prelude;
using System.Collections.Generic;

namespace Tema3
{
    class Program
    {
        private static readonly Random random = new Random();
        static async Task Main(string[] args)
        {
            //var testNumber = StudentRegistrationNumber.TryParse("LM12345");
            //var studentExists = await testNumber.Match(
            //    Some: testNumber => CheckStudentExists(testNumber).Match(Succ: value => value, exception=>false),
            //    None: () => Task.FromResult(false)
            //);

            //var result = from studentNumber in StudentRegistrationNumber.TryParse("LM12345")
            //                                        .ToEitherAsync(() => "Invlid student registration number.")
            //             from exists in CheckStudentExists(studentNumber)
            //                                        .ToEither(ex =>
            //                                        {
            //                                            Console.Error.WriteLine(ex.ToString());
            //                                            return "Could not validate student reg. number";
            //                                        })
            //             select exists;

            //await result.Match(
            //     Left: message => Console.WriteLine(message),
            //     Right: flag => Console.WriteLine(flag));



            var listOfGrades = ReadListOfGrades().ToArray();
            PublishCarutzCommand command = new(listOfGrades);
            CarutzWorkflow workflow = new();
            var result = await workflow.ExecuteAsync(command, CheckItenExists);

            result.Match(
                    whenCarutzPublishFaildEvent: @event =>
                    {
                        Console.WriteLine($"Publish failed: {@event.Reason}");
                        return @event;
                    },
                    whenCarutzPublishScucceededEvent: @event =>
                    {
                        Console.WriteLine($"Publish succeeded.");
                        Console.WriteLine(@event.PublishedDate);
                        Console.WriteLine(@event.Adress);
                        Console.WriteLine(@event.Csv);
                        Console.WriteLine($"Total: {@event.Sum}");
                        return @event;
                    }
                );
        }

        private static List<UnValidatedItem> ReadListOfGrades()
        {
            List<UnValidatedItem> listOfGrades = new();
            do
            {
                //read registration number and grade and create a list of greads
                var id = ReadValue("Id: ");
                if (string.IsNullOrEmpty(id))
                {
                    break;
                }

                var qty = ReadValue("Quantity: ");
                if (string.IsNullOrEmpty(qty))
                {
                    break;
                }

                listOfGrades.Add(new(id, qty));
            } while (true);
            return listOfGrades;
        }

        private static string? ReadValue(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }

        private static TryAsync<Item> CheckItenExists(string id)
        {
            Func<Task<Item>> func = async () =>
            {
                //HttpClient client = new HttpClient();

                //var response = await client.PostAsync($"www.university.com/checkRegistrationNumber?number={student.Value}", new StringContent(""));

                //response.EnsureSuccessStatusCode(); //200

                
                var max_qty = 0;
                var price = 0;
                if (random.Next(0, 100) < 90)// no db so if the item exists is totally random
                {
                    
                    price = random.Next(1, 100);
                    max_qty = random.Next(0, 40);
                }
                return new(id, max_qty, price);
            };
            return TryAsync(func);
        }
    }
}
