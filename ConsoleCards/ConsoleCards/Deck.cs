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

        private List<Card> _cards = new List<Card>();
        internal List<Card> Cards { get => _cards; set => _cards = value; }

        public Deck()
        {
            Cards = GenerateDeck();
        }

        public List<Card> GenerateDeck()
        {
            Commentary.GeneratingDeck();

            var Cards = new List<Card>();

            int suitPosition = 0;
            int valuePosition = 0;
            int id = 0;
            while (suitPosition <= DeckRules.SuitCount)
            {
                while (valuePosition <= DeckRules.ValueCount)
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
            //for (int i = 1; i < DeckRules.JokerCount; i++)
            //{
            id++;
            Cards.Add(new Card(id.ToString(), Suit.Red, Value.Joker));
            id++;
            Cards.Add(new Card(id.ToString(), Suit.Black, Value.Joker));
            //}
            //TotalValueDebug();

            return Cards;
        }

        public void Shuffle() //deck can shuffle itself.. why even have a dealer? I should refactor dealer to here at some point
        {
            Cards.Shuffle();
        }
        public void TotalValueDebug() // debugging method for calculating the new total value of the deck after adjustments to card.tier
        {
            int total = 0;
            foreach (var card in Cards)
            {
                total += card.GetTeir();
            }
            Console.WriteLine(total);
            Console.ReadLine();
        }
    }


}

