using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BerekenFeestdagen
{
    class Program
    {
        static int Main(string[] args)
        {
            // geen parameter
            if (args.Length == 0)
            {
                Console.WriteLine("----------------------------------------------------");
                Console.WriteLine("Voer 2 jaartallen in van 4 cijfers");
                Console.WriteLine("Usage: BerekenFeestdagen <van jaartal> <tot jaartal>");
                Console.WriteLine("----------------------------------------------------");

                // default = jaar en jaar+1
                args = new[] { DateTime.Now.Year.ToString(), ((DateTime.Now.Year)+1).ToString() };
                //return 1;
            }

            if (args.Length != 2)
            {
                Console.WriteLine("----------------------------------------------------");
                Console.WriteLine("Voer 2 jaartallen in van 4 cijfers");
                Console.WriteLine("Usage: BerekenFeestdagen <van jaartal> <tot jaartal>");
                Console.WriteLine("----------------------------------------------------");
                return 1;
            }

            // geen nummerieke parameter
            int jaar;
            int jaartot;
            bool test1 = int.TryParse(args[0], out jaar);
            bool test0 = int.TryParse(args[1], out jaartot);
            if (!test1 ^ !test1)
            {
                Console.WriteLine("----------------------------------------------------");
                Console.WriteLine("Voer een jaartal in (4 cijfers)");
                Console.WriteLine("Usage: BerekenFeestdagen <jaartal>");
                Console.WriteLine("----------------------------------------------------");
                return 1;
            }

            for (int i = jaar; i <= jaartot; i++)
            {

                // vaste dagen
                DateTime nieuwjaar = new DateTime(i, 1, 1);
                DateTime bevrijdingsdag = new DateTime(i, 5, 5);
                DateTime kerstmis = new DateTime(i, 12, 25);
                DateTime tweedekerstdag = new DateTime(i, 12, 26);
                DateTime koningsdag = new DateTime(i, 04, 27);

                // Als Koningsdag op zondag valt is het de dag ervoor
                if (koningsdag.DayOfWeek == DayOfWeek.Sunday)
                {
                    koningsdag = koningsdag.AddDays(-1);
                }

                // Reken paasZondag uit
                DateTime paasZondag = BerekenPaasZondag(i);

                // Reken de rest uit via een offset
                DateTime paasmaandag = paasZondag.AddDays(1); // 1 dag na pasen
                DateTime goedevrijdag = paasZondag.AddDays(-2); // 2 dag voor pasen
                DateTime hemelvaart = paasZondag.AddDays(39); // 39 dagen na pasen
                DateTime pinksteren = paasZondag.AddDays(49); // 10 dagen na OLH hemelvaart
                DateTime pinkstermaandag = paasZondag.AddDays(50); // 1 dag na pinksteren

                // Output
                Console.WriteLine($"Nieuwjaar: \t\t" + nieuwjaar.ToString("yyyy-MM-dd") + "\n" +
                                    $"Goede vrijdag: \t\t" + goedevrijdag.ToString("yyyy-MM-dd") + "\n" +
                                    $"Pasen: \t\t\t" + paasZondag.ToString("yyyy-MM-dd") + "\n" +
                                    $"Paasmaandag: \t\t" + paasmaandag.ToString("yyyy-MM-dd") + "\n" +
                                    $"Koningsdag: \t\t" + koningsdag.ToString("yyyy-MM-dd") + "\n" +
                                    $"Bevrijdingsdag: \t" + bevrijdingsdag.ToString("yyyy-MM-dd") + "\n" +
                                    $"Hemelvaart: \t\t" + hemelvaart.ToString("yyyy-MM-dd") + "\n" +
                                    $"Pinksteren: \t\t" + pinksteren.ToString("yyyy-MM-dd") + "\n" +
                                    $"Pinkstermaandag: \t" + pinkstermaandag.ToString("yyyy-MM-dd") + "\n" +
                                    $"Kerstmis: \t\t" + kerstmis.ToString("yyyy-MM-dd") + "\n" +
                                    $"2e Kerstdag: \t\t" + tweedekerstdag.ToString("yyyy-MM-dd") + "\n");
            }

            //Console.ReadLine();

            return 0;
        }

        // Berekening op basis van paschal cycle (https://en.wikipedia.org/wiki/Date_of_Easter)
        public static DateTime BerekenPaasZondag(int year)
        {
            int day = 0;
            int month = 0;

            int g = year % 19;
            int c = year / 100;
            int h = (c - (int)(c / 4) - (int)((8 * c + 13) / 25) + 19 * g + 15) % 30;
            int i = h - (int)(h / 28) * (1 - (int)(h / 28) * (int)(29 / (h + 1)) * (int)((21 - g) / 11));

            day = i - ((year + (int)(year / 4) + i + 2 - c + (int)(c / 4)) % 7) + 28;
            month = 3;

            if (day > 31)
            {
                month++;
                day -= 31;
            }

            return new DateTime(year, month, day);
        }
    }
}
