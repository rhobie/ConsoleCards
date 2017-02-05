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
        public static int RunningPlayerCount;
        public static int NPCtotal = 3;

        public static bool PlayersHaveCards = false;

        List<NPC> AllPlayers = new List<NPC>();

        public GameLoop()
        {
            //create player:
            //var player = new Player();

            //create NPCs
            for (int i = 0; i < NPCtotal; i++)
            {
                AllPlayers.Add(new NPC());
            }

            //create dealer and deck
            var dealer = new Dealer(0);
            dealer.CreateDeck();

            Debug.ShowCards("THE DEALER", dealer.dealerDeck.Cards);

            //dealer shuffles
            dealer.ShuffleDeck();

            Debug.ShowCards("THE DEALER", dealer.dealerDeck.Cards);

            //dealer deals
            int CardsPerPlayer = dealer.dealerDeck.Cards.Count / NPCtotal;
            dealer.Deal(AllPlayers, CardsPerPlayer);

            for (int i = 0; i < RunningPlayerCount; i++)
            {
                AllPlayers[i].RevealHand();
            }

            Debug.ShowCards("THE DEALER", dealer.dealerDeck.Cards);

            Console.WriteLine("GAME START");

            while (PlayersHaveCards)
            {
                CheckRemainingCards(AllPlayers);

                for (int i = 0; i < AllPlayers.Count; i++)
                {
                    AllPlayers[i].Play();
                }
                DiscardPile.ShowTopCard();
            }

            Console.WriteLine("GAME OVER");
        }

        public void CheckRemainingCards(List<NPC> allPlayers)
        {
            if (DiscardPile.Cards.Count >= Deck.DeckRules.CardCount)
            {
                PlayersHaveCards = false;
            }
        }
    }

    class NPC
    {
        public int Id;
        public List<Card> Hand { get; set; }
        public bool hasCards = false;

        public NPC()
        {
            //change to delegate later
            GameLoop.RunningPlayerCount++;
            Id = GameLoop.RunningPlayerCount;

            Hand = new List<Card>();
        }

        public int SelectCardFromHand()
        {
            int index;
            //Card SelectedCard;

            Card TopDiscard = DiscardPile.GetTopCard();
            if (TopDiscard == null && Hand.Contains(Hand.Find(x => x.GetId() == "StartingCard")))
            {
                index = Hand.FindIndex(x => x.GetId() == "StartingCard");
            }
            else
            {
                //need to complete this:
                index = 0;// new Card(Suit.clubs, Value.ace);
            }

            return index;
        }

        public void Play()
        {
            if (Hand.Count != 0)
            {
                int index = SelectCardFromHand();

                if (Hand[index] == null)
                {
                    Console.WriteLine("NPC {0} PASSES", Id);
                }
                else
                {
                    Console.WriteLine("NPC {0} Plays {1} of {2}", Id.ToString(), Hand[index].Value, Hand[index].Suit);
                   
                    DiscardPile.Cards.Add(Hand[index]);
                    Hand.RemoveAt(index);
                }

            }
        }

        public void RevealHand()
        {
            Debug.ShowCards("NPC " + Id.ToString(), Hand);
        }
    }

    class DiscardPile
    {
        public static int CardCount = 0;

        private static List<Card> cards = new List<Card>();
        public static List<Card> Cards { get => cards; set => cards = value; }

        public static Card GetTopCard()
        {
            if (cards.Count == 0)
            {
                return null;
            }
            else
            {
                Card topCard = Cards[Cards.Count - 1];
                return topCard;
            }

        }

        public static void ShowTopCard()
        {
            Card topCard = Cards[Cards.Count - 1];
            Console.WriteLine("\nCurrent Card: {0} of {1}", topCard.Value, topCard.Suit);
        }
    }


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

            //set id of three of clubs to "StartingCard"
            Cards[0].Id = "StartingCard";

            Console.WriteLine("\nDECK GENERATED.");

            return Cards;
        }

        public void Shuffle()
        {
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

