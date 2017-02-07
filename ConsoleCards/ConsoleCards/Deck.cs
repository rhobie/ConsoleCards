using System;
using System.Collections.Generic;

namespace ConsoleCards
{
    class Deck
    {
        public static class DeckRules
        {
            public const int cardCount = 54;
            public const int suitCount = 3; // (zero based)
            public const int valueCount = 12; // (zero based) //including jokers 13
            public const int jokerCount = 2;
        }

        private List<Card> _cards = new List<Card>();
        internal List<Card> Cards { get => _cards; set => _cards = value; }


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
            int id = 0;
            while (suitPosition <= DeckRules.suitCount)
            {
                while (valuePosition <= DeckRules.valueCount)
                {
                    Cards.Add(new Card(id.ToString(), (Suit)suitPosition, (Value)valuePosition));
                    valuePosition++;
                    id++;
                }
                valuePosition = 0;
                suitPosition++;
                id++;
            }

            //add jokers:
            for (int i = 0; i < DeckRules.jokerCount; i++)
            {
                id++;
                Cards.Add(new Card(id.ToString(), Suit.Wild, Value.Joker));
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

