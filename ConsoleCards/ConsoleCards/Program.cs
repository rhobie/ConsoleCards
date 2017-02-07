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
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            //add select game if there is ever more than one..
            var newGame = new PresidentsAndAssholes(1, 3);

            Console.ReadLine();
        }
    }

    public class Game
    {
        public static bool playersHaveCards = false;

        public Game()
        {
            //create NPCs
            for (int i = 0; i < PresidentsAndAssholes.npcTotal; i++)
            {
                GameLists.SeatingPlan.Add(new NPC(i));
                GameLists.PlayersInRound.Add(GameLists.SeatingPlan[i]);
            }

            //Dealer shuffles deck
            var dealer = new Dealer(0);
            dealer.CreateDeck();
            dealer.ShuffleDeck();

            //Dealer Deals all cards to players
            int cardsPerPlayer = dealer.dealerDeck.Cards.Count / PresidentsAndAssholes.npcTotal;
            dealer.Deal(GameLists.SeatingPlan, cardsPerPlayer);

            //Players Sort their hands
            SortPlayerHands();

            ShowAllHands();

            Console.WriteLine("\nGAME START");

            StartGameLoop();

            ShowAllHands();

            Console.WriteLine("\nGAME OVER");
        }

        public void StartGameLoop()
        {
            var discardPileMain = new CardPile();
            var roundDiscardPile = new CardPile();

            while (AllCardsRemaining() > 0) //WHILE PLAYERS HAVE CARDS
            {
                //START NEW ROUND
                var roundLoop = new Round();

                foreach (var player in GameLists.SeatingPlan) //FOR EVERY PLAYER
                {
                    if (GameLists.PlayersInRound.Count > 1) //IF MORE THAN ONE PLAYERS REMAIN IN ROUND
                    {
                        if (GameLists.PlayersInRound.Contains(player)) //IF PLAYER IS IN ROUND
                        {
                            //SELECT A CARD
                            Card cardToPlay = player.SelectCardFromHand(discardPileMain.GetTopCard(), roundDiscardPile.GetTopCard());

                            if (cardToPlay.name != "none") //IF CARD IS NOT NULL OR SKIP
                            {
                                player.Hand.Remove(cardToPlay); //REMOVE CARD FROM HAND
                                roundDiscardPile.Cards.Add(cardToPlay); //ADD CARD TO PLAYED CARDS PILE

                                if (player.Hand.Count == 0) //IF PLAYER HAS NO MORE CARDS
                                {
                                    GameLists.PlayersInRound.Remove(player); //REMOVE PLAYER FROM ROUND
                                    GameLists.PlayerRanking.Add(player); //ADD PLAYER TO RANKING LIST
                                }

                                Console.WriteLine("NPC {0} Plays {1} {2}", player.Id.ToString(), cardToPlay.name, cardToPlay.shorthand);
                                Console.ReadLine();
                            }
                        }
                    }
                }

                if (GameLists.PlayersInRound.Count <= 1) //IF ONE PLAYER REMAINS, START NEW ROUND
                {
                    Console.WriteLine("\nNEW ROUND:");
                    GameLists.SeatingPlan = PlayerWhoWonRoundStarts();

                    roundLoop.ResetPlayersInRound(); //ERRORS HERE.....
                    roundDiscardPile.MoveCardsToPile(discardPileMain);
                }
            }
        }


        //REFACTOR FROM HERE DOWN:

        public List<NPC> PlayerWhoWonRoundStarts()
        {
            var tempList = new List<NPC>();
            tempList.Add(GameLists.PlayersInRound[0]);
            for (int i = 0; i < GameLists.SeatingPlan.Count-1; i++)
            {
                tempList.Add(GameLists.SeatingPlan.Find(x => x.Id == GetNextId(tempList[tempList.Count - 1].Id)));
            }
            return tempList;
        }

        public int GetNextId(int currentId)
        {
            int next = currentId + 1;

            if (next > PresidentsAndAssholes.npcTotal)
            {
                next = 1;
            }

            return next;
        }
        
        public void SortPlayerHands()
        {
            foreach (var player in GameLists.SeatingPlan)
            {
                player.SortCards();
            }
        }


        public int AllCardsRemaining()
        {
            int count = 0;
            foreach (var player in GameLists.SeatingPlan)
            {
                count += player.Hand.Count;
            }
            return count;
        }

        public void ShowAllHands()
        {
            for (int i = 0; i < GameLists.SeatingPlan.Count; i++)
            {
                GameLists.SeatingPlan[i].RevealHand();
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

