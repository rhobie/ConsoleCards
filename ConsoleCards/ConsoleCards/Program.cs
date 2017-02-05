using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleCards
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new GameLoop();

            Console.ReadLine();
        }
    }

    class GameLoop
    {
        public GameLoop()
        {
            var myDeck = new Deck();
            var discardPile = new DiscardPile();

            myDeck.ShowAllCards();

            myDeck.Shuffle();

            myDeck.ShowAllCards();
        }
    }
    class Hand
    {
        private List<Card> cards = new List<Card>();
        internal List<Card> Cards { get => cards; set => cards = value; }

    }


    class DiscardPile
    {
        private int cardCount = 0;

        private List<Card> cards = new List<Card>();
        internal List<Card> Cards { get => cards; set => cards = value; }

        public DiscardPile()
        {

        }

    }

    class Deck
    {
        static class DeckRules
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

            for (int i = 0; i < DeckRules.JokerCount; i++)
            {
                Cards.Add(new Card(Suit.wild, Value.joker));
            }

            Console.WriteLine("\nDECK GENERATED.");

            return Cards;
        }

        public void ShowAllCards()
        {
            Console.WriteLine("\nSHOWING ALL CARDS...");

            foreach (var Card in Cards)
            {
                Console.WriteLine("{0,6} of {1}", Card.Value.ToString(), Card.Suit.ToString());
            }

        }

        public void Shuffle()
        {
            Console.WriteLine("\nSHUFFLING...");
            Cards.Shuffle();
        }

    }

    static class MyExtensions
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }


    }
    public static class ThreadSafeRandom
    {
        [ThreadStatic] private static Random Local;

        public static Random ThisThreadsRandom
        {
            get { return Local ?? (Local = new Random(unchecked(Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId))); }
        }
    }

}

