using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleCards
{
    public class Commentary
    {
        public static void GameStart() { Console.WriteLine("\n GAME START"); }
        public static void NewRound() { Console.WriteLine("\n NEW ROUND:\n"); }
        public static void Shuffling() { Console.WriteLine("\n SHUFFLING.."); }
        public static void DealerDealing() { Console.WriteLine("\n DEALER IS DEALING.."); }
        public static void GameOver() { Console.WriteLine("\n GAME OVER"); }
        public static void GeneratingDeck() { Console.WriteLine("\n GENERATING DECK..."); }

        public static void ShowCards(string who, List<Card> Cards)
        {
            Console.WriteLine("\n NPC {0} IS REVEALING REMAINING CARDS...", who);

            int num = 0;
            foreach (var card in Cards)
            {
                num++;
                Console.WriteLine(" #{0,2} T:{1,3} {2} {3}", num, card.Tier, card.Shorthand, card.Name);
            }
        }


        public static void ShowCards(List<NPC> playerList)
        {
            foreach (var player in playerList)
            {
                Console.WriteLine("\n NPC {0} IS REVEALING REMAINING CARDS...", player.Id);

                int num = 0;
                foreach (var card in player.Hand)
                {
                    num++;
                    Console.WriteLine(" #{0,2} T:{1,3} {2} {3}", num, card.Tier, card.Shorthand, card.Name);
                }
            }
        }

        public static void StartingCard(CardPile roundDiscardPile, NPC player, List<Card> cardToPlay)
        {
            if (roundDiscardPile.GetTopCard().cardDupCount == 1)
            {
                Console.WriteLine("  NPC {0} Plays the starting card ({1})", player.Id.ToString(), cardToPlay[0].Name);
            }
            else if (roundDiscardPile.GetTopCard().cardDupCount == 2)
            {
                Console.WriteLine("  NPC {0} Plays a {1} and {2} other {3}", player.Id.ToString(), cardToPlay[0].Name, cardToPlay[0].cardDupCount - 1, cardToPlay[0].Value);
            }
            else
            {
                Console.WriteLine("  NPC {0} Plays a {1} and {2} other {3}s", player.Id.ToString(), cardToPlay[0].Name, cardToPlay[0].cardDupCount - 1, cardToPlay[0].Value);
            }
        }

        public static void Pass(NPC player)
        {
            Console.WriteLine("  NPC {0} PASSES", player.Id);
        }

        public static void Play(CardPile roundDiscardPile, List<Card> cardToPlay, NPC player)
        {
            if (roundDiscardPile.GetTopCard().cardDupCount == 1 || roundDiscardPile.GetTopCard() == null)
            {
                Console.WriteLine("  NPC {0} Plays {1} {2}",
                    player.Id.ToString(), cardToPlay[0].cardDupCount, cardToPlay[0].Name);
            }
            else
            {
                Console.WriteLine("  NPC {0} Plays {1} {2}",
                    player.Id.ToString(), cardToPlay[0].cardDupCount, cardToPlay[0].Value + "s");
            }
        }

        public static void PlayerRanked(NPC player, List<NPC> playersInRound)
        {//THIS ISNT WORKING RIGHT
            string rankingName;

            if (Ranking.PlayerRanking.IndexOf(player) == 0)
            {
                rankingName = "President";
            }
            else if (Ranking.PlayerRanking.IndexOf(player) == 1)
            {
                rankingName = "Vice President";
            }

            else if (player ==  playersInRound[0])
            {
                rankingName = "Asshole"; //fix this
            }
            else if (Ranking.PlayerRanking.IndexOf(player) == Ranking.PlayerRanking.Count - 1)
            {
                rankingName = "Vice Asshole";
            }
            else
            {
                rankingName = "Neutral";
            }

            Console.WriteLine("\n ** NPC {0} is out and has ranked {1} **\n", player.Id, rankingName);
        }


        public static void SwapCards(NPC president, Card low1, Card low2, NPC Asshole, Card high1, Card high2)
        {
            Console.WriteLine("\nThe president ({0}) gave the asshole ({1}) {2} and {3} in exchange for {4} and {5}",
                president.Id.ToString(), Asshole.Id.ToString(), low1.Shorthand, low2.Shorthand, high1.Shorthand, high2.Shorthand);
        }
        public static void SwapCards(NPC vicePresident, Card low, NPC viceAsshole, Card high)
        {
            Console.WriteLine("The vice-president ({0}) gave the vice-asshole ({1}) {2} in exchange for {3}\n",
                vicePresident.Id.ToString(), viceAsshole.Id.ToString(), low.Shorthand, high.Shorthand);
        }

        public static void ScoreBoard()
        {
            var sb = new StringBuilder();
            foreach (var player in PresidentsAndAssholes.AllPlayers)
            {
                int pnaScore = 0 + player.Score[0] - player.Score[3];
                sb.Append(String.Format("\nNPC {0} Score: P:{1,2} | VP:{2,2} | VA:{3,2} | A:{4,2}   P&A: {5}",
                    player.Id, player.Score[0], player.Score[1],player.Score[2], player.Score[3],pnaScore));

            }
            Console.WriteLine(sb);
        }
    }
}

