using System;
using System.Collections.Generic;

namespace ConsoleCards
{
    public class Debug
    {
        public static void ShowCards(List<Card> Cards)
        {
            Console.WriteLine("\n ? IS SHOWING ALL CARDS...");

            foreach (var Card in Cards)
            {
                Console.WriteLine("{0,6} of {1}", Card.Value.ToString(), Card.Suit.ToString());
            }
        }

        public static void ShowCards(string who, List<Card> Cards)
        {
            Console.WriteLine("\n {0} IS SHOWING ALL CARDS...",who);

            int num = 0;
            foreach (var Card in Cards)
            {
                num++;
                Console.WriteLine(" #{0,2} T:{1,3}  {2,6} of {3}", num, Card.Tier, Card.Value.ToString(), Card.Suit.ToString());
            }
        }
    }

}

