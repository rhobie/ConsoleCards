using System;
using System.Collections.Generic;
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

    public class Game
    {
        public static bool PlayersHaveCards = false;

        public Game()
        {
            //create NPCs
            for (int i = 0; i < PresidentsAndAssholes.NpcTotal; i++)
            {
                GameLists.SeatingPlan.Add(new NPC(GameLists.SeatingPlan.Count));
                GameLists.PlayersInRound.Add(GameLists.SeatingPlan[i]);
            }

            //Dealer shuffles deck
            var dealer = new Dealer(0);
            dealer.CreateDeck();
            dealer.ShuffleDeck();

            //Dealer Deals all cards to players
            int CardsPerPlayer = dealer.dealerDeck.Cards.Count / PresidentsAndAssholes.NpcTotal;
            dealer.Deal(GameLists.SeatingPlan, CardsPerPlayer);
            
            //Players Sort their hands
            SortPlayerHands();

            ShowAllHands();

            Console.WriteLine("GAME START");

            StartGameLoop();

            ShowAllHands();

            Console.WriteLine("GAME OVER");
        }

        public void StartGameLoop()
        {
            var discardPileMain = new DiscardPile();
            var discardPileRound = new RoundDiscardPile();

            int roundWinnerIndex;

            while (PlayersHaveCards)
            {
                //ROUND:
                var roundLoop = new Round();
                if (GameLists.PlayersInRound.Count > 1)
                {
                    foreach (var player in GameLists.SeatingPlan)
                    {
                        if (GameLists.PlayersInRound.Contains(player))
                        {
                            Card cardToPlay = player.SelectCardFromHand(discardPileMain.GetTopCard(), discardPileRound.GetTopCard());


                            //int selectedCard = player.SelectCardFromHand(discardPileMain.GetTopCard(), discardPileRound.GetTopCard());
                            Console.WriteLine("NPC {0} Plays {1}", player.Id.ToString(), cardToPlay);
                            discardPileRound.Cards.Add(player.TakeCardFromHand(cardToPlay));
                            

                        }
                    }
                }
                else
                {
                    //HERE
                    //GameLists.PlayersInRound
                    //NEW ROUND:
                    Console.WriteLine("\nNEW ROUND:");
                    roundLoop.ResetPlayersInRound();
                    discardPileRound.MoveRoundPileToDiscardPile(discardPileMain);
                }
                CheckRemainingCards();
            }
            
        }

        public void SortPlayerHands()
        {
            foreach (var player in GameLists.SeatingPlan)
            {
                player.SortCards();
            }
        }

        public void CheckRemainingCards()
        {
            PlayersHaveCards = false;
            foreach (var player in GameLists.SeatingPlan)
            {
                if (player.Hand.Count > 0)
                {
                    PlayersHaveCards = true;
                }
            }
        }

        public void ShowAllHands()
        {
            for (int i = 0; i < GameLists.SeatingPlan.Count; i++)
            {
                GameLists.SeatingPlan[i].RevealHand();
            }
        }
    }

    public class RoundDiscardPile : DiscardPile
    {
        //difference from RoundDiscardPile: (need to make them inheret)
        public void MoveRoundPileToDiscardPile(DiscardPile _discardPile)
        {
            foreach (var card in Cards)
            {
                _discardPile.Cards.Add(card);
            }
            Cards.Clear();
        }

    }

    public class DiscardPile
    {
        public List<Card> Cards { get; set; }

        public DiscardPile()
        {
            Cards = new List<Card>();
        }

        public Card GetTopCard()
        {
            Card topCard;
            if (Cards.Count == 0)
            {
                //return null card
                topCard = new Card();
                return topCard;
            }
            else
            {
                topCard = Cards[Cards.Count - 1];
                return topCard;
            }
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

