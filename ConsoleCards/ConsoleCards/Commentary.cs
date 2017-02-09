using System;
using System.Collections.Generic;

namespace ConsoleCards
{
    public class Commentary
    {
        public static void ShowCards(List<Card> Cards)
        {
            Console.WriteLine("\n ? IS SHOWING ALL CARDS...");

            foreach (var Card in Cards)
            {
                Console.WriteLine(Card.name);
            }
        }

        public static void ShowCards(string who, List<Card> Cards)
        {
            Console.WriteLine("\n {0} IS SHOWING ALL CARDS...", who);

            int num = 0;
            foreach (var Card in Cards)
            {
                num++;
                Console.WriteLine(" #{0,2} T:{1,3} {2} {3}", num, Card.tier, Card.shorthand, Card.name);
            }
        }
    }
}

