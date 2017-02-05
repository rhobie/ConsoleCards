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
            //add select game if there is ever more than one..
            var newGame = new PresidentsAndAssholes(1, 3);

            Console.ReadLine();
        }
    }

    class PresidentsAndAssholes
    {
        public static int NpcTotal { get => npcTotal; }
        private static int npcTotal;

        public PresidentsAndAssholes(int rounds, int _npcTotal)
        {
            npcTotal = _npcTotal;

            for (int i = 0; i <= rounds; i++)
            {
                
                var game = new GameLoop();
            }

        }

    }

    class GameLoop
    {
        //public static int RunningPlayerCount;
        public static bool PlayersHaveCards = false;

        List<NPC> ActivePlayers = new List<NPC>();
        List<NPC> InactivePlayers = new List<NPC>(); //not implemented but will need when rounds are in

        public GameLoop()
        {
            //create player:
            //var player = new Player();

            //create NPCs
            for (int i = 0; i < PresidentsAndAssholes.NpcTotal; i++)
            {
                ActivePlayers.Add(new NPC(ActivePlayers.Count));
            }

            //create dealer and deck
            var dealer = new Dealer(0);
            dealer.CreateDeck();

            Debug.ShowCards("THE DEALER", dealer.dealerDeck.Cards);

            //dealer shuffles
            dealer.ShuffleDeck();

            Debug.ShowCards("THE DEALER", dealer.dealerDeck.Cards);

            //dealer deals
            int CardsPerPlayer = dealer.dealerDeck.Cards.Count / PresidentsAndAssholes.NpcTotal;
            dealer.Deal(ActivePlayers, CardsPerPlayer);

            ShowAllHands();

            Debug.ShowCards("THE DEALER", dealer.dealerDeck.Cards);

            Console.WriteLine("GAME START");

            DiscardPile.ShowTopCard();

            //while (PlayersHaveCards)
            //{
            CheckRemainingCards(ActivePlayers);

            for (int i = 0; i < ActivePlayers.Count; i++)
            {
                ActivePlayers[i].Play();
            }
            DiscardPile.ShowTopCard();
            //}

            Console.WriteLine("GAME OVER");
        }

        public void CheckRemainingCards(List<NPC> allPlayers)
        {
            if (DiscardPile.Cards.Count >= Deck.DeckRules.CardCount)
            {
                PlayersHaveCards = false;
            }
        }

        public void ShowAllHands()
        {
            for (int i = 0; i < ActivePlayers.Count; i++)
            {
                ActivePlayers[i].RevealHand();
            }
        }
    }

    class NPC
    {
        public int Id;
        public List<Card> Hand { get; set; }
        public bool hasCards = false;

        public NPC(int _activePlayers)
        {
            Id = _activePlayers + 1;

            Hand = new List<Card>();
        }

        public int SelectCardFromHand()
        {
            int index;

            Card TopDiscard = DiscardPile.GetTopCard();
            if (TopDiscard == null)
            {
                //if (Hand.Contains(Hand.Find(x => x.GetId() == "StartingCard")))
                //{
                //    index = Hand.FindIndex(x => x.GetId() == "StartingCard");
                //}


                if (Hand.Contains(Hand.Find(x => (x.Value == 0 && x.Suit == 0)))) //three of clubs
                {
                    index = Hand.FindIndex(x => (x.Value == 0 && x.Suit == 0));
                }

                //if (Hand.Contains(Hand.Find(x => x.Tier == 9))) //three of clubs
                //{
                //    index = Hand.FindIndex(x => x.Tier == 9);
                //}

                else
                {

                    //change this so that NPC does not pass before the round has started.
                    index = -1;
                }
            }
            else
            {
                index = Hand.FindIndex(x => x.Value < TopDiscard.Value);
            }

            return index;
        }

        public void Play()
        {
            if (Hand.Count != 0)
            {
                int index = SelectCardFromHand();

                if (index == -1)
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
        //public static int CardCount = 0;

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
            if (Cards.Count != 0)
            {
                Card topCard = Cards[Cards.Count - 1];
                Console.WriteLine("\nCurrent Card: {0} of {1}", topCard.Value, topCard.Suit);
            }
            else
            {
                Console.WriteLine("\nCurrent Card: none");
            }
            
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

