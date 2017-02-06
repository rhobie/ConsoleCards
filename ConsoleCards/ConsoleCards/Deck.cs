using System;
using System.Collections.Generic;

namespace ConsoleCards
{
    class Deck
    {
        public static class DeckRules
        {
            public const int CardCount = 54;
            public const int SuitCount = 3; // (zero based)
            public const int ValueCount = 12; // (zero based) //including jokers 13
            public const int JokerCount = 2;
        }

        private List<Card> cards = new List<Card>();
        internal List<Card> Cards { get => cards; set => cards = value; }

        public Deck()
        {
            Cards = GenerateDeck();
        }

        public List<Card> GenerateDeck()
        {
            Console.WriteLine("\nGENERATING DECK...");

            var Cards = new List<Card>();

            int suitPosition = 0;
            int valuePosition = 0;
            while (suitPosition <= DeckRules.SuitCount)
            {
                while (valuePosition <= DeckRules.ValueCount)
                {
                    Cards.Add(new Card((Suit)suitPosition, (Value)valuePosition));
                    valuePosition++;
                }
                valuePosition = 0;
                suitPosition++;
            }

            //add jokers:
            for (int i = 0; i < DeckRules.JokerCount; i++)
            {
                Cards.Add(new Card(Suit.wild, Value.joker));
            }

            Console.WriteLine("\nDECK GENERATED.");

            return Cards;
        }

        public void Shuffle()
        {
            Cards.Shuffle();
        }
    }

}

