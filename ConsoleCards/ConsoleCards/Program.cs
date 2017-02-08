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
                GameLists.AllPlayers.Add(new NPC(i));
                GameLists.SeatingPlan.Add(GameLists.AllPlayers[i]);
                GameLists.PlayersInRound.Add(GameLists.AllPlayers[i]);
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

            RunRounds();

            ShowAllHands();

            Console.WriteLine("\nGAME OVER");
        }

        public void RunRounds()
        {
            //var roundLoop = new Round();
            var roundDiscardPile = new CardPile();
            var discardPile = new CardPile();

            while (GameLists.PlayerRanking.Count < GameLists.AllPlayers.Count) //WHILE PLAYERS HAVEN'T FINISHED
            {
                //start new round
                foreach (var player in GameLists.PassedPlayers)
                {
                    GameLists.PlayersInRound.Add(player);
                }
                GameLists.PassedPlayers.Clear();

                while (GameLists.PlayersInRound.Count > 1)//while more than 1 players remain in round
                {
                    foreach (var player in GameLists.SeatingPlan) //FOR EACH PLAYER (RESERVING SEATING ORDER)
                    {
                        if (GameLists.PlayersInRound.Contains(player) && GameLists.PlayersInRound.Count > 1 && !GameLists.PassedPlayers.Contains(player))
                        {
                            Card cardToPlay = player.SelectCardFromHand(discardPile.GetTopCard(), roundDiscardPile.GetTopCard());

                            switch (cardToPlay.tag)
                            {
                                case "StartingCard":
                                    player.Hand.Remove(cardToPlay); //REMOVE STARTING CARD FROM HAND
                                    roundDiscardPile.Cards.Add(cardToPlay); //ADD CARD TO PLAYED CARDS PILE
                                    Console.WriteLine("NPC {0} Plays {1} {2}", player.Id.ToString(), cardToPlay.name, cardToPlay.shorthand);
                                    break;

                                case "NoStartingCard":
                                    player.Hand.Remove(cardToPlay);
                                    break;

                                case "Pass":
                                    player.Hand.Remove(cardToPlay);
                                    GameLists.PassedPlayers.Add(player);
                                    GameLists.PlayersInRound.Remove(player);
                                    Console.WriteLine("NPC {0} PASSES", player.Id);
                                    break;

                                case "default":
                                    player.Hand.Remove(cardToPlay); //REMOVE CARD FROM HAND
                                    roundDiscardPile.Cards.Add(cardToPlay); //ADD CARD TO PLAYED CARDS PILE
                                    Console.WriteLine("NPC {0} Plays {1} {2}", player.Id.ToString(), cardToPlay.name, cardToPlay.shorthand);
                                    if (player.Hand.Count == 0) //IF PLAYER HAS NO MORE CARDS
                                    {
                                        //WIN GAME
                                        GameLists.PlayersInRound.Remove(player);//REMOVE PLAYER FROM ROUND
                                        GameLists.PlayerRanking.Add(player); //ADD PLAYER TO RANKING LIST
                                        Console.Write("**NPC {0} is out and is ranked number {1}**\n", player.Id, GameLists.PlayerRanking.Count);
                                    }
                                    break;
                            }
                            Console.ReadLine();
                        }
                    }
                }
                if (GameLists.PlayerRanking.Count == GameLists.AllPlayers.Count -1) //THIS PLAYER IS ASSHOLE
                {
                    GameLists.PlayerRanking.Add(GameLists.PlayersInRound[0]);
                    Console.Write("**NPC {0} lost and is now the Asshole**\n", GameLists.PlayersInRound[0].Id, GameLists.PlayerRanking.Count);
                    break;
                }
                else
                {
                    //WON ROUND
                    roundDiscardPile.MoveCardsToPile(discardPile);
                    GameLists.SeatingPlan = ListItemToFrontOfListReservingOrder(GameLists.SeatingPlan, GameLists.PlayersInRound.Find(x => x.hasCards == true)); //PLAYER WHO WON ROUND STARTS
                    Console.WriteLine("\nNEW ROUND:\n");
                }

            }
            Console.WriteLine("GG");
            Console.ReadLine();
        }

        //REFACTOR FROM HERE DOWN:

        public List<NPC> ListItemToFrontOfListReservingOrder(List<NPC> list, NPC NewFirstListItem)
        {
            var tempList = new List<NPC>();
            tempList.Add(NewFirstListItem);
            for (int i = 0; i < list.Count - 1; i++)
            {
                tempList.Add(list.Find(x => x.Id == GetNextId(tempList[tempList.Count - 1].Id, list.Count)));
            }
            return tempList;
        }
        public int GetNextId(int currentId, int listCount)
        {
            return (currentId + 1 > listCount) ? (1) : (currentId + 1);
        }


        public void SortPlayerHands()
        {
            foreach (var player in GameLists.SeatingPlan)
            {
                player.SortCards();
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

