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

            var newGame = new PresidentsAndAssholes(3, 10);

            Console.ReadLine();
        }
    }

    public class Game
    {
        public static List<NPC> PlayersInRound = new List<NPC>();
        public static List<NPC> SeatingPlan = new List<NPC>();
        public static List<NPC> PassedPlayers = new List<NPC>();

        public Game()
        {
            //add NPCs to table
            for (int i = 0; i < PresidentsAndAssholes.npcTotal; i++)
            {
                SeatingPlan.Add(PresidentsAndAssholes.AllPlayers[i]);
                PlayersInRound.Add(PresidentsAndAssholes.AllPlayers[i]);
            }

            Console.WriteLine("\nGAME START");

            NewSetup();
            StartRounds();

            Console.WriteLine("\nGAME OVER");
        }

        public void NewSetup()
        {
            //Dealer shuffles deck
            var dealer = new Dealer(0);
            dealer.CreateDeck();
            dealer.ShuffleDeck();

            //Dealer Deals all cards to players
            int cardsPerPlayer = dealer.dealerDeck.Cards.Count / PresidentsAndAssholes.npcTotal;
            dealer.Deal(SeatingPlan, cardsPerPlayer);

            //Players Sort their hands
            foreach (var player in PresidentsAndAssholes.AllPlayers)
            {
                player.SortCards();
                player.GroupCardsInList(player.Hand);
            }
        }

        public void StartRounds()
        {
            var roundDiscardPile = new CardPile();
            var discardPile = new CardPile();

            while (PresidentsAndAssholes.PlayerRanking.Count < PresidentsAndAssholes.AllPlayers.Count) //WHILE PLAYERS HAVEN'T FINISHED
            {
                //start new round
                foreach (var player in PassedPlayers)
                {
                    PlayersInRound.Add(player);
                }
                PassedPlayers.Clear();

                while (PlayersInRound.Count > 1)//WHILE MORE THAN ONE PLAYERS REMAIN
                {
                    foreach (var player in SeatingPlan) //FOR EACH PLAYER (RESERVING SEATING ORDER)
                    {
                        if (PlayersInRound.Count > 1 
                            && PlayersInRound.Contains(player) 
                            && !PassedPlayers.Contains(player))
                        {
                            var cardToPlay = new List<Card>();
                            cardToPlay.AddRange(player.SelectCardFromHand(discardPile.GetTopCard(), roundDiscardPile.GetTopCard()));

                           switch (cardToPlay[0].tag)
                            {
                                case "StartingCard":
                                    player.Hand.RemoveAll(x => x.tier == cardToPlay[0].tier); //REMOVE STARTING CARD FROM HAND
                                    roundDiscardPile.Cards.AddRange(cardToPlay); //ADD CARD TO PLAYED CARDS PILE

                                    //refactor all concole.writelines out to commentary class
                                    if (roundDiscardPile.GetTopCard().cardDupCount == 1)
                                    {
                                        Console.WriteLine("NPC {0} Plays the starting card ({1})", player.Id.ToString(), cardToPlay[0].name);
                                    }
                                    else if (roundDiscardPile.GetTopCard().cardDupCount == 2)
                                    {
                                        Console.WriteLine("NPC {0} Plays a {1} and {2} other {3}", player.Id.ToString(), cardToPlay[0].name, cardToPlay[0].cardDupCount - 1, cardToPlay[0].value);
                                    }
                                    else
                                    {
                                        Console.WriteLine("NPC {0} Plays a {1} and {2} other {3}s", player.Id.ToString(), cardToPlay[0].name, cardToPlay[0].cardDupCount - 1, cardToPlay[0].value);
                                    }
                                    break;

                                case "NoStartingCard":
                                    player.Hand.Remove(cardToPlay[0]);
                                    break;

                                case "Pass":
                                    player.Hand.Remove(cardToPlay[0]);
                                    PassedPlayers.Add(player);
                                    PlayersInRound.Remove(player);
                                    Console.WriteLine("NPC {0} PASSES", player.Id);
                                    break;

                                case "default":
                                    foreach (var card in cardToPlay)
                                    {
                                        player.Hand.Remove(card); //REMOVE CARDS FROM HAND
                                    }
                                    //player.Hand.RemoveAll(x => x.tier == cardToPlay[0].tier); //REMOVE CARD FROM HAND
                                    roundDiscardPile.Cards.AddRange(cardToPlay); //ADD CARD TO PLAYED CARDS PILE
                                    player.GroupCardsInList(player.Hand);
                                    if (roundDiscardPile.GetTopCard().cardDupCount == 1)
                                    {
                                        Console.WriteLine("NPC {0} Plays {1} {2}",
                                            player.Id.ToString(), cardToPlay[0].cardDupCount, cardToPlay[0].name);
                                    }
                                    else
                                    {
                                        Console.WriteLine("NPC {0} Plays {1} {2}",
                                            player.Id.ToString(), cardToPlay[0].cardDupCount, cardToPlay[0].value +"s");
                                    }
                                    if (player.Hand.Count == 0) //IF PLAYER HAS NO MORE CARDS
                                    {
                                        //FINISHED HAND
                                        PlayersInRound.Remove(player);//REMOVE PLAYER FROM ROUND
                                        PresidentsAndAssholes.PlayerRanking.Add(player); //ADD PLAYER TO RANKING LIST
                                        Console.Write("**NPC {0} is out and is ranked number {1}**\n", player.Id, PresidentsAndAssholes.PlayerRanking.Count);
                                    }
                                    break;
                            }
                            //Console.ReadLine(); //turn on to step through each turn
                        }
                    }
                }
                if (PresidentsAndAssholes.PlayerRanking.Count == PresidentsAndAssholes.AllPlayers.Count -1) //THIS PLAYER IS ASSHOLE
                {
                    PresidentsAndAssholes.PlayerRanking.Add(PlayersInRound[0]);
                    Console.Write("**NPC {0} lost and is now the Asshole**\n", PlayersInRound[0].Id, PresidentsAndAssholes.PlayerRanking.Count);
                    Commentary.ShowCards("NPC " + PlayersInRound[0].Id.ToString(), PlayersInRound[0].Hand);
                    break;
                }
                else
                {
                    //WON ROUND
                    roundDiscardPile.MoveCardsToPile(discardPile);
                    SeatingPlan = ListItemToFrontOfListReservingOrder(
                        SeatingPlan, PlayersInRound.Find(x => x.hasCards == true)); //PLAYER WHO WON ROUND STARTS

                    Console.WriteLine("\nNEW ROUND:\n");
                }
            }
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

        public void ShowAllHands()
        {
            for (int i = 0; i < SeatingPlan.Count; i++)
            {
                SeatingPlan[i].RevealHand();
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

