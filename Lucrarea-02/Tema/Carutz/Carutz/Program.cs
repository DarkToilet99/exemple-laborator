using System;
using System.Collections.Generic;
using Carutz.Domain;


namespace Carutz
{
    class Program
    {
        static void Main(string[] args)
        {
            int opt=0;
            Carut cart1 = new Carut(new List<UnValidatedItem>());
            Random random = new Random();
            do
            {
                Console.WriteLine("1.Adauga Produs");
                Console.WriteLine("2.Sterge Produs");
                Console.WriteLine("3.Plateste");
                Console.WriteLine("4.Afisare");
                Console.WriteLine("0.Exit");
                opt = Int16.Parse(ReadValue("Opt:"));

                switch(opt)
                {
                    case 0:
                        Environment.Exit(0);
                        break;
                    case 1:
                        var cod = ReadValue("Cod:");
                        var cantit = ReadValue("Cantitate:");
                        cart1.addItem(new UnValidatedItem(new Item(cod, Int16.Parse(cantit))));

                        break;
                    case 2:
                        var cod2 = ReadValue("Cod:");
                        var cantit2 = ReadValue("Cantitate:");
                        cart1.removeItem(new UnValidatedItem(new Item(cod2, Int16.Parse(cantit2))));
                        break;
                    case 3:
                        var adress = ReadValue("Adress:");
                        cart1.pay(adress);
                        break;
                    case 4:
                        Console.WriteLine(cart1.ToString());
                        break;
                }

            } while (true);
        }
        private static string? ReadValue(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }
    }
}
