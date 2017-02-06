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

            for (int i = 1; i <= rounds; i++)
            {
                var game = new Game();
            }
        }
    }

    public static class GameLists
    {
        public static List<NPC> PlayersInRound = new List<NPC>();
        public static List<NPC> ActivePlayers = new List<NPC>();
        public static List<NPC> InactivePlayers = new List<NPC>(); //not implemented but will need when rounds are in
    }

    public class Game
    {
        public static bool PlayersHaveCards = false;

        public Game()
        {
            //create NPCs
            for (int i = 0; i < PresidentsAndAssholes.NpcTotal; i++)
            {
                GameLists.ActivePlayers.Add(new NPC(GameLists.ActivePlayers.Count));
                GameLists.PlayersInRound.Add(GameLists.ActivePlayers[i]);
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
            dealer.Deal(GameLists.ActivePlayers, CardsPerPlayer);
            SortPlayerCards();

            ShowAllHands();

            Debug.ShowCards("THE DEALER", dealer.dealerDeck.Cards);

            Console.WriteLine("GAME START");

            StartGameLoop();

            ShowAllHands();

            Console.WriteLine("GAME OVER");
        }

        public void StartGameLoop()
        {
            while (PlayersHaveCards)
            {
                //ROUND:
                if (GameLists.PlayersInRound.Count > 1)
                {
                    for (int i = 0; i < GameLists.PlayersInRound.Count; i++)
                    {
                        GameLists.PlayersInRound[i].Play();
                    }
                }
                else
                {
                    //NEW ROUND:
                    Console.WriteLine("\nNEW ROUND:");
                    ResetPlayersInRound();
                    MoveRoundPileToDiscardPile();
                }
                CheckRemainingCards();
            }
            
        }

        public void MoveRoundPileToDiscardPile()
        {
            foreach (var card in RoundPile.Cards)
            {
                DiscardPile.Cards.Add(card);
            }
            RoundPile.Cards.Clear();
        }

        public void ResetPlayersInRound()
        {
            //need to make the player who won the round start the next round but still keep the same play order
            //GameLists.PlayersInRound.Clear();
            foreach (var player in GameLists.ActivePlayers)
            {
                if (!GameLists.PlayersInRound.Contains(player) && player.Hand.Count != 0)
                {
                    GameLists.PlayersInRound.Add(player);
                }
                if (player.Hand.Count == 0)
                {
                    GameLists.InactivePlayers.Add(player);
                }
            }
            //remove inactive players from active player list
            foreach (var player in GameLists.InactivePlayers)
            {
                if (GameLists.ActivePlayers.Contains(player))
                {
                    GameLists.ActivePlayers.Remove(player);
                }
            }
        }

        public void SortPlayerCards()
        {
            foreach (var player in GameLists.ActivePlayers)
            {
                player.SortCards();
            }
        }

        public void CheckRemainingCards()
        {
            PlayersHaveCards = false;
            foreach (var player in GameLists.ActivePlayers)
            {
                if (player.Hand.Count > 0)
                {
                    PlayersHaveCards = true;
                }
            }
        }

        public void ShowAllHands()
        {
            for (int i = 0; i < GameLists.ActivePlayers.Count; i++)
            {
                GameLists.ActivePlayers[i].RevealHand();
            }
        }
    }

    public class NPC
    {
        public int Id;
        public List<Card> Hand { get; set; }
        public bool hasCards = false;

        public NPC(int _activePlayers)
        {
            Id = _activePlayers + 1;
            Hand = new List<Card>();
        }

        public void SortCards()
        {
            Hand = Hand.OrderBy(x => x.Tier).ToList();
        }

        public int SelectCardFromHand()
        {
            int index;

            Card TopDiscard = DiscardPile.GetTopCard();
            Card TopRoundCard = RoundPile.GetTopCard();

            if (TopDiscard == null && TopRoundCard == null)
            {
                if (Hand.Contains(Hand.Find(x => x.Tier == 9))) //three of clubs is the starting card
                {
                    index = Hand.FindIndex(x => x.Tier == 9);
                }
                else
                {
                    //NPC does not pass before the round has started.
                    index = -9;
                }
            }
            else if (TopRoundCard != null && Hand.Contains(Hand.Find(x => x.Tier > TopRoundCard.Tier)))
            {
                //add next lot of decsion making here:
                index = Hand.FindIndex(x => x.Tier > TopRoundCard.Tier);
            }
            else if (TopRoundCard == null)
            {
                index = 0; //play first card in hand
            }
            else
            {
                index = -1;
            }

            return index;
        }

        public void Play()
        {
            if (Hand.Count != 0)
            {
                int index = SelectCardFromHand();

                if (index == -9)
                {
                    //First round, no starting card in hand (SKIP)
                }
                else if (index == -1)
                {
                    Pass();
                }
                else
                {
                    Console.WriteLine("NPC {0} Plays {1} of {2}", Id.ToString(), Hand[index].Value, Hand[index].Suit);

                    RoundPile.Cards.Add(Hand[index]);
                    Hand.RemoveAt(index);
                }
            }
        }

        public void Pass()
        {
            Console.WriteLine("NPC {0} PASSES", Id);
            GameLists.PlayersInRound.Remove(this);
        }

        public void RevealHand()
        {
            Debug.ShowCards("NPC " + Id.ToString(), Hand);
        }
    }

    class RoundPile //: DiscardPile
    {
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

    class DiscardPile
    {
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

