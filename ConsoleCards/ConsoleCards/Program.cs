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
            //while (true)
            //{
                var newGame = new PresidentsAndAssholes(3, 4);
            //}

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
            Commentary.GameStart();

            NewSetup();
            StartRounds();

            Commentary.GameOver();
        }

        public void NewSetup()
        {
            PlayersInRound.Clear();
            SeatingPlan.Clear();

            //add NPCs to table
            for (int i = 0; i < PresidentsAndAssholes.npcTotal; i++)
            {
                //PresidentsAndAssholes.AllPlayers[i].Hand.Clear();
                SeatingPlan.Add(PresidentsAndAssholes.AllPlayers[i]);
                PlayersInRound.Add(PresidentsAndAssholes.AllPlayers[i]);
            }

            //Dealer shuffles deck
            var dealer = new Dealer(0);
            dealer.CreateDeck();
            dealer.ShuffleDeck();

            //Dealer Deals all cards to players
            int cardsPerPlayer = dealer.DealerDeck.Cards.Count / PresidentsAndAssholes.npcTotal;
            dealer.Deal(SeatingPlan, cardsPerPlayer);

            //Players Sort their hands
            foreach (var player in PresidentsAndAssholes.AllPlayers)
            {
                player.SortCards();
                player.GroupCards(player.Hand);
            }

            if (Ranking.PlayerRanking.Count != 0)
            {
                Ranking.SwapCards();
            }

        }

        public void StartRounds()
        {
            var roundDiscardPile = new CardPile();
            var discardPile = new CardPile();

            while (PlayersInRound[0].Hand.Count > 0) // (PresidentsAndAssholes.PlayerRanking.Count < PresidentsAndAssholes.AllPlayers.Count) //WHILE PLAYERS HAVEN'T FINISHED
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

                            switch (cardToPlay[0].Tag)
                            {
                                case "StartingCard":
                                    player.Hand.RemoveAll(x => x.Tier == cardToPlay[0].Tier); //REMOVE STARTING CARD FROM HAND
                                    roundDiscardPile.Cards.AddRange(cardToPlay); //ADD CARD TO PLAYED CARDS PILE
                                    Commentary.StartingCard(roundDiscardPile, player, cardToPlay);
                                    break;

                                case "NoStartingCard":
                                    player.Hand.Remove(cardToPlay[0]);
                                    break;

                                case "Pass":
                                    player.Hand.Remove(cardToPlay[0]);
                                    PassedPlayers.Add(player);
                                    PlayersInRound.Remove(player);
                                    Commentary.Pass(player);
                                    break;

                                case "default":
                                    foreach (var card in cardToPlay)
                                    {
                                        player.Hand.Remove(card); //REMOVE CARDS FROM HAND
                                    }
                                    //player.Hand.RemoveAll(x => x.tier == cardToPlay[0].tier); //REMOVE CARD FROM HAND
                                    roundDiscardPile.Cards.AddRange(cardToPlay); //ADD CARD TO PLAYED CARDS PILE
                                    Commentary.Play(roundDiscardPile, cardToPlay, player);

                                    if (player.Hand.Count == 0) //IF PLAYER HAS NO MORE CARDS
                                    {
                                        //FINISHED HAND
                                        PlayersInRound.Remove(player);//REMOVE PLAYER FROM ROUND
                                        Ranking.PlayerRanking.Add(player); //ADD PLAYER TO RANKING LIST
                                        Commentary.PlayerRanked(player);
                                    }
                                    else
                                    {
                                        player.GroupCards(player.Hand);
                                    }
                                    break;
                            }
                            //Console.ReadLine(); //turn on to step through each turn
                        }
                    }
                }
                if (Ranking.PlayerRanking.Count == PresidentsAndAssholes.AllPlayers.Count - 1) //THIS PLAYER IS ASSHOLE //change to if playersnround == 1
                {
                    Ranking.PlayerRanking.Add(PlayersInRound[0]);
                    Commentary.PlayerRanked(PlayersInRound[0]);
                    Commentary.ShowCards(PlayersInRound[0].Id.ToString(), PlayersInRound[0].Hand);
                    //asshole cleans up cards
                    for (int i = 0; i < PresidentsAndAssholes.AllPlayers.Count - 1; i++)
                    {
                        PresidentsAndAssholes.AllPlayers[i].Hand.Clear();
                    }
                    break;
                }
                else
                {
                    //WON ROUND
                    roundDiscardPile.MoveCardsToPile(discardPile);
                    SeatingPlan = ListItemToFrontOfListReservingOrder(
                        SeatingPlan, PlayersInRound.Find(x => x.hasCards == true)); //PLAYER WHO WON ROUND STARTS
                    Commentary.NewRound();

                }
            }
        }
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

